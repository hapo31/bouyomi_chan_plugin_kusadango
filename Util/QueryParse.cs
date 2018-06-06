using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Hapo31.kusidango.Util
{
    public static class QueryStringExtension
    {
        public static Dictionary<string, string> QueryParse(this HttpListenerRequest req)
        {
            var url = req.RawUrl;
            var queryStr = url.Substring(url.IndexOf('?') + 1);
            var queryStrings = queryStr.Split('&');
            if (queryStrings.Length > 0)
            {
                return queryStrings
                    .Where(v => v.IndexOf('=') >= 0)
                    .Select(v => v.Split('='))
                    .ToDictionary(q => q[0], q => q[1]);
            }
            else
            {
                return new Dictionary<string, string>();
            }
        }
    }

}
