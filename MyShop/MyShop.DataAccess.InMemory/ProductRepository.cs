using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using MyShop.Core.Models;

namespace MyShop.DataAccess.InMemory
{
    public class ProductRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<ProductModel> products;

        public ProductRepository()
        {
            products = cache["products"] as List<ProductModel>;

            if (products == null)
            {
                products = new List<ProductModel>();
            }
        }

        public void Commit()
        {
            cache["products"] = products;
        }

        public void Insert(ProductModel product)
        {
            products.Add(product);
        }

        public void Update(ProductModel product)
        {
            ProductModel productToUpdate = products.Find(p => p.Id == product.Id);

            if (productToUpdate != null)
            {
                productToUpdate = product;
            }
            else
            {
                throw new Exception("Product not found.");
            }
        }

        public ProductModel Find(string id)
        {
            ProductModel productToFind = products.Find(p => p.Id == id);

            if (productToFind != null)
            {
                return productToFind;
            }
            else
            {
                throw new Exception("Product not found.");
            }
        }

        public IQueryable<ProductModel> Collection()
        {
            return products.AsQueryable();
        }

        public void Delete(string id)
        {
            ProductModel productToDelete = products.Find(p => p.Id == id);

            if (productToDelete != null)
            {
                products.Remove(productToDelete);
            }
            else
            {
                throw new Exception("Product not found.");
            }
        }
    }
}
