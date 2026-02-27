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
    /// <summary>
    /// A wizard-style dialog for configuring the options used when generating a C# data-access method
    /// that wraps a non-query stored procedure (INSERT / UPDATE / DELETE).
    /// Settings are persisted to <c>NonQuerySp.cfg</c> and reloaded on the next launch.
    /// On finish, the dialog returns <see cref="DialogResult.OK"/> and the caller reads
    /// <see cref="CurrentSettings"/> to drive code generation.
    /// </summary>
    public partial class NonQuerySp : Form
    {
        /// <summary>Gets the full path to the settings file used to persist and restore dialog state.</summary>
        public string SettingsFileName { get { return MainForm.DataStorageDir + "\\NonQuerySp.cfg"; } }

        /// <summary>
        /// Gets a <see cref="GenerateNonQuerySpModelSettings"/> snapshot that reflects the current state
        /// of all wizard checkboxes. Each call constructs and returns a new settings instance.
        /// </summary>
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
        /// <summary>Initializes a new instance of <see cref="NonQuerySp"/> and sets up the designer components.</summary>
        public NonQuerySp()
        {
            InitializeComponent();
        }

        /// <summary>Serializes the current dialog state to <see cref="SettingsFileName"/> as XML.</summary>
        private void SaveSettings()
        {
            GenerateNonQuerySpModelSettings curSettings = CurrentSettings;
            curSettings.SerializeToXmlFile(SettingsFileName);
        }
        /// <summary>
        /// Deserializes settings from <see cref="SettingsFileName"/>.
        /// If the file does not exist or deserialization fails, sensible defaults are applied
        /// and written to the file for future sessions.
        /// </summary>
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
