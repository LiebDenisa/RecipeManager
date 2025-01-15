namespace RecipeManager
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        // Handle the Login Button Click
        async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            var user = await App.Database.LoginUserAsync(usernameEntry.Text, passwordEntry.Text);

            if (user != null)
            {
                await DisplayAlert("Success", "Login successful!", "OK");

                // Navigate to AppShell
                Application.Current.MainPage = new AppShell();
            }
            else
            {
                await DisplayAlert("Error", "Invalid username or password.", "OK");
            }
        }

        // Navigate to the RegisterPage
        async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}
