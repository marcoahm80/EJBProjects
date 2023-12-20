using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EJBMes.Models;

public partial class ScrapReport
{
    public int Id { get; set; }

    public string EmployeeNum { get; set; } = string.Empty;

    public string JobNum { get; set; } = string.Empty;

    public int AssemblyNum { get; set; }

    public int OpSeq { get; set; }
    [Display(Name = "Scrap Date")]
    [DataType(DataType.Date)]
    public DateTime ScrapDate { get; set; }

    public string ResourceGroup { get; set; } = string.Empty;

    public string ResourceId { get; set; } = string.Empty;

    public string? ReferenceNotes { get; set; } = string.Empty;
    [Column(TypeName = "decimal(18, 2)")]
    public decimal ScrapQty { get; set; }
    [Display(Name = "Reason")]
    public string ReasonCode { get; set; } = string.Empty;
    [Display(Name = "Warehouse")]
    public string WhseCode { get; set; } = string.Empty;

    public string BinNum { get; set; } = string.Empty;

    public bool Procesed { get; set; } = false;
}
