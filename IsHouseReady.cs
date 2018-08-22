using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Southworks
{
    public static class IsHouseReady
    {
        [FunctionName("IsHouseReady")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            string houseId = req.Query["houseId"];

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            houseId = houseId ?? data?.houseId;

            IDictionary<string,bool> houses = new Dictionary<string,bool>();

            houses.Add("AXA123", false);
            houses.Add("AXA910", true);
            houses.Add("AXA143", true);
            houses.Add("AXA363", false);
            houses.Add("AXA127", false);

            bool isReady = (houseId != null) ? houses.ContainsKey(houseId) : false;

            string ready = (isReady) ? "READY" : "NOT READY";
            

            return houseId != null
                ? (ActionResult)new OkObjectResult($"The house {houseId} is {ready} to show!")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
