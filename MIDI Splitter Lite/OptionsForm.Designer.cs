namespace MIDI_Splitter_Lite
{
    partial class OptionsForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            this.CopyFirstTrackBox = new System.Windows.Forms.CheckBox();
            this.ReadTrackNamesBox = new System.Windows.Forms.CheckBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.FilePrefixBox = new System.Windows.Forms.CheckBox();
            this.ReadTrackInstrumentBox = new System.Windows.Forms.CheckBox();
            this.RemoveTracksBox = new System.Windows.Forms.CheckBox();
            this.ExportSubBox = new System.Windows.Forms.CheckBox();
            this.colorTextBox4 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.colorTextBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.colorTextBox7 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.colorTextBox2 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.colorTextBox5 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.colorTextBox3 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.colorTextBox6 = new System.Windows.Forms.TextBox();
            this.colorPicker1 = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.colorPicker2 = new System.Windows.Forms.Button();
            this.colorPicker3 = new System.Windows.Forms.Button();
            this.colorPicker4 = new System.Windows.Forms.Button();
            this.colorPicker5 = new System.Windows.Forms.Button();
            this.colorPicker6 = new System.Windows.Forms.Button();
            this.colorPicker7 = new System.Windows.Forms.Button();
            this.MinBytesTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.ManualColorBox = new System.Windows.Forms.CheckBox();
            this.AutoColorBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // CopyFirstTrackBox
            // 
            this.CopyFirstTrackBox.AutoSize = true;
            this.CopyFirstTrackBox.Location = new System.Drawing.Point(12, 12);
            this.CopyFirstTrackBox.Name = "CopyFirstTrackBox";
            this.CopyFirstTrackBox.Size = new System.Drawing.Size(173, 17);
            this.CopyFirstTrackBox.TabIndex = 0;
            this.CopyFirstTrackBox.Text = "Copy first track to output tracks";
            this.toolTip.SetToolTip(this.CopyFirstTrackBox, "Copies the first track in the list to the selected tracks to be exported.\r\nExport" +
        "ed MIDIs will contain two tracks.");
            this.CopyFirstTrackBox.UseVisualStyleBackColor = true;
            // 
            // ReadTrackNamesBox
            // 
            this.ReadTrackNamesBox.AutoSize = true;
            this.ReadTrackNamesBox.Location = new System.Drawing.Point(12, 35);
            this.ReadTrackNamesBox.Name = "ReadTrackNamesBox";
            this.ReadTrackNamesBox.Size = new System.Drawing.Size(113, 17);
            this.ReadTrackNamesBox.TabIndex = 1;
            this.ReadTrackNamesBox.Text = "Read track names";
            this.toolTip.SetToolTip(this.ReadTrackNamesBox, "Reads the name of each track from the MIDI file and displays it in the list.\r\nIf " +
        "multiple names exists for a given track, the latest one will be shown.");
            this.ReadTrackNamesBox.UseVisualStyleBackColor = true;
            this.ReadTrackNamesBox.CheckedChanged += new System.EventHandler(this.ReadTrackNamesBox_CheckedChanged);
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 32767;
            this.toolTip.InitialDelay = 500;
            this.toolTip.ReshowDelay = 100;
            // 
            // FilePrefixBox
            // 
            this.FilePrefixBox.AutoSize = true;
            this.FilePrefixBox.Location = new System.Drawing.Point(12, 81);
            this.FilePrefixBox.Name = "FilePrefixBox";
            this.FilePrefixBox.Size = new System.Drawing.Size(132, 17);
            this.FilePrefixBox.TabIndex = 2;
            this.FilePrefixBox.Text = "Use file name as prefix";
            this.toolTip.SetToolTip(this.FilePrefixBox, "Reads the name of each track from the MIDI file and displays it in the list.\r\nIf " +
        "multiple names exists for a given track, the latest one will be shown.");
            this.FilePrefixBox.UseVisualStyleBackColor = true;
            // 
            // ReadTrackInstrumentBox
            // 
            this.ReadTrackInstrumentBox.AutoSize = true;
            this.ReadTrackInstrumentBox.Location = new System.Drawing.Point(12, 58);
            this.ReadTrackInstrumentBox.Name = "ReadTrackInstrumentBox";
            this.ReadTrackInstrumentBox.Size = new System.Drawing.Size(135, 17);
            this.ReadTrackInstrumentBox.TabIndex = 24;
            this.ReadTrackInstrumentBox.Text = "Read track instruments";
            this.toolTip.SetToolTip(this.ReadTrackInstrumentBox, "Reads the name of each track from the MIDI file and displays it in the list.\r\nIf " +
        "multiple names exists for a given track, the latest one will be shown.");
            this.ReadTrackInstrumentBox.UseVisualStyleBackColor = true;
            this.ReadTrackInstrumentBox.CheckedChanged += new System.EventHandler(this.ReadTrackInstrumentBox_CheckedChanged);
            // 
            // RemoveTracksBox
            // 
            this.RemoveTracksBox.AutoSize = true;
            this.RemoveTracksBox.Location = new System.Drawing.Point(12, 190);
            this.RemoveTracksBox.Name = "RemoveTracksBox";
            this.RemoveTracksBox.Size = new System.Drawing.Size(15, 14);
            this.RemoveTracksBox.TabIndex = 25;
            this.toolTip.SetToolTip(this.RemoveTracksBox, "Removes tracks that are under x bytes, default is 105.");
            this.RemoveTracksBox.UseVisualStyleBackColor = true;
            // 
            // ExportSubBox
            // 
            this.ExportSubBox.AutoSize = true;
            this.ExportSubBox.Location = new System.Drawing.Point(12, 104);
            this.ExportSubBox.Name = "ExportSubBox";
            this.ExportSubBox.Size = new System.Drawing.Size(114, 17);
            this.ExportSubBox.TabIndex = 28;
            this.ExportSubBox.Text = "Export to subfolder";
            this.toolTip.SetToolTip(this.ExportSubBox, "Choose a master folder and have the program automatically create a folder\r\nwith t" +
        "he name of the original midi file when exporting.");
            this.ExportSubBox.UseVisualStyleBackColor = true;
            // 
            // colorTextBox4
            // 
            this.colorTextBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorTextBox4.Location = new System.Drawing.Point(9, 346);
            this.colorTextBox4.Name = "colorTextBox4";
            this.colorTextBox4.Size = new System.Drawing.Size(100, 20);
            this.colorTextBox4.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 330);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Color 4";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 213);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Color 1";
            // 
            // colorTextBox1
            // 
            this.colorTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorTextBox1.Location = new System.Drawing.Point(9, 229);
            this.colorTextBox1.Name = "colorTextBox1";
            this.colorTextBox1.Size = new System.Drawing.Size(100, 20);
            this.colorTextBox1.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 447);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Color 7";
            // 
            // colorTextBox7
            // 
            this.colorTextBox7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorTextBox7.Location = new System.Drawing.Point(9, 463);
            this.colorTextBox7.Name = "colorTextBox7";
            this.colorTextBox7.Size = new System.Drawing.Size(100, 20);
            this.colorTextBox7.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 252);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Color 2";
            // 
            // colorTextBox2
            // 
            this.colorTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorTextBox2.Location = new System.Drawing.Point(9, 268);
            this.colorTextBox2.Name = "colorTextBox2";
            this.colorTextBox2.Size = new System.Drawing.Size(100, 20);
            this.colorTextBox2.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 369);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Color 5";
            // 
            // colorTextBox5
            // 
            this.colorTextBox5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorTextBox5.Location = new System.Drawing.Point(9, 385);
            this.colorTextBox5.Name = "colorTextBox5";
            this.colorTextBox5.Size = new System.Drawing.Size(100, 20);
            this.colorTextBox5.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 291);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Color 3";
            // 
            // colorTextBox3
            // 
            this.colorTextBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorTextBox3.Location = new System.Drawing.Point(9, 307);
            this.colorTextBox3.Name = "colorTextBox3";
            this.colorTextBox3.Size = new System.Drawing.Size(100, 20);
            this.colorTextBox3.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 408);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "Color 6";
            // 
            // colorTextBox6
            // 
            this.colorTextBox6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorTextBox6.Location = new System.Drawing.Point(9, 424);
            this.colorTextBox6.Name = "colorTextBox6";
            this.colorTextBox6.Size = new System.Drawing.Size(100, 20);
            this.colorTextBox6.TabIndex = 15;
            // 
            // colorPicker1
            // 
            this.colorPicker1.Location = new System.Drawing.Point(115, 229);
            this.colorPicker1.Name = "colorPicker1";
            this.colorPicker1.Size = new System.Drawing.Size(62, 20);
            this.colorPicker1.TabIndex = 17;
            this.colorPicker1.Text = "Choose";
            this.colorPicker1.UseVisualStyleBackColor = true;
            this.colorPicker1.Click += new System.EventHandler(this.colorPicker1_Click);
            // 
            // colorPicker2
            // 
            this.colorPicker2.Location = new System.Drawing.Point(115, 268);
            this.colorPicker2.Name = "colorPicker2";
            this.colorPicker2.Size = new System.Drawing.Size(62, 20);
            this.colorPicker2.TabIndex = 18;
            this.colorPicker2.Text = "Choose";
            this.colorPicker2.UseVisualStyleBackColor = true;
            this.colorPicker2.Click += new System.EventHandler(this.colorPicker2_Click);
            // 
            // colorPicker3
            // 
            this.colorPicker3.Location = new System.Drawing.Point(115, 307);
            this.colorPicker3.Name = "colorPicker3";
            this.colorPicker3.Size = new System.Drawing.Size(62, 20);
            this.colorPicker3.TabIndex = 19;
            this.colorPicker3.Text = "Choose";
            this.colorPicker3.UseVisualStyleBackColor = true;
            this.colorPicker3.Click += new System.EventHandler(this.colorPicker3_Click);
            // 
            // colorPicker4
            // 
            this.colorPicker4.Location = new System.Drawing.Point(115, 346);
            this.colorPicker4.Name = "colorPicker4";
            this.colorPicker4.Size = new System.Drawing.Size(62, 20);
            this.colorPicker4.TabIndex = 20;
            this.colorPicker4.Text = "Choose";
            this.colorPicker4.UseVisualStyleBackColor = true;
            this.colorPicker4.Click += new System.EventHandler(this.colorPicker4_Click);
            // 
            // colorPicker5
            // 
            this.colorPicker5.Location = new System.Drawing.Point(115, 385);
            this.colorPicker5.Name = "colorPicker5";
            this.colorPicker5.Size = new System.Drawing.Size(62, 20);
            this.colorPicker5.TabIndex = 21;
            this.colorPicker5.Text = "Choose";
            this.colorPicker5.UseVisualStyleBackColor = true;
            this.colorPicker5.Click += new System.EventHandler(this.colorPicker5_Click);
            // 
            // colorPicker6
            // 
            this.colorPicker6.Location = new System.Drawing.Point(115, 424);
            this.colorPicker6.Name = "colorPicker6";
            this.colorPicker6.Size = new System.Drawing.Size(62, 20);
            this.colorPicker6.TabIndex = 22;
            this.colorPicker6.Text = "Choose";
            this.colorPicker6.UseVisualStyleBackColor = true;
            this.colorPicker6.Click += new System.EventHandler(this.colorPicker6_Click);
            // 
            // colorPicker7
            // 
            this.colorPicker7.Location = new System.Drawing.Point(115, 463);
            this.colorPicker7.Name = "colorPicker7";
            this.colorPicker7.Size = new System.Drawing.Size(62, 20);
            this.colorPicker7.TabIndex = 23;
            this.colorPicker7.Text = "Choose";
            this.colorPicker7.UseVisualStyleBackColor = true;
            this.colorPicker7.Click += new System.EventHandler(this.colorPicker7_Click);
            // 
            // MinBytesTextBox
            // 
            this.MinBytesTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MinBytesTextBox.Location = new System.Drawing.Point(33, 188);
            this.MinBytesTextBox.Name = "MinBytesTextBox";
            this.MinBytesTextBox.Size = new System.Drawing.Size(144, 20);
            this.MinBytesTextBox.TabIndex = 26;
            this.MinBytesTextBox.Text = "105";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 170);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(110, 13);
            this.label8.TabIndex = 27;
            this.label8.Text = "Remove empty tracks";
            // 
            // ManualColorBox
            // 
            this.ManualColorBox.AutoSize = true;
            this.ManualColorBox.Location = new System.Drawing.Point(12, 127);
            this.ManualColorBox.Name = "ManualColorBox";
            this.ManualColorBox.Size = new System.Drawing.Size(113, 17);
            this.ManualColorBox.TabIndex = 29;
            this.ManualColorBox.Text = "Use manual colors";
            this.toolTip.SetToolTip(this.ManualColorBox, "Choose a master folder and have the program automatically create a folder\r\nwith t" +
        "he name of the original midi file when exporting.");
            this.ManualColorBox.UseVisualStyleBackColor = true;
            this.ManualColorBox.CheckedChanged += new System.EventHandler(this.ManualColorBox_CheckedChanged);
            // 
            // AutoColorBox
            // 
            this.AutoColorBox.AutoSize = true;
            this.AutoColorBox.Location = new System.Drawing.Point(12, 150);
            this.AutoColorBox.Name = "AutoColorBox";
            this.AutoColorBox.Size = new System.Drawing.Size(117, 17);
            this.AutoColorBox.TabIndex = 30;
            this.AutoColorBox.Text = "Color tracks by size";
            this.toolTip.SetToolTip(this.AutoColorBox, "Choose a master folder and have the program automatically create a folder\r\nwith t" +
        "he name of the original midi file when exporting.");
            this.AutoColorBox.UseVisualStyleBackColor = true;
            this.AutoColorBox.CheckedChanged += new System.EventHandler(this.AutoColorBox_CheckedChanged);
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(185, 498);
            this.Controls.Add(this.AutoColorBox);
            this.Controls.Add(this.ManualColorBox);
            this.Controls.Add(this.ExportSubBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.MinBytesTextBox);
            this.Controls.Add(this.RemoveTracksBox);
            this.Controls.Add(this.ReadTrackInstrumentBox);
            this.Controls.Add(this.colorPicker7);
            this.Controls.Add(this.colorPicker6);
            this.Controls.Add(this.colorPicker5);
            this.Controls.Add(this.colorPicker4);
            this.Controls.Add(this.colorPicker3);
            this.Controls.Add(this.colorPicker2);
            this.Controls.Add(this.colorPicker1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.colorTextBox6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.colorTextBox5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.colorTextBox3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.colorTextBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.colorTextBox7);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.colorTextBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.colorTextBox4);
            this.Controls.Add(this.FilePrefixBox);
            this.Controls.Add(this.ReadTrackNamesBox);
            this.Controls.Add(this.CopyFirstTrackBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OptionsForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox CopyFirstTrackBox;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.CheckBox ReadTrackNamesBox;
        private System.Windows.Forms.CheckBox FilePrefixBox;
        private System.Windows.Forms.TextBox colorTextBox4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox colorTextBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox colorTextBox7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox colorTextBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox colorTextBox5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox colorTextBox3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox colorTextBox6;
        private System.Windows.Forms.Button colorPicker1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button colorPicker2;
        private System.Windows.Forms.Button colorPicker3;
        private System.Windows.Forms.Button colorPicker4;
        private System.Windows.Forms.Button colorPicker5;
        private System.Windows.Forms.Button colorPicker6;
        private System.Windows.Forms.Button colorPicker7;
        private System.Windows.Forms.CheckBox ReadTrackInstrumentBox;
        private System.Windows.Forms.CheckBox RemoveTracksBox;
        private System.Windows.Forms.TextBox MinBytesTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox ExportSubBox;
        private System.Windows.Forms.CheckBox ManualColorBox;
        private System.Windows.Forms.CheckBox AutoColorBox;
    }
}