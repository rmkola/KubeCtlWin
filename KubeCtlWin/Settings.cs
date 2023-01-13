using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KubeCtlWin
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            txtConfigFile.Text = Properties.Settings.Default.ConfigFile;
            txtPrefix.Text = Properties.Settings.Default.Prefix;
            foreach (var item in Properties.Settings.Default.Commands.Split('|'))
            {
                if (!string.IsNullOrEmpty(item))
                    lstCommands.Items.Add(item);
            }
        }

        private void btnVazgec_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ConfigFile = txtConfigFile.Text;
            Properties.Settings.Default.Prefix = txtPrefix.Text;
            string comma = "";
            for (int i = 0; i < lstCommands.Items.Count; i++)
            {
                comma += lstCommands.Items[i].ToString() + "|";
            }
            Properties.Settings.Default.Commands = comma;

            Properties.Settings.Default.Save();
            DialogResult = DialogResult.OK;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Yaml Dosyası(*.yaml)|*.yaml|Tüm Dosyalar(*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtConfigFile.Text = openFileDialog1.FileName;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (var item in lstCommands.Items)
                {
                    if (item != txtAdd.Text && !string.IsNullOrEmpty(txtAdd.Text))
                    {
                        lstCommands.Items.Add(txtAdd.Text);
                        break;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lstCommands.SelectedItems.Count > 0)
            {

                var cevap = MessageBox.Show("Bu komutu silmek istediğinizden emin misiniz?", "Dikkat!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (cevap == DialogResult.Yes)
                {
                    lstCommands.Items.Remove(lstCommands.SelectedItem.ToString());
                }
            }

        }
    }
}
