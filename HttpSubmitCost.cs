using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.Storage;

namespace BudgetMaster {
    public static class HttpSubmitCost {

        [FunctionName("HttpSubmitCost")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] Cost cost,
            [Queue("costqueue"),StorageAccount("AzureWebJobsStorage")] ICollector<string> msg,
            ILogger log) {

            log.LogInformation("HttpSubmitCost triggered");

            //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //dynamic data = JsonConvert.DeserializeObject(requestBody);
            //string item = name ?? data?.name;

            if (String.IsNullOrWhiteSpace(cost.Item)) {
                log.LogInformation("HttpSubmitCost validation failure (Item)");
                return new BadRequestObjectResult("Please pass an Item in the request body.");
            }

            if (cost.Value == null) {
                log.LogInformation("HttpSubmitCost validation failure (Value)");
                return new BadRequestObjectResult("Please pass a Value in the request body.");
            }

            cost.CreationDate = DateTime.UtcNow;
            cost.User = "Bill"; // get authorised user from http request

            msg.Add(JsonConvert.SerializeObject(cost));

            return new ObjectResult("Cost accepted") { StatusCode = StatusCodes.Status202Accepted };
        }
    }
}
