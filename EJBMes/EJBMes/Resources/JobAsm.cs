using Microsoft.AspNetCore.Mvc.Rendering;

namespace EJBMes.Resources
{
    public class JobAsm
    {
        public string JobNum { get; set; }

        public int AssemblySeq { get; set; }

        public string PartNum { get; set; }

        public string Description { get; set; }

    }
}
