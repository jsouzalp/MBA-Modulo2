using System.Text.Json.Serialization;

namespace FinPlanner360.Business.Models;

public class GeneralBudget : Entity
{
    #region Attributes

    public Guid GeneralBudgetId { get; set; }
    public decimal? Amount { get; set; }
    public decimal? Percentage { get; set; }

    #endregion Attributes

    #region Helper only for EF Mapping

    [JsonIgnore]
    public User User { get; set; }

    #endregion Helper only for EF Mapping
}