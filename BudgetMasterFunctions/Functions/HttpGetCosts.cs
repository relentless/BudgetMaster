using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using BudgetMaster.Models;
using System.Linq;
using BudgetMaster.DataAccess;

// useless comment

namespace BudgetMaster.Functions {
    public static class HttpGetCosts {
        [FunctionName("HttpGetCosts")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Costs/{User}")] HttpRequest req,
            [Table("Costs")] CloudTable costsTable,
            string User,
            ILogger log) {

            log.LogInformation($"Get Costs/{User} Called");

            var costs = await CostTable.GetCostsForUser(User, costsTable);

            var sortedCosts = costs.OrderByDescending(cost => cost.Date);

            var outputCosts = sortedCosts.Select( cost => new CostOutputModel {
                    Item = cost.Item,
                    Value = cost.Value,
                    Date = cost.Date.LocalDateTime.ToString("dd-MMM-yyyy")
                });

            return new JsonResult(outputCosts);
        }
    }
}
