
namespace Projekt6_6
{
    partial class WelcomeForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.radioButtonZacinaHrac = new System.Windows.Forms.RadioButton();
            this.buttonExit = new System.Windows.Forms.Button();
            this.radioButtonZacinaPocitac = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(56, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(304, 28);
            this.label1.TabIndex = 0;
            this.label1.Text = "Zadej počet karet v paklu. Max:30";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(56, 75);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(150, 27);
            this.numericUpDown1.TabIndex = 1;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // radioButtonZacinaHrac
            // 
            this.radioButtonZacinaHrac.AutoSize = true;
            this.radioButtonZacinaHrac.Checked = true;
            this.radioButtonZacinaHrac.Location = new System.Drawing.Point(56, 157);
            this.radioButtonZacinaHrac.Name = "radioButtonZacinaHrac";
            this.radioButtonZacinaHrac.Size = new System.Drawing.Size(106, 24);
            this.radioButtonZacinaHrac.TabIndex = 2;
            this.radioButtonZacinaHrac.TabStop = true;
            this.radioButtonZacinaHrac.Text = "Začíná hráč";
            this.radioButtonZacinaHrac.UseVisualStyleBackColor = true;
            // 
            // buttonExit
            // 
            this.buttonExit.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.buttonExit.Location = new System.Drawing.Point(428, 184);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(173, 95);
            this.buttonExit.TabIndex = 3;
            this.buttonExit.Text = "Potvrdit";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // radioButtonZacinaPocitac
            // 
            this.radioButtonZacinaPocitac.AutoSize = true;
            this.radioButtonZacinaPocitac.Location = new System.Drawing.Point(56, 213);
            this.radioButtonZacinaPocitac.Name = "radioButtonZacinaPocitac";
            this.radioButtonZacinaPocitac.Size = new System.Drawing.Size(127, 24);
            this.radioButtonZacinaPocitac.TabIndex = 4;
            this.radioButtonZacinaPocitac.Text = "Začíná počítač";
            this.radioButtonZacinaPocitac.UseVisualStyleBackColor = true;
            // 
            // WelcomeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(625, 310);
            this.Controls.Add(this.radioButtonZacinaPocitac);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.radioButtonZacinaHrac);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label1);
            this.Name = "WelcomeForm";
            this.Text = "Začíná počítač";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.RadioButton radioButtonZacinaHrac;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.RadioButton radioButtonZacinaPocitac;
    }
}