using EJBMesInterfase.Data;
using EJBMesInterfase.Kinetic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace EJBMesInterfase
{
    public partial class EJBInterfase: ServiceBase
    {
        bool _active = false;
        bool _canceled = false;
        private System.Timers.Timer timer1;
        private clsKinetic oKinetic;


        public EJBInterfase()
        {
            InitializeComponent();            
            oKinetic = new clsKinetic(global::EJBMesInterfase.Properties.Settings.Default.URLKinetic, global::EJBMesInterfase.Properties.Settings.Default.UserKinetic, global::EJBMesInterfase.Properties.Settings.Default.PwdKinetic, global::EJBMesInterfase.Properties.Settings.Default.CompanyID);
        }

        protected override void OnStart(string[] args)
        {
            _canceled = false;
            _active = false;
            timer1 = new System.Timers.Timer();
            timer1.Interval = global::EJBMesInterfase.Properties.Settings.Default.FrequencySec * 1000;
            this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(this.timer1_Tick);
            timer1.Enabled = true;
            timer1.Start();            
        }

        protected override void OnStop()
        {
            timer1.Stop();
            timer1.Enabled = false;
            while (_active)
            {
                _canceled = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!_active)
            {
                timer1.Stop();
                _active = true;
                try
                {
                    do
                    {
                        clsData oData = new clsData();
                        string MsgError = "";
                        foreach(ProdReport oRow in oData.GetProdJobReports())
                        {
                            if(_canceled)
                            {
                                break;
                            }
                            //Send to Kinetic                            
                            if (oKinetic.CreateLabor(oRow, out MsgError))
                            {
                                //Procesed
                                oRow.Procesed = true;
                                MsgError = oData.SaveRecordAsProcesed(oRow);
                                if (MsgError != "")
                                {
                                    this.EventLog.WriteEntry(MsgError, EventLogEntryType.Error);
                                }
                            }
                            else
                            {
                                if (MsgError != "")
                                {
                                    this.EventLog.WriteEntry(MsgError, EventLogEntryType.Error);
                                }
                            }
                        }

                        foreach (ScrapReport oRow in oData.GetScrapReports())
                        {
                            if (_canceled)
                            {
                                break;
                            }
                            //Send to Kinetic
                            if (oKinetic.CreateScrap(oRow, out MsgError))
                            {
                                //Procesed
                                oRow.Procesed = true;
                                MsgError = oData.SaveRecordAsProcesed(oRow);
                                if (MsgError != "")
                                {
                                    this.EventLog.WriteEntry(MsgError, EventLogEntryType.Error);
                                }
                            }
                            else
                            {
                                if (MsgError != "")
                                {
                                    this.EventLog.WriteEntry(MsgError, EventLogEntryType.Error);
                                }
                            }
                        }

                        foreach (DowntimeReport oRow in oData.GetDownTimeReports())
                        {
                            if (_canceled)
                            {
                                break;
                            }
                            //Send to Kinetic
                            if (oKinetic.CreateDownTime(oRow, out MsgError))
                            {
                                //Procesed
                                oRow.Procesed = true;
                                MsgError = oData.SaveRecordAsProcesed(oRow);
                                if (MsgError != "")
                                {
                                    this.EventLog.WriteEntry(MsgError, EventLogEntryType.Error);
                                }
                            }
                            else
                            {
                                if (MsgError != "")
                                {
                                    this.EventLog.WriteEntry(MsgError, EventLogEntryType.Error);
                                }
                            }
                        }


                        _active = false;
                    } while (!_canceled && _active);
                    timer1.Start();
                } 
                catch (Exception ex) {
                    _active = false;
                    this.EventLog.WriteEntry(ex.Message, EventLogEntryType.Error);
                }
                
            }
        }

    }
}
