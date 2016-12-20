namespace Classifier
{
    partial class Grayscale_Parameters
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
            this.TBblackPointPercent = new System.Windows.Forms.TextBox();
            this.TBwhitePointPercent = new System.Windows.Forms.TextBox();
            this.LBblack = new System.Windows.Forms.Label();
            this.LBwhite = new System.Windows.Forms.Label();
            this.BTAccept = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TBblackPointPercent
            // 
            this.TBblackPointPercent.Location = new System.Drawing.Point(92, 12);
            this.TBblackPointPercent.Name = "TBblackPointPercent";
            this.TBblackPointPercent.Size = new System.Drawing.Size(100, 20);
            this.TBblackPointPercent.TabIndex = 0;
            // 
            // TBwhitePointPercent
            // 
            this.TBwhitePointPercent.Location = new System.Drawing.Point(92, 38);
            this.TBwhitePointPercent.Name = "TBwhitePointPercent";
            this.TBwhitePointPercent.Size = new System.Drawing.Size(100, 20);
            this.TBwhitePointPercent.TabIndex = 1;
            // 
            // LBblack
            // 
            this.LBblack.AutoSize = true;
            this.LBblack.Location = new System.Drawing.Point(1, 15);
            this.LBblack.Name = "LBblack";
            this.LBblack.Size = new System.Drawing.Size(85, 13);
            this.LBblack.TabIndex = 2;
            this.LBblack.Text = "Percent of black";
            // 
            // LBwhite
            // 
            this.LBwhite.AutoSize = true;
            this.LBwhite.Location = new System.Drawing.Point(1, 41);
            this.LBwhite.Name = "LBwhite";
            this.LBwhite.Size = new System.Drawing.Size(84, 13);
            this.LBwhite.TabIndex = 3;
            this.LBwhite.Text = "Percent of white";
            // 
            // BTAccept
            // 
            this.BTAccept.Location = new System.Drawing.Point(4, 70);
            this.BTAccept.Name = "BTAccept";
            this.BTAccept.Size = new System.Drawing.Size(188, 23);
            this.BTAccept.TabIndex = 4;
            this.BTAccept.Text = "Accept";
            this.BTAccept.UseVisualStyleBackColor = true;
            this.BTAccept.Click += new System.EventHandler(this.BTAccept_Click);
            // 
            // Grayscale_Parameters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(202, 105);
            this.Controls.Add(this.BTAccept);
            this.Controls.Add(this.LBwhite);
            this.Controls.Add(this.LBblack);
            this.Controls.Add(this.TBwhitePointPercent);
            this.Controls.Add(this.TBblackPointPercent);
            this.Name = "Grayscale_Parameters";
            this.Text = "Grayscale_Parameters";
            this.Load += new System.EventHandler(this.Grayscale_Parameters_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TBblackPointPercent;
        private System.Windows.Forms.TextBox TBwhitePointPercent;
        private System.Windows.Forms.Label LBblack;
        private System.Windows.Forms.Label LBwhite;
        private System.Windows.Forms.Button BTAccept;
    }
}