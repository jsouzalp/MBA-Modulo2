using FinPlanner360.Busines.Models;

namespace FinPlanner360.Busines.Extensions;

public static class TransactionExtension
{
    public static Transaction FillAttributes(this Transaction transaction)
    {
        if (transaction.TransactionId == Guid.Empty) { transaction.TransactionId = Guid.NewGuid(); }
        if (transaction.TransactionDate == DateTime.MinValue || transaction.TransactionDate == DateTime.MaxValue) { transaction.TransactionDate = DateTime.Now; }
        if (transaction.CreatedDate == DateTime.MinValue || transaction.CreatedDate == DateTime.MaxValue) { transaction.CreatedDate = DateTime.Now; }

        return transaction;
    }
}