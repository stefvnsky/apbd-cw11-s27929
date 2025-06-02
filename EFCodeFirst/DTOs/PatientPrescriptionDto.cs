namespace EFCodeFirst.DTOs;

public class PatientPrescriptionDto
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public List<PrescriptionResponseDto> Prescriptions { get; set; }
}