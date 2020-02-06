using System;

namespace BudgetMaster {
    public class Cost {
        public string Item{ get; set; }
        public decimal? Value { get; set; }
        public DateTime? CreationDate { get; set; }
        public string User { get; set; }
    }
}
