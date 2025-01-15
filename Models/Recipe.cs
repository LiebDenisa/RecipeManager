using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.ObjectModel;

namespace RecipeManager.Models
{
    public class Recipe
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [MaxLength(250)]
        public string Name { get; set; }

        public string Description { get; set; }

        // Use ObservableCollection for automatic UI updates
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public ObservableCollection<RecipeIngredient> Ingredients { get; set; }

        // Constructor to initialize the collection
        public Recipe()
        {
            Ingredients = new ObservableCollection<RecipeIngredient>();
        }

        public TimeSpan ReminderTime { get; set; }
    }
}
