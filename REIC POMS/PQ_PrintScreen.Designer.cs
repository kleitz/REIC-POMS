﻿namespace REIC_POMS
{
    partial class PQ_PrintScreen
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
            this.CrystalReportViewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.PQ_Printout_IFOSupplier1 = new REIC_POMS.PQ_Printout_IFOSupplier();
            this.PQPrintout1 = new REIC_POMS.PQPrintout();
            this.SuspendLayout();
            // 
            // CrystalReportViewer
            // 
            this.CrystalReportViewer.ActiveViewIndex = -1;
            this.CrystalReportViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CrystalReportViewer.Cursor = System.Windows.Forms.Cursors.Default;
            this.CrystalReportViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CrystalReportViewer.Font = new System.Drawing.Font("Source Sans Pro", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CrystalReportViewer.Location = new System.Drawing.Point(0, 0);
            this.CrystalReportViewer.Name = "CrystalReportViewer";
            this.CrystalReportViewer.ShowGroupTreeButton = false;
            this.CrystalReportViewer.ShowLogo = false;
            this.CrystalReportViewer.ShowParameterPanelButton = false;
            this.CrystalReportViewer.Size = new System.Drawing.Size(701, 529);
            this.CrystalReportViewer.TabIndex = 0;
            this.CrystalReportViewer.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            // 
            // PQ_PrintScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(701, 529);
            this.Controls.Add(this.CrystalReportViewer);
            this.Font = new System.Drawing.Font("Source Sans Pro", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "PQ_PrintScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Print Preview";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.PQ_PrintScreen_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer CrystalReportViewer;
        private PQPrintout PQPrintout1;
        private PQ_Printout_IFOSupplier PQ_Printout_IFOSupplier1;
    }
}