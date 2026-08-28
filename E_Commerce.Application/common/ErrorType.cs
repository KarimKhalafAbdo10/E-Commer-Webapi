using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_Commerce.Application.common
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ErrorType
    {   Failure=0,
        validation=1,
        NotFound=2,
        Conflict=3,
        Unauthorized=4,
        Forbidden=5,
        InvalidCredentials=6,

    }
}
