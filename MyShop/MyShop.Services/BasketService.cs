using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.Services
{
    public class BasketService
    {
        IRepository<ProductModel> productContext;
        IRepository<BasketModel> basketContext;

        public const string BasketSessionName = "eCommerceBasket";

        public BasketService(IRepository<ProductModel> productContext, IRepository<BasketModel> basketContext)
        {
            this.productContext = productContext;
            this.basketContext = basketContext;
        }

        private BasketModel GetBasket(HttpContextBase httpContext, bool createIfNull)
        {
            HttpCookie cookie = httpContext.Request.Cookies.Get(BasketSessionName);

            BasketModel basket = new BasketModel();

            if (cookie != null)
            {
                string basketId = cookie.Value;
                if (!String.IsNullOrEmpty(basketId))
                {
                    basket = basketContext.Find(basketId);
                }
                else 
                {
                    if (createIfNull)
                    {
                        basket = CreateNewBasket(httpContext);
                    }
                }
            }
            else
            {
                if (createIfNull)
                {
                    basket = CreateNewBasket(httpContext);
                }
            }

            return basket;
        }

        private BasketModel CreateNewBasket(HttpContextBase httpContext)
        {
            BasketModel basket = new BasketModel();

            basketContext.Insert(basket);
            basketContext.Commit();

            HttpCookie cookie = new HttpCookie(BasketSessionName);
            cookie.Value = basket.Id;
            cookie.Expires = DateTime.Now.AddDays(9);

            httpContext.Response.Cookies.Add(cookie);

            return basket;
        }

        public void AddToBasket(HttpContextBase httpContext, string productId)
        {
            BasketModel basket = GetBasket(httpContext, true);

            BasketItemModel item = basket.BasketItems.FirstOrDefault(i => i.ProductId == productId);

            if (item == null)
            {
                item = new BasketItemModel
                {
                    BasketId = basket.Id,
                    ProductId = productId,
                    Quantity = 1
                };

                basket.BasketItems.Add(item);
            }
            else
            {
                item.Quantity += 1;
            }

            basketContext.Commit();
        }

        public void RemoveFromBasket(HttpContextBase httpContext, string itemId)
        {
            BasketModel basket = GetBasket(httpContext, true);
            BasketItemModel item = basket.BasketItems.FirstOrDefault(i => i.Id == itemId);

            if (item != null)
            {
                basket.BasketItems.Remove(item);
                basketContext.Commit();
            }
        }
    }
}
