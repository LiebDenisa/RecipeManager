using SQLite;

namespace RecipeManager.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
