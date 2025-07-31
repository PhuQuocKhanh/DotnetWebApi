using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTTPMethods.PUT.Models
{
    public class ProductPUTRepository
    {
        public List<ProductPUT> Products = new List<ProductPUT>()
        {
            new ProductPUT() { Id = 1, Name= "Laptop", Price = 1000, Quantity = 10 },
            new ProductPUT() { Id = 2, Name= "Desktop", Price = 2000, Quantity = 20 }
        };
        public async Task<ProductPUT> UpdateProductAsync(ProductPUT product)
        {
            //Set the Product Id
            var existingProduct = Products.FirstOrDefault(u => u.Id == product.Id);
            if(existingProduct != null)
            {
                existingProduct.Price = product.Price;
                existingProduct.Quantity = product.Quantity;
                existingProduct.Name = product.Name;
                //Update the Product into the Data
            }
            
            await Task.Delay(TimeSpan.FromSeconds(1));
            return existingProduct;
        }
    }
}