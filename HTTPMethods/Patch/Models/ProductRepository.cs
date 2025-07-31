using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTTPMethods.Patch.Models
{
    public class ProductRepository
    {
        public List<Product> Products = new List<Product>()
        {
            new Product() { Id = 1, Name= "Laptop", Price = 1000, Quantity = 10, Description = "A high-performance Laptop." },
            new Product() { Id = 2, Name= "Desktop", Price = 2000, Quantity = 20 } //Description is Optional
        };
        public Product GetById(int Id)
        {
            // Find a product by ID
            return Products.FirstOrDefault(p => p.Id == Id);
        }
        public void Update(Product product)
        {
            // Find the index of the product to update
            var index = Products.FindIndex(p => p.Id == product.Id);
            if (index != -1)
            {
                // Update the product at the found index
                Products[index] = product;
            }
        }
    }
}