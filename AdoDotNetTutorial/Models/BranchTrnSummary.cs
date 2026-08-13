using System;
using System.Collections.Generic;

namespace VideoGameCharacterApi.Models;

public partial class BranchTrnSummary
{
    public DateTime TrnDate { get; set; }

    public string TrnType { get; set; } = null!;

    public int? Code { get; set; }

    public double Till { get; set; }

    public int? Zed { get; set; }

    public double? InAmt { get; set; }

    public double? OutAmt { get; set; }

    public string? User { get; set; }

    public string TrnTime { get; set; } = null!;

    public string DocumentNumber { get; set; } = null!;

    public string? Comments { get; set; }

    public DateTime? DocDate { get; set; }

    public DateTime? PunchDate { get; set; }

    public double? Profit { get; set; }

    public int? PettyCashNo { get; set; }

    public string? Glcode { get; set; }

    public string? AssAssetGlcode { get; set; }
}
