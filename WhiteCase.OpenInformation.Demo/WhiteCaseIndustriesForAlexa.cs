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
    public static class WhiteCaseIndustriesForAlexa
    {
        public const string SKILL_NAME = "White & Case industries skill";
        public const string DEFAULT_RESPONSE = "Welcome to White & Case Industries skill. It provides information about the industries where White & case practices.\n Ask list of industries for White & Case or does White & case works in Aviation?";

        [FunctionName("WhiteCaseIndustriesForAlexa")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("ZZZ C# HTTP trigger function processed a request. Modified.");

            //Get Request Body
            dynamic data = await req.Content.ReadAsAsync<object>();

            log.Info($"Content = {data}");
            log.Info($"Request Type = {data.request.type}");

            string message = "";
            HttpResponseMessage response = GenerateResponse(req, DEFAULT_RESPONSE, SKILL_NAME);

            if (data.request.type == "IntentRequest")
            {

                // Set name to query string or body data
                string intentName = data.request.intent.name;
                log.Info($"Intent Name = {data.request.intent.name}");

                switch (intentName)
                {
                    case "FindIntent":
                        string industry = (string)data.request.intent.slots["industry"].value;
                        log.Info($"Industry = {industry}");

                        message = PrepareFindIntentResponse(industry);
                        response = GenerateResponse(req, message, SKILL_NAME);
                        break;
                    case "ListIntent":
                        string startingCharacter = (string)data.request.intent.slots["startingCharacter"].value;
                        log.Info($"Starting Character = {startingCharacter}");

                        message = PrepareListIntentResponse(startingCharacter);
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

        private static string PrepareFindIntentResponse(string industry)
        {
            string output = "";

            bool result = WhitecaseInformation.FindIndustry(industry);

            if (result == true)
            {
                output = $"White & Case works in the {industry} industry.";
            }
            else
            {
                output = $"White & Case does not work in the {industry} industry.";
            }

            return output;
        }

        private static string PrepareListIntentResponse(string startingCharacter)
        {
            string output = "";

            var data = WhitecaseInformation.ListIndustry(startingCharacter);

            foreach(var item in data)
            {
                output = output + item + ", ";
            }

            if(output.EndsWith(", "))
            {
                output = output.Substring(0, output.Length - 2);
            }

            return output;
        }
    }
}
