using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Nomina.Models
{
    public partial class FerreteriaContext : DbContext
    {
        public FerreteriaContext()
        {
        }

        public FerreteriaContext(DbContextOptions<FerreteriaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Producto> Productos { get; set; }
        public virtual DbSet<Seccion> Seccions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=localhost;database=Ferreteria;password=Chivas12;user=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.23-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.ToTable("productos");

                entity.HasIndex(e => e.Seccion, "fkProductosSeccion_idx");

                entity.Property(e => e.Descripcion).HasColumnType("tinytext");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.Property(e => e.Precio).HasPrecision(10);

                entity.HasOne(d => d.SeccionNavigation)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.Seccion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkProductosSeccion");
            });

            modelBuilder.Entity<Seccion>(entity =>
            {
                entity.HasKey(e => e.Numero)
                    .HasName("PRIMARY");

                entity.ToTable("seccion");

                entity.Property(e => e.Numero).ValueGeneratedNever();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(45);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
