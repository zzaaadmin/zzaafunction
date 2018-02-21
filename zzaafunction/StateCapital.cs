using System;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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
                        string state = (string) data.request.intent.slots["state"].value;
                        log.Info($"state={state}");
                        string capital = GetCapital(state,log);
                        log.Info($"capital={capital}");
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

        private static string GetCapital(string state, TraceWriter log)
        {
            string output = "";

            //TraceWriter log = new 

            try
            {
                log.Info($"GetCapital=GetCapital");
                //SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                //builder.DataSource = "zzaasqlserver.database.windows.net";
                //builder.UserID = "zzaauid";
                //builder.Password = "ZZaapwd12345!";
                //builder.InitialCatalog = "zzaasqldemo";

                //log.Info($"ConnectionString={builder.ConnectionString}");

                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString;
                log.Info($"ConnectionString={connectionString}");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT sc.State, sc.Capital ");
                    sb.Append("FROM [dbo].[StateCapital] sc ");
                    sb.Append("Where sc.State = '");
                    sb.Append(state);
                    sb.Append("';");
                    String sql = sb.ToString();
                    log.Info($"Query={sql}");

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                output = reader.GetString(1);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                log.Error("Error Message", ex);
                throw;

            }

            return output;
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
