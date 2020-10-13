using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.ViewModels
{
    public class ProductManagerViewModel
    {
        public ProductModel Product { get; set; }
        public IEnumerable<ProductCategoryModel> ProductCategories { get; set; }
    }
}
