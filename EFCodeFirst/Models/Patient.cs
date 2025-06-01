using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCodeFirst.Models;

[Table("Patient")]
public class Patient
{
    [Key]
    public int IdPatient { get; set; }
    
    [MaxLength(100)]
    public string FirstName { get; set; }
    
    [MaxLength(100)]
    public string LastName { get; set; }
    
    
    public DateTime BirthDate { get; set; }
    
    //wlasciwosc nawigacyjna, relacja
    //kolekcja recept przypisanych do pacjenta, 1 .. *
    //jeden pacjent - wiele recept
    public ICollection<Prescription> Prescriptions { get; set; }   
}