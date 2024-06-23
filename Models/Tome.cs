using System;
using System.Collections.Generic;

namespace API_LesChevaliersEmeraude.Models;

public partial class Tome
{
    public int IdTome { get; set; }

    public int IdSerie { get; set; }

    public int? Numero { get; set; }

    public string? Titre { get; set; }

    public string? Resume { get; set; }

    public int? Page { get; set; }

    public DateOnly? DateParution { get; set; }

    public string? LieuParution { get; set; }

    public string? Isbn { get; set; }

    public int? IdAuteur { get; set; }

    public int? IdEditeur { get; set; }

    public virtual Auteur? IdAuteurNavigation { get; set; }

    public virtual Editeur? IdEditeurNavigation { get; set; }

    public virtual Serie IdSerieNavigation { get; set; } = null!;

    public virtual ICollection<Personnage> PersonnageIdTomeApparitionNavigations { get; set; } = new List<Personnage>();

    public virtual ICollection<Personnage> PersonnageIdTomeDecesNavigations { get; set; } = new List<Personnage>();
}
