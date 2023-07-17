﻿namespace KeyPointApp.Controls
{
    partial class ParamControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            distanceNumUpDown = new NumericUpDown();
            numericUpDown2 = new NumericUpDown();
            angleNumericUpDown = new NumericUpDown();
            vectorCheckBox = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)distanceNumUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)angleNumericUpDown).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(33, 9);
            label1.Name = "label1";
            label1.Size = new Size(105, 32);
            label1.TabIndex = 0;
            label1.Text = "Distance";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(3, 52);
            label2.Name = "label2";
            label2.Size = new Size(135, 32);
            label2.TabIndex = 1;
            label2.Text = "PointRange";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(62, 94);
            label3.Name = "label3";
            label3.Size = new Size(76, 32);
            label3.TabIndex = 2;
            label3.Text = "Angle";
            // 
            // distanceNumUpDown
            // 
            distanceNumUpDown.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            distanceNumUpDown.Increment = new decimal(new int[] { 50, 0, 0, 0 });
            distanceNumUpDown.Location = new Point(153, 8);
            distanceNumUpDown.Maximum = new decimal(new int[] { 5000, 0, 0, 0 });
            distanceNumUpDown.Name = "distanceNumUpDown";
            distanceNumUpDown.Size = new Size(116, 37);
            distanceNumUpDown.TabIndex = 3;
            distanceNumUpDown.Value = new decimal(new int[] { 1000, 0, 0, 0 });
            // 
            // numericUpDown2
            // 
            numericUpDown2.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            numericUpDown2.Location = new Point(153, 51);
            numericUpDown2.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            numericUpDown2.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Size = new Size(117, 37);
            numericUpDown2.TabIndex = 4;
            numericUpDown2.Value = new decimal(new int[] { 2, 0, 0, 0 });
            // 
            // angleNumericUpDown
            // 
            angleNumericUpDown.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            angleNumericUpDown.Location = new Point(151, 94);
            angleNumericUpDown.Maximum = new decimal(new int[] { 90, 0, 0, 0 });
            angleNumericUpDown.Name = "angleNumericUpDown";
            angleNumericUpDown.Size = new Size(118, 37);
            angleNumericUpDown.TabIndex = 5;
            angleNumericUpDown.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // vectorCheckBox
            // 
            vectorCheckBox.AutoSize = true;
            vectorCheckBox.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            vectorCheckBox.Location = new Point(285, 61);
            vectorCheckBox.Name = "vectorCheckBox";
            vectorCheckBox.Size = new Size(22, 21);
            vectorCheckBox.TabIndex = 7;
            vectorCheckBox.UseVisualStyleBackColor = true;
            // 
            // ParamControl
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlLight;
            Controls.Add(vectorCheckBox);
            Controls.Add(angleNumericUpDown);
            Controls.Add(numericUpDown2);
            Controls.Add(distanceNumUpDown);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "ParamControl";
            Size = new Size(310, 150);
            ((System.ComponentModel.ISupportInitialize)distanceNumUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).EndInit();
            ((System.ComponentModel.ISupportInitialize)angleNumericUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private NumericUpDown distanceNumUpDown;
        private NumericUpDown numericUpDown2;
        private NumericUpDown angleNumericUpDown;
        private CheckBox vectorCheckBox;
    }
}
