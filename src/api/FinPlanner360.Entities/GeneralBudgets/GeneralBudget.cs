using FinPlanner360.Entities.Users;
using System.Text.Json.Serialization;

namespace FinPlanner360.Entities.GeneralBudgets
{
    public class GeneralBudget
    {
        #region Attributes
        public Guid GeneralBudgetId { get; set; }
        public Guid UserId { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Percentage { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime? RemovedDate { get; set; }
        #endregion

        #region Helper only for EF Mapping
        [JsonIgnore]
        public User User { get; set; }
        #endregion
    }
}
