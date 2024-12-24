using FinPlanner360.Entities.Users;
using System.Text.Json.Serialization;

namespace FinPlanner360.Entities.Budgets
{
    public class Budget
    {
        #region Attributes
        public Guid BudgetId { get; set; }
        public Guid UserId { get; set; }
        public Guid CategoryId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? RemovedDate { get; set; }
        #endregion

        #region Helper only for EF Mapping
        [JsonIgnore]
        public User User { get; set; }
        #endregion
    }
}
