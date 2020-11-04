using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class OrderManagerController : Controller
    {
        IOrderService orderService;

        public OrderManagerController(IOrderService orderService)
        {
            this.orderService = orderService;
        }
        // GET: OrderManager
        public ActionResult Index()
        {
            List<OrderModel> orders = orderService.GetOrderList();

            return View(orders);
        }

        public ActionResult UpdateOrder(string id)
        {
            ViewBag.StatusList = new List<string>()
            { 
                "Order Created",
                "Payment Processed",
                "Order Shipped", 
                "Order Complete"
            };

            OrderModel order = orderService.GetOrder(id);

            return View(order);

        }

        [HttpPost]
        public ActionResult UpdateOrder(OrderModel updatedOrder, string id)
        {
            OrderModel order = orderService.GetOrder(id);

            order.OrderStatus = updatedOrder.OrderStatus;

            orderService.UpdateOrder(order);

            return RedirectToAction("Index");
        }
    }
}