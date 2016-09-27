namespace TimeOfWork_manual
{
    partial class NewStatus
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
            this.btOK = new System.Windows.Forms.Button();
            this.TimeLabel = new System.Windows.Forms.Label();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.tbStatusTime = new System.Windows.Forms.TextBox();
            this.tbNewStatus = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btOK
            // 
            this.btOK.Location = new System.Drawing.Point(111, 38);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(35, 26);
            this.btOK.TabIndex = 7;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // TimeLabel
            // 
            this.TimeLabel.AutoSize = true;
            this.TimeLabel.Location = new System.Drawing.Point(64, 9);
            this.TimeLabel.Name = "TimeLabel";
            this.TimeLabel.Size = new System.Drawing.Size(27, 13);
            this.TimeLabel.TabIndex = 5;
            this.TimeLabel.Text = "Час";
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Location = new System.Drawing.Point(12, 9);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(41, 13);
            this.StatusLabel.TabIndex = 6;
            this.StatusLabel.Text = "Статус";
            // 
            // tbStatusTime
            // 
            this.tbStatusTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbStatusTime.Location = new System.Drawing.Point(64, 38);
            this.tbStatusTime.Name = "tbStatusTime";
            this.tbStatusTime.Size = new System.Drawing.Size(29, 29);
            this.tbStatusTime.TabIndex = 3;
            this.tbStatusTime.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbStatusTime_KeyPress);
            // 
            // tbNewStatus
            // 
            this.tbNewStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbNewStatus.Location = new System.Drawing.Point(15, 38);
            this.tbNewStatus.Name = "tbNewStatus";
            this.tbNewStatus.Size = new System.Drawing.Size(29, 29);
            this.tbNewStatus.TabIndex = 4;
            this.tbNewStatus.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbNewStatus_KeyPress);
            // 
            // NewStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(161, 86);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.TimeLabel);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.tbStatusTime);
            this.Controls.Add(this.tbNewStatus);
            this.Name = "NewStatus";
            this.Text = "Добавити статус";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NewStatus_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.NewStatus_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Label TimeLabel;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.TextBox tbStatusTime;
        private System.Windows.Forms.TextBox tbNewStatus;
    }
}