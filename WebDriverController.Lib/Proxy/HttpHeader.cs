using System.Collections.Generic;
using System.Linq;

namespace RafaelEstevam.WebDriverController.Lib.Proxy
{
    public class HttpHeader
    {
        public KeyValuePair<string, string>[] Element { get; set; }

        public static HttpHeader Build(IEnumerable<KeyValuePair<string, string>> items)
        {
            return new HttpHeader()
            {
                Element = items.ToArray()
            };
        }
    }
}
