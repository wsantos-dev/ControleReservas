using ControleReservas.Domain;
using ControleReservas.Domain.Entities;
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

    public DbSet<EmailConfiguration> emailConfigurations => Set<EmailConfiguration>();



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Sala>(entity =>
        {
            entity.ToTable("Salas");
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuarios");
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.ToTable("Reservas");
            entity.HasKey(e => e.Id);

            entity.HasOne(r => r.Sala)
                  .WithMany(s => s.Reservas)
                  .HasForeignKey(r => r.SalaId);

            entity.HasOne(r => r.Usuario)
                  .WithMany(u => u.Reservas)
                  .HasForeignKey(r => r.UsuarioId);

            // Índice para otimizar verificação de conflitos
            entity.HasIndex(r => new { r.SalaId, r.DataHoraInicio, r.DataHoraFim });
        });

        modelBuilder.Entity<EmailConfiguration>(entity =>
        {
            entity.ToTable("EmailConfigurations");
            entity.HasKey(e => e.Id);


            entity.Property(e => e.Id)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

            entity.Property(e => e.Provider)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.EncryptedApiKey)
               .IsRequired();

            entity.Property(e => e.FromEmail)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.FromName)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime2");
        });

    }
}
