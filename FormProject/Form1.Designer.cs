namespace FormProject
{
    partial class Form
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            GraphPanel = new Panel();
            SuspendLayout();
            // 
            // GraphPanel
            // 
            GraphPanel.BackColor = SystemColors.Control;
            GraphPanel.Location = new Point(12, 12);
            GraphPanel.Name = "GraphPanel";
            GraphPanel.Size = new Size(600, 600);
            GraphPanel.TabIndex = 0;
            GraphPanel.Paint += GraphPanel_Paint;
            // 
            // Form
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonShadow;
            ClientSize = new Size(1264, 681);
            Controls.Add(GraphPanel);
            Name = "Form";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Panel GraphPanel;
    }
}
