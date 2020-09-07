using System;

namespace BudgetMaster.Models {
    public class CostStorageModel: Cost {
        public string User { get; set; }
        public DateTimeOffset CreationDate { get; set; }
    }
}
