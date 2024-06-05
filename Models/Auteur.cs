using System;
using System.Collections.Generic;

namespace API_LesChevaliersEmeraude.Models;

public partial class Auteur
{
    public int IdAuteur { get; set; }

    public string? Prenom { get; set; }

    public string? Nom { get; set; }

    public virtual ICollection<Tome> Tomes { get; set; } = new List<Tome>();
}
