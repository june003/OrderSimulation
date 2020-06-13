//-----------------------------------------------------------------------------
// File Name   : Shelf
// Author      : junlei
// Date        : 6/13/2020 6:21:06 PM
// Description : 
// Version     : 1.0.0      
// Updated     : 
//
//-----------------------------------------------------------------------------

using Newtonsoft.Json.Converters;
using NLog;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace OrderSimulation.Handler
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ShelfType
    {
        [EnumMember(Value = "overflow")]
        Overflow = 0,
        [EnumMember(Value = "hot")]
        Hot = 1,
        [EnumMember(Value = "cold")]
        Cold = 2,
        [EnumMember(Value = "frozen")]
        Frozen = 3,
    };

    public class Shelf
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public int Count => _orders.Count;

        private readonly int _capacity;
        private readonly ShelfType _name;

        // keys are used here only here to replace for threadsafe HashSet 
        private readonly ConcurrentDictionary<Order, byte> _orders = new ConcurrentDictionary<Order, byte>();

        private readonly Random _rand = new Random();
        public Shelf(ShelfType name, int capacity)
        {
            _name = name;
            _capacity = capacity;
        }

        // private readonly object _lock = new object();
        /// <summary>
        /// place on the shelf
        /// </summary>
        /// <param name="order">the new order to place on the shelf</param>
        /// <param name="forceTo">if true, then randomly discard the existing one and place the new one</param>
        /// <returns>true if successfully</returns>
        internal bool Place(Order order, bool forceTo = false)
        {
            if (_orders.Count >= _capacity)
            {
                if (!forceTo) // hot/cold/frozen full
                {
                    return false;
                }

                // for overflow shelf
                var randOrder = _orders.ElementAt(_rand.Next(_orders.Count)).Key;
                Logger.Info($"Order discarded randomly: {randOrder.Name}-{randOrder.ShelfType}-{randOrder.Value}");

                _ = _orders.TryRemove(order, out byte val);
            }

            order.ActuallyPlacedShelfType = _name;
            Logger.Info($"Order placed: {order.Name}-{order.ShelfType}-{order.Value} on shelf: {_name}");
            _ = _orders.TryAdd(order, 0);
            order.OnDie += Order_OnDie;
            return true;
        }

        internal bool Remove(Order order)
        {
            if (_orders.ContainsKey(order))
            {
                order.OnDie -= Order_OnDie;
                _ = _orders.TryRemove(order, out byte val);
                return true;
            }

            return false;
        }

        private void Order_OnDie(Order order)
        {
            Remove(order);
            Logger.Warn($"Order dies: {order.Name}-{order.Value}");
        }

        internal string GetInfo()
        {
            var builder = new StringBuilder($"Shelf '{_name}' has {_orders.Count} orders: \r\n");
            foreach (var ord in _orders.Keys)
            {
                builder.AppendLine($"    {ord.Name}-{ord.Value}");
            }

            return builder.ToString();
        }
    }
}
