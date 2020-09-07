using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace BudgetMaster.Models {
    public class CostTableEntry: TableEntity {
        public string Item { get; set; }
        public int ValuePence { get; set; }
        public DateTimeOffset Date { get; set; }
        public DateTimeOffset CreationDate { get; set; }
    }
}
