using System;

namespace BudgetMaster.Models {

    public class CostInputModel {
        public string Item { get; set; }
        public decimal? Value { get; set; }
        public string User { get; set; }
        public string Date { get; set; }
    }
}
