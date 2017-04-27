namespace Milestone_3
{
    partial class View_Checkins
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.CustomLabel customLabel1 = new System.Windows.Forms.DataVisualization.Charting.CustomLabel();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.NCheckGraph = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.NCheckGraph)).BeginInit();
            this.SuspendLayout();
            // 
            // NCheckGraph
            // 
            customLabel1.Text = "Monday";
            chartArea1.AxisX.CustomLabels.Add(customLabel1);
            chartArea1.Name = "ChartArea1";
            this.NCheckGraph.ChartAreas.Add(chartArea1);
            this.NCheckGraph.Location = new System.Drawing.Point(12, 12);
            this.NCheckGraph.Name = "NCheckGraph";
            this.NCheckGraph.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.SeaGreen;
            series1.ChartArea = "ChartArea1";
            series1.Name = "Series1";
            this.NCheckGraph.Series.Add(series1);
            this.NCheckGraph.Size = new System.Drawing.Size(652, 311);
            this.NCheckGraph.TabIndex = 0;
            this.NCheckGraph.Text = "Number of Checkins by Day of Week";
            // 
            // View_Checkins
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(790, 436);
            this.Controls.Add(this.NCheckGraph);
            this.Name = "View_Checkins";
            this.Text = "View_Checkins";
            ((System.ComponentModel.ISupportInitialize)(this.NCheckGraph)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart NCheckGraph;
    }
}