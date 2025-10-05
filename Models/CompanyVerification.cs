using System;
using System.Collections.Generic;

namespace TimDongDoi.API.Models;

public partial class CompanyVerification
{
    public int Id { get; set; }

    public int CompanyId { get; set; }

    public string? BusinessLicense { get; set; }

    public string? TaxCode { get; set; }

    public string? Status { get; set; }

    public string? RejectReason { get; set; }

    public int? ApprovedBy { get; set; }

    public DateTime? SubmittedAt { get; set; }

    public DateTime? ApprovedAt { get; set; }

    public virtual User? ApprovedByNavigation { get; set; }

    public virtual Company Company { get; set; } = null!;
}
