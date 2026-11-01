using System.ComponentModel.DataAnnotations;

namespace ProductsValidation.Models
{
    public class Product
    {
        public enum Category { Toy, Technique, Clothes, Transport }

        public int Id { get; set; }

        [EnumDataType(typeof(Category))]
        public Category Type { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [ProductDescription]
        public string Description { get; set; }

        [Range(typeof(decimal), "0", "100000", ParseLimitsInInvariantCulture = true,
            ErrorMessage = "Price must be between 0 and 100000.")]
        public decimal Price { get; set; }
    }
}