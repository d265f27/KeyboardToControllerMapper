using System.ComponentModel;
using System.Windows.Forms;

namespace KeyboardToControllerMapper
{
    partial class KeyboardToControllerMapper
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.controllerNum_lbl = new System.Windows.Forms.Label();
            this.controllerNum = new System.Windows.Forms.NumericUpDown();
            this.plugin = new System.Windows.Forms.Button();
            this.unplug = new System.Windows.Forms.Button();
            this.unplugAll = new System.Windows.Forms.Button();
            this.mapKeyButton = new System.Windows.Forms.Button();
            this.buttonListBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.easyFindKeyButton = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.removeMappingButton = new System.Windows.Forms.Button();
            this.resetMappingsButton = new System.Windows.Forms.Button();
            this.MappingDataGridView = new System.Windows.Forms.DataGridView();
            this.saveButton = new System.Windows.Forms.Button();
            this.loadButton = new System.Windows.Forms.Button();
            this.newKeyListBox = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.controllerNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MappingDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // controllerNum_lbl
            // 
            this.controllerNum_lbl.AutoSize = true;
            this.controllerNum_lbl.Location = new System.Drawing.Point(12, 17);
            this.controllerNum_lbl.Name = "controllerNum_lbl";
            this.controllerNum_lbl.Size = new System.Drawing.Size(94, 13);
            this.controllerNum_lbl.TabIndex = 0;
            this.controllerNum_lbl.Text = "Controller Number:";
            // 
            // controllerNum
            // 
            this.controllerNum.Location = new System.Drawing.Point(112, 13);
            this.controllerNum.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.controllerNum.Name = "controllerNum";
            this.controllerNum.Size = new System.Drawing.Size(42, 20);
            this.controllerNum.TabIndex = 1;
            this.controllerNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // plugin
            // 
            this.plugin.Location = new System.Drawing.Point(160, 12);
            this.plugin.Name = "plugin";
            this.plugin.Size = new System.Drawing.Size(75, 22);
            this.plugin.TabIndex = 2;
            this.plugin.Text = "Plug In";
            this.plugin.UseVisualStyleBackColor = true;
            this.plugin.Click += new System.EventHandler(this.plugin_Click);
            // 
            // unplug
            // 
            this.unplug.Location = new System.Drawing.Point(241, 12);
            this.unplug.Name = "unplug";
            this.unplug.Size = new System.Drawing.Size(75, 22);
            this.unplug.TabIndex = 3;
            this.unplug.Text = "Unplug";
            this.unplug.UseVisualStyleBackColor = true;
            this.unplug.Click += new System.EventHandler(this.unplug_Click);
            // 
            // unplugAll
            // 
            this.unplugAll.Location = new System.Drawing.Point(322, 12);
            this.unplugAll.Name = "unplugAll";
            this.unplugAll.Size = new System.Drawing.Size(92, 22);
            this.unplugAll.TabIndex = 4;
            this.unplugAll.Text = "Unplug All";
            this.unplugAll.UseVisualStyleBackColor = true;
            this.unplugAll.Click += new System.EventHandler(this.unplugAll_Click);
            // 
            // mapKeyButton
            // 
            this.mapKeyButton.Location = new System.Drawing.Point(347, 119);
            this.mapKeyButton.Margin = new System.Windows.Forms.Padding(2);
            this.mapKeyButton.Name = "mapKeyButton";
            this.mapKeyButton.Size = new System.Drawing.Size(68, 47);
            this.mapKeyButton.TabIndex = 11;
            this.mapKeyButton.Text = "Map Key";
            this.mapKeyButton.UseVisualStyleBackColor = true;
            this.mapKeyButton.Click += new System.EventHandler(this.mapKeyButtonClick);
            // 
            // buttonListBox
            // 
            this.buttonListBox.FormattingEnabled = true;
            this.buttonListBox.Items.AddRange(new object[] {
            "ButtonA",
            "ButtonB",
            "ButtonX",
            "ButtonY",
            "DpadLeft",
            "DpadRight",
            "DpadUp",
            "DpadDown",
            "BumperLeft",
            "BumperRight",
            "ButtonXbox",
            "ButtonStart",
            "ButtonBack",
            "LeftStickClick",
            "LeftStickLeft",
            "LeftStickRight",
            "LeftStickUp",
            "LeftStickDown",
            "RightStickClick",
            "RightStickLeft",
            "RightStickRight",
            "RightStickUp",
            "RightStickDown",
            "TriggerLeft",
            "TriggerRight"});
            this.buttonListBox.Location = new System.Drawing.Point(193, 76);
            this.buttonListBox.Margin = new System.Windows.Forms.Padding(2);
            this.buttonListBox.Name = "buttonListBox";
            this.buttonListBox.Size = new System.Drawing.Size(140, 121);
            this.buttonListBox.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(236, 43);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Controller";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(53, 43);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Keyboard";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(170, 136);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(19, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "-->";
            // 
            // easyFindKeyButton
            // 
            this.easyFindKeyButton.Location = new System.Drawing.Point(12, 202);
            this.easyFindKeyButton.Margin = new System.Windows.Forms.Padding(2);
            this.easyFindKeyButton.Name = "easyFindKeyButton";
            this.easyFindKeyButton.Size = new System.Drawing.Size(153, 42);
            this.easyFindKeyButton.TabIndex = 18;
            this.easyFindKeyButton.Text = "Find By Key Press";
            this.easyFindKeyButton.UseVisualStyleBackColor = true;
            this.easyFindKeyButton.Click += new System.EventHandler(this.easyFindKeyButtonClick);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(217, 260);
            this.statusLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(40, 13);
            this.statusLabel.TabIndex = 19;
            this.statusLabel.Text = "Status:";
            // 
            // removeMappingButton
            // 
            this.removeMappingButton.Location = new System.Drawing.Point(12, 433);
            this.removeMappingButton.Name = "removeMappingButton";
            this.removeMappingButton.Size = new System.Drawing.Size(184, 23);
            this.removeMappingButton.TabIndex = 21;
            this.removeMappingButton.Text = "Remove Mapping";
            this.removeMappingButton.UseVisualStyleBackColor = true;
            this.removeMappingButton.Click += new System.EventHandler(this.removeMappingButtonClick);
            // 
            // resetMappingsButton
            // 
            this.resetMappingsButton.Location = new System.Drawing.Point(12, 462);
            this.resetMappingsButton.Name = "resetMappingsButton";
            this.resetMappingsButton.Size = new System.Drawing.Size(184, 23);
            this.resetMappingsButton.TabIndex = 22;
            this.resetMappingsButton.Text = "Reset Mappings";
            this.resetMappingsButton.UseVisualStyleBackColor = true;
            this.resetMappingsButton.Click += new System.EventHandler(this.resetMappingsButtonClick);
            // 
            // MappingDataGridView
            // 
            this.MappingDataGridView.AllowUserToAddRows = false;
            this.MappingDataGridView.AllowUserToDeleteRows = false;
            this.MappingDataGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.MappingDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MappingDataGridView.GridColor = System.Drawing.SystemColors.ControlLightLight;
            this.MappingDataGridView.Location = new System.Drawing.Point(13, 260);
            this.MappingDataGridView.MultiSelect = false;
            this.MappingDataGridView.Name = "MappingDataGridView";
            this.MappingDataGridView.ReadOnly = true;
            this.MappingDataGridView.ShowEditingIcon = false;
            this.MappingDataGridView.Size = new System.Drawing.Size(184, 167);
            this.MappingDataGridView.TabIndex = 23;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(323, 433);
            this.saveButton.Margin = new System.Windows.Forms.Padding(2);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(92, 24);
            this.saveButton.TabIndex = 24;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButtonClick);
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(220, 433);
            this.loadButton.Margin = new System.Windows.Forms.Padding(2);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(92, 24);
            this.loadButton.TabIndex = 25;
            this.loadButton.Text = "Load";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButtonClick);
            // 
            // newKeyListBox
            // 
            this.newKeyListBox.FormattingEnabled = true;
            this.newKeyListBox.Location = new System.Drawing.Point(12, 76);
            this.newKeyListBox.Name = "newKeyListBox";
            this.newKeyListBox.Size = new System.Drawing.Size(153, 121);
            this.newKeyListBox.TabIndex = 26;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 56);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(157, 13);
            this.label4.TabIndex = 27;
            this.label4.Text = "Official Name (/ Locale Version)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(217, 467);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 28;
            this.label5.Text = "Version: 1.0";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // KeyboardToControllerMapper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 500);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.newKeyListBox);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.MappingDataGridView);
            this.Controls.Add(this.resetMappingsButton);
            this.Controls.Add(this.removeMappingButton);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.easyFindKeyButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonListBox);
            this.Controls.Add(this.mapKeyButton);
            this.Controls.Add(this.unplugAll);
            this.Controls.Add(this.unplug);
            this.Controls.Add(this.plugin);
            this.Controls.Add(this.controllerNum);
            this.Controls.Add(this.controllerNum_lbl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "KeyboardToControllerMapper";
            this.Text = "Keyboard to Controller Mapper";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.onClose);
            ((System.ComponentModel.ISupportInitialize)(this.controllerNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MappingDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label controllerNum_lbl;
        private NumericUpDown controllerNum;
        private Button plugin;
        private Button unplug;
        private Button unplugAll;
        private Button mapKeyButton;
        private ListBox buttonListBox;
        private Label label1;
        private Label label2;
        private Label label3;
        private Button easyFindKeyButton;
        private Label statusLabel;
        private Button removeMappingButton;
        private Button resetMappingsButton;
        private DataGridView MappingDataGridView;
        private Button saveButton;
        private Button loadButton;
        private ListBox newKeyListBox;
        private Label label4;
        private Label label5;
    }
}

