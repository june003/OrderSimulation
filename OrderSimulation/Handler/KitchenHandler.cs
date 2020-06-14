//-----------------------------------------------------------------------------
// File Name   : Kitchen
// Author      : junlei
// Date        : 6/13/2020 6:20:32 PM
// Description : 
// Version     : 1.0.0      
// Updated     : 
//
//-----------------------------------------------------------------------------
using OrderSimulation.Model;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderSimulation.Handler
{
    /// <summary>
    /// kitchen subscriber handles the order(cook/place in the shelf)
    /// </summary>
    public class KitchenHandler : ISubscriber
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly Dictionary<ShelfType, Shelf> _shelfDic = new Dictionary<ShelfType, Shelf> {
            {ShelfType.Hot,    new Shelf(ShelfType.Hot,10) },
            {ShelfType.Cold,   new Shelf(ShelfType.Cold, 10) },
            {ShelfType.Frozen, new Shelf(ShelfType.Frozen, 10) },
            {ShelfType.Overflow, new Shelf(ShelfType.Overflow, 15) },
        };

        private readonly object _lockObj = new object();
        public KitchenHandler(OrderHandler orderHandler)
        {
            orderHandler.Subscribe(this);
        }

        public void Process(Order order)
        {
            Task.Run(() => ReceiveOrder(order));  // create thread in the thread pool
        }

        internal void ReceiveOrder(Order order)
        {
            if (!_shelfDic.ContainsKey(order.ShelfType))
            {
                Logger.Warn($"Order not valid: {order.Name}-{order.ShelfType}-{order.ID}");
                return;
            }

            lock (_lockObj)
            {
                // place the original shelf, if full place overflow
                if (_shelfDic[order.ShelfType].Place(order))
                {
                    Logger.Info("Shelves' info: \r\n" + GetShelvesInfo());
                    return;
                }

                //overflow shelf
                _shelfDic[ShelfType.Overflow].Place(order, true);
                Logger.Info("Shelves' info: \r\n" + GetShelvesInfo());
            }
        }

        private string GetShelvesInfo()
        {
            var builder = new StringBuilder();
            foreach (var shf in _shelfDic.Values)
            {
                builder.AppendLine($"  {shf.GetInfo()}");
            }

            return builder.ToString();
        }
    }
}
