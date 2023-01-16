using System;
using System.Diagnostics;

namespace KubeCtlWin
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            ButtonDoldur();
            GetPods();
        }

        private void ButtonDoldur()
        {
            var list = Properties.Settings.Default.Commands.Split('|');
            if (cm.Items.Count > 0) { cm.Items.Clear(); }
            foreach (var cmd in list)
            {
                if (!string.IsNullOrEmpty(cmd))
                {
                    ToolStripMenuItem btn = new ToolStripMenuItem() { Text = cmd };
                    btn.Width = 300;
                    btn.Height = 80;
                    btn.Click += Button_click;
                    cm.Items.Add(btn);
                }
            }
        }

        private void Button_click(object? sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem)
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "wt";

                string command = Properties.Settings.Default.Prefix;
                var extraArguments = menuItem.Text;
                if (menuItem.Text.Contains("%1"))
                {

                    extraArguments = menuItem.Text.Replace("%1", cm.SourceControl.Text);
                    string arguments = $"{command} --kubeconfig={Properties.Settings.Default.ConfigFile} {extraArguments}";

                    psi.Arguments = arguments;
                    Process.Start(psi);

                }
                else if (menuItem.Text.Contains("%2"))
                {
                    ParameterForm f = new ParameterForm();
                    if (f.ShowDialog() == DialogResult.OK)
                    {

                        extraArguments = menuItem.Text.Replace("%2", f.txtExtraArgs.Text);
                        GetPods(extraArguments);

                    }
                    else return;
                }
                else
                    GetPods(menuItem.Text);

            }
        }

        private string GetPods(string args = "get pods")
        {

            Process process = new Process();
            string arguments = $"--kubeconfig={Properties.Settings.Default.ConfigFile} {args}";
            process.StartInfo.FileName = Properties.Settings.Default.Prefix;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = false;

            process.Start();
            //* Read the output (or the error)
            string output = process.StandardOutput.ReadToEnd();
            string[] entered = output.Split('\n');

            txtOutput.AppendText($"{Properties.Settings.Default.Prefix} {args}");
            txtOutput.AppendText(Environment.NewLine);

            foreach (var item in entered)
            {
                txtOutput.AppendText(item);
                txtOutput.AppendText(Environment.NewLine);

                if (args == "get pods")
                    if (!item.StartsWith("NAME") && !string.IsNullOrEmpty(item))
                    {
                        Button btn = new Button();
                        btn.ContextMenuStrip = cm;
                        btn.Width = 500;
                        btn.Height = 50;
                        btn.Text = item.Split(' ')[0];
                        flVM.Controls.Add(btn);
                    }
            }
            //txtOutput.Text = (output);

            process.WaitForExit();
            return output;
        }

        private void ayarlarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (new Settings().ShowDialog() == DialogResult.OK)
            {
                ButtonDoldur();
            }

        }

        private void güncelleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flVM.Controls.Clear();
            txtOutput.Clear();
            GetPods();
        }
    }
}