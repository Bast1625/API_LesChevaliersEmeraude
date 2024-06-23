using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace API_LesChevaliersEmeraude.Models;

public partial class LesChevaliersEmeraudeContext : DbContext
{
    public LesChevaliersEmeraudeContext()
    {
    }

    public LesChevaliersEmeraudeContext(DbContextOptions<LesChevaliersEmeraudeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Auteur> Auteurs { get; set; }

    public virtual DbSet<Chevalier> Chevaliers { get; set; }

    public virtual DbSet<Editeur> Editeurs { get; set; }

    public virtual DbSet<Lieu> Lieus { get; set; }

    public virtual DbSet<Personnage> Personnages { get; set; }

    public virtual DbSet<RelationFamiliale> RelationFamiliales { get; set; }

    public virtual DbSet<Royaute> Royautes { get; set; }

    public virtual DbSet<Serie> Series { get; set; }

    public virtual DbSet<Tome> Tomes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=les_chevaliers_emeraude;Username=api_chevaliers_emeraude;Password=Pr*g23BDPGAP!Cheval!ers");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auteur>(entity =>
        {
            entity.HasKey(e => e.IdAuteur).HasName("auteur_pkey");

            entity.ToTable("auteur", "data");

            entity.Property(e => e.IdAuteur)
                .HasDefaultValueSql("nextval('data.\"auteur_auteurID_seq\"'::regclass)")
                .HasColumnName("id_auteur");
            entity.Property(e => e.Nom)
                .HasMaxLength(500)
                .HasColumnName("nom");
            entity.Property(e => e.Prenom)
                .HasMaxLength(500)
                .HasColumnName("prenom");
        });

        modelBuilder.Entity<Chevalier>(entity =>
        {
            entity.HasKey(e => e.IdChevalier).HasName("chevalier_pkey");

            entity.ToTable("chevalier", "data");

            entity.Property(e => e.IdChevalier)
                .ValueGeneratedNever()
                .HasColumnName("id_chevalier");
            entity.Property(e => e.Generation).HasColumnName("generation");

            entity.HasOne(d => d.IdChevalierNavigation).WithOne(p => p.Chevalier)
                .HasForeignKey<Chevalier>(d => d.IdChevalier)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fK_id_chevalier");

            entity.HasMany(d => d.IdEcuyers).WithMany(p => p.IdMaitres)
                .UsingEntity<Dictionary<string, object>>(
                    "MaitreEcuyer",
                    r => r.HasOne<Chevalier>().WithMany()
                        .HasForeignKey("IdEcuyer")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_id_ecuyer"),
                    l => l.HasOne<Chevalier>().WithMany()
                        .HasForeignKey("IdMaitre")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_id_maitre"),
                    j =>
                    {
                        j.HasKey("IdMaitre", "IdEcuyer").HasName("maitre_ecuyer_pkey");
                        j.ToTable("maitre_ecuyer", "data");
                        j.IndexerProperty<int>("IdMaitre").HasColumnName("id_maitre");
                        j.IndexerProperty<int>("IdEcuyer").HasColumnName("id_ecuyer");
                    });

            entity.HasMany(d => d.IdMaitres).WithMany(p => p.IdEcuyers)
                .UsingEntity<Dictionary<string, object>>(
                    "MaitreEcuyer",
                    r => r.HasOne<Chevalier>().WithMany()
                        .HasForeignKey("IdMaitre")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_id_maitre"),
                    l => l.HasOne<Chevalier>().WithMany()
                        .HasForeignKey("IdEcuyer")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_id_ecuyer"),
                    j =>
                    {
                        j.HasKey("IdMaitre", "IdEcuyer").HasName("maitre_ecuyer_pkey");
                        j.ToTable("maitre_ecuyer", "data");
                        j.IndexerProperty<int>("IdMaitre").HasColumnName("id_maitre");
                        j.IndexerProperty<int>("IdEcuyer").HasColumnName("id_ecuyer");
                    });
        });

