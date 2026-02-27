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
    /// that wraps a query stored procedure (SELECT).
    /// The user selects a return-type strategy — a single object, a <c>List&lt;T&gt;</c>, or a
    /// StoredProcedureResult (SPR) — and optionally specifies the class or primitive name.
    /// Settings are persisted to <c>QuerySp.cfg</c> and reloaded on the next launch.
    /// On finish, the caller reads <see cref="CurrentSettings"/> to drive code generation.
    /// </summary>
    public partial class QuerySp : Form
    {
        /// <summary>Gets the full path to the settings file used to persist and restore dialog state.</summary>
        public string SettingsFileName { get { return MainForm.DataStorageDir + "\\QuerySp.cfg"; } }

        /// <summary>
        /// Gets a <see cref="GenerateQuerySpModelSettings"/> snapshot that reflects the current state
        /// of all wizard controls. The <see cref="GenerateQuerySpModelSettings.ReturnName"/> is resolved
        /// from either the class-name text box or the primitives combo box, depending on the selected
        /// return-type radio button.
        /// </summary>
        public GenerateQuerySpModelSettings CurrentSettings
        {
            get
            {
                string freakingName;

                if (radObject.Checked)
                    freakingName = String.IsNullOrEmpty(txtClassName.Text) ? "ClassX" : txtClassName.Text;
                else
                    freakingName = cmbPrimitives.SelectedIndex >= 0 ? cmbPrimitives.SelectedItem.ToString() : "string";

                return new GenerateQuerySpModelSettings()
                    {
                        InsideRegion          = chkRegion.Checked,
                        IsList                = radList.Checked,
                        IsSPR                 = radSPR.Checked,
                        IsObject              = radObject.Checked,
                        LogEnd                = chkLogEnd.Checked,
                        LogException          = chkLogExc.Checked,
                        LogStart              = chkLogStart.Checked,
                        MeasureTimeElapsed    = chkTimeElapsed.Checked,
                        ReturnName            = freakingName,
                        SaveRowsAffectedCount = chkSaveRowsAffected.Checked,
                        SaveRowsReadCount     = chkSaveRowsRead.Checked,
                        UseTransaction        = chkUseTransaction.Checked
                    };
            }
        }
        /// <summary>Initializes a new instance of <see cref="QuerySp"/> and sets up the designer components.</summary>
        public QuerySp()
        {
            InitializeComponent();
        }

        /// <summary>Serializes the current dialog state to <see cref="SettingsFileName"/> as XML.</summary>
        private void SaveSettings()
        {
            GenerateQuerySpModelSettings curSettings = CurrentSettings;
            curSettings.SerializeToXmlFile(SettingsFileName);
        }
        /// <summary>
        /// Deserializes settings from <see cref="SettingsFileName"/> and applies them to the wizard controls.
        /// If the file does not exist or deserialization fails, sensible defaults are applied
        /// and written to the file for future sessions.
        /// </summary>
        private void LoadSettings()
        {
            GenerateQuerySpModelSettings settings = null;
            if (File.Exists(SettingsFileName))
            {
                settings = SettingsFileName.DeserializeFromXmlFile() as GenerateQuerySpModelSettings;
            }

            if (settings == null)
            {
                settings = new GenerateQuerySpModelSettings()
                {
                    InsideRegion          = true,
                    IsList                = false,
                    IsObject              = false,
                    IsSPR                 = false,
                    LogEnd                = false,
                    LogException          = true,
                    LogStart              = false,
                    MeasureTimeElapsed    = true,
                    ReturnName            = "string",
                    SaveRowsAffectedCount = false,
                    SaveRowsReadCount     = true,
                    UseTransaction        = false
                };
                settings.SerializeToXmlFile(SettingsFileName);
            }
            
            chkRegion.Checked           = settings.InsideRegion;
            if (settings.IsSPR)
            {
                radSPR.Checked = true;
                radList.Checked = false;
                radObject.Checked = false;

                txtClassName.Text = "ClassX";
                cmbPrimitives.SelectedItem = settings.ReturnName;

            }
            else if (settings.IsList)
            {
                radList.Checked = true;
                radSPR.Checked = false;
                radObject.Checked = false;

                txtClassName.Text = "ClassX";
                cmbPrimitives.SelectedItem = settings.ReturnName;
            }
            else
            {
                radObject.Checked = true;
                radSPR.Checked = false;
                radList.Checked = false;

                txtClassName.Text = settings.ReturnName;
                cmbPrimitives.SelectedItem = "string";
            }

            chkLogEnd.Checked           = settings.LogEnd;
            chkLogExc.Checked           = settings.LogException;
            chkLogStart.Checked         = settings.LogStart;
            chkTimeElapsed.Checked      = settings.MeasureTimeElapsed;
            chkSaveRowsAffected.Checked = settings.SaveRowsAffectedCount;
            chkSaveRowsRead.Checked     = settings.SaveRowsReadCount;
            chkUseTransaction.Checked   = settings.UseTransaction;
            
        }

        private void wizardControl1_FinishClick(object sender, EventArgs e)
        {
            SaveSettings();
            DialogResult = DialogResult.OK;
        }

        private void wizardControl1_CancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void QuerySp_Load(object sender, EventArgs e)
        {
            LoadSettings();
        }


    }
}
