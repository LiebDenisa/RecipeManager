using SQLite;
using SQLiteNetExtensions.Attributes;

namespace RecipeManager.Models
{
    public class RecipeIngredient
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [ForeignKey(typeof(Recipe))]
        public int RecipeID { get; set; }

        [ForeignKey(typeof(Ingredient))]
        public int IngredientID { get; set; }

        // Navigare inversă
        [ManyToOne]
        public Recipe Recipe { get; set; }

        [ManyToOne]
        public Ingredient Ingredient { get; set; }
    }
}
