//-----------------------------------------------------------------------------
// File Name   : OrderHandler
// Author      : junlei
// Date        : 6/13/2020 5:50:10 PM
// Description : 
// Version     : 1.0.0      
// Updated     : 
//
//-----------------------------------------------------------------------------
using OrderSimulation.Config;
using OrderSimulation.Model;
using System.Collections.Generic;

namespace OrderSimulation.Handler
{
    /// <summary>
    /// the meditator events between subscribers and publisher
    /// </summary>
    public class OrderHandler
    {
        public OrderConfig Config { get; }

        private readonly ISet<ISubscriber> _subscribers = new HashSet<ISubscriber>();

        public OrderHandler(OrderConfig config)
        {
            Config = config;
        }

        public void Publish(Order order)
        {
            foreach (var sub in _subscribers)
            {
                sub.Process(order);
            }
        }

        public void Subscribe(ISubscriber subscriber)
        {
            _subscribers.Add(subscriber);
        }

        public void UnSbscribe(ISubscriber subscriber)
        {
            if (_subscribers.Contains(subscriber))
            {
                _subscribers.Remove(subscriber);
            }
        }
    }
}
