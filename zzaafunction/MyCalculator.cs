using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace zzaafunction
{
    public static class MyCalculator
    {
        [FunctionName("MyCalculator")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            //Simple Function

            //Get Request Body
            dynamic data = await req.Content.ReadAsAsync<object>();

            log.Info($"Content={data}");

            if(data.request.type == "LaunchRequest")
            {
                log.Info($"Default LaunchRequest made");

                return DefaultRequest(req);
            }
            else
            {
                return DefaultRequest(req);
            }
            //log.Info("C# HTTP trigger function processed a request.");

            //// parse query parameter
            //string name = req.GetQueryNameValuePairs()
            //    .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
            //    .Value;

            //// Get request body
            //dynamic data = await req.Content.ReadAsAsync<object>();

            //// Set name to query string or body data
            //name = name ?? data?.name;

            //return name == null
            //    ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
            //    : req.CreateResponse(HttpStatusCode.OK, "Hello " + name);
        }

        private static HttpResponseMessage DefaultRequest(HttpRequestMessage req)
        {
            return req.CreateResponse(HttpStatusCode.OK, new
            {
                version = "1.0",
                sessionAttributes = new { },
                response = new
                {
                    outputSpeech = new
                    {
                        type = "PlainText",
                        text = "Welcome to this calculator that only knows how to add two numbers.\n Ask add two plus three"
                    },
                    card = new
                    {
                        type = "Simple",
                        title = "Alexa-Azure Simple Calculator",
                        content = "Welcome to this calculator that only adds two number.\n Ask 2 + 3"
                    },
                    shouldEndSession = true
                }
            });
        }

    }
}
