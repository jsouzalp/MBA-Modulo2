using FinPlanner360.Entities.Budgets;
using FinPlanner360.Entities.Categories;
using FinPlanner360.Entities.GeneralBudgets;
using FinPlanner360.Entities.Transactions;
using System.Text.Json.Serialization;

namespace FinPlanner360.Entities.Users
{
    public class User
    {
        #region Attributes
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Guid AuthenticationId { get; set; }
        #endregion

        #region Helper only for EF Mapping
        [JsonIgnore]
        public ICollection<Transaction> Transactions { get; set; }

        [JsonIgnore]
        public ICollection<Category> Categories { get; set; }

        [JsonIgnore]
        public ICollection<Budget> Budgets { get; set; }

        [JsonIgnore]
        public ICollection<GeneralBudget> GeneralBudgets { get; set; }
        #endregion

    }
}
