using System.Text.Json.Serialization;

namespace FinPlanner360.Busines.Models
{
    public class GeneralBudget : Entity
    {
        #region Attributes
        public Guid GeneralBudgetId { get; set; }
        public Guid UserId { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Percentage { get; set; }
        #endregion

        #region Helper only for EF Mapping
        [JsonIgnore]
        public User User { get; set; }
        #endregion
    }
}
