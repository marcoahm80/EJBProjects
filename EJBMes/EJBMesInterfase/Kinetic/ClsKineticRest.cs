using System.Globalization;
using System;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.Data;



namespace EJBMesInterfase.Kinetic
{
    public class ClsKineticRest
    {
        private string oUrl = "";
        private Uri oRestAPIURL;
        private string uID = "";
        private string uAuthCode = "";
        private bool bSSLEnforceValidate = true;
        private bool Initialiced = false;

        public ClsKineticRest()
        {
            Initialiced = false;
            CultureInfo.CurrentCulture = new CultureInfo("en-US", false);
            oRestAPIURL = new Uri("https://erp.tydenbrooks.com/");
        }

        public ClsKineticRest(bool SSLEnforceValidate)
        {
            bSSLEnforceValidate = SSLEnforceValidate;
            //Incluir esta validacion en la inicialización de vuestro desarrollo. 
            if (bSSLEnforceValidate)
            {
                EnforceSSLValidation();
            }
            Initialiced = false;
            CultureInfo.CurrentCulture = new CultureInfo("en-US", false);
            oRestAPIURL = new Uri("https://erp.tydenbrooks.com/");
        }

        /// <summary>
        /// Fuerza a validar el certificado SSL del servidor 
        /// </summary>
        private void EnforceSSLValidation()
        {
            if (bSSLEnforceValidate)
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            }
        }

        public void InitialiceConnection()
        {
            oRestAPIURL = new Uri(oUrl);
            Initialiced = true;
        }

        public string URL
        {
            get
            {
                return oUrl;
            }
            set
            {
                oUrl = value;
                if (bSSLEnforceValidate)
                {
                    EnforceSSLValidation();
                }
                oRestAPIURL = new Uri(oUrl);
            }
        }

        public string UserID
        {
            get
            {
                return uID;
            }
            set
            {
                uID = value;
            }
        }

        public string AuthCode
        {
            get
            {
                return uAuthCode;
            }
            set
            {
                uAuthCode = value;
            }
        }

        public void Dispose()
        {
            oRestAPIURL = null;
        }

        private string ReplaceDatesEmptysToNull(string BodyJSon)
        {
            string newString = BodyJSon.Replace("Date\":\"\"", "Date\": null");
            newString = newString.Replace("Date\": \"\"", "Date\": null");
            newString = newString.Replace("GUIDateOfExport\":\"\"", "GUIDateOfExport\": null");
            newString = newString.Replace("CreatedOn\":\"\"", "CreatedOn\": null");
            newString = newString.Replace("SignedOn\":\"\"", "SignedOn\": null");
            newString = newString.Replace("ELIEInvUpdatedOn\":\"\"", "ELIEInvUpdatedOn\": null");
            newString = newString.Replace("ChangeDateTime\":\"\"", "ChangeDateTime\": null");
            newString = newString.Replace("DefRevStart\":\"\"", "DefRevStart\": null");
            newString = newString.Replace("DateCollectionPosted\":\"\"", "DateCollectionPosted\": null");
            newString = newString.Replace("ExternalCRMLastSync\":\"\"", "ExternalCRMLastSync\": null");
            newString = newString.Replace("TaxPoint\":\"\"", "TaxPoint\": null");
            newString = newString.Replace("Date1\":\"\"", "Date1\": null");
            newString = newString.Replace("Date2\":\"\"", "Date2\": null");
            newString = newString.Replace("Date3\":\"\"", "Date3\": null");
            newString = newString.Replace("Date4\":\"\"", "Date4\": null");
            newString = newString.Replace("Date5\":\"\"", "Date5\": null");
            newString = newString.Replace("Date6\":\"\"", "Date6\": null");
            newString = newString.Replace("Date7\":\"\"", "Date7\": null");
            newString = newString.Replace("Date8\":\"\"", "Date8\": null");
            newString = newString.Replace("Date9\":\"\"", "Date9\": null");
            newString = newString.Replace("Date10\":\"\"", "Date10\": null");
            newString = newString.Replace("Date11\":\"\"", "Date11\": null");
            newString = newString.Replace("Date12\":\"\"", "Date12\": null");
            newString = newString.Replace("Date13\":\"\"", "Date13\": null");
            newString = newString.Replace("Date14\":\"\"", "Date14\": null");
            newString = newString.Replace("Date15\":\"\"", "Date15\": null");
            newString = newString.Replace("Date16\":\"\"", "Date16\": null");
            newString = newString.Replace("Date17\":\"\"", "Date17\": null");
            newString = newString.Replace("Date18\":\"\"", "Date18\": null");
            newString = newString.Replace("Date19\":\"\"", "Date19\": null");
            newString = newString.Replace("Date20\":\"\"", "Date20\": null");
            newString = newString.Replace("DatePickTicketPrinted\":\"\"", "DatePickTicketPrinted\": null");
            return newString;
        }

