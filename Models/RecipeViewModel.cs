using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace RecipeManager.Models
{
    public class RecipeViewModel
    {
        public int RecipeID { get; set; }
        public string RecipeName { get; set; }

        public string Description { get; set; }
        public ObservableCollection<Ingredient> Ingredients { get; set; }

        public RecipeViewModel()
        {
            Ingredients = new ObservableCollection<Ingredient>();
        }

    }
}