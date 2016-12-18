namespace Modbus_2._0
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
            this.tbPathToSettingFile = new System.Windows.Forms.TextBox();
            this.btBrowse = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbPathToSettingFile
            // 
            this.tbPathToSettingFile.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.tbPathToSettingFile.Location = new System.Drawing.Point(13, 13);
            this.tbPathToSettingFile.Name = "tbPathToSettingFile";
            this.tbPathToSettingFile.Size = new System.Drawing.Size(304, 20);
            this.tbPathToSettingFile.TabIndex = 0;
            this.tbPathToSettingFile.Text = "Шлях до файлу налаштувань";
            // 
            // btBrowse
            // 
            this.btBrowse.Location = new System.Drawing.Point(324, 12);
            this.btBrowse.Name = "btBrowse";
            this.btBrowse.Size = new System.Drawing.Size(75, 23);
            this.btBrowse.TabIndex = 1;
            this.btBrowse.Text = "Огляд";
            this.btBrowse.UseVisualStyleBackColor = true;
            this.btBrowse.Click += new System.EventHandler(this.btBrowse_Click);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 303);
            this.Controls.Add(this.btBrowse);
            this.Controls.Add(this.tbPathToSettingFile);
            this.Name = "Settings";
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbPathToSettingFile;
        private System.Windows.Forms.Button btBrowse;
    }
}