        modelBuilder.Entity<Editeur>(entity =>
        {
            entity.HasKey(e => e.IdEditeur).HasName("editeur_pkey");

            entity.ToTable("editeur", "data");

            entity.Property(e => e.IdEditeur)
                .HasDefaultValueSql("nextval('data.\"editeur_editeurID_seq\"'::regclass)")
                .HasColumnName("id_editeur");
            entity.Property(e => e.DateFondation).HasColumnName("dateFondation");
            entity.Property(e => e.Nom)
                .HasMaxLength(1000)
                .HasColumnName("nom");
        });

        modelBuilder.Entity<Lieu>(entity =>
        {
            entity.HasKey(e => e.IdLieu).HasName("lieu_pkey");

            entity.ToTable("lieu", "data");

            entity.Property(e => e.IdLieu)
                .HasDefaultValueSql("nextval('data.\"royaume_royaumeID_seq\"'::regclass)")
                .HasColumnName("id_lieu");
            entity.Property(e => e.Gentile)
                .HasMaxLength(500)
                .HasColumnName("gentile");
            entity.Property(e => e.Nom)
                .HasMaxLength(500)
                .HasColumnName("nom");
        });

        modelBuilder.Entity<Personnage>(entity =>
        {
            entity.HasKey(e => e.IdPersonnage).HasName("personnage_pkey");

            entity.ToTable("personnage", "data");

            entity.Property(e => e.IdPersonnage)
                .HasDefaultValueSql("nextval('data.\"personnage_personnageID_seq\"'::regclass)")
                .HasColumnName("id_personnage");
            entity.Property(e => e.IdLieuOrigine).HasColumnName("id_lieu_origine");
            entity.Property(e => e.IdLieuResidence).HasColumnName("id_lieu_residence");
            entity.Property(e => e.IdTomeApparition).HasColumnName("id_tome_apparition");
            entity.Property(e => e.IdTomeDeces).HasColumnName("id_tome_deces");
            entity.Property(e => e.Nom)
                .HasMaxLength(500)
                .HasColumnName("nom");
            entity.Property(e => e.Sexe)
                .HasMaxLength(1)
                .HasColumnName("sexe");

            entity.HasOne(d => d.IdLieuOrigineNavigation).WithMany(p => p.PersonnageIdLieuOrigineNavigations)
                .HasForeignKey(d => d.IdLieuOrigine)
                .HasConstraintName("FK_id_lieu_origine");

            entity.HasOne(d => d.IdLieuResidenceNavigation).WithMany(p => p.PersonnageIdLieuResidenceNavigations)
                .HasForeignKey(d => d.IdLieuResidence)
                .HasConstraintName("FK_id_lieu_residence");

            entity.HasOne(d => d.IdTomeApparitionNavigation).WithMany(p => p.PersonnageIdTomeApparitionNavigations)
                .HasForeignKey(d => d.IdTomeApparition)
                .HasConstraintName("FK_tome_apparition");

            entity.HasOne(d => d.IdTomeDecesNavigation).WithMany(p => p.PersonnageIdTomeDecesNavigations)
                .HasForeignKey(d => d.IdTomeDeces)
                .HasConstraintName("FK_tome_deces");
        });

        modelBuilder.Entity<RelationFamiliale>(entity =>
        {
            entity.HasKey(e => e.IdEnfant).HasName("relation_familiale_pkey");

            entity.ToTable("relation_familiale", "data");

            entity.Property(e => e.IdEnfant)
                .ValueGeneratedNever()
                .HasColumnName("id_enfant");
            entity.Property(e => e.IdGeniteur1).HasColumnName("id_geniteur1");
            entity.Property(e => e.IdGeniteur2).HasColumnName("id_geniteur2");
            entity.Property(e => e.IdParent1).HasColumnName("id_parent1");
            entity.Property(e => e.IdParent2).HasColumnName("id_parent2");

            entity.HasOne(d => d.IdEnfantNavigation).WithOne(p => p.RelationFamilialeIdEnfantNavigation)
                .HasForeignKey<RelationFamiliale>(d => d.IdEnfant)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_enfantID");

            entity.HasOne(d => d.IdGeniteur1Navigation).WithMany(p => p.RelationFamilialeIdGeniteur1Navigations)
                .HasForeignKey(d => d.IdGeniteur1)
                .HasConstraintName("FK_adoptant1ID");

            entity.HasOne(d => d.IdGeniteur2Navigation).WithMany(p => p.RelationFamilialeIdGeniteur2Navigations)
                .HasForeignKey(d => d.IdGeniteur2)
                .HasConstraintName("FK_adoptant2ID");

            entity.HasOne(d => d.IdParent1Navigation).WithMany(p => p.RelationFamilialeIdParent1Navigations)
                .HasForeignKey(d => d.IdParent1)
                .HasConstraintName("FK_geniteur1ID");

            entity.HasOne(d => d.IdParent2Navigation).WithMany(p => p.RelationFamilialeIdParent2Navigations)
                .HasForeignKey(d => d.IdParent2)
                .HasConstraintName("FK_geniteur2ID");
        });

