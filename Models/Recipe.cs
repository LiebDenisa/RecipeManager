using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace RecipeManager.Models
{
    public class Recipe
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [MaxLength(250)]
        public string Name { get; set; }

        public string Description { get; set; }

        // Relația cu Ingredient
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<RecipeIngredient> Ingredients { get; set; }

        // Constructor pentru inițializarea listei
        public Recipe()
        {
            Ingredients = new List<RecipeIngredient>();
        }

    }
}
