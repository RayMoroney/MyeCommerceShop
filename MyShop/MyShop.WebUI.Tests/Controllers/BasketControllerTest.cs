using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.Services;
using MyShop.WebUI.Controllers;
using MyShop.WebUI.Tests.Mocks;

namespace MyShop.WebUI.Tests.Controllers
{
    [TestClass]
    public class BasketControllerTest
    {
        [TestMethod]
        public void CanAddBasketItem()
        {
            // Setup.
            IRepository<BasketModel> baskets = new MockContext<BasketModel>();
            IRepository<ProductModel> products = new MockContext<ProductModel>();
            IBasketService basketService = new BasketService(products, baskets);

            var controller = new BasketController(basketService);
            
            var httpContext = new MockHttpContext();

            controller.ControllerContext = new System.Web.Mvc.ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            // Act.
            // basketService.AddToBasket(httpContext, "1");
            controller.AddToBasket("1");

            BasketModel basket = baskets.Collection().FirstOrDefault();

            // Assert.
            Assert.IsNotNull(basket);
            Assert.AreEqual(basket.BasketItems.Count, 1);
            Assert.AreEqual(basket.BasketItems.ToList().FirstOrDefault().ProductId, "1");
        }

        [TestMethod]
        public void CanGetSummaryViewModel()
        {
            IRepository<BasketModel> baskets = new MockContext<BasketModel>();
            IRepository<ProductModel> products = new MockContext<ProductModel>();
            IBasketService basketService = new BasketService(products, baskets);

            var httpContext = new MockHttpContext();            

            var controller = new BasketController(basketService);
            controller.ControllerContext = new System.Web.Mvc.ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            products.Insert(new ProductModel() { Id = "1", Price = 10.00m });
            products.Insert(new ProductModel() { Id = "2", Price = 5.00m });

            BasketModel basket = new BasketModel();

            httpContext.Request.Cookies.Add(new System.Web.HttpCookie("eCommerceBasket", basket.Id));

            basket.BasketItems.Add(new BasketItemModel() { ProductId = "1", Quantity = 2 });
            basket.BasketItems.Add(new BasketItemModel() { ProductId = "2", Quantity = 1 });
            baskets.Insert(basket);

            var result = controller.BasketSummary() as PartialViewResult;
            var basketSummary = (BasketSummaryViewModel)result.ViewData.Model;

            Assert.AreEqual(basketSummary.BasketCount, 3);
            Assert.AreEqual(basketSummary.BasketTotal, 25m);
        }
    }
}
