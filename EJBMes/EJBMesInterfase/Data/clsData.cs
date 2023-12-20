using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EJBMesInterfase.Data
{
    internal class clsData
    {
        System.Data.SqlClient.SqlConnection conn;
        public clsData() 
        {
            conn = new System.Data.SqlClient.SqlConnection(global::EJBMesInterfase.Properties.Settings.Default.EJBConnection);
        }

        public List<ProdReport> GetProdJobReports()
        {
            string SQLQry = "Select Id,EmployeeNum,JobNum,AssemblyNum,OpSeq,StartDate,EndDate,ResourceGroup,ResourceId,isnull(ReferenceNotes,'') ReferenceNotes,LaborQty,ActiveLabor,Procesed, EM.Site from [dbo].[ProdReport] PR inner join [dbo].[UserMES] EM on PR.EmployeeNum = EM.EmployeeID where Procesed = 0 And ActiveLabor = 0";
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = SQLQry;
            cmd.CommandType = CommandType.Text;
            SqlDataReader reader = cmd.ExecuteReader();

            List<ProdReport> oList = new List<ProdReport>();
            while (reader.Read())
            {
                oList.Add(new ProdReport()
                {
                    Id = reader.GetInt32(0),
                    EmployeeNum = reader.GetString(1),
                    JobNum = reader.GetString(2),
                    AssemblyNum = reader.GetInt32(3),
                    OpSeq = reader.GetInt32(4),
                    StartDate = reader.GetDateTime(5),
                    EndDate = reader.GetDateTime(6),
                    ResourceGroup = reader.GetString(7),
                    ResourceId = reader.GetString(8),
                    ReferenceNotes = reader.GetString(9),
                    LaborQty = reader.GetDecimal(10),
                    ActiveLabor = reader.GetBoolean(11),
                    Procesed = reader.GetBoolean(12),
                    SiteID = reader.GetString(13)
                });
            }

            reader.Close();
            conn.Close();
            return oList;
        }

        public List<ScrapReport> GetScrapReports()
        {
            string SQLQry = "Select Id,EmployeeNum,JobNum,AssemblyNum,OpSeq,ScrapDate,ResourceGroup,ResourceId,isnull(ReferenceNotes,'') ReferenceNotes,ScrapQty,ReasonCode,WhseCode,BinNum,Procesed, EM.Site from [dbo].[ScrapReport] SR inner join [dbo].[UserMES] EM on SR.EmployeeNum = EM.EmployeeID where Procesed = 0";
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = SQLQry;
            cmd.CommandType = CommandType.Text;
            SqlDataReader reader = cmd.ExecuteReader();

            List<ScrapReport> oList = new List<ScrapReport>();
            while (reader.Read())
            {
                oList.Add(new ScrapReport()
                {
                    Id = reader.GetInt32(0),
                    EmployeeNum = reader.GetString(1),
                    JobNum = reader.GetString(2),
                    AssemblyNum = reader.GetInt32(3),
                    OpSeq = reader.GetInt32(4),
                    ScrapDate = reader.GetDateTime(5),                    
                    ResourceGroup = reader.GetString(6),
                    ResourceId = reader.GetString(7),
                    ReferenceNotes = reader.GetString(8),
                    ScrapQty = reader.GetDecimal(9),
                    ReasonCode = reader.GetString(10),
                    WhseCode = reader.GetString(11),
                    BinNum = reader.GetString(12),
                    Procesed = reader.GetBoolean(13),
                    SiteID = reader.GetString(14)
                });
            }

            reader.Close();
            conn.Close();
            return oList;
        }

        public List<DowntimeReport> GetDownTimeReports()
        {
            string SQLQry = "Select Id,EmployeeNum,JobNum,AssemblyNum,OpSeq,DownTimeStartDate,DownTimeEndDate,ResourceGroup,ResourceId,isnull(ReferenceNotes,'') ReferenceNotes,ReasonCode,Procesed,ActiveDowntime, EM.Site from [dbo].[DowntimeReport] DR inner join [dbo].[UserMES] EM on DR.EmployeeNum = EM.EmployeeID where Procesed = 0 and ActiveDowntime = 0";
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = SQLQry;
            cmd.CommandType = CommandType.Text;
            SqlDataReader reader = cmd.ExecuteReader();

            List<DowntimeReport> oList = new List<DowntimeReport>();
            while (reader.Read())
            {
                oList.Add(new DowntimeReport()
                {
                    Id = reader.GetInt32(0),
                    EmployeeNum = reader.GetString(1),
                    JobNum = reader.GetString(2),
                    AssemblyNum = reader.GetInt32(3),
                    OpSeq = reader.GetInt32(4),
                    DownTimeStartDate = reader.GetDateTime(5),
                    DownTimeEndDate = reader.GetDateTime(6),
                    ResourceGroup = reader.GetString(7),
                    ResourceId = reader.GetString(8),
                    ReferenceNotes = reader.GetString(9),
                    ReasonCode = reader.GetString(10),
                    Procesed = reader.GetBoolean(11),
                    ActiveDowntime = reader.GetBoolean(12),
                    SiteID = reader.GetString(13)
                });
            }

            reader.Close();
            conn.Close();
            return oList;
        }

        public string SaveRecordAsProcesed(ProdReport prodReport)
        {
            string MsgError = "";
            string SQLQuery = "Update ProdReport Set [Procesed] = @Procesed WHERE Id = @Id";
            try
            {                
                conn.Open();
                SqlCommand oComm = conn.CreateCommand();
                oComm.CommandText = SQLQuery;
                oComm.CommandType = CommandType.Text;
                SqlParameter oParam;
                oParam = oComm.Parameters.Add("@Procesed", SqlDbType.Bit);
                oParam.Value = prodReport.Procesed;
                oParam = oComm.Parameters.Add("@Id", SqlDbType.Int);
                oParam.Value = prodReport.Id;
                oComm.ExecuteNonQuery();
            } catch (Exception ex)
            {
                MsgError = ex.Message;
            }
            finally
            {
                conn.Close();
            }            
            return MsgError;
        }

        public string SaveRecordAsProcesed(ScrapReport prodReport)
        {
            string MsgError = "";
            string SQLQuery = "Update ScrapReport Set [Procesed] = @Procesed WHERE Id = @Id";
            try
            {
                conn.Open();
                SqlCommand oComm = conn.CreateCommand();
                oComm.CommandText = SQLQuery;
                oComm.CommandType = CommandType.Text;
                SqlParameter oParam;
                oParam = oComm.Parameters.Add("@Procesed", SqlDbType.Bit);
                oParam.Value = prodReport.Procesed;
                oParam = oComm.Parameters.Add("@Id", SqlDbType.Int);
                oParam.Value = prodReport.Id;
                oComm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MsgError = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return MsgError;
        }

        public string SaveRecordAsProcesed(DowntimeReport prodReport)
        {
            string MsgError = "";
            string SQLQuery = "Update DowntimeReport Set [Procesed] = @Procesed WHERE Id = @Id";
            try
            {
                conn.Open();
                SqlCommand oComm = conn.CreateCommand();
                oComm.CommandText = SQLQuery;
                oComm.CommandType = CommandType.Text;
                SqlParameter oParam;
                oParam = oComm.Parameters.Add("@Procesed", SqlDbType.Bit);
                oParam.Value = prodReport.Procesed;
                oParam = oComm.Parameters.Add("@Id", SqlDbType.Int);
                oParam.Value = prodReport.Id;
                oComm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MsgError = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return MsgError;
        }
    }
}
