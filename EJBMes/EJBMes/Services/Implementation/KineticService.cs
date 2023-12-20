using Microsoft.EntityFrameworkCore;
using EJBMes.Resources;
using EJBMes.Services.Contract;
using KineticInterface;
using System.Data;

namespace EJBMes.Services.Implementation
{
    public class KineticService : IKineticService
    {
        private static ClsKineticRest? oService;
        private static string? vURLKinetic;
        private static string? vUserKinetic;
        private static string? vPwdKinetic;
        private static string? vCompanyID;

        public KineticService()
        {
            vURLKinetic = "";
            vUserKinetic = "";
            vPwdKinetic = "";
            vCompanyID = "";
        }

        public KineticService(string URLKinetic, string UserKinetic, string PwdKinetic, string CompanyID) {
            oService = new ClsKineticRest();
            oService.URL = URLKinetic;
            oService.UserID = UserKinetic;
            oService.AuthCode = Utilities.DecodeString(PwdKinetic);
            oService.InitialiceConnection();
            vURLKinetic = URLKinetic;
            vUserKinetic = UserKinetic;
            vPwdKinetic = Utilities.DecodeString(PwdKinetic);
            vCompanyID = CompanyID;
        }

        #region JobsData

        public List<JobData> GetJobs(string Plant)
        {
            string ErrMsg = string.Empty;
            bool bError = false;
            DataSet dsJobs = new DataSet();
            if (oService != null)
            {
                if (vCompanyID != null)
                {
                    dsJobs = oService.JobGetList(vCompanyID, Plant, "(JobReleased = true AND Plant = '" + Plant + "') AND JobNum >= '' AND JobType = 'MFG' AND Jobclosed = False and  JobComplete = false and JobReleased = True and JobFirm = True BY JobNum", 0, 0, out bError, out ErrMsg);
                }
            }
            

            List<JobData> modelList = new List<JobData>();

            if (dsJobs != null)
            {
                if (dsJobs.Tables.Count > 0)
                {
                    //modelList = dsJobs.Tables["JobHeadList"].Select(c => new JobData
                    //{
                    //    JobNum = c.JobNum,
                    //    PartNum = c.PartNum,
                    //    AssemblySeq = 0,
                    //    OprSeq = 0
                    //});
                    foreach(DataRow dr in dsJobs.Tables["JobHeadList"].Rows) 
                    {
                        modelList.Add(new JobData { JobNum = dr["JobNum"].ToString(), PartNum = dr["PartNum"].ToString() });
                    }
                }
            }
            return modelList;
        }

        public List<JobAsm> GetJobsAsmblys(string Plant, string pJobNum)
        {
            string ErrMsg = string.Empty;
            bool bError = false;
            DataSet dsJobAsm = new DataSet();
            if (oService != null)
            {
                if (vCompanyID != null)
                {
                    if (pJobNum != null)
                    {
                        dsJobAsm = oService.BOReaderGetList(vCompanyID, Plant, "Erp:BO:JobAsmSearch", "(JobNum = '" + pJobNum + "')", "AssemblySeq,PartNum,Description", out bError, out ErrMsg);
                    }
                }
            }

            List<JobAsm> modelList = new List<JobAsm>();

            if (dsJobAsm != null)
            {
                if (dsJobAsm.Tables.Count > 0)
                {
                    foreach (DataRow dr in dsJobAsm.Tables["JobAsmblList"].Rows)
                    {
                        modelList.Add(new JobAsm { JobNum = pJobNum, AssemblySeq = Convert.ToInt32(dr["AssemblySeq"]), PartNum = dr["PartNum"].ToString() });
                    }
                }
            }
            return modelList;
        }

        public List<JobOper> GetJobsAsmOprs(string Plant, string pJobNum, int pAssemblySeq)
        {
            string ErrMsg = string.Empty;
            bool bError = false;
            DataSet dsJobOper = new DataSet();
            if (oService != null)
            {
                if (vCompanyID != null)
                {
                    if (pJobNum != null)
                    {
                        dsJobOper = oService.BOReaderGetList(vCompanyID, Plant, "Erp:BO:JobOperSearch", "( (JobNum = '" + pJobNum + "') AND (AssemblySeq = " + pAssemblySeq.ToString("N0") + ") )", "SubContract,LaborEntryMethod,OprSeq,OpCode,OpCodeOpDesc", out bError, out ErrMsg);
                    }
                }
            }

            List<JobOper> modelList = new List<JobOper>();

            if (dsJobOper != null)
            {
                if (dsJobOper.Tables.Count > 0)
                {
                    foreach (DataRow dr in dsJobOper.Tables["JobOperList"].Rows)
                    {
                        modelList.Add(new JobOper { JobNum = pJobNum, AssemblySeq =  pAssemblySeq, OprSeq = Convert.ToInt32(dr["OprSeq"]), 
                                                    OpCode = dr["OpCode"].ToString(), OpCodeOpDesc = dr["OpCodeOpDesc"].ToString(),
                            SubContract = Convert.ToBoolean(dr["SubContract"]), LaborEntryMethod = dr["LaborEntryMethod"].ToString()
                        });
                    }
                }
            }
            return modelList;
        }