        #region Part
        public DataSet PartGetRows(string CompanyID, string Plant, string whereClausePart, string whereClausePartAttch, string whereClausePartCOO, string whereClausePartDim,
                                                                   string whereClausePartLangDesc, string whereClausePartPlanningPool, string whereClausePartPlant, string whereClausePartPlantPlanningAttribute,
                                                                   string whereClausePartRestriction, string whereClausePartRestrictSubst, string whereClausePartRev, string whereClausePartRevAttch,
                                                                   string whereClausePartAudit, string whereClausePartCOPart, string whereClausePartRevInspPlan, string whereClausePartRevInspPlanAttch,
                                                                   string whereClausePartRevInspVend, string whereClausePartSubs, string whereClausePartPC, string whereClausePartWhse,
                                                                   string whereClausePartBinInfo, string whereClausePartUOM, string whereClauseEntityGLC, string whereClauseTaxExempt,
                                                                   Int32 pageSize, Int32 absolutePage, out bool bError, out string ErrMsg)
        {
            bError = false;
            ErrMsg = string.Empty;
            DataSet dsQueryResult = new DataSet();
            try
            {
                if (!Initialiced)
                {
                    InitialiceConnection();
                }
                var options = new RestClientOptions(oRestAPIURL);
                options.Authenticator = new HttpBasicAuthenticator(uID, uAuthCode);
                RestClient client = new RestClient(options);
                RestRequest request = new RestRequest("Erp.BO.PartSvc/GetRows?whereClausePart=" + whereClausePart + "&whereClausePartAttch=" + whereClausePartAttch + "&whereClausePartCOO=" + whereClausePartCOO + "&whereClausePartDim=" + whereClausePartDim +
                                                                             "&whereClausePartLangDesc=" + whereClausePartLangDesc + "&whereClausePartPlanningPool=" + whereClausePartPlanningPool + "&whereClausePartPlant=" + whereClausePartPlant + "&whereClausePartPlantPlanningAttribute=" + whereClausePartPlantPlanningAttribute +
                                                                             "&whereClausePartRestriction=" + whereClausePartRestriction + "&whereClausePartRestrictSubst=" + whereClausePartRestrictSubst + "&whereClausePartRev=" + whereClausePartRev + "&whereClausePartRevAttch=" + whereClausePartRevAttch +
                                                                             "&whereClausePartAudit=" + whereClausePartAudit + "&whereClausePartCOPart=" + whereClausePartCOPart + "&whereClausePartRevInspPlan=" + whereClausePartRevInspPlan + "&whereClausePartRevInspPlanAttch=" + whereClausePartRevInspPlanAttch +
                                                                             "&whereClausePartRevInspVend=" + whereClausePartRevInspVend + "&whereClausePartSubs=" + whereClausePartSubs + "&whereClausePartPC=" + whereClausePartPC + "&whereClausePartWhse=" + whereClausePartWhse +
                                                                             "&whereClausePartBinInfo=" + whereClausePartBinInfo + "&whereClausePartUOM=" + whereClausePartUOM + "&whereClauseEntityGLC=" + whereClauseEntityGLC + "&whereClauseTaxExempt=" + whereClauseTaxExempt +
                                                                             "&pageSize=" + pageSize.ToString() + "&absolutePage=" + absolutePage.ToString() + "", Method.Get);
                request.AddHeader("CallSettings", "{ \"Company\":\"" + CompanyID + "\", \"Plant\":\"" + Plant + "\", \"TimeZoneOffset\" : \"\", \"Language\": \"\", \"FormatCulture\":\"en-US\"}");
                request.RequestFormat = DataFormat.Json;

                var response = client.Execute(request);
                dynamic dsResult = JsonConvert.DeserializeObject<dynamic>(response.Content);

                string jsResult = Convert.ToString(dsResult);
                if (jsResult.Contains("ErrorMessage"))
                {
                    int indexStr = jsResult.IndexOf("\"ErrorMessage");
                    int IndexStr2 = jsResult.IndexOf("\"ErrorType");
                    ErrMsg = jsResult.Substring(indexStr, IndexStr2 - indexStr - 5);
                    bError = true;
                    return dsQueryResult;
                }
                jsResult = jsResult.Replace("returnObj", "ds");

                int indexStrParam = jsResult.IndexOf("\"parameters") - 5;
                jsResult = jsResult.Substring(0, indexStrParam) + " \r\n}";

                var jsDataSet = JsonConvert.DeserializeXmlNode(jsResult);

                dsQueryResult = new DataSet();
                dsQueryResult.ReadXml(new XmlNodeReader(jsDataSet), XmlReadMode.Auto);
            }
            catch (Exception ex)
            {
                bError = true;
                ErrMsg = ex.Message.ToString();
            }

            return dsQueryResult;
        }

