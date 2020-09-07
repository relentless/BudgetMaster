using NUnit.Framework;
using Microsoft.Extensions.Logging.Abstractions;
using BudgetMaster.Models;
using BudgetMaster.Functions;
using Microsoft.Azure.WebJobs;

namespace BudgetMaster.Tests {

    class FakeQueueCollector : ICollector<string> {
        public void Add(string item) {
            
        }
    }

    [TestFixture]
    public class QueueStoreCostTests {

        [Test]
        public void Run_HappyPath_TableEntryPartitionedByUser() {

            var inputCost = new CostStorageModel { User = "x" };

            var result = QueueStoreCost.Run(inputCost, new FakeQueueCollector(), new NullLogger<string>());

            Assert.That(result.PartitionKey, Is.EqualTo("x"));
        }
    }
}
