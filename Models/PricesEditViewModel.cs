using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ProductsValidation.Models
{
    public class PricesEditViewModel
    {
        public Product.Category Category { get; set; }
        public List<ProductPriceItem> Items { get; set; } = new List<ProductPriceItem>();
    }

    public class ProductPriceItem
    {
        public int Id { get; set; }

        [BindNever] // Name тільки для показу, з форми не береться
        public string Name { get; set; }

        [Range(typeof(decimal), "0", "100000", ParseLimitsInInvariantCulture = true,
            ErrorMessage = "Price must be between 0 and 100000.")]
        public decimal Price { get; set; }
    }
}