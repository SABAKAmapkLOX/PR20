using System;
using System.Collections.Generic;

namespace PR20.Models;

public partial class VolumeWorkObject
{
    public int IdObject { get; set; }

    public int IdWork { get; set; }

    public decimal VolumeWork { get; set; }

    public virtual DirectoryObject IdObject1 { get; set; } = null!;

    public virtual DirectoryCompletionWork IdObjectNavigation { get; set; } = null!;

    public virtual DirectoryPrice IdWorkNavigation { get; set; } = null!;
}
