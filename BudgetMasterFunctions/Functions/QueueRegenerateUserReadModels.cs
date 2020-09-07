using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;
using BudgetMaster.DataAccess;
using BudgetMaster.Models;
using System.Linq;

namespace BudgetMaster.Functions
{
    public static class QueueRegenerateUserReadModels
    {
        [FunctionName("QueueRegenerateUserReadModels")]
        public static async Task RunAsync(
            [QueueTrigger("userUpdatesQueue")]string user,
            [Table("Costs")] CloudTable costsTable,
            [Table("CostsByMonth")] CloudTable costsByMonthTable,
            ILogger log) {

            log.LogInformation($"Regenerating Read Models for User '{user}'");

            var costs = await CostTable.GetCostsForUser(user, costsTable);

            var monthlyUserCosts =
                costs
                .GroupBy(cost => cost.Date.Year.ToString() + "-" +  cost.Date.Month.ToString("D2"))
                .Select(monthGroup => 
                    new MonthlySummaryTableEntity { 
                        PartitionKey = user,
                        RowKey = monthGroup.Key,
                        Month = monthGroup.Key, 
                        User = user, 
                        ValuePence = (int)(monthGroup.Sum(cost => cost.Value) * 100) });

            foreach (var monthlyUserCost in monthlyUserCosts) {
                await costsByMonthTable.ExecuteAsync(TableOperation.InsertOrReplace(monthlyUserCost));
            }
        }
    }
}
