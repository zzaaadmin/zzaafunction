using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace zzaafunction
{
    public static class HelloGoogle
    {
        [FunctionName("HelloGoogle")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");


            // parse query parameter
            //string name = req.GetQueryNameValuePairs()
            //    .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
            //    .Value;

            // Get request body
            //var googleHomeRequest = await req.Content.ReadAsAsync<GoogleHomeRequest>();

            string jsonContent = await req.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject(jsonContent);

            //log.Info($"WebHook was triggered! Comment: {data.ToString()}");

            //log.Info($"source : {data.source.ToString()}");
            //log.Info($"Number : {data.originalRequest.source.ToString()}");

            //var googleHomeParameters = googleHomeRequest.Result.Parameters;

            log.Info($"Color : {data.result.parameters.color}");
            log.Info($"Number : {data.result.parameters.number}");

            string message = $"Your funny name is {data.result.parameters.color} {data.result.parameters.number}";

            //var response = new Response();
            //if (!string.IsNullOrEmpty(googleHomeParameters.number) && !string.IsNullOrEmpty(googleHomeParameters.color))
            //{
            //    var now = System.DateTime.Now.ToLocalTime();

            //    response.DisplayText = $"{googleHomeParameters.color} won a game of {googleHomeParameters.number}";
            //    response.Source = "webhook";
            //    response.Speech = $"{googleHomeParameters.color} won a game of {googleHomeParameters.number}";
            //}

                //response.DisplayText = $"Hello world!!!!";
                //response.Source = "webhook";
                //response.Speech = $"Hello world!!!!";


            return req.CreateResponse(HttpStatusCode.OK, new {
                speech = message,  // ASCII characters only
                displayText = message
            });
        }
    }

    public class GoogleHomeRequest
    {
        public GoogleHomeResult Result { get; set; }
    }

    public class GoogleHomeResult
    {
        public GoogleHomeParameters Parameters { get; set; }
    }

    public class GoogleHomeParameters
    {
        public string number { get; set; }
        public string color { get; set; }
    }

    public class Response
    {
        public string Speech { get; set; }
        public string DisplayText { get; set; }
        public string Source { get; set; }
    }
}


//{
//  "speech": "...",  // ASCII characters only
//  "displayText": "...",
//  "data": {
//    "google": {
//      "expect_user_response": true,
//      "is_ssml": true,
//      "permissions_request": {
//        "opt_context": "...",
//        "permissions": [
//          "NAME",
//          "DEVICE_COARSE_LOCATION",
//          "DEVICE_PRECISE_LOCATION"
//        ]
//      }
//    }
//  },
//  "contextOut": [...],
//}