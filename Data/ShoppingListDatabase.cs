using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using RecipeManager.Models;

namespace RecipeManager.Data
{
    public class ShoppingListDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public ShoppingListDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<ShopList>().Wait();
            _database.CreateTableAsync<Ingredient>().Wait();
            _database.CreateTableAsync<Recipe>().Wait();
            _database.CreateTableAsync<RecipeIngredient>().Wait();
            _database.CreateTableAsync<User>().Wait();
        }

        public async Task<List<Recipe>> GetRecipesAsync()
        {
            var recipes = await _database.Table<Recipe>().ToListAsync();

            // Convert ReminderTime from string to TimeSpan
            foreach (var recipe in recipes)
            {
                if (!string.IsNullOrEmpty(recipe.ReminderTime.ToString()))
                {
                    recipe.ReminderTime = TimeSpan.Parse(recipe.ReminderTime.ToString());
                }
            }

            return recipes;
        }


        public Task<List<ShopList>> GetShopListsAsync()
        {
            return _database.Table<ShopList>().ToListAsync();
        }

        public Task<ShopList> GetShopListAsync(int id)
        {
            return _database.Table<ShopList>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveShopListAsync(ShopList slist)
        {
            if (slist.ID != 0)
            {
                return _database.UpdateAsync(slist);
            }
            else
            {
                return _database.InsertAsync(slist);
            }
        }

        public Task<int> DeleteShopListAsync(ShopList slist)
        {
            return _database.DeleteAsync(slist);
        }

        public Task<List<Ingredient>> GetIngredientsAsync()
        {
            return _database.Table<Ingredient>().ToListAsync();
        }

        public Task<int> SaveIngredientAsync(Ingredient ingredient)
        {
            if (ingredient.ID != 0)
            {
                return _database.UpdateAsync(ingredient);
            }
            else
            {
                return _database.InsertAsync(ingredient);
            }
        }

        public Task<int> DeleteIngredientAsync(Ingredient ingredient)
        {
            return _database.DeleteAsync(ingredient);
        }

        public Task<int> SaveRecipeIngredientAsync(RecipeIngredient recipeIngredient)
        {
            return _database.InsertAsync(recipeIngredient);
        }

        public async Task<int> SaveRecipeAsync(Recipe recipe)
        {
            if (recipe.ID != 0)
            {
                await _database.UpdateAsync(recipe);
            }
            else
            {
                await _database.InsertAsync(recipe);
                recipe.ID = await _database.ExecuteScalarAsync<int>("SELECT last_insert_rowid()");
            }

            // Save associated ingredients
            if (recipe.Ingredients != null)
            {
                foreach (var ingredient in recipe.Ingredients)
                {
                    ingredient.RecipeID = recipe.ID;

                    // Check if the ingredient already exists in the RecipeIngredient table
                    var existing = await _database.Table<RecipeIngredient>()
                        .Where(ri => ri.RecipeID == recipe.ID && ri.IngredientID == ingredient.IngredientID)
                        .FirstOrDefaultAsync();

                    if (existing == null)
                    {
                        await SaveRecipeIngredientAsync(ingredient);
                    }
                }
            }

            return recipe.ID;
        }



        public async Task<List<RecipeIngredient>> GetIngredientsForRecipeAsync(int recipeID)
        {
            var query = "SELECT ri.ID, ri.RecipeID, ri.IngredientID, i.Name " +
                        "FROM RecipeIngredient ri " +
                        "INNER JOIN Ingredient i ON ri.IngredientID = i.ID " +
                        "WHERE ri.RecipeID = ?";

            var result = await _database.QueryAsync<RecipeIngredient>(query, recipeID);

            foreach (var recipeIngredient in result)
            {
                recipeIngredient.Ingredient = await _database.Table<Ingredient>()
                                                              .Where(i => i.ID == recipeIngredient.IngredientID)
                                                              .FirstOrDefaultAsync();
            }

            return result;
        }

        public async Task<int> DeleteRecipeAsync(Recipe recipe)
        {
            // Delete associated ingredients
            await _database.ExecuteAsync("DELETE FROM RecipeIngredient WHERE RecipeID = ?", recipe.ID);

            // Delete the recipe
            return await _database.DeleteAsync(recipe);
        }

        public Task<int> DeleteRecipeIngredientAsync(RecipeIngredient recipeIngredient)
        {
            return _database.DeleteAsync(recipeIngredient);
        }

        // Get recipes with reminders
        public async Task<List<Recipe>> GetRecipesWithReminderAsync()
        {
            return await _database.QueryAsync<Recipe>("SELECT * FROM Recipe WHERE ReminderTime IS NOT NULL");
        }
        // Register a new user
        public async Task<bool> RegisterUserAsync(string username, string password)
        {
            // Check if the username already exists
            var existingUser = await _database.Table<User>().FirstOrDefaultAsync(u => u.Username == username);
            if (existingUser != null)
            {
                return false; // Username already taken
            }

            // Hash the password before storing it
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            // Create a new user and insert it into the database
            var newUser = new User
            {
                Username = username,
                Password = hashedPassword
            };

            await _database.InsertAsync(newUser);
            return true;
        }

        // Login an existing user
        public async Task<User> LoginUserAsync(string username, string password)
        {
            // Find the user by username
            var user = await _database.Table<User>().FirstOrDefaultAsync(u => u.Username == username);

            // If user is found, verify the password
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return user;
            }

            // If the credentials are incorrect, return null
            return null;
        }

        // Check if a user is logged in
        public async Task<bool> IsUserLoggedInAsync()
        {
            var user = await _database.Table<User>().FirstOrDefaultAsync();
            return user != null;
        }

        // Logout the user
        public Task<int> LogoutUserAsync()
        {
            return _database.ExecuteAsync("DELETE FROM User");
        }

    }
}