        public DataSet PartGetByID(string CompanyID, string Plant, string PartNum, out bool bError, out string ErrMsg)
        {
            bError = false;
            ErrMsg = string.Empty;
            DataSet dsQueryResult = new DataSet();
            try
            {
                if (!Initialiced)
                {
                    InitialiceConnection();
                }
                var options = new RestClientOptions(oRestAPIURL);
                options.Authenticator = new HttpBasicAuthenticator(uID, uAuthCode);
                RestClient client = new RestClient(options);
                RestRequest request = new RestRequest("Erp.BO.PartSvc/GetByID?partNum=" + PartNum + "", Method.Get);
                request.AddHeader("CallSettings", "{ \"Company\":\"" + CompanyID + "\", \"Plant\":\"" + Plant + "\", \"TimeZoneOffset\" : \"\", \"Language\": \"\", \"FormatCulture\":\"en-US\"}");
                request.RequestFormat = DataFormat.Json;

                var response = client.Execute(request);
                dynamic dsResult = JsonConvert.DeserializeObject<dynamic>(response.Content);

                string jsResult = Convert.ToString(dsResult);
                if (jsResult.Contains("ErrorMessage"))
                {
                    int indexStr = jsResult.IndexOf("\"ErrorMessage");
                    int IndexStr2 = jsResult.IndexOf("\"ErrorType");
                    ErrMsg = jsResult.Substring(indexStr, IndexStr2 - indexStr - 5);
                    bError = true;
                    return dsQueryResult;
                }
                jsResult = jsResult.Replace("returnObj", "ds");

                var jsDataSet = JsonConvert.DeserializeXmlNode(jsResult);

                dsQueryResult = new DataSet();
                dsQueryResult.ReadXml(new XmlNodeReader(jsDataSet), XmlReadMode.Auto);
            }
            catch (Exception ex)
            {
                bError = true;
                ErrMsg = ex.Message.ToString();
            }

            return dsQueryResult;
        }

        #endregion

