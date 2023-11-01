using System;
using System.Collections.Generic;

namespace Lab_10___Anropa_databasen.Models.DbModels;

public partial class SalesTotalsByAmount
{
    public decimal? SaleAmount { get; set; }

    public int OrderId { get; set; }

    public string CompanyName { get; set; } = null!;

    public DateTime? ShippedDate { get; set; }
}
