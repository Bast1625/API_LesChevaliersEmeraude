using System;
using System.Collections.Generic;

namespace API_LesChevaliersEmeraude.Models;

public partial class Editeur
{
    public int IdEditeur { get; set; }

    public string? Nom { get; set; }

    public DateOnly? DateFondation { get; set; }

    public virtual ICollection<Tome> Tomes { get; set; } = new List<Tome>();
}
