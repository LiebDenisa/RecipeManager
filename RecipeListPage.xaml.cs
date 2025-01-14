using RecipeManager.Models;

namespace RecipeManager
{
    public partial class RecipeListPage : ContentPage
    {
        public RecipeListPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            recipeListView.ItemsSource = await App.Database.GetRecipesAsync();
        }

        async void OnRecipeTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedRecipe = e.Item as Recipe;
            if (selectedRecipe != null)
            {
                await Navigation.PushAsync(new RecipePage(selectedRecipe));
            }
        }

        async void OnAddRecipeClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RecipePage(new Recipe()));
        }
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
