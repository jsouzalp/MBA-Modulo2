﻿using System.Text.Json.Serialization;

namespace FinPlanner360.Business.Models;

public class Transaction : Entity
{
    #region Attributes

    public Guid TransactionId { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public Guid CategoryId { get; set; }
    public DateTime TransactionDate { get; set; }

    #endregion Attributes

    #region Helper only for EF Mapping

    [JsonIgnore]
    public User User { get; set; }

    [JsonIgnore]
    public Category Category { get; set; }

    #endregion Helper only for EF Mapping
}