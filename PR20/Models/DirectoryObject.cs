using System;
using System.Collections.Generic;

namespace PR20.Models;

public partial class DirectoryObject
{
    public int IdObject { get; set; }

    public string NameObject { get; set; } = null!;

    public string Town { get; set; } = null!;

    public string AddressObject { get; set; } = null!;

    public DateTime DateBeginingWork { get; set; }

    public virtual VolumeWorkObject? VolumeWorkObject { get; set; }
}
