﻿using MIDI_Splitter_Lite.Properties;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Forms;

namespace MIDI_Splitter_Lite
{
    public partial class OptionsForm : Form
    {
        private MainForm mainForm;

        public OptionsForm(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;

            MinBytesTextBox.Text = Settings.Default.MinBytes.ToString();

            colorTextBox1.BackColor = Settings.Default.Color1;
            colorTextBox2.BackColor = Settings.Default.Color2;
            colorTextBox3.BackColor = Settings.Default.Color3;
            colorTextBox4.BackColor = Settings.Default.Color4;
            colorTextBox5.BackColor = Settings.Default.Color5;
            colorTextBox6.BackColor = Settings.Default.Color6;
            colorTextBox7.BackColor = Settings.Default.Color7;

            colorTextBoxMax.BackColor = Settings.Default.MaxColor;
            colorTextBoxMin.BackColor = Settings.Default.MinColor;

            string color1 = "";
            string color2 = "";
            string color3 = "";
            string color4 = "";
            string color5 = "";
            string color6 = "";
            string color7 = "";

            updateTextBox(color1, colorTextBox1, Settings.Default.ColorText1);
            updateTextBox(color2, colorTextBox2, Settings.Default.ColorText2);
            updateTextBox(color3, colorTextBox3, Settings.Default.ColorText3);
            updateTextBox(color4, colorTextBox4, Settings.Default.ColorText4);
            updateTextBox(color5, colorTextBox5, Settings.Default.ColorText5);
            updateTextBox(color6, colorTextBox6, Settings.Default.ColorText6);
            updateTextBox(color7, colorTextBox7, Settings.Default.ColorText7);

            this.ActiveControl = label1;
        }

        private void updateTextBox(string color, TextBox textbox, StringCollection stringCollection)
        {
            if (stringCollection?.Count > 0)
            {
                foreach (string item in stringCollection)
                {
                    color += item.ToString() + ",";
                }

                textbox.Text = color.Remove(color.Length - 1, 1).Replace(", ", ",").Replace(" ", "_");
            }
        }

        private StringCollection saveTextBox(TextBox textbox)
        {
            List<string> tempList = textbox.Text.ToLower().Replace(", ", ",").Replace(" ", "_").Split(',').ToList();
            StringCollection tempCollection = new StringCollection();
            tempCollection.AddRange(tempList.ToArray());
            return tempCollection;
        }

        private void OptionsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.MinBytes = ConvertTextBoxTextToInt(MinBytesTextBox);

            Settings.Default.Color1 = colorTextBox1.BackColor;
            Settings.Default.Color2 = colorTextBox2.BackColor;
            Settings.Default.Color3 = colorTextBox3.BackColor;
            Settings.Default.Color4 = colorTextBox4.BackColor;
            Settings.Default.Color5 = colorTextBox5.BackColor;
            Settings.Default.Color6 = colorTextBox6.BackColor;
            Settings.Default.Color7 = colorTextBox7.BackColor;

            Settings.Default.MaxColor = colorTextBoxMax.BackColor;
            Settings.Default.MinColor = colorTextBoxMin.BackColor;

            Settings.Default.ColorText1 = saveTextBox(colorTextBox1);
            Settings.Default.ColorText2 = saveTextBox(colorTextBox2);
            Settings.Default.ColorText3 = saveTextBox(colorTextBox3);
            Settings.Default.ColorText4 = saveTextBox(colorTextBox4);
            Settings.Default.ColorText5 = saveTextBox(colorTextBox5);
            Settings.Default.ColorText6 = saveTextBox(colorTextBox6);
            Settings.Default.ColorText7 = saveTextBox(colorTextBox7);

            Settings.Default.Save();

            mainForm.RequestRestart();
        }

        private void openColorPicker(TextBox textBox)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.AllowFullOpen = true;
                colorDialog.AnyColor = true;
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    textBox.BackColor = colorDialog.Color;
                }
            }
        }

        private void colorPicker1_Click(object sender, EventArgs e)
        {
            openColorPicker(colorTextBox1);
        }

        private void colorPicker2_Click(object sender, EventArgs e)
        {
            openColorPicker(colorTextBox2);
        }

        private void colorPicker3_Click(object sender, EventArgs e)
        {
            openColorPicker(colorTextBox3);
        }

        private void colorPicker4_Click(object sender, EventArgs e)
        {
            openColorPicker(colorTextBox4);
        }

        private void colorPicker5_Click(object sender, EventArgs e)
        {
            openColorPicker(colorTextBox5);
        }

        private void colorPicker6_Click(object sender, EventArgs e)
        {
            openColorPicker(colorTextBox6);
        }

        private void colorPicker7_Click(object sender, EventArgs e)
        {
            openColorPicker(colorTextBox7);
        }

        private int ConvertTextBoxTextToInt(TextBox maxBytesTextBox)
        {
            int result;
            if (int.TryParse(maxBytesTextBox.Text, out result))
            {
                return result;
            }
            else
            {
                maxBytesTextBox.Text = Settings.Default.MinBytes.ToString();
                return Settings.Default.MinBytes;
            }
        }

        private void colorPickerMax_Click(object sender, EventArgs e)
        {
            openColorPicker(colorTextBoxMax);
        }

        private void colorPickerMin_Click(object sender, EventArgs e)
        {
            openColorPicker(colorTextBoxMin);
        }
    }
}
