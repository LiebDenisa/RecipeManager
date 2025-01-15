namespace RecipeManager
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        }

        // Logout logic
        async void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            // Clear the user's session
            await App.Database.LogoutUserAsync();

            // Reset the navigation stack and navigate to LoginPage
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }
}
