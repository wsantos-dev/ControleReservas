using ControleReservas.Domain;
using Microsoft.EntityFrameworkCore;

namespace ControleReservas.Infrastructure.Persistence;

public class ControleReservasDbContext : DbContext
{
   public ControleReservasDbContext(DbContextOptions<ControleReservasDbContext> options)
        : base(options)
    {
    }

    public DbSet<Sala> Salas => Set<Sala>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Reserva> Reservas => Set<Reserva>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Sala>(entity =>
        {
            entity.ToTable("Salas");
            entity.HasKey(e => e.Id); // Define Id como chave primária
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuarios");
            entity.HasKey(e => e.Id); // Define Id como chave primária
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.ToTable("Reservas");
            entity.HasKey(e => e.Id); // Define Id como chave primária

            entity.HasOne(r => r.Sala)
                  .WithMany(s => s.Reservas)
                  .HasForeignKey(r => r.SalaId);

            entity.HasOne(r => r.Usuario)
                  .WithMany(u => u.Reservas)
                  .HasForeignKey(r => r.UsuarioId);

            // Índice para otimizar verificação de conflitos
            entity.HasIndex(r => new { r.SalaId, r.DataHoraInicio, r.DataHoraFim });
        });
    }
}
