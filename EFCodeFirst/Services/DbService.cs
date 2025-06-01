using EFCodeFirst.Data;
using EFCodeFirst.DTOs;
using EFCodeFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCodeFirst.Services;

public class DbService : IDbService
{
    private readonly DatabaseContext _context;

    public DbService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task AddPrescriptionAsync(PrescriptionRequestDto dto)
    {
        //czy lekarz istnieje
        var doctor = await _context.Doctors.FindAsync(dto.DoctorId);
        if (doctor == null)
        {
            throw new Exception($"Nie znaleziono doktora o id {dto.DoctorId} not found");
        }
        
        //czy pacjent juz istnieje
        var patient = await _context.Patients.FirstOrDefaultAsync(patient => 
            patient.FirstName == dto.Patient.FirstName && 
            patient.LastName == dto.Patient.LastName && 
            patient.BirthDate == dto.Patient.BirthDate);

        if (patient == null)
        {
            //tworzenie nowego obiektu + przypisanie wartosci do jego wlasciwosci
            patient = new Patient();
            patient.FirstName = dto.Patient.FirstName;
            patient.LastName = dto.Patient.LastName;
            patient.BirthDate = dto.Patient.BirthDate;
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }
        
        //walidacja dat
        if (dto.DueDate < dto.Date)
        {
            throw new Exception("Termin waznosci musi byc wiekszy lub rowny dacie wystawienia");
        }

    }
}