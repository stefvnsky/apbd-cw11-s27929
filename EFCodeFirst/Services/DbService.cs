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

    //HTTP POST
    public async Task AddPrescriptionAsync(PrescriptionRequestDto dto)
    {
        //czy lekarz istnieje
        var doctor = await _context.Doctors.FindAsync(dto.IdDoctor);
        if (doctor == null)
        {
            throw new Exception($"Nie znaleziono doktora o id {dto.IdDoctor} not found");
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
            _context.Patients.Add(patient); //dodanie obiektu do bazy
            await _context.SaveChangesAsync(); //zapis danych do bazy
        }
        //czy lek istnieje

        //pobranie id lekow ktore istnieja
        var existingMedsId = await _context.Medicaments //Medicaments - baza lekow w db
            .Where(med => dto.Medicaments
                .Select(medDto => medDto.IdMedicament) //lista przeslanych id przez klietna
                .Contains(med.IdMedicament)) //czy ten lek nalezy do listy lekow z bazy
            .Select(med => med.IdMedicament)
            .ToListAsync();

        //nieznalezione leki wsrod podanych
        var notFoundMedsId = dto.Medicaments
            .Select(medDto => medDto.IdMedicament)
            .Where(id => !existingMedsId //te ktorych nie ma w bazie
                .Contains(id))
            .ToList();

        //jesl jakiekolwiek id nie istnieje(lek)
        if (notFoundMedsId.Any())
        {
            throw new Exception($"Te leki nie zostaly znalezione w bazie: {string.Join(", ", notFoundMedsId)}");
        }

        //walidacja liczby lekow
        if (dto.Medicaments.Count > 10)
        {
            throw new Exception("Recepta nie moze zawierac wiecej niz 10 lekow");
        }

        //walidacja dat
        if (dto.DueDate < dto.Date)
        {
            throw new Exception("Termin waznosci recepty musi byc wiekszy lub rowny dacie wystawienia");
        }

        //utworzenie recepty
        var prescription = new Prescription();
        prescription.Date = dto.Date;
        prescription.DueDate = dto.DueDate;
        prescription.IdDoctor = doctor.IdDoctor;
        prescription.IdPatient = patient.IdPatient;

        _context.Prescriptions.Add(prescription);
        await _context.SaveChangesAsync();

        //dodanie leku do recepty
        foreach (var medicamentDto in dto.Medicaments)
        {
            _context.PrescriptionMedicaments.Add(new PrescriptionMedicament
            {
                IdPrescription = prescription.IdPrescription,
                IdMedicament = medicamentDto.IdMedicament,
                Dose = medicamentDto.Dose,
                Details = medicamentDto.Description
            });
        }

        await _context.SaveChangesAsync();
    }

    //HTTP GET
    public async Task<PatientPrescriptionDto> GetPatientWithPrescriptionAsync(int id)
    {
        var patient = await _context.Patients
            .Where(patient => patient.IdPatient == id)
            .Select(patient => new PatientPrescriptionDto
            {
                IdPatient = patient.IdPatient,
                FirstName = patient.FirstName,
                Prescriptions = patient.Prescriptions
                    .OrderBy(prescription => prescription.DueDate)
                    .Select(prescription => new PrescriptionResponseDto
                    {
                        IdPrescription = prescription.IdPrescription,
                        Date = prescription.DueDate,
                        DueDate = prescription.DueDate,
                        Doctor = new DoctorResponseDto
                        {
                            IdDoctor = prescription.IdDoctor,
                            FirstName = prescription.Doctor.FirstName,
                        },
                        Medicaments = prescription.PrescriptionMedicaments
                            .Select(pm => new MedicamentResponseDto
                            {
                                IdMedicament = pm.Medicament.IdMedicament,
                                Name = pm.Medicament.Name,
                                Dose = pm.Dose,
                                Description = pm.Details,
                            }).ToList()
                    }).ToList()
            }).FirstOrDefaultAsync();
        
        if (patient == null)
            throw new Exception($"Nie istnieje pacjent o ID {id}");

        return patient;
    }
}
