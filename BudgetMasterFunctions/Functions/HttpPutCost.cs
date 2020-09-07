using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using BudgetMaster.Models;

namespace BudgetMaster.Functions {

    public static class HttpPutCost {

        [FunctionName("HttpSubmitCost")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "Costs/{User}")] CostInputModel inputCost,
            [Queue("costqueue"), StorageAccount("AzureWebJobsStorage")] ICollector<CostStorageModel> costStorageQueue,
            ILogger log) {

            log.LogInformation("HttpSubmitCost triggered");

            if (string.IsNullOrWhiteSpace(inputCost.Item)) {
                log.LogInformation("HttpSubmitCost validation failure (Item)");
                return new BadRequestObjectResult("Please pass an Item in the request body.");
            }

            if (inputCost.Value == null) {
                log.LogInformation("HttpSubmitCost validation failure (Value)");
                return new BadRequestObjectResult("Please pass a Value in the request body.");
            }

            if (string.IsNullOrWhiteSpace(inputCost.Date)) {
                log.LogInformation("HttpSubmitCost validation failure (Date)");
                return new BadRequestObjectResult("Please pass a Date in the request body.");
            }

            if (string.IsNullOrWhiteSpace(inputCost.User)) { // in real system user would come from login info
                log.LogInformation("HttpSubmitCost validation failure (User)");
                return new BadRequestObjectResult("Please pass a User in the URL.");
            }

            DateTime costDate;
            
            if(! DateTime.TryParse(inputCost.Date, out costDate)) {
                log.LogInformation("HttpSubmitCost validation failure (Date)");
                return new BadRequestObjectResult("Please supply the Date in the format mm/dd/yyyy");
            }

            var outputCost = new CostStorageModel {
                Item = inputCost.Item,
                Value = inputCost.Value.Value,
                User = inputCost.User,
                Date = new DateTimeOffset(costDate),
                CreationDate = DateTimeOffset.Now
            };

            costStorageQueue.Add(outputCost);

            return new ObjectResult("Cost accepted") { StatusCode = StatusCodes.Status202Accepted };
        }
    }
}
