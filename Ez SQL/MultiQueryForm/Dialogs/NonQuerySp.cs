using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ez_SQL.Common_Code;

namespace Ez_SQL.MultiQueryForm.Dialogs
{
    public partial class NonQuerySp : Form
    {
        public string SettingsFileName { get { return MainForm.DataStorageDir + "\\NonQuerySp.cfg"; } }
        public GenerateNonQuerySpModelSettings CurrentSettings
        {
            get
            {
                return new GenerateNonQuerySpModelSettings()
                {
                    InsideRegion = chkRegion.Checked,
                    LogEnd = chkLogEnd.Checked,
                    LogException = chkLogExc.Checked,
                    LogStart = chkLogStart.Checked,
                    MeasureTimeElapsed = chkTimeElapsed.Checked,
                    SaveRowsAffectedCount = chkSaveRowsAffected.Checked,
                    SaveRowsReadCount = chkSaveRowsRead.Checked,
                    UseTransaction = chkUseTransaction.Checked,
                    ReturnSPR = chkReturnSPR.Checked
                };
            }
        }
        public NonQuerySp()
        {
            InitializeComponent();
        }

        private void SaveSettings()
        {
            GenerateNonQuerySpModelSettings curSettings = CurrentSettings;
            curSettings.SerializeToXmlFile(SettingsFileName);
        }
        private void LoadSettings()
        {
            GenerateNonQuerySpModelSettings settings = null;
            if (File.Exists(SettingsFileName))
            {
                settings = SettingsFileName.DeserializeFromXmlFile() as GenerateNonQuerySpModelSettings;
            }

            if (settings == null)
            {
                settings = new GenerateNonQuerySpModelSettings()
                {
                    InsideRegion = true,
                    LogEnd = false,
                    LogException = true,
                    LogStart = false,
                    MeasureTimeElapsed = true,
                    SaveRowsAffectedCount = true,
                    SaveRowsReadCount = false,
                    UseTransaction = false,
                    ReturnSPR = false
                };
                settings.SerializeToXmlFile(SettingsFileName);
            }

            chkRegion.Checked = settings.InsideRegion;
            chkLogEnd.Checked = settings.LogEnd;
            chkLogExc.Checked = settings.LogException;
            chkLogStart.Checked = settings.LogStart;
            chkTimeElapsed.Checked = settings.MeasureTimeElapsed;
            chkSaveRowsAffected.Checked = settings.SaveRowsAffectedCount;
            chkSaveRowsRead.Checked = settings.SaveRowsReadCount;
            chkUseTransaction.Checked = settings.UseTransaction;
            chkReturnSPR.Checked = settings.ReturnSPR;

        }

        private void wizardNonQuery_FinishClick(object sender, EventArgs e)
        {
            SaveSettings();
            DialogResult = DialogResult.OK;
        }

        private void wizardNonQuery_CancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void NonQuerySp_Load(object sender, EventArgs e)
        {
            LoadSettings();
        }
    }
}
