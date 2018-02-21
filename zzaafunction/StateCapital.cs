using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace zzaafunction
{
    public static class StateCapital
    {
        [FunctionName("StateCapital")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            //Simple Function

            //Get Request Body
            dynamic data = await req.Content.ReadAsAsync<object>();

            log.Info($"Content={data}");

            if (data.request.type == "LaunchRequest")
            {
                log.Info($"Default LaunchRequest made");

                return DefaultRequest(req);
            }
            else if (data.request.type == "IntentRequest")
            {
                // Set name to query string or body data
                string intentName = data.request.intent.name;
                log.Info($"intentName={intentName}");
                switch (intentName)
                {
                    case "FindIntent":
                        var state = data.request.intent.slots["state"].value;



                        string capital = GetCapital(state);

                        string message = "";

                        if (String.IsNullOrEmpty(capital))
                        {
                            message = $"The capital of {state} is not configured.";
                        }
                        else
                        {
                            message = $"The capital of {state} is {capital}.";
                        }


                        return req.CreateResponse(HttpStatusCode.OK, new
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
                                    title = "Alexa-State capital skill",
                                    content = message
                                },
                                shouldEndSession = true
                            }
                        });
                    // Add more intents and default responses
                    default:
                        return DefaultRequest(req);
                }
            }

            else
            {
                return DefaultRequest(req);
            }

        }

        private static string GetCapital(string state)
        {
            return "Phoenix";
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
                        text = "Welcome to this State Capital skill. It queries database for capital of US states.\n Ask what is the capital of Arizona?"
                    },
                    card = new
                    {
                        type = "Simple",
                        title = "Alexa-State capital skill",
                        content = "Welcome to this State Capital skill. It queries database for capital of US states.\n Ask what is the capital of Arizona?"
                    },
                    shouldEndSession = true
                }
            });
        }
    }
}
