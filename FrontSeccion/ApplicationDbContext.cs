using FrontSeccion.Models;
using Microsoft.EntityFrameworkCore;

namespace FrontSeccion
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public virtual DbSet<Alumno> Alumno { get; set; }
        public virtual DbSet<Curso> Curso { get; set; }
        public virtual DbSet<Seccion> Seccion { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Alumno>()
          .Property(e => e.dni_alu)
          .IsFixedLength()
          .IsUnicode(false);

            modelBuilder.Entity<Alumno>()
                .Property(e => e.nombre_alu)
                .IsUnicode(false);

            modelBuilder.Entity<Alumno>()
                .Property(e => e.apellidos_alu)
                .IsUnicode(false);

            modelBuilder.Entity<Alumno>()
                .HasMany(e => e.Seccion)
                .WithMany(e => e.Alumno)
                .UsingEntity<Dictionary<string, object>>(
                joinEntity =>
                {
                    joinEntity.ToTable("Detalle_asig_alumno_seccion");
                    joinEntity.HasOne<Alumno>().WithMany().HasForeignKey("Alumnoid_alu");
                    joinEntity.HasOne<Seccion>().WithMany().HasForeignKey("Seccionid_sec");

                }
                );

            modelBuilder.Entity<Curso>()
                .Property(e => e.descripcion_cur)
                .IsUnicode(false);

            modelBuilder.Entity<Curso>()
                .HasMany(e => e.Seccion)
                .WithOne(e => e.Curso)
                //.HasForeignKey(e => e.Curso)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
