using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Models
{
    public class BasketModel : BaseEntityModel
    {
        public virtual ICollection<BasketItemModel> BasketItems { get; set; }
        public BasketModel()
        {
            this.BasketItems = new List<BasketItemModel>();
        }
    }
}
