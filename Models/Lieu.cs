using System;
using System.Collections.Generic;

namespace API_LesChevaliersEmeraude.Models;

public partial class Lieu
{
    public int IdLieu { get; set; }

    public string Nom { get; set; } = null!;

    public string? Gentile { get; set; }

    public virtual ICollection<Personnage> PersonnageIdLieuOrigineNavigations { get; set; } = new List<Personnage>();

    public virtual ICollection<Personnage> PersonnageIdLieuResidenceNavigations { get; set; } = new List<Personnage>();

    public virtual ICollection<Royaute> Royautes { get; set; } = new List<Royaute>();
}
