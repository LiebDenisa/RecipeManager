namespace RecipeManager
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            CheckLoginStatus();
        }

        private async void CheckLoginStatus()
        {
            var isUserLoggedIn = await App.Database.IsUserLoggedInAsync();

            if (isUserLoggedIn)
            {
                LoginButton.IsVisible = false;
                ShoppingListButton.IsVisible = true;
            }
            else
            {
                LoginButton.IsVisible = true;
                ShoppingListButton.IsVisible = false;
            }
        }

        // Navigate to LoginPage when Login button is clicked
        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }

        // Navigate to the Shopping Lists page
        private async void OnGoToListEntryPageClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListEntryPage());
        }
    }
}
