using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.InMemory
{
    public class ProductCategoryRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<ProductCategoryModel> categories;

        public ProductCategoryRepository()
        {
            categories = cache["categories"] as List<ProductCategoryModel>;

            if (categories == null)
            {
                categories = new List<ProductCategoryModel>();
            }
        }

        public void Commit()
        {
            cache["categories"] = categories;
        }

        public void Insert(ProductCategoryModel category)
        {
            categories.Add(category);
        }

        public void Update(ProductCategoryModel category)
        {
            ProductCategoryModel categoryToUpdate = categories.Find(c => c.Id == category.Id);

            if (categoryToUpdate != null)
            {
                categoryToUpdate = category;
            }
            else
            {
                throw new Exception("Category not found.");
            }
        }

        public ProductCategoryModel Find(string id)
        {
            ProductCategoryModel categoryToFind = categories.Find(c => c.Id == id);

            if (categoryToFind != null)
            {
                return categoryToFind;
            }
            else
            {
                throw new Exception("Category not found.");
            }
        }

        public IQueryable<ProductCategoryModel> Collection()
        {
            return categories.AsQueryable();
        }

        public void Delete(string id)
        {
            ProductCategoryModel categoryToDelete = categories.Find(c => c.Id == id);

            if (categoryToDelete != null)
            {
                categories.Remove(categoryToDelete);
            }
            else
            {
                throw new Exception("Category not found.");
            }
        }
    }
}
