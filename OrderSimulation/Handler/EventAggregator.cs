//-----------------------------------------------------------------------------
// File Name   : EventAggregator
// Author      : junlei
// Date        : 6/13/2020 5:50:10 PM
// Description : 
// Version     : 1.0.0      
// Updated     : 
//
//-----------------------------------------------------------------------------
using OrderSimulation.Config;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OrderSimulation.Handler
{
    public class EventAggregator
    {
        private readonly object _lockObj = new object();
        private readonly Dictionary<Type, IList> _subscriberDic = new Dictionary<Type, IList>();
        public OrderConfig Config { get; private set; }

        public EventAggregator(OrderConfig config)
        {
            Config = config;
        }

        public void Publish<MessageType>(MessageType message)
        {
            Type t = typeof(MessageType);

            if (_subscriberDic.ContainsKey(t))
            {
                IList subscribers;
                lock (_lockObj)
                {
                    subscribers = new List<Subscription<MessageType>>(_subscriberDic[t].Cast<Subscription<MessageType>>());
                }

                foreach (Subscription<MessageType> sub in subscribers)
                {
                    sub.CreatAction()?.Invoke(message);
                }
            }
        }

        public Subscription<MessageType> Subscribe<MessageType>(Action<MessageType> action)
        {
            Type type = typeof(MessageType);
            var actionDetails = new Subscription<MessageType>(action, this);

            lock (_lockObj)
            {
                if (!_subscriberDic.TryGetValue(type, out IList actions))
                {
                    actions = new List<Subscription<MessageType>> { actionDetails };
                    _subscriberDic.Add(type, actions);
                }
                else
                {
                    actions.Add(actionDetails);
                }
            }

            return actionDetails;
        }

        public void UnSbscribe<MessageType>(Subscription<MessageType> subscription)
        {
            Type type = typeof(MessageType);
            if (_subscriberDic.ContainsKey(type))
            {
                lock (_lockObj)
                {
                    _subscriberDic[type].Remove(subscription);
                }

                subscription = null;
            }
        }
    }
}
