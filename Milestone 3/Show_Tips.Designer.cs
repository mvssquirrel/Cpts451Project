namespace Milestone_3
{
    partial class Show_Tips
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
            this.Show_Tips_Pop = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.Show_Tips_Pop)).BeginInit();
            this.SuspendLayout();
            // 
            // Show_Tips_Pop
            // 
            this.Show_Tips_Pop.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Show_Tips_Pop.Location = new System.Drawing.Point(12, 12);
            this.Show_Tips_Pop.Name = "Show_Tips_Pop";
            this.Show_Tips_Pop.Size = new System.Drawing.Size(730, 412);
            this.Show_Tips_Pop.TabIndex = 0;
            // 
            // Show_Tips
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 436);
            this.Controls.Add(this.Show_Tips_Pop);
            this.Name = "Show_Tips";
            this.Text = "Show_Tips";
            ((System.ComponentModel.ISupportInitialize)(this.Show_Tips_Pop)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView Show_Tips_Pop;
    }
}