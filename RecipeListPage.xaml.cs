using RecipeManager.Models;

namespace RecipeManager
{
    public partial class RecipeListPage : ContentPage
    {
        public RecipeListPage()
        {
            InitializeComponent();
        }

        // Check if the user is logged in before loading the page
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Check if user is logged in
            var isUserLoggedIn = await App.Database.IsUserLoggedInAsync();
            if (!isUserLoggedIn)
            {
                await DisplayAlert("Access Denied", "You need to log in to access this page.", "OK");

                // Remove the current page from the navigation stack
                Navigation.RemovePage(this);

                // Navigate to the Login Page
                await Navigation.PushAsync(new LoginPage());
                return;
            }

            // Load the recipes if the user is logged in
            recipeListView.ItemsSource = await App.Database.GetRecipesAsync();
        }

        // Navigate to the Recipe Page when a recipe is tapped
        async void OnRecipeTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedRecipe = e.Item as Recipe;
            if (selectedRecipe != null)
            {
                await Navigation.PushAsync(new RecipePage(selectedRecipe));
            }
        }

        // Navigate to the Recipe Page to add a new recipe
        async void OnAddRecipeClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RecipePage(new Recipe()));
        }

        // Delete a recipe
        async void OnDeleteRecipeClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var recipeToDelete = button?.CommandParameter as Recipe;

            if (recipeToDelete != null)
            {
                bool confirmDelete = await DisplayAlert("Confirm Delete", $"Are you sure you want to delete the recipe \"{recipeToDelete.Name}\"?", "Yes", "No");

                if (confirmDelete)
                {
                    await App.Database.DeleteRecipeAsync(recipeToDelete);
                    recipeListView.ItemsSource = await App.Database.GetRecipesAsync();
                }
            }
        }
    }
}
