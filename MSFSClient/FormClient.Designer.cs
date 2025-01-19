namespace MSFSClient
{
    partial class FormClient
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormClient));
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.button_Connect = new System.Windows.Forms.Button();
            this.txtResults = new System.Windows.Forms.TextBox();
            this.but_Lights_Taxi = new System.Windows.Forms.Button();
            this.pict_Light_Beacon = new System.Windows.Forms.PictureBox();
            this.label_Beacon = new System.Windows.Forms.Label();
            this.pict_Light_Landing = new System.Windows.Forms.PictureBox();
            this.label_Landing = new System.Windows.Forms.Label();
            this.pict_Lights_Taxi = new System.Windows.Forms.PictureBox();
            this.label_Taxi = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pict_Light_Beacon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pict_Light_Landing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pict_Lights_Taxi)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(102, 62);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(235, 31);
            this.textBoxIP.TabIndex = 0;
            this.textBoxIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(378, 62);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(100, 31);
            this.textBoxPort.TabIndex = 1;
            // 
            // button_Connect
            // 
            this.button_Connect.Location = new System.Drawing.Point(530, 31);
            this.button_Connect.Name = "button_Connect";
            this.button_Connect.Size = new System.Drawing.Size(146, 62);
            this.button_Connect.TabIndex = 2;
            this.button_Connect.Text = "Connect";
            this.button_Connect.UseVisualStyleBackColor = true;
            this.button_Connect.Click += new System.EventHandler(this.button_Connect_Click);
            // 
            // txtResults
            // 
            this.txtResults.Location = new System.Drawing.Point(102, 117);
            this.txtResults.Multiline = true;
            this.txtResults.Name = "txtResults";
            this.txtResults.Size = new System.Drawing.Size(574, 141);
            this.txtResults.TabIndex = 3;
            // 
            // but_Lights_Taxi
            // 
            this.but_Lights_Taxi.Location = new System.Drawing.Point(305, 544);
            this.but_Lights_Taxi.Name = "but_Lights_Taxi";
            this.but_Lights_Taxi.Size = new System.Drawing.Size(201, 68);
            this.but_Lights_Taxi.TabIndex = 5;
            this.but_Lights_Taxi.Text = "Lights Taxi";
            this.but_Lights_Taxi.UseVisualStyleBackColor = true;
            // 
            // pict_Light_Beacon
            // 
            this.pict_Light_Beacon.Image = ((System.Drawing.Image)(resources.GetObject("pict_Light_Beacon.Image")));
            this.pict_Light_Beacon.Location = new System.Drawing.Point(546, 290);
            this.pict_Light_Beacon.Name = "pict_Light_Beacon";
            this.pict_Light_Beacon.Size = new System.Drawing.Size(201, 201);
            this.pict_Light_Beacon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pict_Light_Beacon.TabIndex = 6;
            this.pict_Light_Beacon.TabStop = false;
            // 
            // label_Beacon
            // 
            this.label_Beacon.AutoSize = true;
            this.label_Beacon.Location = new System.Drawing.Point(602, 494);
            this.label_Beacon.Name = "label_Beacon";
            this.label_Beacon.Size = new System.Drawing.Size(85, 25);
            this.label_Beacon.TabIndex = 7;
            this.label_Beacon.Text = "Beacon";
            this.label_Beacon.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pict_Light_Landing
            // 
            this.pict_Light_Landing.Image = ((System.Drawing.Image)(resources.GetObject("pict_Light_Landing.Image")));
            this.pict_Light_Landing.Location = new System.Drawing.Point(62, 290);
            this.pict_Light_Landing.Name = "pict_Light_Landing";
            this.pict_Light_Landing.Size = new System.Drawing.Size(201, 201);
            this.pict_Light_Landing.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pict_Light_Landing.TabIndex = 6;
            this.pict_Light_Landing.TabStop = false;
            // 
            // label_Landing
            // 
            this.label_Landing.AutoSize = true;
            this.label_Landing.Location = new System.Drawing.Point(117, 494);
            this.label_Landing.Name = "label_Landing";
            this.label_Landing.Size = new System.Drawing.Size(89, 25);
            this.label_Landing.TabIndex = 7;
            this.label_Landing.Text = "Landing";
            this.label_Landing.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pict_Lights_Taxi
            // 
            this.pict_Lights_Taxi.Image = ((System.Drawing.Image)(resources.GetObject("pict_Lights_Taxi.Image")));
            this.pict_Lights_Taxi.Location = new System.Drawing.Point(305, 290);
            this.pict_Lights_Taxi.Name = "pict_Lights_Taxi";
            this.pict_Lights_Taxi.Size = new System.Drawing.Size(201, 201);
            this.pict_Lights_Taxi.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pict_Lights_Taxi.TabIndex = 6;
            this.pict_Lights_Taxi.TabStop = false;
            // 
            // label_Taxi
            // 
            this.label_Taxi.AutoSize = true;
            this.label_Taxi.Location = new System.Drawing.Point(378, 494);
            this.label_Taxi.Name = "label_Taxi";
            this.label_Taxi.Size = new System.Drawing.Size(53, 25);
            this.label_Taxi.TabIndex = 7;
            this.label_Taxi.Text = "Taxi";
            this.label_Taxi.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // FormClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 766);
            this.Controls.Add(this.label_Taxi);
            this.Controls.Add(this.label_Landing);
            this.Controls.Add(this.label_Beacon);
            this.Controls.Add(this.pict_Lights_Taxi);
            this.Controls.Add(this.pict_Light_Landing);
            this.Controls.Add(this.pict_Light_Beacon);
            this.Controls.Add(this.but_Lights_Taxi);
            this.Controls.Add(this.txtResults);
            this.Controls.Add(this.button_Connect);
            this.Controls.Add(this.textBoxPort);
            this.Controls.Add(this.textBoxIP);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormClient";
            this.Text = "Client Panel";
            ((System.ComponentModel.ISupportInitialize)(this.pict_Light_Beacon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pict_Light_Landing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pict_Lights_Taxi)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Button button_Connect;
        private System.Windows.Forms.TextBox txtResults;
        private System.Windows.Forms.Button but_Lights_Taxi;
        private System.Windows.Forms.PictureBox pict_Light_Beacon;
        private System.Windows.Forms.Label label_Beacon;
        private System.Windows.Forms.PictureBox pict_Light_Landing;
        private System.Windows.Forms.Label label_Landing;
        private System.Windows.Forms.PictureBox pict_Lights_Taxi;
        private System.Windows.Forms.Label label_Taxi;
    }
}

