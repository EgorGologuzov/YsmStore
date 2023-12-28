using System;
using System.Collections.Generic;
using YsmStore.Models;

namespace YsmStore.Data
{
    public static class OrderAdapter
    {
        private static Dictionary<int, Order> _list = new Dictionary<int, Order>();
        public static int LocalIdCounter = -1;

        public static Order Create(Customer customer)
        {
            Order order = new Order(LocalIdCounter);
            LocalIdCounter--;
            order.CustomerEmail = customer.Login;

            return order;
        }

        public static Order Get(int id)
        {
            Order order = new Order(id)
            {
                CustomerEmail = $"login@gmail.com",
                OrderDate = DateTime.Now,
                City = "Нижний Тагил",
                PickUpAdress = "Нижний Тагил, Улица1, дом1",
                PhoneNumber = "+7 (999) 999-99-00",
                DeliveryDate = DateTime.Now + TimeSpan.FromDays(14),
                Status = OrderStatus.Processed
            };

            _list[id] = order;

            return order;
        }

        public static string[] GetCities()
        {
            return new string[]
            {
                "Нижний Тагил",
                "Москва",
                "Санкт-Петербург"
            };
        }

        public static string[] GetAdresses(string city)
        {
            if (city == string.Empty || city == null)
                return new string[0];

            return new string[]
            {
                $"{city}, Улица1, Дом1",
                $"{city}, Улица2, Дом2",
                $"{city}, Улица3, Дом3",
                $"{city}, Улица4, Дом4",
                $"{city}, Улица5, Дом5",
                $"{city}, Улица6, Дом6"
            };
        }

        public static void Push(Order order, IList<ProductAmount> orderedProducts)
        {

        }

        public static int Execute(CustomerOrdersQuery query)
        {
            int count = query.TargetList.Count;

            for (int i = 0; i < query.LoadStep; i++)
            {
                Order order = Get(count + i);
                order.CustomerEmail = $"login_{query.CustomerToken}@gmail.com";
                query.TargetList.Add(order);
            }

            return query.TargetList.Count - count;
        }

        public static void Push(Order order)
        {

        }

        public static int Execute(AdminOrdersQuery query)
        {
            int count = query.TargetList.Count;

            if (query.OrderId != null)
            {
                query.TargetList.Add(Get(query.OrderId.Value));
                query.IsEndReached = true;
            }
            else
            {
                for (int i = 0; i < query.LoadStep; i++)
                {
                    Order order = Get(count + i);
                    order.Status = query.StatusFilter == null ? order.Status : query.StatusFilter.Value;
                    order.OrderDate = query.EndDate;
                    query.TargetList.Add(order);
                }
            }

            return query.TargetList.Count - count;
        }

        public static void Pull(Order order)
        {
            order.OrderDate = DateTime.Now;
            order.Status = OrderStatus.Processed;
        }
    }
}
