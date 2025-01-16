namespace b9_SimPanel
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.TaxiLightsToggle = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.TaxiLightsToggle)).BeginInit();
            this.SuspendLayout();
            // 
            // TaxiLightsToggle
            // 
            this.TaxiLightsToggle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.TaxiLightsToggle.Image = global::b9_SimPanel.Properties.Resources.sw_br_down;
            this.TaxiLightsToggle.InitialImage = ((System.Drawing.Image)(resources.GetObject("TaxiLightsToggle.InitialImage")));
            this.TaxiLightsToggle.Location = new System.Drawing.Point(100, 100);
            this.TaxiLightsToggle.Margin = new System.Windows.Forms.Padding(0);
            this.TaxiLightsToggle.Name = "TaxiLightsToggle";
            this.TaxiLightsToggle.Size = new System.Drawing.Size(201, 201);
            this.TaxiLightsToggle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.TaxiLightsToggle.TabIndex = 0;
            this.TaxiLightsToggle.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(142, 320);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 37);
            this.label1.TabIndex = 1;
            this.label1.Text = "Taxi Light";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TaxiLightsToggle);
            this.Name = "MainForm";
            this.Text = "SimPanel Main";
            ((System.ComponentModel.ISupportInitialize)(this.TaxiLightsToggle)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox TaxiLightsToggle;
        private System.Windows.Forms.Label label1;
    }
}

