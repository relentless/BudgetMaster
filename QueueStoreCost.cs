using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BudgetMaster
{
    public class TableEntry {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Text { get; set; }
    }

    public static class QueueStoreCost
    {
        [FunctionName("QueueStoreCost")]
        [return: Table("Costs")]
        public static TableEntry Run([QueueTrigger("costqueue", Connection = "")]Cost cost, ILogger log)
        {
            log.LogInformation($"QueueStoreCost triggered");

            return new TableEntry {
                PartitionKey = cost.User,
                RowKey = Guid.NewGuid().ToString(),
                Timestamp = new DateTimeOffset(cost.CreationDate.Value),
                Text = JsonConvert.SerializeObject(cost)
            };
        }
    }
}
