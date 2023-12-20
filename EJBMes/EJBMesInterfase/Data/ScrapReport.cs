using System;
using System.Collections.Generic;

namespace EJBMesInterfase.Data
{
    public partial class ScrapReport
    {
        public int Id { get; set; }

        public string EmployeeNum { get; set; } = string.Empty;

        public string JobNum { get; set; } = string.Empty;

        public int AssemblyNum { get; set; }

        public int OpSeq { get; set; }

        public DateTime ScrapDate { get; set; }

        public string ResourceGroup { get; set; } = string.Empty;

        public string ResourceId { get; set; } = string.Empty;

        public string ReferenceNotes { get; set; } = string.Empty;

        public decimal ScrapQty { get; set; }

        public string ReasonCode { get; set; } = string.Empty;

        public string WhseCode { get; set; } = string.Empty;

        public string BinNum { get; set; } = string.Empty;

        public bool Procesed { get; set; } = false;

        public string SiteID { get; set; } = string.Empty;
    }
}