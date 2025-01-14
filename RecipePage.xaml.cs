using Plugin.LocalNotification;
using RecipeManager.Models;
using Microsoft.Maui.Dispatching;

namespace RecipeManager
{
    public partial class RecipePage : ContentPage
    {
        private IDispatcherTimer _timer;

        public RecipePage(Recipe recipe)
        {
            InitializeComponent();
            BindingContext = recipe ?? new Recipe { Ingredients = new List<RecipeIngredient>() };

            // Load ingredients for the recipe immediately when the page is opened
            LoadIngredientsForRecipe(recipe);

            // Start the timer to check for reminders
            StartReminderTimer();

            // Subscribe to ingredient addition
            MessagingCenter.Subscribe<IngredientPage, Ingredient>(this, "AddIngredientToRecipe", (sender, ingredient) =>
            {
                var viewModel = (Recipe)BindingContext;
                viewModel.Ingredients ??= new List<RecipeIngredient>();

                var recipeIngredient = new RecipeIngredient
                {
                    RecipeID = viewModel.ID,
                    IngredientID = ingredient.ID,
                    Ingredient = ingredient
                };

                viewModel.Ingredients.Add(recipeIngredient);

                ingredientListView.ItemsSource = null;
                ingredientListView.ItemsSource = viewModel.Ingredients;
            });
        }

        public RecipePage() : this(new Recipe())
        {
        }

        // Load ingredients for the current recipe
        private async void LoadIngredientsForRecipe(Recipe recipe)
        {
            if (recipe != null && recipe.ID != 0)
            {
                // Fetch the ingredients from the database
                recipe.Ingredients = await App.Database.GetIngredientsForRecipeAsync(recipe.ID);

                // Bind the list to the ListView
                ingredientListView.ItemsSource = recipe.Ingredients.Select(ri => ri.Ingredient).ToList();
            }
        }

        // Start a timer to check for reminders
        private void StartReminderTimer()
        {
            _timer = Dispatcher.CreateTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += CheckReminderTime;
            _timer.Start();
        }

        // Check if the current time matches the reminder time
        private void CheckReminderTime(object sender, EventArgs e)
        {
            var recipe = (Recipe)BindingContext;
            var currentTime = DateTime.Now.TimeOfDay;

            if (recipe.ReminderTime.Hours == currentTime.Hours && recipe.ReminderTime.Minutes == currentTime.Minutes)
            {
                TriggerInAppNotification(recipe);

                // Stop the timer to avoid repeated notifications
                _timer.Stop();
            }
        }

        // Trigger an in-app notification
        private void TriggerInAppNotification(Recipe recipe)
        {
            var request = new NotificationRequest
            {
                NotificationId = recipe.ID,
                Title = "Recipe Reminder",
                Description = $"It's time to prepare the recipe: {recipe.Name}"
            };

            LocalNotificationCenter.Current.Show(request);
        }

        // Set a reminder time
        async void OnSetReminderClicked(object sender, EventArgs e)
        {
            var recipe = (Recipe)BindingContext;
            var reminderTime = reminderTimePicker.Time;
            recipe.ReminderTime = reminderTime;

            await DisplayAlert("Reminder Set", $"You will be reminded at {reminderTime}.", "OK");
        }

        // Navigate to IngredientPage to add an ingredient
        async void OnAddIngredientClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new IngredientPage());
        }

        // Save the recipe to the database
        async void OnSaveRecipeClicked(object sender, EventArgs e)
        {
            var recipe = (Recipe)BindingContext;

            // Save the recipe
            await App.Database.SaveRecipeAsync(recipe);

            // Refresh the ingredient list after saving
            ingredientListView.ItemsSource = recipe.Ingredients.Select(ri => ri.Ingredient).ToList();

            await DisplayAlert("Success", "Recipe saved successfully!", "OK");

            // Disable editing fields
            ingredientListView.IsEnabled = false;
            addIngredientButton.IsEnabled = false;
            recipeNameEditor.IsEnabled = false;
            descriptionEditor.IsEnabled = false;
        }

        // Delete an ingredient from the recipe
        async void OnDeleteIngredientClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var ingredientToDelete = button?.CommandParameter as RecipeIngredient;

            if (ingredientToDelete != null)
            {
                bool confirmDelete = await DisplayAlert("Confirm Delete", $"Are you sure you want to delete the ingredient \"{ingredientToDelete.Ingredient.Name}\"?", "Yes", "No");

                if (confirmDelete)
                {
                    var recipe = (Recipe)BindingContext;

                    // Remove the ingredient from the database
                    await App.Database.DeleteRecipeIngredientAsync(ingredientToDelete);

                    // Update the UI
                    recipe.Ingredients.Remove(ingredientToDelete);
                    ingredientListView.ItemsSource = recipe.Ingredients.Select(ri => ri.Ingredient).ToList();
                }
            }
        }

        // Stop the timer when the page disappears
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _timer?.Stop();
        }
    }
}
