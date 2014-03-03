using System;

using System.Windows.Forms;

namespace xk3yScanner
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
            LoadSettings();
        }

        public class Store
        {
            public string Text;
            public string Place;
            public override string ToString()
            {
                return Text;
            }
        }
        public void LoadSettings()
        {
            txtSelectGameFolder.Text = Properties.Settings.Default.ActiveFolder;
            txtSelectInactiveFolder.Text = Properties.Settings.Default.InActiveFolder;
            chkInactive.Checked = Properties.Settings.Default.InActiveFolders;
            comboFolderStructure.SelectedIndex = Properties.Settings.Default.FolderStructure;
            chkRename.Checked = Properties.Settings.Default.menuRename;
            txtSelectInactiveFolder.Enabled = chkInactive.Checked;
            butSelectInactiveFolder.Enabled = chkInactive.Checked;
            comboRegion.SelectedIndex = Properties.Settings.Default.DefaultRegion;
            textTemplate.Text = Properties.Settings.Default.txtTemplate;
            chkOverwriteImages.Checked = Properties.Settings.Default.chkOverwriteImages;
            comboStore.Items.Clear();
            Store s = new Store {Place = string.Empty, Text = "[Game Region Default Store]"};
            comboStore.Items.Add(s);
            int cnt = 1;
            int defs = 0;
            foreach (string str in Properties.Settings.Default.xboxStores)
            {
                string[] spl = str.Split('|');
                Store s1 = new Store {Place = spl[1], Text = spl[0]};
                comboStore.Items.Add(s1);
                if (spl[1] == Properties.Settings.Default.DefaultStore)
                    defs = cnt;
                cnt++;
            }
            comboStore.SelectedIndex = defs;
        }
        private void chkInactive_CheckedChanged(object sender, EventArgs e)
        {
            txtSelectInactiveFolder.Enabled = chkInactive.Checked;
            butSelectInactiveFolder.Enabled = chkInactive.Checked;
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            if (Program.Wizard)
                Application.Exit();
            else
                this.Close();
        }

        public bool VerifySettings(out string output,bool obj)
        {
            if (string.IsNullOrEmpty(txtSelectGameFolder.Text) || (!System.IO.Directory.Exists(txtSelectGameFolder.Text)))
            {
                output = "Active Game Directory is invalid";
                if (obj)
                    errorProvider.SetError(txtSelectGameFolder,output);   
                return false;
            }
            if (obj)
                errorProvider.SetError(txtSelectGameFolder, string.Empty);
            if (chkInactive.Checked)
            {
                if (string.IsNullOrEmpty(txtSelectInactiveFolder.Text) || (!System.IO.Directory.Exists(txtSelectInactiveFolder.Text)))
                {
                    output = "Inactive Game Directory is invalid";
                    if (obj)
                        errorProvider.SetError(txtSelectInactiveFolder, output);   
                    return false;
                }
            }
            if (!textTemplate.Text.Contains("{MediaId}"))
            {
                output = "{MediaId} is mandatory";
                if (obj)
                    errorProvider.SetError(textTemplate, output);
                return false;
            }
            errorProvider.SetError(textTemplate, string.Empty);
            if (obj)
                errorProvider.SetError(txtSelectInactiveFolder, string.Empty);
            output = string.Empty;
            return true;
        }

        private void butOk_Click(object sender, EventArgs e)
        {
            string output;
            bool res = VerifySettings(out output, true);
            if (res)
            {
                Properties.Settings.Default.ActiveFolder=txtSelectGameFolder.Text;
                Properties.Settings.Default.InActiveFolder=txtSelectInactiveFolder.Text;
                Properties.Settings.Default.InActiveFolders = chkInactive.Checked;
                Properties.Settings.Default.FolderStructure = comboFolderStructure.SelectedIndex;
                Properties.Settings.Default.menuRename = chkRename.Checked;
                Properties.Settings.Default.DefaultRegion = comboRegion.SelectedIndex;
                Properties.Settings.Default.DefaultStore = ((Store) comboStore.SelectedItem).Place;
                Properties.Settings.Default.chkOverwriteImages = chkOverwriteImages.Checked;
                Properties.Settings.Default.txtTemplate = textTemplate.Text;
                Properties.Settings.Default.Save();
                if (Program.Wizard)
                {
                    Program.Form.Enabled = true;
                    Program.Wizard = false;
                }
                if (Properties.Settings.Default.InActiveFolders)
                {
                    Program.Form.butGameActives.Enabled = true;
                    Program.Form.butGamesInactive.Enabled = true;
                }
                else
                {
                    Program.Form.butGameActives.Enabled = false;
                    Program.Form.butGamesInactive.Enabled = false;
                }
                Program.Form.Scan(false);
                this.Close();
                
            }
        }

        private void butSelectGameFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog f=new FolderBrowserDialog();
            if (!string.IsNullOrEmpty(txtSelectGameFolder.Text))
                f.SelectedPath = txtSelectGameFolder.Text;
            if (f.ShowDialog() == DialogResult.OK)
            {
                txtSelectGameFolder.Text = f.SelectedPath;
            }
        }

        private void butSelectInactiveFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog f = new FolderBrowserDialog();
            if (!string.IsNullOrEmpty(txtSelectInactiveFolder.Text))
                f.SelectedPath = txtSelectInactiveFolder.Text;
            if (f.ShowDialog() == DialogResult.OK)
            {
                txtSelectInactiveFolder.Text = f.SelectedPath;
            }
        }



    }
}
