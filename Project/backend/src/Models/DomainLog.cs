using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class DomainLog
{
    public int DomainLogId { get; set; }

    public string LogAction { get; set; } = null!;

    public DateTime LogDate { get; set; }

    public int DomainId { get; set; }

    public string Name { get; set; } = null!;

    public byte[]? Logo { get; set; }

    public string? Edition { get; set; }

    public bool IsSsoEnabled { get; set; }

    public string? Comment { get; set; }

    public string? ParentCompany { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? ModifiedBy { get; set; }
}
