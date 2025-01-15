using RecipeManager.Models;
using System.Collections.ObjectModel;


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

            var isUserLoggedIn = await App.Database.IsUserLoggedInAsync();
            if (!isUserLoggedIn)
            {
                await Navigation.PushAsync(new LoginPage());
            }
            listView.ItemsSource = await App.Database.GetIngredientsAsync();
        }


       
        async void OnSaveIngredientClicked(object sender, EventArgs e)
        {
            var ingredient = (Ingredient)BindingContext;

            if (!string.IsNullOrWhiteSpace(ingredient.Name))
            {
                // Save the ingredient to the database
                await App.Database.SaveIngredientAsync(ingredient);

                // Refresh the list immediately after saving
                listView.ItemsSource = await App.Database.GetIngredientsAsync();

                // Reset the BindingContext to a new Ingredient
                BindingContext = new Ingredient();

                await DisplayAlert("Success", "Ingredient saved successfully!", "OK");
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
            var selectedIngredient = button?.CommandParameter as Ingredient;

            if (selectedIngredient != null)
            {
                MessagingCenter.Send(this, "AddIngredientToRecipe", selectedIngredient);
                await Navigation.PopAsync();
            }
        }



    }
}
