using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCodeFirst.Models;

[Table("Prescription")]
public class Prescription
{
    [Key]
    public int IdPrescription { get; set; }
    
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    
    [ForeignKey(nameof(Patient))]
    public int IdPatient { get; set; }
    
    [ForeignKey(nameof(Doctor))]
    public int IdDoctor { get; set; }
    
    //wlasciwosc nawigacyjna, relacja, wiele recept moze miec jednego lekrza i jednego pacjenta
    public Patient Patient { get; set; }        //dostep do pelnego obiektu pacjenta
    public Doctor Doctor { get; set; }          //dostep do pelnego obiektu doktora
    
    //jedna recepta wiele lekow
    public ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; }    // 1 .. *
}