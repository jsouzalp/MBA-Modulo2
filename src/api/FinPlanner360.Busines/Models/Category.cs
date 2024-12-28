using System.ComponentModel;
using System.Text.Json.Serialization;

namespace FinPlanner360.Busines.Models
{
    public class Category : Entity
    {
        #region Attributes
        public Guid CategoryId { get; set; }
        public Guid? UserId { get; set; }
        public string Description { get; set; }
        public enCategoryType Type { get; set; }
        #endregion

        #region Helper only for EF Mapping
        [JsonIgnore]
        public User User { get; set; }

        [JsonIgnore]
        public ICollection<Transaction> Transactions { get; set; }
        #endregion
    }

    public enum enCategoryType
    {
        [Description("Receitas")]
        Income = 1,
        [Description("Despesas")]
        Expense = 2
    }
}
