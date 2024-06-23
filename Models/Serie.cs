using System;
using System.Collections.Generic;

namespace API_LesChevaliersEmeraude.Models;

public partial class Serie
{
    public int IdSerie { get; set; }

    public string? Titre { get; set; }

    public virtual ICollection<Tome> Tomes { get; set; } = new List<Tome>();
}
