using System;
using System.Collections.Generic;

namespace EJBMesInterfase.Data
{
    public partial class ProdReport
    {
        public int Id { get; set; }

        public string EmployeeNum { get; set; } = string.Empty;

        public string JobNum { get; set; } = string.Empty;

        public int AssemblyNum { get; set; }

        public int OpSeq { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string ResourceGroup { get; set; } = string.Empty;

        public string ResourceId { get; set; } = string.Empty;

        public string ReferenceNotes { get; set; } = string.Empty;

        public decimal LaborQty { get; set; }

        public bool ActiveLabor { get; set; } = false;

        public bool Procesed { get; set; } = false;

        public string SiteID {  get; set; } = string.Empty;

    }
}