        public List<JobResource> GetJobResources(string Plant, string pJobNum, int pAssemblySeq, int pOprSeq)
        {
            string ErrMsg = string.Empty;
            bool bError = false;
            DataSet dsJobEntry = new DataSet();
            if (oService != null)
            {
                if (vCompanyID != null)
                {
                    if (pJobNum != null && pOprSeq>0)
                    {
                        dsJobEntry = oService.JobEntryGetByID(vCompanyID, Plant, pJobNum, out bError, out ErrMsg);
                    }
                }
            }

            List<JobResource> modelList = new List<JobResource>();
            if (pJobNum != null)
            {
                if (dsJobEntry != null)
                {
                    if (dsJobEntry.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsJobEntry.Tables["JobOpDtl"].Rows)
                        {
                            if (dr["JobNum"].ToString() == pJobNum && dr["AssemblySeq"].ToString() == pAssemblySeq.ToString() && dr["OprSeq"].ToString() == pOprSeq.ToString())
                            {
                                modelList.Add(new JobResource { JobNum = pJobNum, AssemblySeq = pAssemblySeq, OprSeq = pOprSeq, ResourceGroup = dr["ResourceGrpID"].ToString(), ResourceID = dr["ResourceID"].ToString() });
                            }
                        }
                    }
                }
            }
            return modelList;
        }

        #endregion

        #region Warehouse Data
        public List<WareHouseData> GetWareHouses(string Plant)
        {
            string ErrMsg = string.Empty;
            bool bError = false;
            DataSet dsData = new DataSet();
            if (oService != null)
            {
                if (vCompanyID != null)
                {
                    dsData = oService.WarehseGetList(vCompanyID, Plant, "Plant = '" + Plant + "' BY WarehouseCode", 0, 0, out bError, out ErrMsg);                    
                }
            }

            List<WareHouseData> modelList = new List<WareHouseData>();
            if (dsData != null)
            {
                if (dsData.Tables.Count > 0)
                {
                    foreach (DataRow dr in dsData.Tables["WarehseList"].Rows)
                    {
                        modelList.Add(new WareHouseData
                        {
                           WareHouseCode = dr["WarehouseCode"].ToString(),
                           WareHouseDescription = dr["Description"].ToString()
                        });
                    }
                }
            }
            return modelList;
        }

        public List<WhseBinData> GetBinNums(string Plant, string pWarehouseCode)
        {
            string ErrMsg = string.Empty;
            bool bError = false;
            DataSet dsData = new DataSet();
            if (oService != null)
            {
                if (vCompanyID != null && pWarehouseCode != null)
                {
                    dsData = oService.WhseBinGetList(vCompanyID, Plant, "WarehouseCode = '" + pWarehouseCode + "' BY BinNum", 0, 0, out bError, out ErrMsg);
                }
            }

            List<WhseBinData> modelList = new List<WhseBinData>();
            if (dsData != null)
            {
                if (dsData.Tables.Count > 0)
                {
                    foreach (DataRow dr in dsData.Tables["WhseBinList"].Rows)
                    {
                        modelList.Add(new WhseBinData
                        {
                            WareHouseCode = dr["WarehouseCode"].ToString(),
                            BinNum = dr["BinNum"].ToString(),
                            BinNumDescription = dr["Description"].ToString()
                        });
                    }
                }
            }
            return modelList;
        }

        #endregion

        #region Reason

        public List<Reason> GetReasons(string Plant)
        {
            string ErrMsg = string.Empty;
            bool bError = false;
            DataSet dsData = new DataSet();
            if (oService != null)
            {
                if (vCompanyID != null)
                {
                    dsData = oService.ReasonGetRows(vCompanyID, Plant, "ReasonType='S' BY ReasonCode", "", 0, 0, out bError, out ErrMsg);
                }
            }

            List<Reason> modelList = new List<Reason>();
            if (dsData != null)
            {
                if (dsData.Tables.Count > 0)
                {
                    foreach (DataRow dr in dsData.Tables["Reason"].Rows)
                    {
                        modelList.Add(new Reason
                        {
                            ReasonCode = dr["ReasonCode"].ToString(),
                            ReasonDescription = dr["Description"].ToString()
                        });
                    }
                }
            }
            return modelList;
        }

        public List<Indirect> GetIndirectReeasons(string Plant)
        {
            string ErrMsg = string.Empty;
            bool bError = false;
            DataSet dsData = new DataSet();
            if (oService != null)
            {
                if (vCompanyID != null)
                {
                    dsData = oService.IndirectGetRows(vCompanyID, Plant, "DownTime=1 BY Description", 0, 0, out bError, out ErrMsg);
                }
            }

            List<Indirect> modelList = new List<Indirect>();
            if (dsData != null)
            {
                if (dsData.Tables.Count > 0)
                {
                    foreach (DataRow dr in dsData.Tables["Indirect"].Rows)
                    {
                        modelList.Add(new Indirect
                        {
                            IndirectCode = dr["IndirectCode"].ToString(),
                            IndirectDescription = dr["Description"].ToString()
                        });
                    }
                }
            }
            return modelList;
        }

        #endregion



    }
}
