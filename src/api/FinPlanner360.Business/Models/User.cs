using System.Text.Json.Serialization;

namespace FinPlanner360.Business.Models;

public class User : Entity
{
    #region Attributes

    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public Guid AuthenticationId { get; set; }

    #endregion Attributes

    #region Helper only for EF Mapping

    [JsonIgnore]
    public ICollection<Transaction> Transactions { get; set; }

    [JsonIgnore]
    public ICollection<Category> Categories { get; set; }

    [JsonIgnore]
    public ICollection<Budget> Budgets { get; set; }

    [JsonIgnore]
    public ICollection<GeneralBudget> GeneralBudgets { get; set; }

    #endregion Helper only for EF Mapping
}