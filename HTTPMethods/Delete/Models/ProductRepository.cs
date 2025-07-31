using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTTPMethods.Delete.Models
{
    public class ProductRepository
    {
         public List<Product> Products = new List<Product>()
        {
            new Product() { Id = 1, Name= "Laptop", Price = 1000, Quantity = 10, Description = "A high-performance Laptop." },
            new Product() { Id = 2, Name= "Desktop", Price = 2000, Quantity = 20 }
        };

        public async Task<Product> GetByIdAsync(int Id)
        {
            await Task.Delay(TimeSpan.FromSeconds(1)); // Giả lập thời gian truy vấn DB
            return Products.FirstOrDefault(p => p.Id == Id);
        }

        public async Task DeleteProductAsync(Product product)
        {
            await Task.Delay(TimeSpan.FromSeconds(1)); // Giả lập thao tác xóa
            Products.Remove(product);
        }
    }
}