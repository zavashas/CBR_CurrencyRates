using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CurrencyTracker.Models;

public partial class CurrencyRate
{

    public int IdCurrencyRate { get; set; }

    public int CurrencyId { get; set; }

    public int? Nominal { get; set; }

    public decimal? VunitRate { get; set; }

    public decimal? RateValue { get; set; }

    public DateTime? DateCurrencyRate { get; set; }

    public virtual Currency Currency { get; set; } = null!;
}
