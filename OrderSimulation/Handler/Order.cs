//-----------------------------------------------------------------------------
// File Name   : Order
// Author      : junlei
// Date        : 6/11/2020 6:20:51 PM
// Description : 
// Version     : 1.0.0      
// Updated     : 
//
//-----------------------------------------------------------------------------
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace OrderSimulation.Handler
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Temperature
    {
        [EnumMember(Value = "others")]
        Otehrs = 0,
        [EnumMember(Value = "hot")]
        Hot = 1,
        [EnumMember(Value = "cold")]
        Cold = 2,
        [EnumMember(Value = "frozen")]
        Frozen = 3,
    };

    public class Order
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("temp")]
        public Temperature Temperature { get; set; }
        [JsonProperty("shelfLife")]
        public int ShelfLife { get; set; }
        [JsonProperty("decayRate")]
        public decimal DecayRate { get; set; }

        public decimal Value
        {
            get
            {
                if (ShelfLife <= 0)
                {
                    return 0;
                }

                return (ShelfLife - DecayRate * _age * ShelfDecayModifier) / ShelfLife;
            }
        }

        private int ShelfDecayModifier => (Temperature == Temperature.Cold || Temperature == Temperature.Hot
                                        || Temperature == Temperature.Frozen) ? 1 : 2;

        public Shelf Shelf { get; internal set; }
        internal Courier Courier { get; set; }

        private int _age = 0; // by seconds
        internal void Start()
        {
            Task.Run(() => Grow()).ConfigureAwait(false);
        }

        private void Grow()
        {
            while (true)
            {
                Task.Delay(1000).Wait();
                ++_age;
            }
        }
    }
}
