﻿using CrashEdit.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit
{
    public partial class ConfigEditor : UserControl
    {
        public static readonly List<string> Languages = new List<string> { "en", "ja" };

        public ConfigEditor()
        {
            InitializeComponent();
            foreach (string lang in Languages)
                dpdLang.Items.Add(Resources.ResourceManager.GetString("Language", new System.Globalization.CultureInfo(lang)));
            dpdLang.SelectedItem = Resources.ResourceManager.GetString("Language", new System.Globalization.CultureInfo(Settings.Default.Language));
            dpdLang.SelectedIndexChanged += new EventHandler(dpdLang_SelectedIndexChanged);
            numW.Value = Settings.Default.DefaultFormW;
            numH.Value = Settings.Default.DefaultFormH;
            chkNormalDisplay.Checked = Settings.Default.DisplayNormals;
            chkCollisionDisplay.Checked = Settings.Default.DisplayFrameCollision;
            chkUseAnimLinks.Checked = Settings.Default.UseAnimLinks;
            picClearCol.BackColor = Settings.Default.ClearColor;
            chkDeleteInvalidEntries.Checked = Settings.Default.DeleteInvalidEntries;
            chkAnimGrid.Checked = Settings.Default.DisplayAnimGrid;
            numAnimGrid.Value = Settings.Default.AnimGridLen;
            fraLang.Text = Resources.Config_FraLang;
            fraSize.Text = Resources.Config_fraSize;
            lblW.Text = Resources.Config_lblW;
            lblH.Text = Resources.Config_lblH;
            fraClearCol.Text = Resources.Config_fraClearCol;
            fraAnimGrid.Text = Resources.Config_fraAnimGrid;
            chkAnimGrid.Text = Resources.Config_chkAnimGrid;
            lblAnimGrid.Text = Resources.Config_lblAnimGrid;
            chkNormalDisplay.Text = Resources.Config_chkNormalDisplay;
            chkCollisionDisplay.Text = Resources.Config_chkCollisionDisplay;
            chkDeleteInvalidEntries.Text = Resources.Config_chkDeleteInvalidEntries;
            chkUseAnimLinks.Text = Resources.Config_chkUseAnimLinks;
            chkPatchNSDSavesNSF.Text = Resources.Config_chkPatchNSDSavesNSF;
            chkPatchNSDSavesNSF.Checked = Settings.Default.PatchNSDSavesNSF;
            chkOldPatchNSD.Text = Resources.Config_chkOldPatchNSD;
            chkOldPatchNSD.Checked = Settings.Default.OldPatchNSD;
        }

        private void dpdLang_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Default.Language = Languages[dpdLang.SelectedIndex];
            Settings.Default.Save();
        }

        private void cmdReset_Click(object sender, EventArgs e)
        {
            Settings.Default.Reset();
            ((OldMainForm)TopLevelControl).ResetConfig();
        }

        private void numW_ValueChanged(object sender, EventArgs e)
        {
            Settings.Default.DefaultFormW = (int)numW.Value;
            Settings.Default.Save();
        }

        private void numH_ValueChanged(object sender, EventArgs e)
        {
            Settings.Default.DefaultFormH = (int)numH.Value;
            Settings.Default.Save();
        }

        private void chkNormalDisplay_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.DisplayNormals = chkNormalDisplay.Checked;
            Settings.Default.Save();
        }

        private void chkCollisionDisplay_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.DisplayFrameCollision = chkCollisionDisplay.Checked;
            Settings.Default.Save();
        }

        private void chkUseAnimLinks_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.UseAnimLinks = chkUseAnimLinks.Checked;
            Settings.Default.Save();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (cdlClearCol.ShowDialog(this) == DialogResult.OK)
            {
                picClearCol.BackColor = Settings.Default.ClearColor = cdlClearCol.Color;
                Settings.Default.Save();
            }
        }

        private void cmdClearCol_EnabledChanged(object sender, EventArgs e)
        {
            if (cmdClearCol.Enabled)
            {
                cmdClearCol.BackColor = Color.Black;
            }
            else
            {
                cmdClearCol.BackColor = Color.White;
            }
        }

        private void cmdClearCol_Click(object sender, EventArgs e)
        {
            picClearCol.BackColor = Settings.Default.ClearColor = System.Drawing.Color.Black;
            Settings.Default.Save();
            cmdClearCol.Enabled = false;
            //背景色を灰色にしない
            cmdClearCol.BackColor = cmdClearCol.BackColor;
        }

        private void chkDeleteInvalidEntries_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.DeleteInvalidEntries = chkDeleteInvalidEntries.Checked;
            Settings.Default.Save();
        }

        private void chkAnimGrid_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.DisplayAnimGrid = chkAnimGrid.Checked;
            Settings.Default.Save();
        }

        private void numAnimGrid_ValueChanged(object sender, EventArgs e)
        {
            Settings.Default.AnimGridLen = (int)numAnimGrid.Value;
            Settings.Default.Save();
        }

        private void chkPatchNSDSavesNSF_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.PatchNSDSavesNSF = chkPatchNSDSavesNSF.Checked;
            Settings.Default.Save();
        }

        private void ChkOldPatchNSD_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.OldPatchNSD = chkOldPatchNSD.Checked;
            Settings.Default.Save();
        }
    }
}
