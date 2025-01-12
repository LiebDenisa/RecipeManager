using SQLite;
using SQLiteNetExtensions.Attributes;

namespace RecipeManager.Models
{
    public class RecipeIngredient
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public int RecipeID { get; set; }

        public int IngredientID { get; set; }

        [ManyToOne]
        public Ingredient Ingredient { get; set; }
    }
}