        #region Jobs
        public DataSet JobGetList(string CompanyID, string Plant, string whereClause, Int32 pageSize, Int32 absolutePage, out bool bError, out string ErrMsg)
        {
            bError = false;
            ErrMsg = string.Empty;
            DataSet dsQueryResult = new DataSet();
            try
            {
                if (!Initialiced)
                {
                    InitialiceConnection();
                }
                var options = new RestClientOptions(oRestAPIURL);
                options.Authenticator = new HttpBasicAuthenticator(uID, uAuthCode);
                RestClient client = new RestClient(options);
                RestRequest request = new RestRequest("Erp.BO.JobEntrySvc/GetList?whereClause=" + whereClause + "&pageSize=" + pageSize.ToString() + "&absolutePage=" + absolutePage.ToString() + "", Method.Get);
                request.AddHeader("CallSettings", "{ \"Company\":\"" + CompanyID + "\", \"Plant\":\"" + Plant + "\", \"TimeZoneOffset\" : \"\", \"Language\": \"\", \"FormatCulture\":\"en-US\"}");
                request.RequestFormat = DataFormat.Json;

                var response = client.Execute(request);
                dynamic dsResult = JsonConvert.DeserializeObject<dynamic>(response.Content);

                string jsResult = Convert.ToString(dsResult);
                if (jsResult.Contains("ErrorMessage"))
                {
                    int indexStr = jsResult.IndexOf("\"ErrorMessage");
                    int IndexStr2 = jsResult.IndexOf("\"ErrorType");
                    ErrMsg = jsResult.Substring(indexStr, IndexStr2 - indexStr - 5);
                    bError = true;
                    return dsQueryResult;
                }
                jsResult = jsResult.Replace("returnObj", "ds");

                int indexStrParam = jsResult.IndexOf("\"parameters") - 5;
                jsResult = jsResult.Substring(0, indexStrParam) + " \r\n}";

                var jsDataSet = JsonConvert.DeserializeXmlNode(jsResult);

                dsQueryResult = new DataSet();
                dsQueryResult.ReadXml(new XmlNodeReader(jsDataSet), XmlReadMode.Auto);
            }
            catch (Exception ex)
            {
                bError = true;
                ErrMsg = ex.Message.ToString();
            }

            return dsQueryResult;
        }

        public DataSet JobEntryGetByID(string CompanyID, string Plant, string JobNum, out bool bError, out string ErrMsg)
        {
            bError = false;
            ErrMsg = string.Empty;
            DataSet dsQueryResult = new DataSet();
            try
            {
                if (!Initialiced)
                {
                    InitialiceConnection();
                }
                var options = new RestClientOptions(oRestAPIURL);
                options.Authenticator = new HttpBasicAuthenticator(uID, uAuthCode);
                RestClient client = new RestClient(options);
                RestRequest request = new RestRequest("Erp.BO.JobEntrySvc/GetByID?jobNum=" + JobNum + "", Method.Get);
                request.AddHeader("CallSettings", "{ \"Company\":\"" + CompanyID + "\", \"Plant\":\"" + Plant + "\", \"TimeZoneOffset\" : \"\", \"Language\": \"\", \"FormatCulture\":\"en-US\"}");
                request.RequestFormat = DataFormat.Json;

                var response = client.Execute(request);
                dynamic dsResult = JsonConvert.DeserializeObject<dynamic>(response.Content);

                string jsResult = Convert.ToString(dsResult);
                if (jsResult.Contains("ErrorMessage"))
                {
                    int indexStr = jsResult.IndexOf("\"ErrorMessage");
                    int IndexStr2 = jsResult.IndexOf("\"ErrorType");
                    ErrMsg = jsResult.Substring(indexStr, IndexStr2 - indexStr - 5);
                    bError = true;
                    return dsQueryResult;
                }
                jsResult = jsResult.Replace("returnObj", "ds");

                var jsDataSet = JsonConvert.DeserializeXmlNode(jsResult);

                dsQueryResult = new DataSet();
                dsQueryResult.ReadXml(new XmlNodeReader(jsDataSet), XmlReadMode.Auto);
            }
            catch (Exception ex)
            {
                bError = true;
                ErrMsg = ex.Message.ToString();
            }

            return dsQueryResult;
        }
        #endregion

