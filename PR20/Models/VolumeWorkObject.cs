using System;
using System.Collections.Generic;

namespace PR20.Models;

public partial class VolumeWorkObject
{
    public int Id { get; set; }

    public int IdWork { get; set; }

    public decimal VolumeWork { get; set; }

    public int IdObject { get; set; }

    public virtual DirectoryObject IdObjectNavigation { get; set; } = null!;

    public virtual DirectoryPrice IdWorkNavigation { get; set; } = null!;
}
