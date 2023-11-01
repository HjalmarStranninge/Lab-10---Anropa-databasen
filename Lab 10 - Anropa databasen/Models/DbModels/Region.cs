using System;
using System.Collections.Generic;

namespace Lab_10___Anropa_databasen.Models.DbModels;

public partial class Region
{
    public int RegionId { get; set; }

    public string RegionDescription { get; set; } = null!;

    public virtual ICollection<Territory> Territories { get; set; } = new List<Territory>();
}
