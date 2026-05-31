using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFLearn.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? CreateId { get; set; }
        public int CategoryId { get; set; }

        
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

    }
}