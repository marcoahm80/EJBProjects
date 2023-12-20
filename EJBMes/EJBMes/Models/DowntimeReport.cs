using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EJBMes.Models
{
    public partial class DowntimeReport
    {
        public int Id { get; set; }

        public string EmployeeNum { get; set; } = string.Empty;

        public string JobNum { get; set; } = string.Empty;

        public int AssemblyNum { get; set; }

        public int OpSeq { get; set; }
        [Display(Name = "Start Date")]
        [DataType(DataType.DateTime)]
        public DateTime DownTimeStartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.DateTime)]
        public DateTime DownTimeEndDate { get; set; }

        public string ResourceGroup { get; set; } = string.Empty;

        public string ResourceId { get; set; } = string.Empty;

        public string? ReferenceNotes { get; set; } = string.Empty;
        [Display(Name = "Reason")]
        public string ReasonCode { get; set; } = string.Empty;
        [Display(Name = "Active")]
        public bool ActiveDowntime { get; set; } = false;

        public bool Procesed { get; set; } = false;
    }
}
