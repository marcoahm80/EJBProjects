using EJBMes.Resources;

namespace EJBMes.Services.Contract
{
    public interface IKineticService
    { 
        List<JobData> GetJobs(string Plant);

        List<JobAsm> GetJobsAsmblys(string Plant, string pJobNum);

        List<JobOper> GetJobsAsmOprs(string Plant, string pJobNum, int pAssemblySeq);

        List<JobResource> GetJobResources(string Plant, string pJobNum, int pAssemblySeq, int OprSeq);

        List<WareHouseData> GetWareHouses(string Plant);

        List<WhseBinData> GetBinNums(string Plant, string pWarehouseCode);

        List<Reason> GetReasons(string Plant);

        List<Indirect> GetIndirectReeasons(string Plant);

    }
}
