using System.ComponentModel.DataAnnotations;

namespace CarShop.Data
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public Category()
        {

        }
    }
}
