using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace zzaafunction
{
    public static class GoodMorning
    {
        [FunctionName("GoodMorning")]
        public static async Task<SkillResponse> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] [FromBody] SkillRequest request,  TraceWriter log)
        {
            log.Info("request received...");
            SkillResponse response = null;
            //if (request?.Session?.User?.AccessToken != null)
            //{
            //    ClaimsPrincipal principal = await Security.ValidateTokenAsync(request.Session.User.AccessToken);
            //    if (principal != null)
            //    {
                    PlainTextOutputSpeech outputSpeech = new PlainTextOutputSpeech();
                    string firstName = (request.Request as IntentRequest)?.Intent.Slots.FirstOrDefault(s => s.Key == "FirstName").Value?.Value;
                    outputSpeech.Text = "Hello " + firstName;
                    response = ResponseBuilder.Tell(outputSpeech);
            //    }
            //}

            log.Info("Sending response..");
            return response;
        }
    }
}
