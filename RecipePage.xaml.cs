using RecipeManager.Models;

namespace RecipeManager
{
    public partial class RecipePage : ContentPage
    {
        public RecipePage()
        {
            InitializeComponent();
            BindingContext = new RecipeViewModel();
            MessagingCenter.Subscribe<IngredientPage, Ingredient>(this, "AddIngredientToRecipe", (sender, ingredient) =>
            {
                var viewModel = (RecipeViewModel)BindingContext;
                viewModel.Ingredients.Add(ingredient);
                ingredientListView.ItemsSource = null; // Resetează lista pentru a forța actualizarea
                ingredientListView.ItemsSource = viewModel.Ingredients;
            });
        }

        // Navighează către IngredientPage pentru a adăuga un ingredient
        async void OnAddIngredientClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new IngredientPage());
        }

        // Salvează rețeta în baza de date
        async void OnSaveRecipeClicked(object sender, EventArgs e)
        {
            var recipeViewModel = (RecipeViewModel)BindingContext;

            var recipe = new Recipe
            {
                Name = recipeViewModel.RecipeName,
                Description = recipeViewModel.Description
            };

            await App.Database.SaveRecipeAsync(recipe);

            await DisplayAlert("Success", "Recipe saved successfully!", "OK");

            // Blochează câmpurile pentru editare
            ingredientListView.IsEnabled = false;
            addIngredientButton.IsEnabled = false;
            recipeNameEditor.IsEnabled = false;
            descriptionEditor.IsEnabled = false;
        }

    }
}
