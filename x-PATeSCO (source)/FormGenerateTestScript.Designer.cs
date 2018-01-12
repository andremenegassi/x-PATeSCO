namespace CrossPlatformCompatibility
{
    partial class FormGenerateTestScript
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnGenerateProject = new System.Windows.Forms.Button();
            this.rbIndividualExp = new System.Windows.Forms.RadioButton();
            this.rbCombinedExpInOrder = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.cbCRef2 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbCRef1 = new System.Windows.Forms.ComboBox();
            this.rbCombinedExpMultiLocator = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbIndividualExp = new System.Windows.Forms.ComboBox();
            this.btnGenerateProjectEvaluation = new System.Windows.Forms.Button();
            this.ckOnlyClass = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(649, 222);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnGenerateProject
            // 
            this.btnGenerateProject.Location = new System.Drawing.Point(254, 222);
            this.btnGenerateProject.Name = "btnGenerateProject";
            this.btnGenerateProject.Size = new System.Drawing.Size(166, 23);
            this.btnGenerateProject.TabIndex = 58;
            this.btnGenerateProject.Text = "Generate Test Project";
            this.btnGenerateProject.UseVisualStyleBackColor = true;
            this.btnGenerateProject.Click += new System.EventHandler(this.btnGenerateProject_Click);
            // 
            // rbIndividualExp
            // 
            this.rbIndividualExp.AutoSize = true;
            this.rbIndividualExp.Checked = true;
            this.rbIndividualExp.Location = new System.Drawing.Point(21, 27);
            this.rbIndividualExp.Name = "rbIndividualExp";
            this.rbIndividualExp.Size = new System.Drawing.Size(127, 17);
            this.rbIndividualExp.TabIndex = 59;
            this.rbIndividualExp.TabStop = true;
            this.rbIndividualExp.Text = "Individual Expression:";
            this.rbIndividualExp.UseVisualStyleBackColor = true;
            // 
            // rbCombinedExpInOrder
            // 
            this.rbCombinedExpInOrder.AutoSize = true;
            this.rbCombinedExpInOrder.Location = new System.Drawing.Point(283, 27);
            this.rbCombinedExpInOrder.Name = "rbCombinedExpInOrder";
            this.rbCombinedExpInOrder.Size = new System.Drawing.Size(178, 17);
            this.rbCombinedExpInOrder.TabIndex = 60;
            this.rbCombinedExpInOrder.Text = "Combined Expressions - In Order";
            this.rbCombinedExpInOrder.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(157, 13);
            this.label5.TabIndex = 64;
            this.label5.Text = "Device Cref - Reference 2 (iOS)";
            // 
            // cbCRef2
            // 
            this.cbCRef2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCRef2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCRef2.FormattingEnabled = true;
            this.cbCRef2.Location = new System.Drawing.Point(15, 85);
            this.cbCRef2.Name = "cbCRef2";
            this.cbCRef2.Size = new System.Drawing.Size(710, 23);
            this.cbCRef2.TabIndex = 63;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(190, 13);
            this.label4.TabIndex = 62;
            this.label4.Text = "Device Cref - Reference 1 (ANDROID)";
            // 
            // cbCRef1
            // 
            this.cbCRef1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCRef1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCRef1.FormattingEnabled = true;
            this.cbCRef1.Location = new System.Drawing.Point(15, 29);
            this.cbCRef1.Name = "cbCRef1";
            this.cbCRef1.Size = new System.Drawing.Size(710, 23);
            this.cbCRef1.TabIndex = 61;
            // 
            // rbCombinedExpMultiLocator
            // 
            this.rbCombinedExpMultiLocator.AutoSize = true;
            this.rbCombinedExpMultiLocator.Location = new System.Drawing.Point(496, 27);
            this.rbCombinedExpMultiLocator.Name = "rbCombinedExpMultiLocator";
            this.rbCombinedExpMultiLocator.Size = new System.Drawing.Size(194, 17);
            this.rbCombinedExpMultiLocator.TabIndex = 66;
            this.rbCombinedExpMultiLocator.Text = "Combined Expressions - Multilocator";
            this.rbCombinedExpMultiLocator.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbIndividualExp);
            this.groupBox1.Controls.Add(this.rbCombinedExpInOrder);
            this.groupBox1.Controls.Add(this.rbCombinedExpMultiLocator);
            this.groupBox1.Controls.Add(this.rbIndividualExp);
            this.groupBox1.Location = new System.Drawing.Point(15, 124);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(710, 80);
            this.groupBox1.TabIndex = 67;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Locator Strategy";
            // 
            // cbIndividualExp
            // 
            this.cbIndividualExp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbIndividualExp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbIndividualExp.FormattingEnabled = true;
            this.cbIndividualExp.Location = new System.Drawing.Point(40, 47);
            this.cbIndividualExp.Name = "cbIndividualExp";
            this.cbIndividualExp.Size = new System.Drawing.Size(213, 23);
            this.cbIndividualExp.TabIndex = 67;
            // 
            // btnGenerateProjectEvaluation
            // 
            this.btnGenerateProjectEvaluation.Location = new System.Drawing.Point(426, 222);
            this.btnGenerateProjectEvaluation.Name = "btnGenerateProjectEvaluation";
            this.btnGenerateProjectEvaluation.Size = new System.Drawing.Size(217, 23);
            this.btnGenerateProjectEvaluation.TabIndex = 68;
            this.btnGenerateProjectEvaluation.Text = "Generate Test Project - For Evaluation Approach";
            this.btnGenerateProjectEvaluation.UseVisualStyleBackColor = true;
            this.btnGenerateProjectEvaluation.Click += new System.EventHandler(this.btnGenerateProjectEvaluation_Click);
            // 
            // ckOnlyClass
            // 
            this.ckOnlyClass.AutoSize = true;
            this.ckOnlyClass.Location = new System.Drawing.Point(15, 222);
            this.ckOnlyClass.Name = "ckOnlyClass";
            this.ckOnlyClass.Size = new System.Drawing.Size(147, 17);
            this.ckOnlyClass.TabIndex = 69;
            this.ckOnlyClass.Text = "Generate only Test Class ";
            this.ckOnlyClass.UseVisualStyleBackColor = true;
            this.ckOnlyClass.CheckedChanged += new System.EventHandler(this.ckOnlyClass_CheckedChanged);
            // 
            // FormGenerateTestScript
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(742, 260);
            this.Controls.Add(this.ckOnlyClass);
            this.Controls.Add(this.btnGenerateProjectEvaluation);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbCRef2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbCRef1);
            this.Controls.Add(this.btnGenerateProject);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormGenerateTestScript";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Generate Test Script";
            this.Shown += new System.EventHandler(this.FormGenerateTestScript_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnGenerateProject;
        private System.Windows.Forms.RadioButton rbIndividualExp;
        private System.Windows.Forms.RadioButton rbCombinedExpInOrder;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbCRef2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbCRef1;
        private System.Windows.Forms.RadioButton rbCombinedExpMultiLocator;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbIndividualExp;
        private System.Windows.Forms.Button btnGenerateProjectEvaluation;
        private System.Windows.Forms.CheckBox ckOnlyClass;
    }
}