using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EJBMes.Models;

public partial class ProdReport
{
    public int Id { get; set; }

    public string EmployeeNum { get; set; } = string.Empty;

    public string JobNum { get; set; } = string.Empty;

    public int AssemblyNum { get; set; }

    public int OpSeq { get; set; }
    [Display(Name = "Start Date")]
    [DataType(DataType.DateTime)]
    public DateTime StartDate { get; set; }
    [Display(Name = "End Date")]
    [DataType(DataType.DateTime)]
    public DateTime EndDate { get; set; }

    public string ResourceGroup { get; set; } = string.Empty;

    public string ResourceId { get; set; } = string.Empty;

    public string? ReferenceNotes { get; set; } = string.Empty;
    [Column(TypeName = "decimal(18, 2)")]
    public decimal LaborQty { get; set; }
    [Display(Name = "Active")]
    public bool ActiveLabor { get; set; } = false;

    public bool Procesed { get; set; } = false;

}
