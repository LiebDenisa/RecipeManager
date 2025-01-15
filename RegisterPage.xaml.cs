namespace RecipeManager
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        // Handle the Register Button Click
        async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            var isRegistered = await App.Database.RegisterUserAsync(usernameEntry.Text, passwordEntry.Text);

            if (isRegistered)
            {
                await DisplayAlert("Success", "Registration successful! You can now log in.", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "Username is already taken. Please choose a different one.", "OK");
            }
        }
    }
}
