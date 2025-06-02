using EFCodeFirst.DTOs;

namespace EFCodeFirst.Services;

public interface IDbService
{
    Task AddPrescriptionAsync(PrescriptionRequestDto dto);

    Task<PatientPrescriptionDto> GetPatientWithPrescriptionAsync(int id);
}