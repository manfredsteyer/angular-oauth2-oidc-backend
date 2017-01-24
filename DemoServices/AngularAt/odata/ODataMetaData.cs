using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.OData.Query;
using System.Collections;

namespace Demo.Models
{
    public class ODataMetadata
    {
        [JsonProperty(PropertyName = "value")]
        public IEnumerable<object> Value { get; set; }

        [JsonProperty(
            PropertyName = "@odata.count", 
            NullValueHandling = NullValueHandling.Ignore)]
        public int? Count { get; set; }
    }
}