        #region BOReader
        public DataSet BOReaderGetList(string CompanyID, string Plant, string serviceNamespace, string whereClause, string columnList, out bool bError, out string ErrMsg)
        {
            bError = false;
            ErrMsg = string.Empty;
            DataSet dsQueryResult = new DataSet();
            try
            {
                if (!Initialiced)
                {
                    InitialiceConnection();
                }
                var options = new RestClientOptions(oRestAPIURL);
                options.Authenticator = new HttpBasicAuthenticator(uID, uAuthCode);
                RestClient client = new RestClient(options);
                RestRequest request = new RestRequest("Ice.LIB.BOReaderSvc/GetList?serviceNamespace=" + serviceNamespace + "&whereClause=" + whereClause + "&columnList=" + columnList + "", Method.Get);
                request.AddHeader("CallSettings", "{ \"Company\":\"" + CompanyID + "\", \"Plant\":\"" + Plant + "\", \"TimeZoneOffset\" : \"\", \"Language\": \"\", \"FormatCulture\":\"en-US\"}");
                request.RequestFormat = DataFormat.Json;

                var response = client.Execute(request);
                dynamic dsResult = JsonConvert.DeserializeObject<dynamic>(response.Content);

                string jsResult = Convert.ToString(dsResult);
                if (jsResult.Contains("ErrorMessage"))
                {
                    int indexStr = jsResult.IndexOf("\"ErrorMessage");
                    int IndexStr2 = jsResult.IndexOf("\"ErrorType");
                    ErrMsg = jsResult.Substring(indexStr, IndexStr2 - indexStr - 5);
                    bError = true;
                    return dsQueryResult;
                }
                jsResult = jsResult.Replace("returnObj", "ds");

                //int indexStrParam = jsResult.IndexOf("\"parameters") - 5;
                //jsResult = jsResult.Substring(0, indexStrParam) + " \r\n}";

                var jsDataSet = JsonConvert.DeserializeXmlNode(jsResult);

                dsQueryResult = new DataSet();
                dsQueryResult.ReadXml(new XmlNodeReader(jsDataSet), XmlReadMode.Auto);
            }
            catch (Exception ex)
            {
                bError = true;
                ErrMsg = ex.Message.ToString();
            }

            return dsQueryResult;
        }
        #endregion