        modelBuilder.Entity<Royaute>(entity =>
        {
            entity.HasKey(e => new { e.IdLieu, e.IdRoyaute1, e.IdRoyaute2 }).HasName("royaute_pkey");

            entity.ToTable("royaute", "data");

            entity.Property(e => e.IdLieu).HasColumnName("id_lieu");
            entity.Property(e => e.IdRoyaute1).HasColumnName("id_royaute1");
            entity.Property(e => e.IdRoyaute2).HasColumnName("id_royaute2");
            entity.Property(e => e.SuccessionOrder).HasColumnName("succession_order");

            entity.HasOne(d => d.IdLieuNavigation).WithMany(p => p.Royautes)
                .HasForeignKey(d => d.IdLieu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_id_lieu");

            entity.HasOne(d => d.IdRoyaute1Navigation).WithMany(p => p.RoyauteIdRoyaute1Navigations)
                .HasForeignKey(d => d.IdRoyaute1)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_id_royaute1");

            entity.HasOne(d => d.IdRoyaute2Navigation).WithMany(p => p.RoyauteIdRoyaute2Navigations)
                .HasForeignKey(d => d.IdRoyaute2)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_id_royaute2");
        });

        modelBuilder.Entity<Serie>(entity =>
        {
            entity.HasKey(e => e.IdSerie).HasName("serie_pkey");

            entity.ToTable("serie", "data");

            entity.Property(e => e.IdSerie).HasColumnName("id_serie");
            entity.Property(e => e.Titre)
                .HasMaxLength(500)
                .HasColumnName("titre");
        });

        modelBuilder.Entity<Tome>(entity =>
        {
            entity.HasKey(e => e.IdTome).HasName("tome_pkey");

            entity.ToTable("tome", "data");

            entity.Property(e => e.IdTome)
                .HasDefaultValueSql("nextval('data.\"tome_tomeID_seq\"'::regclass)")
                .HasColumnName("id_tome");
            entity.Property(e => e.DateParution).HasColumnName("date_parution");
            entity.Property(e => e.IdAuteur).HasColumnName("id_auteur");
            entity.Property(e => e.IdEditeur).HasColumnName("id_editeur");
            entity.Property(e => e.IdSerie).HasColumnName("id_serie");
            entity.Property(e => e.Isbn)
                .HasMaxLength(17)
                .HasColumnName("isbn");
            entity.Property(e => e.LieuParution)
                .HasMaxLength(500)
                .HasColumnName("lieu_parution");
            entity.Property(e => e.Numero).HasColumnName("numero");
            entity.Property(e => e.Page).HasColumnName("page");
            entity.Property(e => e.Resume)
                .HasMaxLength(100000)
                .HasColumnName("resume");
            entity.Property(e => e.Titre)
                .HasMaxLength(500)
                .HasColumnName("titre");

            entity.HasOne(d => d.IdAuteurNavigation).WithMany(p => p.Tomes)
                .HasForeignKey(d => d.IdAuteur)
                .HasConstraintName("FK_auteur_id");

            entity.HasOne(d => d.IdEditeurNavigation).WithMany(p => p.Tomes)
                .HasForeignKey(d => d.IdEditeur)
                .HasConstraintName("FK_editeur_id");

            entity.HasOne(d => d.IdSerieNavigation).WithMany(p => p.Tomes)
                .HasForeignKey(d => d.IdSerie)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_serie_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
