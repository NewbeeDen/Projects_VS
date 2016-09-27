namespace TimeOfWork_manual
{
    partial class TimeOfWork
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.cbPlants = new System.Windows.Forms.ComboBox();
            this.cbElement = new System.Windows.Forms.ComboBox();
            this.WorkPanel = new System.Windows.Forms.Panel();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.Settings = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbPlants
            // 
            this.cbPlants.FormattingEnabled = true;
            this.cbPlants.Location = new System.Drawing.Point(12, 12);
            this.cbPlants.Name = "cbPlants";
            this.cbPlants.Size = new System.Drawing.Size(139, 21);
            this.cbPlants.TabIndex = 1;
            this.cbPlants.SelectedIndexChanged += new System.EventHandler(this.cbPlants_SelectedIndexChanged);
            // 
            // cbElement
            // 
            this.cbElement.FormattingEnabled = true;
            this.cbElement.Location = new System.Drawing.Point(157, 12);
            this.cbElement.Name = "cbElement";
            this.cbElement.Size = new System.Drawing.Size(446, 21);
            this.cbElement.TabIndex = 1;
            this.cbElement.SelectedIndexChanged += new System.EventHandler(this.cbElement_SelectedIndexChanged);
            this.cbElement.Enter += new System.EventHandler(this.cbElement_Enter);
            // 
            // WorkPanel
            // 
            this.WorkPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.WorkPanel.AutoScroll = true;
            this.WorkPanel.Location = new System.Drawing.Point(12, 39);
            this.WorkPanel.Name = "WorkPanel";
            this.WorkPanel.Size = new System.Drawing.Size(650, 222);
            this.WorkPanel.TabIndex = 3;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.BackgroundImage = global::TimeOfWork_manual.Properties.Resources.Button_Refresh;
            this.buttonRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonRefresh.Location = new System.Drawing.Point(605, 11);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(23, 23);
            this.buttonRefresh.TabIndex = 2;
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // Settings
            // 
            this.Settings.BackgroundImage = global::TimeOfWork_manual.Properties.Resources.settings_icon_14975;
            this.Settings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Settings.Location = new System.Drawing.Point(629, 11);
            this.Settings.Name = "Settings";
            this.Settings.Size = new System.Drawing.Size(23, 23);
            this.Settings.TabIndex = 2;
            this.Settings.UseVisualStyleBackColor = true;
            this.Settings.Click += new System.EventHandler(this.Settings_Click);
            // 
            // TimeOfWork
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 265);
            this.Controls.Add(this.WorkPanel);
            this.Controls.Add(this.buttonRefresh);
            this.Controls.Add(this.Settings);
            this.Controls.Add(this.cbElement);
            this.Controls.Add(this.cbPlants);
            this.MinimumSize = new System.Drawing.Size(560, 300);
            this.Name = "TimeOfWork";
            this.Text = "TimeOfWork";
            this.Activated += new System.EventHandler(this.TimeOfWork_Activated);
            this.Enter += new System.EventHandler(this.TimeOfWork_Enter);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbPlants;
        private System.Windows.Forms.ComboBox cbElement;
        private System.Windows.Forms.Button Settings;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.Panel WorkPanel;
    }
}

