using System;
using System.Collections.Generic;

namespace Lab_10___Anropa_databasen.Models.DbModels;

public partial class OrderSubtotal
{
    public int OrderId { get; set; }

    public decimal? Subtotal { get; set; }
}
