using System;
using System.Collections.Generic;

namespace API_LesChevaliersEmeraude.Models;

public partial class Chevalier
{
    public int IdChevalier { get; set; }

    public int? Generation { get; set; }

    public virtual Personnage IdChevalierNavigation { get; set; } = null!;

    public virtual ICollection<Chevalier> IdEcuyers { get; set; } = new List<Chevalier>();

    public virtual ICollection<Chevalier> IdMaitres { get; set; } = new List<Chevalier>();
}
