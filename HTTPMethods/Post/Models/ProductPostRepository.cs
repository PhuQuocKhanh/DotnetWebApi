using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTTPMethods.Post.Models
{
    public class ProductPostRepository
    {
        public List<ProductPost> Products = new List<ProductPost>()
        {
            new ProductPost() { Id = 1, Name= "Laptop", Price = 1000, Quantity = 10 },
            new ProductPost() { Id = 2, Name= "Desktop", Price = 2000, Quantity = 20 }
        };

        public async Task<int> AddProductAsync(ProductPost product)
        {
            product.Id = 3;
            Products.Add(product);
            await Task.Delay(TimeSpan.FromSeconds(1)); // giả lập độ trễ
            return product.Id;
        }

        public async Task<ProductPost> GetByIdAsync(int Id)
        {
            var product = Products.FirstOrDefault(u => u.Id == Id);
            await Task.Delay(TimeSpan.FromSeconds(1));
            return product;
        }
    }
}