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

namespace OrderSimulation.Model
{
    class Courier
    {
        private static readonly Random Rand = new Random();
        public static int DelaySecond => Rand.Next(2, 6);

        public static bool Pickup(Order order)
        {
            Task.Delay(DelaySecond).Wait();  // 2-6 seconds later, deliver

            return Deliver(order);
        }

        private static bool Deliver(Order order)
        {
            return true;
        }

    }
}
