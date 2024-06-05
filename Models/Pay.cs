using System;
using System.Collections.Generic;

namespace API_LesChevaliersEmeraude.Models;

public partial class Pay
{
    public int IdPays { get; set; }

    public string? Nom { get; set; }

    public string? Gentile { get; set; }

    public virtual ICollection<Personnage> PersonnageIdPaysOrigineNavigations { get; set; } = new List<Personnage>();

    public virtual ICollection<Personnage> PersonnageIdPaysResidenceNavigations { get; set; } = new List<Personnage>();

    public virtual ICollection<Royaute> Royautes { get; set; } = new List<Royaute>();
}
