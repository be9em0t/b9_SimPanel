namespace MSFServer
{
    partial class FormProxy
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProxy));
            this.button_Serve = new System.Windows.Forms.Button();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.but_Lights_Taxi = new System.Windows.Forms.Button();
            this.checkBox_Sim = new System.Windows.Forms.CheckBox();
            this.checkBox_DarkMode = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // button_Serve
            // 
            this.button_Serve.Location = new System.Drawing.Point(449, 30);
            this.button_Serve.Name = "button_Serve";
            this.button_Serve.Size = new System.Drawing.Size(168, 83);
            this.button_Serve.TabIndex = 0;
            this.button_Serve.Text = "Start";
            this.button_Serve.UseVisualStyleBackColor = true;
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(55, 56);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.ReadOnly = true;
            this.textBoxIP.Size = new System.Drawing.Size(213, 31);
            this.textBoxIP.TabIndex = 1;
            this.textBoxIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(302, 56);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(100, 31);
            this.textBoxPort.TabIndex = 2;
            // 
            // but_Lights_Taxi
            // 
            this.but_Lights_Taxi.Location = new System.Drawing.Point(224, 131);
            this.but_Lights_Taxi.Name = "but_Lights_Taxi";
            this.but_Lights_Taxi.Size = new System.Drawing.Size(178, 68);
            this.but_Lights_Taxi.TabIndex = 3;
            this.but_Lights_Taxi.Text = "Lights Taxi";
            this.but_Lights_Taxi.UseVisualStyleBackColor = true;
            this.but_Lights_Taxi.Visible = false;
            // 
            // checkBox_Sim
            // 
            this.checkBox_Sim.AutoSize = true;
            this.checkBox_Sim.Location = new System.Drawing.Point(449, 152);
            this.checkBox_Sim.Name = "checkBox_Sim";
            this.checkBox_Sim.Size = new System.Drawing.Size(160, 29);
            this.checkBox_Sim.TabIndex = 4;
            this.checkBox_Sim.Text = "SimConnect";
            this.checkBox_Sim.UseVisualStyleBackColor = true;
            // 
            // checkBox_DarkMode
            // 
            this.checkBox_DarkMode.AutoSize = true;
            this.checkBox_DarkMode.Location = new System.Drawing.Point(46, 152);
            this.checkBox_DarkMode.Name = "checkBox_DarkMode";
            this.checkBox_DarkMode.Size = new System.Drawing.Size(149, 29);
            this.checkBox_DarkMode.TabIndex = 4;
            this.checkBox_DarkMode.Text = "Dark Mode";
            this.checkBox_DarkMode.UseVisualStyleBackColor = true;
            // 
            // FormProxy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 246);
            this.Controls.Add(this.checkBox_DarkMode);
            this.Controls.Add(this.checkBox_Sim);
            this.Controls.Add(this.but_Lights_Taxi);
            this.Controls.Add(this.textBoxPort);
            this.Controls.Add(this.textBoxIP);
            this.Controls.Add(this.button_Serve);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormProxy";
            this.Text = "Form Proxy";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Serve;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Button but_Lights_Taxi;
        private System.Windows.Forms.CheckBox checkBox_Sim;
        private System.Windows.Forms.CheckBox checkBox_DarkMode;
    }
}

