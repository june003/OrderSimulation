//-----------------------------------------------------------------------------
// File Name   : Courier
// Author      : junlei
// Date        : 6/11/2020 6:21:35 PM
// Description : 
// Version     : 1.0.0      
// Updated     : 
//
//-----------------------------------------------------------------------------
using System;
using System.Threading.Tasks;

namespace OrderSimulation.Handler
{
    class Courier
    {
        private static readonly Random Rand = new Random();
        public static int DelaySecond => Rand.Next(2, 6);

        public bool Pickup(Order order)
        {
            Task.Delay(DelaySecond).Wait();  // 2-6 seconds later, deliver

            return Deliver(order);
        }

        private bool Deliver(Order order)
        {
            if (order.Value <= 0)
            {
                return false;
            }

            order.Courier = this;
            return true;
        }
    }
}
