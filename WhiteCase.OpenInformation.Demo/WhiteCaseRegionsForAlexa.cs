using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace WhiteCase.OpenInformation.Demo
{
    public static class WhiteCaseRegionsForAlexa
    {
        public const string SKILL_NAME = "White & Case regions skill";
        public const string DEFAULT_RESPONSE = "Welcome to White Case Regions skill. It provides information about the regions and countries where White & case practices.\n Ask list regions for white and case \n or does white and case work in Singapore\n or list countries for white and case in the Western Europe region ";

        [FunctionName("WhiteCaseRegionsForAlexa")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("ZZZ C# HTTP trigger function processed a request. Modified. Modified.  ");

            //Get Request Body
            dynamic data = await req.Content.ReadAsAsync<object>();

            log.Info($"Content = {data}");
            log.Info($"Request Type = {data.request.type}");

            string message = "";
            string requestType = data.request.type;

            HttpResponseMessage response = GenerateResponse(req, DEFAULT_RESPONSE, SKILL_NAME);

            if (data.request.type == "IntentRequest")
            {

                // Set name to query string or body data
                string intentName = data.request.intent.name;
                log.Info($"Intent Name = {data.request.intent.name}");

                switch (intentName)
                {
                    case "FindCountryIntent":
                        string country = (string)data.request.intent.slots["country"].value;
                        log.Info($"Country = {country}");

                        message = PrepareFindCountryIntentResponse(log, country);
                        response = GenerateResponse(req, message, SKILL_NAME);
                        break;
                    case "ListCountryIntent":
                        string region = (string)data.request.intent.slots["region"].value;
                        log.Info($"Region = {region}");

                        message = PrepareListCountryIntentResponse(log, region);
                        response = GenerateResponse(req, message, SKILL_NAME);
                        break;
                    case "ListRegionIntent":

                        message = PrepareListRegionIntentResponse(log);
                        response = GenerateResponse(req, message, SKILL_NAME);
                        break;
                    default:
                        break;
                }
            }
            log.Info($"Response = {message}");

            return response;
        }

        private static HttpResponseMessage GenerateResponse(HttpRequestMessage request, string message, string skillName)
        {
            return request.CreateResponse(HttpStatusCode.OK, new
            {
                version = "1.0",
                sessionAttributes = new { },
                response = new
                {
                    outputSpeech = new
                    {
                        type = "PlainText",
                        text = message
                    },
                    card = new
                    {
                        type = "Simple",
                        title = skillName,
                        content = message
                    },
                    shouldEndSession = true
                }
            });
        }

        private static string PrepareFindCountryIntentResponse(TraceWriter log, string country)
        {
            string output = "";

            var data = WhitecaseInformation.FindCountry(log, country);

            if (data.Count == 0)
            {
                output = $"White & Case does not work in the {country}.";
            }
            else
            {
                output = $"White & Case works in the {data.First()}.";
            }

            return output;
        }

        private static string PrepareListCountryIntentResponse(TraceWriter log, string region)
        {
            string output = "";

            var data = WhitecaseInformation.ListCountry(log, region);

            if(data.Count == 0)
            {
                output = "White & Case does not work in any country in the region " + region + ".";
            }
            else
            {
                output = "The list of countries  in the region "  + region + " where White & Case works are as follows: ";

                foreach (var item in data)
                {
                    output = output + item + ", ";
                }

                if (output.EndsWith(", "))
                {
                    output = output.Substring(0, output.Length - 2);
                }
            }



            return output;
        }


        private static string PrepareListRegionIntentResponse(TraceWriter log)
        {
            string output = "";

            var data = WhitecaseInformation.ListRegion(log);

            if (data.Count == 0)
            {
                output = "White & Case does not work in any region.";
            }
            else
            {
                output = "The list of regions where White & Case works are as follows: ";

                foreach (var item in data)
                {
                    output = output + item + ", ";
                }

                if (output.EndsWith(", "))
                {
                    output = output.Substring(0, output.Length - 2);
                }
            }



            return output;
        }

    }
}
