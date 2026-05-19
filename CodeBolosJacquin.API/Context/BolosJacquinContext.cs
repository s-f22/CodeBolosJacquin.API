using System;
using System.Collections.Generic;
using CodeBolosJacquin.API.Domains;
using Microsoft.EntityFrameworkCore;

namespace CodeBolosJacquin.API.Context;

public partial class BolosJacquinContext : DbContext
{
    public BolosJacquinContext()
    {
    }

    public BolosJacquinContext(DbContextOptions<BolosJacquinContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bolo> Bolos { get; set; }

    public virtual DbSet<BoloImagen> BoloImagens { get; set; }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bolo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Bolos__3214EC07F940EB0C");

            entity.HasMany(d => d.Categoria).WithMany(p => p.Bolos)
                .UsingEntity<Dictionary<string, object>>(
                    "BoloCategoria",
                    r => r.HasOne<Categoria>().WithMany()
                        .HasForeignKey("CategoriaId")
                        .HasConstraintName("FK_BoloCategorias_Categorias"),
                    l => l.HasOne<Bolo>().WithMany()
                        .HasForeignKey("BoloId")
                        .HasConstraintName("FK_BoloCategorias_Bolos"),
                    j =>
                    {
                        j.HasKey("BoloId", "CategoriaId").HasName("PK__BoloCate__F634121BCE1BAEE7");
                        j.ToTable("BoloCategorias");
                    });
        });

        modelBuilder.Entity<BoloImagen>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BoloImag__3214EC07ECE40F15");

            entity.HasOne(d => d.Bolo).WithMany(p => p.BoloImagens).HasConstraintName("FK_BoloImagens_Bolos");
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC076A8501AE");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuarios__3214EC073D8647EE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
