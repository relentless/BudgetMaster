using Microsoft.WindowsAzure.Storage.Table;

namespace BudgetMaster.Models {
    public class MonthlySummaryTableEntity: TableEntity {
        public string User { get; set; }
        public string Month { get; set; }
        public int ValuePence { get; set; }
    }
}
