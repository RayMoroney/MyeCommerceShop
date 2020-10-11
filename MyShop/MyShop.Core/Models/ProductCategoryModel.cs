using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Models
{
    public class ProductCategoryModel
    {
        public string Id { get; set; }
        public string Category { get; set; }

        public ProductCategoryModel()
        {
            this.Id = Guid.NewGuid().ToString();
        }
    }
}
