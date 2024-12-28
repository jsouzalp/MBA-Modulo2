using System.ComponentModel;

namespace FinPlanner360.Busines.Models.Enums;

public enum CategoryTypeEnum
{
    [Description("Receitas")]
    Income = 1,

    [Description("Despesas")]
    Expense = 2
}