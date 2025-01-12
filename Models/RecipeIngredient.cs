using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using SQLiteNetExtensions.Attributes;


namespace RecipeManager.Models
{
    public class RecipeIngredient
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [ForeignKey(typeof(ShopList))]
        public int RecipeID { get; set; }

        [ForeignKey(typeof(Ingredient))]
        public int IngredientID { get; set; }
    }
}
