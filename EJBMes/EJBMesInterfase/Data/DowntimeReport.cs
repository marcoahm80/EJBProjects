using System;
using System.Collections.Generic;

namespace EJBMesInterfase.Data
{
    public partial class DowntimeReport
    {
        public int Id { get; set; }

        public string EmployeeNum { get; set; } = string.Empty;

        public string JobNum { get; set; } = string.Empty;

        public int AssemblyNum { get; set; }

        public int OpSeq { get; set; }

        public DateTime DownTimeStartDate { get; set; }

        public DateTime DownTimeEndDate { get; set; }

        public string ResourceGroup { get; set; } = string.Empty;

        public string ResourceId { get; set; } = string.Empty;

        public string ReferenceNotes { get; set; } = string.Empty;

        public string ReasonCode { get; set; } = string.Empty;

        public bool ActiveDowntime { get; set; } = false;

        public bool Procesed { get; set; } = false;

        public string SiteID { get; set; } = string.Empty;
    }
}