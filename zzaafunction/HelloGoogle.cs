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
            log.Info($"intentName : {data.result.metadata.intentName}");

            string intentName = data.result.metadata.intentName;
            string state = data.result.parameters.state;

            string message = "";

            if(intentName == "get_capital")
            {
                message = GetStateCapital(state, log);
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

        private static string GetStateCapital(string state, TraceWriter log)
        {
            log.Info($"GetStateCapital:Entry");
            string output = "";
            string capital = "";


            try
            {
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
                                capital = reader.GetString(1);
                            }
                        }
                    }
                }

                if (String.IsNullOrEmpty(capital))
                {
                    output = $"The capital of {state} is not configured.";
                }
                else
                {
                    output = $"The capital of {state} is {capital}.";
                }
            }
            catch (SqlException ex)
            {
                log.Error("Error Message", ex);
                throw;

            }

            log.Info($"GetStateCapital:Exit");
            return output;
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