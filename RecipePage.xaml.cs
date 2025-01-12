using RecipeManager.Models;

namespace RecipeManager
{
    public partial class RecipePage : ContentPage
    {
        public RecipePage(Recipe recipe)
        {
            InitializeComponent();
            BindingContext = recipe ?? new Recipe { Ingredients = new List<RecipeIngredient>() };

            MessagingCenter.Subscribe<IngredientPage, Ingredient>(this, "AddIngredientToRecipe", (sender, ingredient) =>
            {
                var viewModel = (Recipe)BindingContext;
                viewModel.Ingredients ??= new List<RecipeIngredient>();

                // Creează un nou obiect RecipeIngredient și îl adaugă la listă
                var recipeIngredient = new RecipeIngredient
                {
                    IngredientID = ingredient.ID,
                    Ingredient = ingredient
                };

                viewModel.Ingredients.Add(recipeIngredient);

                // Actualizează lista de ingrediente
                ingredientListView.ItemsSource = null;
                ingredientListView.ItemsSource = viewModel.Ingredients;
            });


        }

        public RecipePage() : this(new Recipe())
        {
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

            await App.Database.SaveRecipeAsync(recipe);

            foreach (var ingredient in recipe.Ingredients)
            {
                await App.Database.SaveRecipeIngredientAsync(ingredient);
            }

            await DisplayAlert("Success", "Recipe saved successfully!", "OK");

            ingredientListView.IsEnabled = false;
            addIngredientButton.IsEnabled = false;
            recipeNameEditor.IsEnabled = false;
            descriptionEditor.IsEnabled = false;
        }

    }
}
