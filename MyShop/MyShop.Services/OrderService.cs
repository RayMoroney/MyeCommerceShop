using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Services
{
    public class OrderService : IOrderService
    {
        IRepository<OrderModel> orderContext;
        public void CreateOrder(OrderModel baseOrder, List<BasketItemViewModel> basketItems)
        {
            foreach (var item in basketItems)
            {
                baseOrder.OrderItems.Add(new OrderItemModel 
                { 
                ProductId = item.Id, 
                Image = item.Image, 
                ProductName = item.ProductName, 
                Price = item.Price, 
                Quantity = item.Quantity
                });
            }

            orderContext.Insert(baseOrder);
            orderContext.Commit();
        }

        public OrderService(IRepository<OrderModel> orderContext)
        {
            this.orderContext = orderContext;
        }
    }
}
