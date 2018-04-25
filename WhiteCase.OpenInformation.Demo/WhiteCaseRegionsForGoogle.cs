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
    public static class WhiteCaseRegionsForGoogle
    {
        public const string SKILL_NAME = "White & Case regions skill";
        public const string DEFAULT_RESPONSE = "Welcome to White Case Regions skill. It provides information about the regions and countries where White & case practices.\n Ask list regions for white and case \n or does white and case work in Singapore\n or list countries for white and case in the Western Europe region ";

        [FunctionName("WhiteCaseRegionsForGoogle")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("ZZZ C# HTTP trigger function processed a request. Modified. Modified.  ");

            //Get Request Body
            dynamic data = await req.Content.ReadAsAsync<object>();

            log.Info($"Content = {data}");

            string intentName = data.result.metadata.intentName;
            string message = "";


            HttpResponseMessage response = GenerateResponse(req, DEFAULT_RESPONSE, SKILL_NAME);

            switch (intentName)
            {
                case "FindCountryIntent":
                    string country = (string)data.result.parameters.country; //data.request.intent.slots["country"].value;
                    log.Info($"Country = {country}");

                    message = PrepareFindCountryIntentResponse(log, country);
                    response = GenerateResponse(req, message, SKILL_NAME);
                    break;
                case "ListCountryIntent":
                    string region = (string)data.result.parameters.region; //data.request.intent.slots["region"].value;
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

            log.Info($"Response = {message}");

            return response;
        }

        private static HttpResponseMessage GenerateResponse(HttpRequestMessage request, string message, string skillName)
        {
            //return request.CreateResponse(HttpStatusCode.OK, new
            //{
            //    version = "1.0",
            //    sessionAttributes = new { },
            //    response = new
            //    {
            //        outputSpeech = new
            //        {
            //            type = "PlainText",
            //            text = message
            //        },
            //        card = new
            //        {
            //            type = "Simple",
            //            title = skillName,
            //            content = message
            //        },
            //        shouldEndSession = true
            //    }
            //});

            return request.CreateResponse(HttpStatusCode.OK, new
            {
                speech = message,  // ASCII characters only
                displayText = message
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
