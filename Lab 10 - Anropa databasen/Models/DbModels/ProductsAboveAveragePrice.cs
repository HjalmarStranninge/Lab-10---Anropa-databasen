using System;
using System.Collections.Generic;

namespace Lab_10___Anropa_databasen.Models.DbModels;

public partial class ProductsAboveAveragePrice
{
    public string ProductName { get; set; } = null!;

    public decimal? UnitPrice { get; set; }
}
