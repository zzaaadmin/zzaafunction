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
            
            string jsonContent = await req.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject(jsonContent);
            
            log.Info($"Color : {data.result.parameters.color}");
            log.Info($"Number : {data.result.parameters.number}");
            log.Info($"intentName : {data.result.intentName}");

            string intentName = data.result.metadata.intentName;
            string color = data.result.parameters.color;
            string number = data.result.parameters.number;

            string message = "";

            if(intentName == "make_name")
            {
                message = $"Your funny name is {color} {number}.";
            }
            else
            {
                message = $"The intent {intentName} is not configured.";
            }
            
            return req.CreateResponse(HttpStatusCode.OK, new {
                speech = message,  // ASCII characters only
                displayText = message
            });
        }
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