namespace CrossPlatformCompatibility
{
    partial class FormCompareStates
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
            this.btnCompare = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tbOutput = new System.Windows.Forms.TextBox();
            this.llOpenWebBrowser = new System.Windows.Forms.LinkLabel();
            this.ckTextSimilarity = new System.Windows.Forms.CheckBox();
            this.ckScreenshotSimilarity = new System.Windows.Forms.CheckBox();
            this.ckOCRSimilarity = new System.Windows.Forms.CheckBox();
            this.cbCRef1 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cbCRef2 = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(666, 201);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnCompare
            // 
            this.btnCompare.Location = new System.Drawing.Point(585, 201);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(75, 23);
            this.btnCompare.TabIndex = 4;
            this.btnCompare.Text = "Compare";
            this.btnCompare.UseVisualStyleBackColor = true;
            this.btnCompare.Click += new System.EventHandler(this.btnCompare_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 235);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(150, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Result (Open in Web Browser)";
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(15, 251);
            this.tbOutput.Name = "tbOutput";
            this.tbOutput.ReadOnly = true;
            this.tbOutput.Size = new System.Drawing.Size(430, 20);
            this.tbOutput.TabIndex = 6;
            // 
            // llOpenWebBrowser
            // 
            this.llOpenWebBrowser.AutoSize = true;
            this.llOpenWebBrowser.Location = new System.Drawing.Point(451, 254);
            this.llOpenWebBrowser.Name = "llOpenWebBrowser";
            this.llOpenWebBrowser.Size = new System.Drawing.Size(100, 13);
            this.llOpenWebBrowser.TabIndex = 7;
            this.llOpenWebBrowser.TabStop = true;
            this.llOpenWebBrowser.Text = "Open Web Browser";
            this.llOpenWebBrowser.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llOpenWebBrowser_LinkClicked);
            // 
            // ckTextSimilarity
            // 
            this.ckTextSimilarity.AutoSize = true;
            this.ckTextSimilarity.Location = new System.Drawing.Point(15, 24);
            this.ckTextSimilarity.Name = "ckTextSimilarity";
            this.ckTextSimilarity.Size = new System.Drawing.Size(90, 17);
            this.ckTextSimilarity.TabIndex = 8;
            this.ckTextSimilarity.Text = "Text Similarity";
            this.ckTextSimilarity.UseVisualStyleBackColor = true;
            // 
            // ckScreenshotSimilarity
            // 
            this.ckScreenshotSimilarity.AutoSize = true;
            this.ckScreenshotSimilarity.Location = new System.Drawing.Point(111, 25);
            this.ckScreenshotSimilarity.Name = "ckScreenshotSimilarity";
            this.ckScreenshotSimilarity.Size = new System.Drawing.Size(123, 17);
            this.ckScreenshotSimilarity.TabIndex = 9;
            this.ckScreenshotSimilarity.Text = "Screenshot Similarity";
            this.ckScreenshotSimilarity.UseVisualStyleBackColor = true;
            // 
            // ckOCRSimilarity
            // 
            this.ckOCRSimilarity.AutoSize = true;
            this.ckOCRSimilarity.Location = new System.Drawing.Point(240, 25);
            this.ckOCRSimilarity.Name = "ckOCRSimilarity";
            this.ckOCRSimilarity.Size = new System.Drawing.Size(92, 17);
            this.ckOCRSimilarity.TabIndex = 10;
            this.ckOCRSimilarity.Text = "OCR Similarity";
            this.ckOCRSimilarity.UseVisualStyleBackColor = true;
            // 
            // cbCRef1
            // 
            this.cbCRef1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCRef1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCRef1.FormattingEnabled = true;
            this.cbCRef1.Location = new System.Drawing.Point(15, 30);
            this.cbCRef1.Name = "cbCRef1";
            this.cbCRef1.Size = new System.Drawing.Size(726, 23);
            this.cbCRef1.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(190, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Device Cref - Reference 1 (ANDROID)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 70);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(157, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Device Cref - Reference 2 (iOS)";
            // 
            // cbCRef2
            // 
            this.cbCRef2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCRef2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCRef2.FormattingEnabled = true;
            this.cbCRef2.Location = new System.Drawing.Point(15, 86);
            this.cbCRef2.Name = "cbCRef2";
            this.cbCRef2.Size = new System.Drawing.Size(726, 23);
            this.cbCRef2.TabIndex = 14;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ckScreenshotSimilarity);
            this.groupBox1.Controls.Add(this.ckTextSimilarity);
            this.groupBox1.Controls.Add(this.ckOCRSimilarity);
            this.groupBox1.Location = new System.Drawing.Point(15, 125);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(726, 61);
            this.groupBox1.TabIndex = 68;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "\"Slow\" comparasion avaliable";
            // 
            // FormCompareStates
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(761, 291);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbCRef2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbCRef1);
            this.Controls.Add(this.llOpenWebBrowser);
            this.Controls.Add(this.tbOutput);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCompare);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCompareStates";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Compare States";
            this.Shown += new System.EventHandler(this.FormCompareStates_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbOutput;
        private System.Windows.Forms.LinkLabel llOpenWebBrowser;
        private System.Windows.Forms.CheckBox ckTextSimilarity;
        private System.Windows.Forms.CheckBox ckScreenshotSimilarity;
        private System.Windows.Forms.CheckBox ckOCRSimilarity;
        private System.Windows.Forms.ComboBox cbCRef1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbCRef2;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}