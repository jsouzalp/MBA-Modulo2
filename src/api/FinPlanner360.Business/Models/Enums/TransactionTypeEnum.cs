using System.ComponentModel;

namespace FinPlanner360.Business.Models.Enums;

public enum TransactionTypeEnum
{
    [Description("Receitas")]
    Income = 1,

    [Description("Despesas")]
    Expense = 2
}