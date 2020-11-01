using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class BasketController : Controller
    {
        IBasketService basketService;
        IOrderService orderService;
        IRepository<CustomerModel> customers;

        public BasketController(IBasketService basketService, IOrderService orderService, IRepository<CustomerModel> customers)
        {
            this.basketService = basketService;
            this.orderService = orderService;
            this.customers = customers;
        }
        // GET: Basket
        public ActionResult Index()
        {
            var model = basketService.GetBasketItems(this.HttpContext);

            return View(model);
        }

        public ActionResult AddToBasket(string id)
        {
            basketService.AddToBasket(this.HttpContext, id);

            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromBasket(string id)
        {
            basketService.RemoveFromBasket(this.HttpContext, id);

            return RedirectToAction("Index");
        }

        public PartialViewResult BasketSummary()
        {
            var basketSummary = basketService.GetBasketSummary(this.HttpContext);

            return PartialView(basketSummary);
        }

        [Authorize]
        public ActionResult Checkout()
        {
            CustomerModel customer = customers.Collection().FirstOrDefault(c => c.Email == User.Identity.Name);

            if (customer != null)
            {
                OrderModel order = new OrderModel()
                {
                    Email = customer.Email,
                    City = customer.City,
                    State = customer.State,
                    Street = customer.Street,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    ZipCode = customer.ZipCode
                };

                return View(order);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult Checkout(OrderModel order)
        {
            var basketItems = basketService.GetBasketItems(this.HttpContext);

            order.OrderStatus = "Order Created";
            order.Email = User.Identity.Name;

            // Payment processing code would go in here.

            order.OrderStatus = "Payment Processed";

            orderService.CreateOrder(order, basketItems);
            basketService.ClearBasket(this.HttpContext);

            return RedirectToAction("ThankYou", new { OrderID = order.Id });
        }

        public ActionResult ThankYou (string orderId)
        {
            ViewBag.OrderId = orderId;

            return View();
        }
    }
}