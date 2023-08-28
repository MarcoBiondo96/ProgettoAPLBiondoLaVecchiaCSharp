namespace ProgettoCsharp
{
    partial class Statistiche
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
            this.labelMatch = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelMatch
            // 
            this.labelMatch.AutoSize = true;
            this.labelMatch.Location = new System.Drawing.Point(12, 9);
            this.labelMatch.Name = "labelMatch";
            this.labelMatch.Size = new System.Drawing.Size(0, 20);
            this.labelMatch.TabIndex = 0;
            // 
            // Statistiche
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.labelMatch);
            this.Name = "Statistiche";
            this.Text = "Statistiche";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label labelMatch;
    }
}