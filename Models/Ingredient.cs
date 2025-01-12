using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace RecipeManager.Models
{
    public class Ingredient
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Name { get; set; }

        [OneToMany]
        public List<RecipeIngredient> RecipeIngredients { get; set; }
    }
}
