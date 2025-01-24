using System;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using System.Threading;
using System.Data.SqlClient;
using MSFSClient;
using System.Drawing;

namespace MSFServer
{
    public partial class FormProxy : Form
    {
        private IniFile iniFile;
        private ConnectionManager connectionManager;
        private bool IsSimAllowed_check;
        private const int WM_USER_SIMCONNECT = 0x0402; // Custom window message for SimConnect

        Color clrBackDark = ColorTranslator.FromHtml("#1A1A1A");
        Color clrBackMedium = ColorTranslator.FromHtml("#212121");
        Color clrBackLight = ColorTranslator.FromHtml("#2B2B2B");
        Color clrBoxOutline = ColorTranslator.FromHtml("#8D8D8D");
        Color clrButtonGray = ColorTranslator.FromHtml("#424242");
        Color clrAccent = ColorTranslator.FromHtml("#569CD6");
        Color clrForeground = ColorTranslator.FromHtml("#FFFFFF");



        public FormProxy()
        {
            InitializeComponent();
            textBoxIP.Text = ConnectionManager.GetLocalIPv4();
            iniFile = new IniFile("Settings.ini");
            textBoxPort.Text = iniFile.Read("Settings", "Port");
            checkBox_Sim.Checked = iniFile.Read("Settings", "EnableSim") == "True";
            checkBox_DarkMode.Checked = iniFile.Read("Settings", "DarkTheme") == "True";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormProxy_FormClosing);
            DarkTheme();
            button_Serve.Click += new EventHandler(button_Serve_Click);
        }

        private void button_Serve_Click(object sender, EventArgs e)
        {
            if (connectionManager == null || !connectionManager.IsListening)
            {
                // Start the server
                connectionManager = new ConnectionManager(this.Handle, int.Parse(textBoxPort.Text)); // Pass the handle here
                connectionManager.IsSimAllowed = checkBox_Sim.Checked;
                connectionManager.Start();
                button_Serve.Text = "Stop";
            }
            else
            {
                // Stop the server
                connectionManager.Stop();
                button_Serve.Text = "Start";
            }
        }

        private void FormProxy_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Save settings on close
            iniFile.Write("Settings", "IPAddress", textBoxIP.Text);
            iniFile.Write("Settings", "Port", textBoxPort.Text);
            iniFile.Write("Settings", "EnableSim", checkBox_Sim.Checked.ToString());
            iniFile.Write("Settings", "DarkTheme", checkBox_DarkMode.Checked.ToString());
            Console.WriteLine("Settings.ini updated");
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_USER_SIMCONNECT)
            {
                if (connectionManager != null && connectionManager.IsListening) // Add IsListening check
                {
                    connectionManager.ReceiveMessage(m);
                }
            }
            base.WndProc(ref m);
        }

        public void DarkTheme()
        {
            if (checkBox_DarkMode.Checked == true)
            {
                this.BackColor = clrBackMedium;
                this.ForeColor = clrForeground;
                // Update other controls' colors
                foreach (Control control in this.Controls)
                {
                    if (control is Button button)
                    {
                        button.BackColor = clrButtonGray;
                        button.ForeColor = clrForeground;
                        button.FlatStyle = FlatStyle.Flat;
                        button.FlatAppearance.BorderColor = clrAccent;
                        button.FlatAppearance.BorderSize = 1;
                    }
                    else if (control is CheckBox checkBox)
                    {
                        checkBox.BackColor = clrBackDark;
                        checkBox.ForeColor = clrForeground;
                        checkBox.CheckedChanged += (sender, e) =>
                        {
                            checkBox.BackColor = clrBackDark; // Keep the background color consistent
                            checkBox.ForeColor = clrForeground; // Keep the text color consistent
                            checkBox.FlatAppearance.CheckedBackColor = checkBox.Checked ? clrAccent: clrButtonGray;
                        };
                    }
                    else if (control is TextBox textBox)
                    {
                        textBox.BackColor = clrButtonGray;
                        textBox.ForeColor = clrForeground;
                        textBox.BorderStyle = BorderStyle.FixedSingle;
                        textBox.Paint += (sender, e) =>
                        {
                            ControlPaint.DrawBorder(e.Graphics, textBox.ClientRectangle, clrBoxOutline, ButtonBorderStyle.Solid);
                        };
                        Size textSize = TextRenderer.MeasureText("Sample Text", textBox.Font);
                        Console.WriteLine(textBox.Top);

                        textBox.AutoSize = false;
                        int verticalAdjust = 4;
                        textBox.Height = textSize.Height + verticalAdjust; // Add some padding for better appearance
                        //textBox.Top += -(verticalAdjust/2); // Add some padding for better appearance
                    }
                    else { 
                        control.BackColor = Color.Orange;
                        control.ForeColor = clrAccent;
                    }
                }
            }
            else
            {
                this.BackColor = SystemColors.Control;
                this.ForeColor = SystemColors.ControlText;
                // Update other controls' colors
                foreach (Control control in this.Controls)
                {
                    control.BackColor = SystemColors.Control;
                    control.ForeColor = SystemColors.ControlText;
                }
            }
        }

        private void AdjustTextBoxHeight(TextBox textBox)
        {
            // Measure the text size
            Size textSize = TextRenderer.MeasureText("Sample Text", textBox.Font);

            // Set the height of the TextBox based on the text height
            textBox.Height = textSize.Height + 42; // Add some padding for better appearance
        }
    }
}