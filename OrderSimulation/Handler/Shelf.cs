//-----------------------------------------------------------------------------
// File Name   : Shelf
// Author      : junlei
// Date        : 6/11/2020 6:21:06 PM
// Description : 
// Version     : 1.0.0      
// Updated     : 
//
//-----------------------------------------------------------------------------

using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderSimulation.Handler
{
    public class Shelf
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly int _capacity;
        private readonly string _name;

        private readonly HashSet<Order> _orders = new HashSet<Order>();

        private readonly Random _rand = new Random();
        public Shelf(string name, int capacity)
        {
            _name = name;
            _capacity = capacity;
        }

        /// <summary>
        /// place on the shelf
        /// </summary>
        /// <param name="order">the new order to place on the shelf</param>
        /// <param name="forceTo">if true, then randomly discard the existing one and place the new one</param>
        /// <returns>true if successfully</returns>
        internal bool Place(Order order, bool forceTo = false)
        {
            if (_orders.Count < _capacity)
            {
                Logger.Info($"Order placed: {order.Name}-{order.Temperature}-{order.Value}");
                _orders.Add(order);
                return true;
            }

            if (forceTo)
            {
                var randOrder = _orders.ElementAt(_rand.Next(_orders.Count));
                Logger.Info($"Order discarded randomly: {randOrder.Name}-{randOrder.Temperature}-{randOrder.Value}");
                _orders.Remove(randOrder);

                Logger.Info($"Order placed: {order.Name}-{order.Temperature}-{order.Value}");
                _orders.Add(order);
                return true;
            }

            return false;
        }

        internal void Remove(Order order)
        {
            _orders.Remove(order);
        }

        internal string GetInfo()
        {
            var builder = new StringBuilder($"Shelf '{_name}' has {_orders.Count} orders: \r\n");
            foreach (var ord in _orders)
            {
                builder.AppendLine($"    {ord.Name}-{ord.Value}");
            }

            return builder.ToString();
        }
    }
}
