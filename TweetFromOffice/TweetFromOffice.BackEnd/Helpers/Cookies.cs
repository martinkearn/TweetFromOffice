using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TweetFromOffice.BackEnd.Helpers
{
    public static class Cookies
    {
        public static void Write(HttpResponseBase response, string key, string value)
        {
            response.Cookies[key].Value = value;
            response.Cookies[key].Expires = DateTime.Now.AddYears(1);
        }

        public static string Read(HttpRequestBase request, HttpServerUtilityBase server, string key)
        {
            if (request.Cookies[key] != null)
            {
                return server.HtmlEncode(request.Cookies[key].Value);
            }
            else
            {
                return null;
            }
        }
    }
}
