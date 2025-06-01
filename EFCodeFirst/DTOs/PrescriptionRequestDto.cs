namespace EFCodeFirst.DTOs;

public class PrescriptionRequestDto
{
    public PatientDto Patient { get; set; }
    public int DoctorId { get; set; }
    public List<MedicamentDto> Medicaments { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
}