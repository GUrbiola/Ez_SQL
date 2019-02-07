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
    public partial class QuerySp : Form
    {
        public string SettingsFileName { get { return MainForm.DataStorageDir + "\\QuerySp.cfg"; } }
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
        public QuerySp()
        {
            InitializeComponent();
        }
        private void SaveSettings()
        {
            GenerateQuerySpModelSettings curSettings = CurrentSettings;
            curSettings.SerializeToXmlFile(SettingsFileName);
        }
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
            radList.Checked             = settings.IsList;
            //radOnlyOne.Checked          = !settings.IsList;
            radObject.Checked           = settings.IsObject;
            //radPrimitive.Checked        = !settings.IsObject;
            chkLogEnd.Checked           = settings.LogEnd;
            chkLogExc.Checked           = settings.LogException;
            chkLogStart.Checked         = settings.LogStart;
            chkTimeElapsed.Checked      = settings.MeasureTimeElapsed;
            chkSaveRowsAffected.Checked = settings.SaveRowsAffectedCount;
            chkSaveRowsRead.Checked     = settings.SaveRowsReadCount;
            chkUseTransaction.Checked   = settings.UseTransaction;
            if (settings.IsObject)
            {
                txtClassName.Text          = settings.ReturnName;
                cmbPrimitives.SelectedItem = "string";
            }
            else
            {
                txtClassName.Text          = "ClassX";
                cmbPrimitives.SelectedItem = settings.ReturnName;
            }


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
