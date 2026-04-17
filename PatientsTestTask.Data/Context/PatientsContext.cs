using Microsoft.EntityFrameworkCore;
using PatientsTestTask.Data.Context.Entities;

namespace PatientsTestTask.Data.Context;

public partial class PatientsContext : DbContext
{
    internal DbSet<Patient> Patients => Set<Patient>();
    internal DbSet<PatientName> PatientNames => Set<PatientName>();
    internal DbSet<PatientGivenName> PatientGivenNames => Set<PatientGivenName>();

    public PatientsContext(DbContextOptions<PatientsContext> options) : base(options)
    {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Patient>()
            .HasKey(x => x.Id);
        modelBuilder.Entity<Patient>()
            .HasOne<PatientName>(x => x.Name)
            .WithOne()
            .HasForeignKey<PatientName>(x => x.Id)
            .IsRequired();

        modelBuilder.Entity<PatientName>()
            .HasKey(x => x.Id);
        modelBuilder.Entity<PatientName>()
            .HasMany<PatientGivenName>(x => x.GivenNames)
            .WithOne()
            .HasForeignKey(x => x.PatientId)
            .HasPrincipalKey(x => x.Id);

        modelBuilder.Entity<PatientGivenName>()
            .HasKey(x => new { x.PatientId, x.Position });
    }
}
