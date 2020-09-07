using System;

namespace BudgetMaster.Models {
    public class Cost {
        public string Item { get; set; }
        public decimal Value { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}
