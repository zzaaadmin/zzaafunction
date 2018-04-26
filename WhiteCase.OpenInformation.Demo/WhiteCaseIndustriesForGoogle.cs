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
    public static class WhiteCaseIndustriesForGoogle
    {
        public const string SKILL_NAME = "White & Case industries skill";
        public const string DEFAULT_RESPONSE = "Welcome to White Case Industry skill. It provides information about the industries where White & case practices.\n Ask list industries for white and case starting with o  \n or does white and case work in aerospace?";

        [FunctionName("WhiteCaseIndustriesForGoogle")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("ZZZ C# HTTP trigger function processed a request. Modified. Modified.  ");

            //Get Request Body
            dynamic data = await req.Content.ReadAsAsync<object>();

            log.Info($"Content = {data}");

            //string intentName = data.result.metadata.intentName;
            string message = "";
            
            HttpResponseMessage response = GenerateResponse(req, DEFAULT_RESPONSE, SKILL_NAME);

            //switch (intentName)
            //{
            //    case "FindIntent":
            //        string industry = (string)data.result.parameters.industry; //data.request.intent.slots["industry"].value;
            //        log.Info($"Industry = {industry}");

            //        message = PrepareFindIntentResponse(log, industry);
            //        response = GenerateResponse(req, message, SKILL_NAME);
            //        break;
            //    case "ListIntent":
            //        string character = (string)data.result.parameters.character; //data.request.intent.slots["character"].value;
            //        log.Info($"Starting Character = {character}");

            //        message = PrepareListIntentResponse(log, character);
            //        response = GenerateResponse(req, message, SKILL_NAME);
            //        break;
            //    default:
            //        break;
            //};

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

        private static string PrepareFindIntentResponse(TraceWriter log, string industry)
        {
            string output = "";

            var data = WhitecaseInformation.FindIndustry(log, industry);

            if (data.Count == 0)
            {
                output = $"White & Case does not work in the {industry} industry.";
            }
            else
            {
                output = $"White & Case works in the {data.First()} industry.";
            }

            return output;
        }

        private static string PrepareListIntentResponse(TraceWriter log, string character)
        {
            string output = "";

            var data = WhitecaseInformation.ListIndustry(log,character);

            if(data.Count == 0)
            {
                output = "White & Case does not work in any industry starting with " + character + ".";
            }
            else
            {
                output = "White & Case works in following industries starting with " + character + ": \n";

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
