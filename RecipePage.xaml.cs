using RecipeManager.Models;

namespace RecipeManager
{
    public partial class RecipePage : ContentPage
    {
        public RecipePage(Recipe recipe)
        {
            InitializeComponent();
            BindingContext = recipe ?? new Recipe { Ingredients = new List<RecipeIngredient>() };

            // Dacă rețeta nu are un ID, nu afișa lista de ingrediente
            if (recipe.ID == 0)
            {
                ingredientListView.IsVisible = false;
            }
            else
            {
                ingredientListView.IsVisible = true;
                LoadIngredientsForRecipe(recipe);
            }

            MessagingCenter.Subscribe<IngredientPage, Ingredient>(this, "AddIngredientToRecipe", (sender, ingredient) =>
            {
                var viewModel = (Recipe)BindingContext;
                viewModel.Ingredients ??= new List<RecipeIngredient>();

                var recipeIngredient = new RecipeIngredient
                {
                    RecipeID = viewModel.ID,
                    IngredientID = ingredient.ID,
                    Ingredient = ingredient
                };

                viewModel.Ingredients.Add(recipeIngredient);

                ingredientListView.ItemsSource = null;
                ingredientListView.ItemsSource = viewModel.Ingredients;
            });
        }


        public RecipePage() : this(new Recipe())
        {
        }

        // Încarcă ingredientele pentru rețeta curentă
        private async void LoadIngredientsForRecipe(Recipe recipe)
        {
            if (recipe != null && recipe.ID != 0)
            {
                recipe.Ingredients = await App.Database.GetIngredientsForRecipeAsync(recipe.ID);
                ingredientListView.ItemsSource = recipe.Ingredients.Select(ri => ri.Ingredient).ToList();
            }
        }



        // Navighează către IngredientPage pentru a adăuga un ingredient
        async void OnAddIngredientClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new IngredientPage());
        }

        // Salvează rețeta în baza de date
        async void OnSaveRecipeClicked(object sender, EventArgs e)
        {
            var recipe = (Recipe)BindingContext;

            // Salvează rețeta
            await App.Database.SaveRecipeAsync(recipe);

            // Actualizează lista de ingrediente doar după ce rețeta este salvată
            ingredientListView.ItemsSource = recipe.Ingredients.Select(ri => ri.Ingredient).ToList();

            await DisplayAlert("Success", "Recipe saved successfully!", "OK");

            // Blochează câmpurile pentru editare
            ingredientListView.IsEnabled = false;
            addIngredientButton.IsEnabled = false;
            recipeNameEditor.IsEnabled = false;
            descriptionEditor.IsEnabled = false;
        }

    }
}
