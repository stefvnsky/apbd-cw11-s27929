﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EFCodeFirst.Models;

[PrimaryKey(nameof(IdPrescription), nameof(IdMedicament))]
[Table("Prescription_Medicament")]

public class PrescriptionMedicament
{
    [ForeignKey(nameof(Medicament))]
    public int IdMedicament { get; set; }
    
    [ForeignKey(nameof(Prescription))]
    public int IdPrescription { get; set; }
    
    public int? Dose { get; set; }

    [MaxLength(100)]
    public string Details { get; set; }
    
    //wlasciwosci nawigacyjne, relacje, * .. *
    public Medicament Medicament { get; set; }
    public Prescription Prescription { get; set; }
}