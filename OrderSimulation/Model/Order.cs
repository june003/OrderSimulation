//-----------------------------------------------------------------------------
// File Name   : Order
// Author      : junlei
// Date        : 6/11/2020 6:20:51 PM
// Description : 
// Version     : 1.0.0      
// Updated     : 
//
//-----------------------------------------------------------------------------

namespace OrderSimulation.Model
{
    public class Order
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Temperature { get; set; }
        public int ShelfLife { get; set; }
        public decimal DecayRate { get; set; }
    }
}
