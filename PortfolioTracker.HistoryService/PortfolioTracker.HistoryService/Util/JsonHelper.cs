using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioTracker.HistoryService.Util
{
    public class JsonHelper
    {
        public static List<string> InvalidJsonElements;
        
        public static IList<T> DeserializeToList<T>(string json)
        {
            InvalidJsonElements = null;
            var array = JArray.Parse(json);
            var objects = new List<T>();

            foreach (var item in array)
            {
                try
                {
                    objects.Add(item.ToObject<T>());
                } catch(Exception e)
                {
                    InvalidJsonElements = InvalidJsonElements?? new List<string>();
                    InvalidJsonElements.Add(item.ToString());
                }
            }

            return objects;
        }
    }
}
