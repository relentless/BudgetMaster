using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using BudgetMaster.Models;

namespace BudgetMaster.Functions {

    public static class QueueStoreCost {

        [FunctionName("QueueStoreCost")]
        [return: Table("Costs")]
        public static CostTableEntry Run(
            [QueueTrigger("costqueue")]CostStorageModel queueCost,
            [Queue("userUpdatesQueue"), StorageAccount("AzureWebJobsStorage")] ICollector<string> userUpdatesQueue,
            ILogger log) {

            log.LogInformation($"QueueStoreCost triggered");

            var tableEntry = new CostTableEntry {
                PartitionKey = queueCost.User,
                RowKey = Guid.NewGuid().ToString(),
                Date = queueCost.Date,
                CreationDate = queueCost.CreationDate,
                Item = queueCost.Item,
                ValuePence = (int)(queueCost.Value * 100)
            };

            userUpdatesQueue.Add(queueCost.User);

            return tableEntry;
        }
    }
}
