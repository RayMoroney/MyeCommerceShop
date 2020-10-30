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
            IRepository<OrderModel> orders = new MockContext<OrderModel>();

            IBasketService basketService = new BasketService(products, baskets);
            IOrderService orderService = new OrderService(orders);

            var controller = new BasketController(basketService, orderService);
            
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
            IRepository<OrderModel> orders = new MockContext<OrderModel>();

            IBasketService basketService = new BasketService(products, baskets);
            IOrderService orderService = new OrderService(orders);

            var httpContext = new MockHttpContext();            

            var controller = new BasketController(basketService, orderService);
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

        [TestMethod]
        public void CanCheckoutAndCreateOrder()
        {
            IRepository<ProductModel> products = new MockContext<ProductModel>();
            IRepository<BasketModel> baskets = new MockContext<BasketModel>();
            IRepository<OrderModel> orders = new MockContext<OrderModel>();

            products.Insert(new ProductModel() { Id = "1", Price = 10.00m });
            products.Insert(new ProductModel() { Id = "2", Price = 5.00m });

            BasketModel basket = new BasketModel();

            basket.BasketItems.Add(new BasketItemModel() { ProductId = "1", Quantity = 2, BasketId = basket.Id });
            basket.BasketItems.Add(new BasketItemModel() { ProductId = "2", Quantity = 1, BasketId = basket.Id });
            baskets.Insert(basket);

            IBasketService basketService = new BasketService(products, baskets);

            IOrderService orderService = new OrderService(orders);

            var httpContext = new MockHttpContext();
            httpContext.Request.Cookies.Add(new System.Web.HttpCookie("eCommerceBasket", basket.Id));

            var controller = new BasketController(basketService, orderService);
            controller.ControllerContext = new System.Web.Mvc.ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            // Act.
            OrderModel order = new OrderModel();
            controller.Checkout(order);

            // Assert.
            Assert.AreEqual(2, order.OrderItems.Count);
            Assert.AreEqual(0, basket.BasketItems.Count);

            OrderModel orderInRep = orders.Find(order.Id);
            Assert.AreEqual(2, orderInRep.OrderItems.Count);
        }
    }
}
