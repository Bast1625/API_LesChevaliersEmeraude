﻿using System;
using System.Collections.Generic;

namespace API_LesChevaliersEmeraude.Models;

public partial class Personnage
{
    public int IdPersonnage { get; set; }

    public string Nom { get; set; } = null!;

    public char? Sexe { get; set; }

    public int? IdLieuOrigine { get; set; }

    public int? IdLieuResidence { get; set; }

    public int? IdTomeApparition { get; set; }

    public int? IdTomeDeces { get; set; }

    public virtual Chevalier? Chevalier { get; set; }

    public virtual Lieu? IdLieuOrigineNavigation { get; set; }

    public virtual Lieu? IdLieuResidenceNavigation { get; set; }

    public virtual Tome? IdTomeApparitionNavigation { get; set; }

    public virtual Tome? IdTomeDecesNavigation { get; set; }

    public virtual RelationFamiliale? RelationFamilialeIdEnfantNavigation { get; set; }

    public virtual ICollection<RelationFamiliale> RelationFamilialeIdGeniteur1Navigations { get; set; } = new List<RelationFamiliale>();

    public virtual ICollection<RelationFamiliale> RelationFamilialeIdGeniteur2Navigations { get; set; } = new List<RelationFamiliale>();

    public virtual ICollection<RelationFamiliale> RelationFamilialeIdParent1Navigations { get; set; } = new List<RelationFamiliale>();

    public virtual ICollection<RelationFamiliale> RelationFamilialeIdParent2Navigations { get; set; } = new List<RelationFamiliale>();

    public virtual ICollection<Royaute> RoyauteIdRoyaute1Navigations { get; set; } = new List<Royaute>();

    public virtual ICollection<Royaute> RoyauteIdRoyaute2Navigations { get; set; } = new List<Royaute>();
}
