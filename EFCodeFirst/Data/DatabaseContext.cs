using EFCodeFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCodeFirst.Data;

//laczenie kodu z baza danych
public class DatabaseContext : DbContext
{
    //stworz tabele Doctor w bazie danych z dostepem do Add() etc.
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

    protected DatabaseContext()
    {
    }

    //konstruktor przekazujacy ustawienia polaczenia z baza z program.cs
    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}