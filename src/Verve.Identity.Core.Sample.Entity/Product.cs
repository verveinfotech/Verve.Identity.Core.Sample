using System;

namespace Verve.Identity.Core.Sample.Entity
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public decimal Price { get; set; }

        public decimal SpecialPrice { get; set; }

        public int Quantity { get; set; }

        public string Sku { get; set; }
    }
}
