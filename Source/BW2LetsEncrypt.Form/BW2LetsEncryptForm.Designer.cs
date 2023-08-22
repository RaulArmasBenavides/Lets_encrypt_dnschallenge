namespace BW2LetsEncrypt.FormApp
{
    partial class BW2LetsEncryptForm
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
            btn_CreateCertificate = new Button();
            txt_answer = new TextBox();
            btnContinue = new Button();
            progressBarCert = new ProgressBar();
            LblProcessStep = new Label();
            SuspendLayout();
            // 
            // btn_CreateCertificate
            // 
            btn_CreateCertificate.Enabled = false;
            btn_CreateCertificate.Location = new Point(30, 19);
            btn_CreateCertificate.Name = "btn_CreateCertificate";
            btn_CreateCertificate.Size = new Size(143, 23);
            btn_CreateCertificate.TabIndex = 0;
            btn_CreateCertificate.Text = "Create Certificate";
            btn_CreateCertificate.UseVisualStyleBackColor = true;
            btn_CreateCertificate.Click += btn_CreateCertificate_Click;
            // 
            // txt_answer
            // 
            txt_answer.Location = new Point(30, 93);
            txt_answer.Multiline = true;
            txt_answer.Name = "txt_answer";
            txt_answer.Size = new Size(961, 338);
            txt_answer.TabIndex = 1;
            // 
            // btnContinue
            // 
            btnContinue.Enabled = false;
            btnContinue.Location = new Point(30, 50);
            btnContinue.Name = "btnContinue";
            btnContinue.Size = new Size(143, 23);
            btnContinue.TabIndex = 2;
            btnContinue.Text = "Continue";
            btnContinue.UseVisualStyleBackColor = true;
            btnContinue.Click += btnContinue_Click;
            // 
            // progressBarCert
            // 
            progressBarCert.Location = new Point(196, 50);
            progressBarCert.Name = "progressBarCert";
            progressBarCert.Size = new Size(795, 23);
            progressBarCert.TabIndex = 3;
            // 
            // LblProcessStep
            // 
            LblProcessStep.AutoSize = true;
            LblProcessStep.Location = new Point(572, 23);
            LblProcessStep.Name = "LblProcessStep";
            LblProcessStep.Size = new Size(49, 15);
            LblProcessStep.TabIndex = 4;
            LblProcessStep.Text = "              ";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1025, 463);
            Controls.Add(LblProcessStep);
            Controls.Add(progressBarCert);
            Controls.Add(btnContinue);
            Controls.Add(txt_answer);
            Controls.Add(btn_CreateCertificate);
            Name = "Form1";
            Text = "BW2 Lets Encrypt";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btn_CreateCertificate;
        private TextBox txt_answer;
        private Button btnContinue;
        private ProgressBar progressBarCert;
        private Label LblProcessStep;
    }
}