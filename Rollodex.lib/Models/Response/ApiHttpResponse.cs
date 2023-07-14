using System.Net;

namespace Rollodex.lib.Models.Response
{
    public class ApiHttpResponse
    {
        public string data { get; set; }
        public HttpStatusCode code { get; set; }

        public Dictionary<string, string> responseHeaders { get; set; } = new Dictionary<string, string>();
    }
}