        #region Warehouse
        public DataSet WarehseGetList(string CompanyID, string Plant, string whereClause, Int32 pageSize, Int32 absolutePage, out bool bError, out string ErrMsg)
        {
            bError = false;
            ErrMsg = string.Empty;
            DataSet dsQueryResult = new DataSet();
            try
            {
                if (!Initialiced)
                {
                    InitialiceConnection();
                }
                var options = new RestClientOptions(oRestAPIURL);
                options.Authenticator = new HttpBasicAuthenticator(uID, uAuthCode);
                RestClient client = new RestClient(options);
                RestRequest request = new RestRequest("Erp.BO.WarehseSvc/GetList?whereClause=" + whereClause + "&pageSize=" + pageSize.ToString() + "&absolutePage=" + absolutePage.ToString() + "", Method.Get);
                request.AddHeader("CallSettings", "{ \"Company\":\"" + CompanyID + "\", \"Plant\":\"" + Plant + "\", \"TimeZoneOffset\" : \"\", \"Language\": \"\", \"FormatCulture\":\"en-US\"}");
                request.RequestFormat = DataFormat.Json;

                var response = client.Execute(request);
                dynamic dsResult = JsonConvert.DeserializeObject<dynamic>(response.Content);

                string jsResult = Convert.ToString(dsResult);
                if (jsResult.Contains("ErrorMessage"))
                {
                    int indexStr = jsResult.IndexOf("\"ErrorMessage");
                    int IndexStr2 = jsResult.IndexOf("\"ErrorType");
                    ErrMsg = jsResult.Substring(indexStr, IndexStr2 - indexStr - 5);
                    bError = true;
                    return dsQueryResult;
                }
                jsResult = jsResult.Replace("returnObj", "ds");

                int indexStrParam = jsResult.IndexOf("\"parameters") - 5;
                jsResult = jsResult.Substring(0, indexStrParam) + " \r\n}";

                var jsDataSet = JsonConvert.DeserializeXmlNode(jsResult);

                dsQueryResult = new DataSet();
                dsQueryResult.ReadXml(new XmlNodeReader(jsDataSet), XmlReadMode.Auto);
            }
            catch (Exception ex)
            {
                bError = true;
                ErrMsg = ex.Message.ToString();
            }

            return dsQueryResult;
        }
        public DataSet WhseBinGetList(string CompanyID, string Plant, string whereClause, Int32 pageSize, Int32 absolutePage, out bool bError, out string ErrMsg)
        {
            bError = false;
            ErrMsg = string.Empty;
            DataSet dsQueryResult = new DataSet();
            try
            {
                if (!Initialiced)
                {
                    InitialiceConnection();
                }
                var options = new RestClientOptions(oRestAPIURL);
                options.Authenticator = new HttpBasicAuthenticator(uID, uAuthCode);
                RestClient client = new RestClient(options);
                RestRequest request = new RestRequest("Erp.BO.WhseBinSvc/GetList?whereClause=" + whereClause + "&pageSize=" + pageSize.ToString() + "&absolutePage=" + absolutePage.ToString() + "", Method.Get);
                request.AddHeader("CallSettings", "{ \"Company\":\"" + CompanyID + "\", \"Plant\":\"" + Plant + "\", \"TimeZoneOffset\" : \"\", \"Language\": \"\", \"FormatCulture\":\"en-US\"}");
                request.RequestFormat = DataFormat.Json;

                var response = client.Execute(request);
                dynamic dsResult = JsonConvert.DeserializeObject<dynamic>(response.Content);

                string jsResult = Convert.ToString(dsResult);
                if (jsResult.Contains("ErrorMessage"))
                {
                    int indexStr = jsResult.IndexOf("\"ErrorMessage");
                    int IndexStr2 = jsResult.IndexOf("\"ErrorType");
                    ErrMsg = jsResult.Substring(indexStr, IndexStr2 - indexStr - 5);
                    bError = true;
                    return dsQueryResult;
                }
                jsResult = jsResult.Replace("returnObj", "ds");

                int indexStrParam = jsResult.IndexOf("\"parameters") - 5;
                jsResult = jsResult.Substring(0, indexStrParam) + " \r\n}";

                var jsDataSet = JsonConvert.DeserializeXmlNode(jsResult);

                dsQueryResult = new DataSet();
                dsQueryResult.ReadXml(new XmlNodeReader(jsDataSet), XmlReadMode.Auto);
            }
            catch (Exception ex)
            {
                bError = true;
                ErrMsg = ex.Message.ToString();
            }

            return dsQueryResult;
        }
        #endregion

