namespace FormProject
{
    partial class MyForm
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
            ZoomIn = new Button();
            ZoomOut = new Button();
            DrawingField = new PictureBox();
            FunctionTextBox = new TextBox();
            ((System.ComponentModel.ISupportInitialize)DrawingField).BeginInit();
            SuspendLayout();
            // 
            // ZoomIn
            // 
            ZoomIn.Cursor = Cursors.Hand;
            ZoomIn.Location = new Point(628, 12);
            ZoomIn.Name = "ZoomIn";
            ZoomIn.Size = new Size(80, 80);
            ZoomIn.TabIndex = 1;
            ZoomIn.Text = "+";
            ZoomIn.UseVisualStyleBackColor = true;
            ZoomIn.Click += ZoomIn_Click;
            // 
            // ZoomOut
            // 
            ZoomOut.Cursor = Cursors.Hand;
            ZoomOut.Location = new Point(628, 98);
            ZoomOut.Name = "ZoomOut";
            ZoomOut.Size = new Size(80, 80);
            ZoomOut.TabIndex = 1;
            ZoomOut.Text = "-";
            ZoomOut.UseVisualStyleBackColor = true;
            ZoomOut.Click += ZoomOut_Click;
            // 
            // GraphDrawingField
            // 
            DrawingField.BackColor = SystemColors.Control;
            DrawingField.Location = new Point(12, 12);
            DrawingField.Name = "GraphDrawingField";
            DrawingField.Size = new Size(600, 600);
            DrawingField.TabIndex = 2;
            DrawingField.TabStop = false;
            DrawingField.Paint += GraphPanel_Paint;
            DrawingField.MouseDown += GraphDrawingField_MouseDown;
            DrawingField.MouseMove += GraphDrawingField_MouseMove;
            DrawingField.MouseUp += GraphDrawingField_MouseUp;
            // 
            // FunctionTextBox
            // 
            FunctionTextBox.Font = new Font("Segoe UI", 20F);
            FunctionTextBox.Location = new Point(628, 184);
            FunctionTextBox.Multiline = true;
            FunctionTextBox.Name = "FunctionTextBox";
            FunctionTextBox.Size = new Size(300, 428);
            FunctionTextBox.TabIndex = 3;
            FunctionTextBox.TextChanged += FunctionTextBox_TextChanged;
            // 
            // MyForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonShadow;
            ClientSize = new Size(940, 624);
            Controls.Add(FunctionTextBox);
            Controls.Add(DrawingField);
            Controls.Add(ZoomOut);
            Controls.Add(ZoomIn);
            Name = "MyForm";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)DrawingField).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button ZoomIn;
        private Button ZoomOut;
        private PictureBox DrawingField;
        private TextBox FunctionTextBox;
    }
}
