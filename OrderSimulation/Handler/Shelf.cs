//-----------------------------------------------------------------------------
// File Name   : Shelf
// Author      : junlei
// Date        : 6/11/2020 6:21:06 PM
// Description : 
// Version     : 1.0.0      
// Updated     : 
//
//-----------------------------------------------------------------------------

namespace OrderSimulation.Handler
{
    public class Shelf
    {
        private readonly int _capacity;
        private int _occupied = 0;

        public Shelf(int capacity)
        {
            _capacity = capacity;
        }

        internal bool Place(Order order)
        {
            if (_occupied < _capacity)
            {
                order.Shelf = this;
                ++_occupied;
            }

            return true;
        }

        internal bool Pickup(Order order)
        {
            if (_occupied > 0)
            {
                --_occupied;
            }

            return true;
        }
    }
}