        #region Reason
        public DataSet ReasonGetRows(string CompanyID, string Plant, string whereClauseReason, string whereClauseEntityGLC, Int32 pageSize, Int32 absolutePage, out bool bError, out string ErrMsg)
        {
            bError = false;
            ErrMsg = string.Empty;
            DataSet dsQueryResult = new DataSet();
            try
            {
                if (!Initialiced)
                {
                    InitialiceConnection();
                }
                var options = new RestClientOptions(oRestAPIURL);
                options.Authenticator = new HttpBasicAuthenticator(uID, uAuthCode);
                RestClient client = new RestClient(options);
                RestRequest request = new RestRequest("Erp.BO.ReasonSvc/GetRows?whereClauseReason=" + whereClauseReason + "&whereClauseEntityGLC=" + whereClauseEntityGLC + "&pageSize=" + pageSize.ToString() + "&absolutePage=" + absolutePage.ToString() + "", Method.Get);
                request.AddHeader("CallSettings", "{ \"Company\":\"" + CompanyID + "\", \"Plant\":\"" + Plant + "\", \"TimeZoneOffset\" : \"\", \"Language\": \"\", \"FormatCulture\":\"en-US\"}");
                request.RequestFormat = DataFormat.Json;

                var response = client.Execute(request);
                dynamic dsResult = JsonConvert.DeserializeObject<dynamic>(response.Content);

                string jsResult = Convert.ToString(dsResult);
                if (jsResult.Contains("ErrorMessage"))
                {
                    int indexStr = jsResult.IndexOf("\"ErrorMessage");
                    int IndexStr2 = jsResult.IndexOf("\"ErrorType");
                    ErrMsg = jsResult.Substring(indexStr, IndexStr2 - indexStr - 5);
                    bError = true;
                    return dsQueryResult;
                }
                jsResult = jsResult.Replace("returnObj", "ds");

                int indexStrParam = jsResult.IndexOf("\"parameters") - 5;
                jsResult = jsResult.Substring(0, indexStrParam) + " \r\n}";

                var jsDataSet = JsonConvert.DeserializeXmlNode(jsResult);

                dsQueryResult = new DataSet();
                dsQueryResult.ReadXml(new XmlNodeReader(jsDataSet), XmlReadMode.Auto);
            }
            catch (Exception ex)
            {
                bError = true;
                ErrMsg = ex.Message.ToString();
            }

            return dsQueryResult;
        }

        #endregion

        #region DownTime
        public DataSet IndirectGetRows(string CompanyID, string Plant, string whereClauseIndirect, Int32 pageSize, Int32 absolutePage, out bool bError, out string ErrMsg)
        {
            bError = false;
            ErrMsg = string.Empty;
            DataSet dsQueryResult = new DataSet();
            try
            {
                if (!Initialiced)
                {
                    InitialiceConnection();
                }
                var options = new RestClientOptions(oRestAPIURL);
                options.Authenticator = new HttpBasicAuthenticator(uID, uAuthCode);
                RestClient client = new RestClient(options);
                RestRequest request = new RestRequest("Erp.BO.IndirectSvc/GetRows?whereClauseIndirect=" + whereClauseIndirect + "&pageSize=" + pageSize.ToString() + "&absolutePage=" + absolutePage.ToString() + "", Method.Get);
                request.AddHeader("CallSettings", "{ \"Company\":\"" + CompanyID + "\", \"Plant\":\"" + Plant + "\", \"TimeZoneOffset\" : \"\", \"Language\": \"\", \"FormatCulture\":\"en-US\"}");
                request.RequestFormat = DataFormat.Json;

                var response = client.Execute(request);
                dynamic dsResult = JsonConvert.DeserializeObject<dynamic>(response.Content);

                string jsResult = Convert.ToString(dsResult);
                if (jsResult.Contains("ErrorMessage"))
                {
                    int indexStr = jsResult.IndexOf("\"ErrorMessage");
                    int IndexStr2 = jsResult.IndexOf("\"ErrorType");
                    ErrMsg = jsResult.Substring(indexStr, IndexStr2 - indexStr - 5);
                    bError = true;
                    return dsQueryResult;
                }
                jsResult = jsResult.Replace("returnObj", "ds");

                int indexStrParam = jsResult.IndexOf("\"parameters") - 5;
                jsResult = jsResult.Substring(0, indexStrParam) + " \r\n}";

                var jsDataSet = JsonConvert.DeserializeXmlNode(jsResult);

                dsQueryResult = new DataSet();
                dsQueryResult.ReadXml(new XmlNodeReader(jsDataSet), XmlReadMode.Auto);
            }
            catch (Exception ex)
            {
                bError = true;
                ErrMsg = ex.Message.ToString();
            }

            return dsQueryResult;
        }
        #endregion
    }
}