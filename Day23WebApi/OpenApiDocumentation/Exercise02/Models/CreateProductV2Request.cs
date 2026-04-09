using System.ComponentModel.DataAnnotations;

namespace Exercise02.Models
{
    public class CreateProductV2Request
    {
        [Required, StringLength(100, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required, Range(0.01, 999999.99)]
        public decimal Price { get; set; }

        [Required]
        public string Category { get; set; } = string.Empty;

        [Range(0, 10000)]
        public int Stock { get; set; }

        public List<string> Tags { get; set; } = new();
    }

  

   
}