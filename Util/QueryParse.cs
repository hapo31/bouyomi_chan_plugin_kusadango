using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hapo31.kusidango.Util
{
    public static class QueryStringExtension
    {
        public static Dictionary<string, string> QueryParse(this string url)
        {
            var queryStr = url.Substring(url.IndexOf('?') + 1);
            var queryStrings = queryStr.Split('&');
            if (queryStrings.Length > 0)
            {
                return queryStrings.Select(v => v.Split('=')).ToDictionary(q => q[0], q => q[1]);
            }
            else
            {
                return new Dictionary<string, string>();
            }
        }
    }

}
