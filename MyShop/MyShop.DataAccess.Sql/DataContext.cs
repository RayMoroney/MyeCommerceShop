using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.Sql
{
    public class DataContext : DbContext
    {
        public DataContext() : base("DefaultConnection")
        {
        }

        public DbSet<ProductModel> Products { get; set; }
        public DbSet<ProductCategoryModel> ProductCategories { get; set; }
        public DbSet<BasketModel> Baskets { get; set; }
        public DbSet<BasketItemModel> BasketItems { get; set; }
    }
}
