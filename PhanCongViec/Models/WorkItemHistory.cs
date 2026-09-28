using System;
using System.Collections.Generic;

namespace PhanCongViec.Models;

public partial class WorkItemHistory
{
    public long Id { get; set; }

    public long WorkItemId { get; set; }

    public string? FromStatus { get; set; }

    public string? ToStatus { get; set; }

    public string? Note { get; set; }

    public string ChangedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual WorkItem WorkItem { get; set; } = null!;
}
