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
    }
}
