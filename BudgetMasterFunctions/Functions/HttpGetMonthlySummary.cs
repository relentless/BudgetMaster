using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using BudgetMaster.DataAccess;

namespace BudgetMaster
{
    public static class HttpGetMonthlySummary
    {
        [FunctionName("HttpGetMonthlySummary")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "MonthlyCosts/{User}")] HttpRequest req,
            [Table("CostsByMonth")] CloudTable costsTable,
            string user,
            ILogger log) {
            {
                log.LogInformation($"Get MonthlyCosts/{user} Called");

                var monthlyCosts = await CostTable.GetCostsByMonth(user, costsTable);

                return new JsonResult(monthlyCosts);
            }
        }
    }
}
