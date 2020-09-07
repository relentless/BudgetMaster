using BudgetMaster.Models;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BudgetMaster.DataAccess {
    public static class CostTable {

        public static async Task<List<Cost>> GetCostsForUser(string user, CloudTable costsTable) {
            var selectCosts = new TableQuery<CostTableEntry>().Where(
                                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, user)
                        );

            var costs = new List<Cost>();

            foreach (var cost in await costsTable.ExecuteQuerySegmentedAsync(selectCosts, null)) {
                costs.Add(new Cost {
                    Item = cost.Item,
                    Value = cost.ValuePence / 100m,
                    Date = cost.Date
                });
            }

            return costs;
        }

        public static async Task<List<MonthlySummaryOutputModel>> GetCostsByMonth(string user, CloudTable monthlyCostsTable) {
            var selectCosts = new TableQuery<MonthlySummaryTableEntity>().Where(
                                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, user)
                        );

            var monthlyCosts = new List<MonthlySummaryOutputModel>();

            foreach (var monthlyCost in await monthlyCostsTable.ExecuteQuerySegmentedAsync(selectCosts, null)) {
                monthlyCosts.Add(new MonthlySummaryOutputModel {
                    User = monthlyCost.User,
                    Month = monthlyCost.Month,
                    Value = monthlyCost.ValuePence / 100m
                });
            }

            return monthlyCosts;
        }
    }
}
