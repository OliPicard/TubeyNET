using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TubeyForWin3
{
    [JsonConverter(typeof(LineDisplayConverter))] //setting up the correct attribute for JSONConverter Objects.
    public class LineDisplay
    {
        public string Name { get; set; }
        public string SeverityDescription { get; set; }
        public string Reason { get; set; }
    }
    public class LineDisplayConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(LineDisplay)); //new boolean overriding objectType with LineDiplay
        }

        public override bool CanWrite { get { return false; } }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader); //new JObject reader (custom ReadJson Deseralizser)


            LineDisplay newItem = new LineDisplay();
            newItem.Name = (string)jo.SelectToken("name"); //Get the name attribute


            newItem.SeverityDescription = (string)jo.SelectToken("lineStatuses[0].statusSeverityDescription");

            newItem.Reason = (string)jo.SelectToken("lineStatuses[0].reason");

            return newItem; //Return all items as newItem objects!
        }


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
   
}