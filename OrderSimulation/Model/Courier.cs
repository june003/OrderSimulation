//-----------------------------------------------------------------------------
// File Name   : Courier
// Author      : junlei
// Date        : 6/11/2020 6:21:35 PM
// Description : 
// Version     : 1.0.0      
// Updated     : 
//
//-----------------------------------------------------------------------------
using System.Threading.Tasks;

namespace OrderSimulation.Model
{
    class Courier
    {
        public static bool Pickup(Order order, int delay)
        {
            Task.Delay(delay).Wait();

            return Deliver(order);
        }

        private static bool Deliver(Order order)
        {
            return true;
        }
    }
}
