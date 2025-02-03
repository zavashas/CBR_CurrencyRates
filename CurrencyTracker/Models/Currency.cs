using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CurrencyTracker.Models;

public partial class Currency
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int IdCurrency { get; set; }

    public string CharCode { get; set; } = null!;

    public string? NumCode { get; set; }

    public string? NameCurrency { get; set; }

    public virtual ICollection<CurrencyRate> CurrencyRates { get; set; } = new List<CurrencyRate>();
}
