﻿using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Contracts
{
    public interface IOrderService
    {
        void CreateOrder(OrderModel baseOrder, List<BasketItemViewModel> basketItems);
        List<OrderModel> GetOrderList();
        OrderModel GetOrder(string id);
        void UpdateOrder(OrderModel updatedOrder);
    }
}
