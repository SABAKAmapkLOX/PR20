using System;
using System.Collections.Generic;

namespace PR20.Models;

public partial class DirectoryTypeWork
{
    public int IdTypeWork { get; set; }

    public string NameTypeWork { get; set; } = null!;

    public int UnitWork { get; set; }

    public virtual ICollection<DirectoryPrice> DirectoryPrices { get; set; } = new List<DirectoryPrice>();
}
