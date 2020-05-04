namespace Engine.NLP.Forms
{
    partial class NLPScottPlotForm
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
            this.Mainly_scottPlotUC = new ScottPlot.FormsPlot();
            this.SuspendLayout();
            // 
            // Mainly_scottPlotUC
            // 
            this.Mainly_scottPlotUC.BackColor = System.Drawing.Color.White;
            this.Mainly_scottPlotUC.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Mainly_scottPlotUC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Mainly_scottPlotUC.Location = new System.Drawing.Point(0, 0);
            this.Mainly_scottPlotUC.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Mainly_scottPlotUC.Name = "Mainly_scottPlotUC";
            this.Mainly_scottPlotUC.Size = new System.Drawing.Size(900, 540);
            this.Mainly_scottPlotUC.TabIndex = 0;
            // 
            // NLPScottPlotForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(900, 540);
            this.Controls.Add(this.Mainly_scottPlotUC);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "NLPScottPlotForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ScottPlotForm";
            this.ResumeLayout(false);
        }

        #endregion

        private  ScottPlot.FormsPlot Mainly_scottPlotUC;
    }
}