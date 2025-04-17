using System;
using System.Collections.Generic;

namespace PR20.Models;

public partial class DirectoryPrice
{
    public int IdWork { get; set; }

    public string NameWork { get; set; } = null!;

    public decimal Price { get; set; }

    public int IdTypeWork { get; set; }

    public virtual DirectoryTypeWork IdTypeWorkNavigation { get; set; } = null!;

    public virtual ICollection<VolumeWorkObject> VolumeWorkObjects { get; set; } = new List<VolumeWorkObject>();
}
