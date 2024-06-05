using System;
using System.Collections.Generic;

namespace API_LesChevaliersEmeraude.Models;

public partial class RelationFamiliale
{
    public int IdEnfant { get; set; }

    public int? IdParent1 { get; set; }

    public int? IdParent2 { get; set; }

    public int? IdGeniteur1 { get; set; }

    public int? IdGeniteur2 { get; set; }

    public virtual Personnage IdEnfantNavigation { get; set; } = null!;

    public virtual Personnage? IdGeniteur1Navigation { get; set; }

    public virtual Personnage? IdGeniteur2Navigation { get; set; }

    public virtual Personnage? IdParent1Navigation { get; set; }

    public virtual Personnage? IdParent2Navigation { get; set; }
}
