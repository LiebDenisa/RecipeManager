using RecipeManager.Models;

namespace RecipeManager
{
    public partial class IngredientPage : ContentPage
    {
        public IngredientPage()
        {
            InitializeComponent();
            BindingContext = new Ingredient();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            listView.ItemsSource = await App.Database.GetIngredientsAsync();
        }

        async void OnSaveIngredientClicked(object sender, EventArgs e)
        {
            var ingredient = (Ingredient)BindingContext;

            if (!string.IsNullOrWhiteSpace(ingredient.Name))
            {
                await App.Database.SaveIngredientAsync(ingredient);

                MessagingCenter.Send(this, "AddIngredientToRecipe", new RecipeIngredient
                {
                    IngredientID = ingredient.ID,
                    Ingredient = ingredient
                });

                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "Ingredient name cannot be empty.", "OK");
            }
        }


        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var ingredient = listView.SelectedItem as Ingredient;
            if (ingredient != null)
            {
                await App.Database.DeleteIngredientAsync(ingredient);
                listView.ItemsSource = await App.Database.GetIngredientsAsync();
            }
        }
        async void OnAddToRecipeClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var selectedIngredient = button.CommandParameter as Ingredient;

            if (selectedIngredient != null)
            {
                MessagingCenter.Send(this, "AddIngredientToRecipe", selectedIngredient);
                await Navigation.PopAsync();
            }
        }


    }
}
