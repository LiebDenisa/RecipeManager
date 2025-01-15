using System;
using System.Globalization;
using System.IO;
using RecipeManager.Data;

namespace RecipeManager
{
    public partial class App : Application
    {
        // Static database instance
        static ShoppingListDatabase database;

        // Database property to initialize the database connection
        public static ShoppingListDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new ShoppingListDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ShoppingList.db3"));
                }
                return database;
            }
        }

        // App constructor to set the initial page
        public App()
        {
            InitializeComponent();

            // Set the MainPage as the initial page
            MainPage = new AppShell();
        }
    }

    // Converter class (if used in XAML bindings)
    public class ZeroToFalseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value > 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
