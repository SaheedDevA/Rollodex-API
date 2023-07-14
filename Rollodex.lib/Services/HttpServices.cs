using RestSharp;
using Rollodex.lib.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Rollodex.lib.Services
{
    public interface IHttpServices
    {
        ApiHttpResponse Get(string url, Dictionary<string, string> headers = null);
        ApiHttpResponse Post(string url, string payload, Dictionary<string, string> headers = null);
    }

    public class HttpServices : IHttpServices
    {
        public ApiHttpResponse Post(string url, string payload, Dictionary<string, string> headers = null)
        {
            ApiHttpResponse responseM = new ApiHttpResponse { data = "", code = HttpStatusCode.InternalServerError };
            try
            {

                var client = new RestClient(url);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                if (headers != null)
                {
                    request.AddHeaders(headers);
                }
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", payload, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                responseM.data = response.Content;
                responseM.code = response.StatusCode;

                Dictionary<string, string> responseHeaders = new Dictionary<string, string>();
                foreach (var item in response.Headers)
                {
                    responseHeaders.Add(item.Name, item.Value.ToString());
                }
                responseM.responseHeaders = responseHeaders;
            }
            catch (Exception ex)
            {
                responseM.data = ex.Message;
                responseM.code = HttpStatusCode.InternalServerError;
            }
            return responseM;
        }


        public ApiHttpResponse Get(string url, Dictionary<string, string> headers = null)
        {
            ApiHttpResponse responseM = new ApiHttpResponse { data = "", code = HttpStatusCode.InternalServerError };
            try
            {

                var client = new RestClient(url);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);

                if (headers != null)
                {
                    request.AddHeaders(headers);
                }

                request.AddHeader("Content-Type", "application/json");


                client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                IRestResponse response = client.Execute(request);
                responseM.data = response.Content;
                responseM.code = response.StatusCode;


                Dictionary<string, string> responseHeaders = new Dictionary<string, string>();
                foreach (var item in response.Headers)
                {
                    responseHeaders.Add(item.Name, item.Value.ToString());
                }
                responseM.responseHeaders = responseHeaders;
            }
            catch (Exception ex)
            {
                responseM.data = ex.Message;
                responseM.code = HttpStatusCode.InternalServerError;

            }
            return responseM;
        }

    }

}
