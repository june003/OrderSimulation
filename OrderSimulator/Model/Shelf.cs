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
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace OrderSimulation.Model
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ShelfType
    {
        Invalid = 0,
        [EnumMember(Value = "hot")]
        Hot = 1,
        [EnumMember(Value = "cold")]
        Cold = 2,
        [EnumMember(Value = "frozen")]
        Frozen = 3,
        [EnumMember(Value = "overflow")]
        Overflow = 4,
    };

    /// <summary>
    /// shelf information
    /// </summary>
    internal class Shelf
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly int _capacity;
        private readonly ShelfType _name;

        private readonly Random _rand = new Random();
        private readonly object _lockObj = new object();

        //private readonly HashSet<Order> _orders = new HashSet<Order>();  // not thread safe
        // use the threadsafe concurrent dictioary
        private readonly ConcurrentDictionary<Order, byte> _orders = new ConcurrentDictionary<Order, byte>();

        public Shelf(ShelfType name, int capacity)
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
            lock (_lockObj)
            {
                if (_orders.Count >= _capacity) // full
                {
                    if (!forceTo)
                    {
                        return false;
                    }

                    // for overflow shelf, discard random order first 
                    var randOrder = _orders.ElementAt(_rand.Next(_orders.Count)).Key;
                    _ = _orders.TryRemove(randOrder, out byte val);
                    Logger.Info($"Order discarded randomly: {randOrder.Name}-{randOrder.Value}");
                }

                _ = _orders.TryAdd(order, 0);
                order.OnFinish += Order_OnFinish;
                Logger.Info($"Order received/processed/placed: {order.Name}-{order.Value}-{order.ShelfType} on shelf: {_name}");

                return true;
            }
        }

        internal bool Contains(Order order)
        {
            return _orders.ContainsKey(order);
        }

        internal void Remove(Order order)
        {
            lock (_lockObj)
            {
                if (_orders.ContainsKey(order))
                {
                    order.OnFinish -= Order_OnFinish;
                    _ = _orders.TryRemove(order, out byte val);
                }
            }
        }

        private void Order_OnFinish(Order order, bool isDelivered)
        {
            Remove(order);
            if (isDelivered)
            {
                Logger.Info($"Order delivered: {order.Name}-{order.Value}");
            }
            else
            {
                Logger.Warn($"Order decayed: {order.Name}-{order.Value}");
            }
        }

        internal string GetInfo()
        {
            var builder = new StringBuilder($"Shelf '{_name}' has {_orders.Count} orders: \r\n");
            foreach (var order in _orders.Keys)
            {
                builder.AppendLine($"    {order.Name}-{order.Value}");
            }

            return builder.ToString();
        }
    }
}
