namespace ModbusConnection
{
    partial class Settings
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
            this.buttonName = new System.Windows.Forms.Button();
            this.buttonAddress = new System.Windows.Forms.Button();
            this.buttonStatus = new System.Windows.Forms.Button();
            this.buttonTime = new System.Windows.Forms.Button();
            this.buttonIP = new System.Windows.Forms.Button();
            this.buttonType = new System.Windows.Forms.Button();
            this.buttonID = new System.Windows.Forms.Button();
            this.buttonDellete = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonStatusTime = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonName
            // 
            this.buttonName.Location = new System.Drawing.Point(12, 46);
            this.buttonName.Name = "buttonName";
            this.buttonName.Size = new System.Drawing.Size(258, 23);
            this.buttonName.TabIndex = 1;
            this.buttonName.Text = "Найменування";
            this.buttonName.UseVisualStyleBackColor = true;
            // 
            // buttonAddress
            // 
            this.buttonAddress.Location = new System.Drawing.Point(394, 46);
            this.buttonAddress.Name = "buttonAddress";
            this.buttonAddress.Size = new System.Drawing.Size(124, 23);
            this.buttonAddress.TabIndex = 1;
            this.buttonAddress.Text = "Адреса";
            this.buttonAddress.UseVisualStyleBackColor = true;
            // 
            // buttonStatus
            // 
            this.buttonStatus.Location = new System.Drawing.Point(642, 46);
            this.buttonStatus.Name = "buttonStatus";
            this.buttonStatus.Size = new System.Drawing.Size(124, 23);
            this.buttonStatus.TabIndex = 1;
            this.buttonStatus.Text = "Статусна адреса";
            this.buttonStatus.UseVisualStyleBackColor = true;
            this.buttonStatus.Click += new System.EventHandler(this.button4_Click);
            // 
            // buttonTime
            // 
            this.buttonTime.Location = new System.Drawing.Point(890, 46);
            this.buttonTime.Name = "buttonTime";
            this.buttonTime.Size = new System.Drawing.Size(124, 23);
            this.buttonTime.TabIndex = 1;
            this.buttonTime.Text = "Інтервал часу";
            this.buttonTime.UseVisualStyleBackColor = true;
            // 
            // buttonIP
            // 
            this.buttonIP.Location = new System.Drawing.Point(270, 46);
            this.buttonIP.Name = "buttonIP";
            this.buttonIP.Size = new System.Drawing.Size(124, 23);
            this.buttonIP.TabIndex = 1;
            this.buttonIP.Text = "IP сервера";
            this.buttonIP.UseVisualStyleBackColor = true;
            // 
            // buttonType
            // 
            this.buttonType.Location = new System.Drawing.Point(518, 46);
            this.buttonType.Name = "buttonType";
            this.buttonType.Size = new System.Drawing.Size(124, 23);
            this.buttonType.TabIndex = 1;
            this.buttonType.Text = "Тип";
            this.buttonType.UseVisualStyleBackColor = true;
            this.buttonType.Click += new System.EventHandler(this.button7_Click);
            // 
            // buttonID
            // 
            this.buttonID.Location = new System.Drawing.Point(1014, 46);
            this.buttonID.Name = "buttonID";
            this.buttonID.Size = new System.Drawing.Size(124, 23);
            this.buttonID.TabIndex = 1;
            this.buttonID.Text = "Идентификатор";
            this.buttonID.UseVisualStyleBackColor = true;
            // 
            // buttonDellete
            // 
            this.buttonDellete.BackColor = System.Drawing.SystemColors.Control;
            this.buttonDellete.BackgroundImage = global::ModbusConnection.Properties.Resources.Buttons_accept_and_delete;
            this.buttonDellete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonDellete.Location = new System.Drawing.Point(47, 12);
            this.buttonDellete.Name = "buttonDellete";
            this.buttonDellete.Size = new System.Drawing.Size(29, 28);
            this.buttonDellete.TabIndex = 0;
            this.buttonDellete.UseVisualStyleBackColor = false;
            this.buttonDellete.Click += new System.EventHandler(this.buttonDellete_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Control;
            this.button1.BackgroundImage = global::ModbusConnection.Properties.Resources.add1__2899;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(29, 28);
            this.button1.TabIndex = 0;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonStatusTime
            // 
            this.buttonStatusTime.Location = new System.Drawing.Point(766, 46);
            this.buttonStatusTime.Name = "buttonStatusTime";
            this.buttonStatusTime.Size = new System.Drawing.Size(124, 23);
            this.buttonStatusTime.TabIndex = 1;
            this.buttonStatusTime.Text = "Статусний час";
            this.buttonStatusTime.UseVisualStyleBackColor = true;
            this.buttonStatusTime.Click += new System.EventHandler(this.buttonStatusTime_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1166, 256);
            this.Controls.Add(this.buttonTime);
            this.Controls.Add(this.buttonStatusTime);
            this.Controls.Add(this.buttonStatus);
            this.Controls.Add(this.buttonIP);
            this.Controls.Add(this.buttonID);
            this.Controls.Add(this.buttonType);
            this.Controls.Add(this.buttonAddress);
            this.Controls.Add(this.buttonName);
            this.Controls.Add(this.buttonDellete);
            this.Controls.Add(this.button1);
            this.Name = "Form2";
            this.Text = "Options";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_FormClosing);
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonName;
        private System.Windows.Forms.Button buttonAddress;
        private System.Windows.Forms.Button buttonStatus;
        private System.Windows.Forms.Button buttonTime;
        private System.Windows.Forms.Button buttonIP;
        private System.Windows.Forms.Button buttonType;
        private System.Windows.Forms.Button buttonID;
        private System.Windows.Forms.Button buttonDellete;
        private System.Windows.Forms.Button buttonStatusTime;
    }
}