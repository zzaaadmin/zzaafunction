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
    public static class WhiteCasePracticesForAlexa
    {
        public const string SKILL_NAME = "White & Case practices skill";
        public const string DEFAULT_RESPONSE = "Welcome to White Case Practice skill. It provides information about the practices for White & case.\n Ask list practices for white and case starting with o  \n or does white and case have aerospace practice?";

        [FunctionName("WhiteCasePracticesForAlexa")]
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
                    case "FindIntent":
                        string practice = (string)data.request.intent.slots["practice"].value;
                        log.Info($"Practice = {practice}");

                        message = PrepareFindIntentResponse(log, practice);
                        response = GenerateResponse(req, message, SKILL_NAME);
                        break;
                    case "ListIntent":
                        string character = (string)data.request.intent.slots["character"].value;
                        log.Info($"Starting Character = {character}");

                        message = PrepareListIntentResponse(log, character);
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

        private static string PrepareFindIntentResponse(TraceWriter log, string practice)
        {
            string output = "";

            var data = WhitecaseInformation.FindPractice(log, practice);

            if (data.Count == 0)
            {
                output = $"White & Case does not work in the {practice} practice.";
            }
            else
            {
                output = $"White & Case works in the {data.First()} practice.";
            }

            return output;
        }

        private static string PrepareListIntentResponse(TraceWriter log, string character)
        {
            string output = "";

            var data = WhitecaseInformation.ListPractice(log,character);

            if(data.Count == 0)
            {
                output = "White & Case does not work in any practice starting with " + character + ".";
            }
            else
            {
                output = "White & Case works in following practices starting with " + character + ": \n";

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
