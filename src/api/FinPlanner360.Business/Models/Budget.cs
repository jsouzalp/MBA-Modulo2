using System.Text.Json.Serialization;

namespace FinPlanner360.Business.Models;

public class Budget : Entity
{
    #region Attributes

    public Guid BudgetId { get; set; }
    public Guid CategoryId { get; set; }
    public decimal Amount { get; set; }

    #endregion Attributes

    #region Helper only for EF Mapping

    [JsonIgnore]
    public User User { get; set; }

    #endregion Helper only for EF Mapping
}