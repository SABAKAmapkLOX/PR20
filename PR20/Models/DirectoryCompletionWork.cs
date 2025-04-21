using System;
using System.Collections.Generic;

namespace PR20.Models;

public partial class DirectoryCompletionWork
{
    public int IdObject { get; set; }

    public DateTime? DateCompletionDate { get; set; }

    public virtual DirectoryObject? DirectoryObject { get; set; }
}
