using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EJBMesInterfase.Data;
using EJBMesInterfase.Resources;


namespace EJBMesInterfase.Kinetic
{
    internal class clsKinetic
    {
        private static ClsKineticRest oService;
        private static string vURLKinetic;
        private static string vUserKinetic;
        private static string vPwdKinetic;
        private static string vCompanyID;

        public clsKinetic()
        {
            vURLKinetic = "";
            vUserKinetic = "";
            vPwdKinetic = "";
            vCompanyID = "";
        }

        public clsKinetic(string URLKinetic, string UserKinetic, string PwdKinetic, string CompanyID)
        {
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

        public bool CreateLabor(ProdReport oData, out string oError)
        {
            oError = String.Empty;
            bool oSuccess = false;
            if (oData != null)
            {
                string ErrMsg = string.Empty;
                bool bError = false;
                DataSet dsData = new DataSet();
                if (oService != null)
                {
                    if (vCompanyID != null)
                    {
                        //dsData = oService.WarehseGetList(vCompanyID, Plant, "Plant = '" + Plant + "' BY WarehouseCode", 0, 0, out bError, out ErrMsg);
                        oSuccess = true;
                    }
                }
            }
            return oSuccess;
        }

        public bool CreateScrap(ScrapReport oData, out string oError)
        {
            oError = String.Empty;
            bool oSuccess = false;
            if (oData != null)
            {
                string ErrMsg = string.Empty;
                bool bError = false;
                DataSet dsData = new DataSet();
                if (oService != null)
                {
                    if (vCompanyID != null)
                    {
                        //dsData = oService.WarehseGetList(vCompanyID, Plant, "Plant = '" + Plant + "' BY WarehouseCode", 0, 0, out bError, out ErrMsg);
                        oSuccess = true;
                    }
                }
            }
            return oSuccess;
        }

        public bool CreateDownTime(DowntimeReport oData, out string oError)
        {
            oError = String.Empty;
            bool oSuccess = false;
            if (oData != null)
            {
                string ErrMsg = string.Empty;
                bool bError = false;
                DataSet dsData = new DataSet();
                if (oService != null)
                {
                    if (vCompanyID != null)
                    {
                        //dsData = oService.WarehseGetList(vCompanyID, Plant, "Plant = '" + Plant + "' BY WarehouseCode", 0, 0, out bError, out ErrMsg);
                        oSuccess = true;
                    }
                }
            }
            return oSuccess;
        }
    }
}
