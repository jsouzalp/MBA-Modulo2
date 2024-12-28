using System.ComponentModel;
using System.Text.Json.Serialization;

namespace FinPlanner360.Busines.Models
{
    public class Transaction : Entity
    {
        #region Attributes
        public Guid TransactionId { get; set; }
        public Guid UserId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public enTransactionType Type { get; set; }
        public Guid CategoryId { get; set; }
        public DateTime TransactionDate { get; set; }
        #endregion

        #region Helper only for EF Mapping
        [JsonIgnore]
        public User User { get; set; }

        [JsonIgnore]
        public Category Category { get; set; }
        #endregion
    }

    public enum enTransactionType
    {
        [Description("Receitas")]
        Income = 1,
        [Description("Despesas")]
        Expense = 2
    }
}
