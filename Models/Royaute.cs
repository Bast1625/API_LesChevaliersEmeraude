using System;
using System.Collections.Generic;

namespace API_LesChevaliersEmeraude.Models;

public partial class Royaute
{
    public int IdPays { get; set; }

    public int IdRoyaute1 { get; set; }

    public int IdRoyaute2 { get; set; }

    public virtual Pay IdPaysNavigation { get; set; } = null!;

    public virtual Personnage IdRoyaute1Navigation { get; set; } = null!;

    public virtual Personnage IdRoyaute2Navigation { get; set; } = null!;
}
