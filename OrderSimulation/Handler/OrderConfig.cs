//-----------------------------------------------------------------------------
// File Name   : Config
// Author      : junlei
// Date        : 6/13/2020 3:39:40 PM
// Description : 
// Version     : 1.0.0      
// Updated     : 
//
//-----------------------------------------------------------------------------

using Newtonsoft.Json;
using System;
using System.IO;

namespace OrderSimulation.Config
{
    public class OrderConfig
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public int IngestionRate { get; set; }

        public static OrderConfig LoadConfig(string path)
        {
            try
            {
                Logger.Info("Loading configuration...");
                var config = JsonConvert.DeserializeObject<OrderConfig>(File.ReadAllText(path));

                if (config.IngestionRate <= 0)
                {
                    Logger.Error("Please specify a valid ingestion rate: (0, 1000].");
                    return null;
                }

                if (config.IngestionRate > 1000)
                {
                    Logger.Error("Please specify a valid ingestion rate(cannot process to many orders): (0, 1000].");
                    return null;
                }

                return config;
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to load config file :{ex.Message}");
                return null;
            }
        }
    }
}
