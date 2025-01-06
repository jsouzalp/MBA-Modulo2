using FinPlanner360.Business.Models.Enums;
using System.Text.Json.Serialization;

namespace FinPlanner360.Business.Models;

public class Category : Entity
{
    #region Attributes

    public Guid CategoryId { get; set; }
    public new Guid? UserId { get; set; }
    public string Description { get; set; }
    public CategoryTypeEnum Type { get; set; }

    #endregion Attributes

    #region Helper only for EF Mapping

    [JsonIgnore]
    public User User { get; set; }

    [JsonIgnore]
    public ICollection<Transaction> Transactions { get; set; }

    [JsonIgnore]
    public ICollection<Budget> Budgeties { get; set; }

    #endregion Helper only for EF Mapping
}