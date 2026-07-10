using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_Commerce.Application.common
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ProductStringOptions
    {       None=0,
            NameASC =1,
            NameDESC=2,
            PriceASC=3,
            PriceDESC=4,

    }
}
