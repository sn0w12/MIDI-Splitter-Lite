﻿using Microsoft.WindowsAPICodePack.Taskbar;
using MIDI_Splitter_Lite.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace MIDI_Splitter_Lite
{
    public partial class MainForm : Form
    {
        List<ushort> trackNumberList = new List<ushort>();
        List<string> trackNamesList = new List<string>();

        private readonly ListViewItemComparer lvwColumnSorter;

        private Size formOriginalSize;
        private Rectangle recLab1;
        private Rectangle recLab2;
        private Rectangle recBut1;
        private Rectangle recBut2;
        private Rectangle recTxt1;
        private Rectangle recTxt2;
        private Rectangle recLis1;
        private Rectangle recBar1;

        private ContextMenuStrip listContextMenu;

        private readonly string[] instruments1 = new string[Settings.Default.ColorText1?.Count ?? 0];
        private readonly string[] instruments2 = new string[Settings.Default.ColorText2?.Count ?? 0];
        private readonly string[] instruments3 = new string[Settings.Default.ColorText3?.Count ?? 0];
        private readonly string[] instruments4 = new string[Settings.Default.ColorText4?.Count ?? 0];
        private readonly string[] instruments5 = new string[Settings.Default.ColorText5?.Count ?? 0];
        private readonly string[] instruments6 = new string[Settings.Default.ColorText6?.Count ?? 0];
        private readonly string[] instruments7 = new string[Settings.Default.ColorText7?.Count ?? 0];

        int goal = 0;
        string BGWorkerExMessage = "";

        public MainForm()
        {
            if (Debugger.IsAttached)
            {
                if (ConfirmationPopup("Do you want to reset your settings?", "Warning", true))
                {
                    if (ConfirmationPopup("Warning: Are you sure you want to reset your settings? This is not reversible.", "Warning", true, MessageBoxIcon.Warning))
                        Settings.Default.Reset();
                }
            }

            InitializeComponent();

            InitializeOptionsStripMenu();

            lvwColumnSorter = new ListViewItemComparer();
            this.MIDIListView.ListViewItemSorter = lvwColumnSorter;

            this.Resize += MainForm_Resize;
            formOriginalSize = this.Size;
            recLab1 = new Rectangle(label1.Location, label1.Size);
            recLab2 = new Rectangle(label2.Location, label2.Size);
            recBut1 = new Rectangle(BrowseBTN.Location, BrowseBTN.Size);
            recBut2 = new Rectangle(ExportBTN.Location, ExportBTN.Size);
            recTxt1 = new Rectangle(MIDIPathBox.Location, MIDIPathBox.Size);
            recTxt2 = new Rectangle(ExportPathBox.Location, ExportPathBox.Size);
            recLis1 = new Rectangle(MIDIListView.Location, MIDIListView.Size);
            recBar1 = new Rectangle(progressBar.Location, progressBar.Size);

            AutoSizeColumnList(MIDIListView);

            CopyStringCollectionToStringArray(Settings.Default.ColorText1, instruments1);
            CopyStringCollectionToStringArray(Settings.Default.ColorText2, instruments2);
            CopyStringCollectionToStringArray(Settings.Default.ColorText3, instruments3);
            CopyStringCollectionToStringArray(Settings.Default.ColorText4, instruments4);
            CopyStringCollectionToStringArray(Settings.Default.ColorText5, instruments5);
            CopyStringCollectionToStringArray(Settings.Default.ColorText6, instruments6);
            CopyStringCollectionToStringArray(Settings.Default.ColorText7, instruments7);

            ExportPathBox.Text = Settings.Default.ExportPath;
            UpdateSplitText();
        }

        private void InitializeOptionsStripMenu()
        {
            ToolTip toolTip1 = new ToolTip();
            ToolStripSeparator seperator = new ToolStripSeparator();
            ToolStripSeparator seperator2 = new ToolStripSeparator();
            ToolStripSeparator seperator3 = new ToolStripSeparator();
            int insertIndex = 0;

            CheckBox copyFirstTrackCheckBox = CreateCheckBox("Copy First Track", Settings.Default.CopyFirstTrack);
            copyFirstTrackCheckBox.Click += (sender, e) =>
            {
                Settings.Default.CopyFirstTrack = copyFirstTrackCheckBox.Checked;
                RequestRestart();
            };
            ToolStripControlHost copyFirstTrackHost = new ToolStripControlHost(copyFirstTrackCheckBox);
            optionsToolStripMenuItem.DropDownItems.Insert(insertIndex++, copyFirstTrackHost);

            CheckBox ReadTrackNamesBox = CreateCheckBox("Read track names", Settings.Default.ReadTrackNames);
            CheckBox ReadTrackInstrumentBox = CreateCheckBox("Read track instruments", Settings.Default.ReadTrackInstruments);
            ReadTrackNamesBox.Click += (sender, e) =>
            {
                Settings.Default.ReadTrackNames = ReadTrackNamesBox.Checked;
                ReadTrackInstrumentBox.Checked = false;
                Settings.Default.ReadTrackInstruments = ReadTrackInstrumentBox.Checked;
                RequestRestart();
            };
            ToolStripControlHost ReadTrackNamesHost = new ToolStripControlHost(ReadTrackNamesBox);
            optionsToolStripMenuItem.DropDownItems.Insert(insertIndex++, ReadTrackNamesHost);

            ReadTrackInstrumentBox.Click += (sender, e) => 
            {
                Settings.Default.ReadTrackInstruments = ReadTrackInstrumentBox.Checked;
                ReadTrackNamesBox.Checked = false;
                Settings.Default.ReadTrackNames = ReadTrackNamesBox.Checked;
                RequestRestart();
            };
            ToolStripControlHost ReadTrackInstrumentHost = new ToolStripControlHost(ReadTrackInstrumentBox);
            optionsToolStripMenuItem.DropDownItems.Insert(insertIndex++, ReadTrackInstrumentHost);

            CheckBox RemoveTracksBox = CreateCheckBox("Remove Empty tracks", Settings.Default.RemoveTracks);
            RemoveTracksBox.Click += (sender, e) =>
            {
                Settings.Default.RemoveTracks = RemoveTracksBox.Checked;
                RequestRestart();
            };
            ToolStripControlHost RemoveTracksHost = new ToolStripControlHost(RemoveTracksBox);
            optionsToolStripMenuItem.DropDownItems.Insert(insertIndex++, RemoveTracksHost);

            optionsToolStripMenuItem.DropDownItems.Insert(insertIndex++, seperator);

            CheckBox FilePrefixBox = CreateCheckBox("Use file name as prefix", Settings.Default.FilePrefixBox);
            FilePrefixBox.Click += (sender, e) => Settings.Default.FilePrefixBox = FilePrefixBox.Checked;
            ToolStripControlHost FilePrefixHost = new ToolStripControlHost(FilePrefixBox);
            optionsToolStripMenuItem.DropDownItems.Insert(insertIndex++, FilePrefixHost);

            CheckBox ExportSubBox = CreateCheckBox("Export to subfolder", Settings.Default.ExportSub);
            ExportSubBox.Click += (sender, e) => Settings.Default.ExportSub = ExportSubBox.Checked;
            ToolStripControlHost ExportSubHost = new ToolStripControlHost(ExportSubBox);
            optionsToolStripMenuItem.DropDownItems.Insert(insertIndex++, ExportSubHost);

            optionsToolStripMenuItem.DropDownItems.Insert(insertIndex++, seperator2);

            CheckBox ManualColorBox = CreateCheckBox("Manual Colors", Settings.Default.ManualColors);
            CheckBox AutoColorBox = CreateCheckBox("Automatic Colors", Settings.Default.AutomaticColors);
            ManualColorBox.Click += (sender, e) =>
            {
                Settings.Default.ManualColors = ManualColorBox.Checked;
                AutoColorBox.Checked = false;
                Settings.Default.AutomaticColors = AutoColorBox.Checked;
                UpdateListViewColors();
            };
            ToolStripControlHost ManualColorHost = new ToolStripControlHost(ManualColorBox);
            optionsToolStripMenuItem.DropDownItems.Insert(insertIndex++, ManualColorHost);

            AutoColorBox.Click += (sender, e) =>
            {
                Settings.Default.AutomaticColors = AutoColorBox.Checked;
                ManualColorBox.Checked = false;
                Settings.Default.ManualColors = ManualColorBox.Checked;
                UpdateListViewColors();
            };
            ToolStripControlHost AutoColorHost = new ToolStripControlHost(AutoColorBox);
            optionsToolStripMenuItem.DropDownItems.Insert(insertIndex++, AutoColorHost);

            optionsToolStripMenuItem.DropDownItems.Insert(insertIndex++, seperator3);

            ToolStripMenuItem AdvancedOptionsButton = new ToolStripMenuItem();
            AdvancedOptionsButton.Text = "Advanced Options";
            AdvancedOptionsButton.Click += optionsToolStripMenuItem_Click;
            optionsToolStripMenuItem.DropDownItems.Insert(insertIndex++, AdvancedOptionsButton);

            toolTip1.SetToolTip(copyFirstTrackCheckBox, "Copies the first track in the list to the selected tracks to be exported.\nExported MIDIs will contain two tracks.");
            toolTip1.SetToolTip(ReadTrackNamesBox, "Reads the name of each track from the MIDI file and displays it in the list.\nIf multiple names exists for a given track, the latest one will be shown.");
            toolTip1.SetToolTip(ReadTrackInstrumentBox, "Reads the instrument name of each track from the MIDI file and displays it in the list.\nIf multiple names exists for a given track, the latest one will be shown.");
            toolTip1.SetToolTip(RemoveTracksBox, "Removes tracks that are under x bytes, default is 105. This value can be changed in advanced options.");
            toolTip1.SetToolTip(ExportSubBox, "Choose a master folder and have the program automatically create a folder\nwith the name of the original midi file when exporting.");
            toolTip1.SetToolTip(ManualColorBox, "Manually choose 7 colors for tracks to be colored as. Write track names you\nwant to be colored in the textbox like this: name1,name2,name3");
            toolTip1.SetToolTip(AutoColorBox, "Choose 2 colors and the program will make a gradient between the largest file and the smallest.");
        }

        private CheckBox CreateCheckBox(string text, bool isChecked)
        {
            CheckBox checkBox = new CheckBox
            {
                Text = text,
                BackColor = Color.Transparent,
                Checked = isChecked,
                AutoSize = true
            };
            return checkBox;
        }

        private void resize_Control(Control c, Rectangle r)
        {
            float xRatio = (float)(this.Width) / (float)(formOriginalSize.Width);
            float yRatio = (float)(this.Height) / (float)(formOriginalSize.Height);
            int newX = (int)(r.X * xRatio);
            int newY = (int)(r.Y * yRatio);

            int newWidth = (int)(r.Width * xRatio);
            int newHeight = (int)(r.Height * yRatio);

            c.Location = new Point(newX, newY);
            c.Size = new Size(newWidth, newHeight);
        }

        private void AutoSizeColumnList(ListView listView)
        {
            listView.BeginUpdate();
            listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

            int[] headerWidths = new int[listView.Columns.Count];
            for (int i = 0; i < listView.Columns.Count; i++)
            {
                headerWidths[i] = listView.Columns[i].Width;
            }

            listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

            for (int i = 0; i < listView.Columns.Count; i++)
            {
                if (i == 1)
                    listView.Columns[i].Width = ((int)(this.Width * 0.595));
                else if (i == 0)
                    listView.Columns[i].Width = ((int)(this.Width * 0.13));
                else
                    listView.Columns[i].Width = ((int)(this.Width * 0.15));
            }

            listView.EndUpdate();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            resize_Control(label1, recLab1);
            resize_Control(label2, recLab2);
            resize_Control(BrowseBTN, recBut1);
            resize_Control(ExportBTN, recBut2);
            resize_Control(MIDIPathBox, recTxt1);
            resize_Control(ExportPathBox, recTxt2);
            resize_Control(MIDIListView, recLis1);
            resize_Control(progressBar, recBar1);
            AutoSizeColumnList(MIDIListView);
        }

        public void RequestRestart()
        {
            string lastOpenedFilePath = MIDIPathBox.Text;
            if (!string.IsNullOrEmpty(lastOpenedFilePath) && File.Exists(lastOpenedFilePath))
            {
                LoadMIDIFile(lastOpenedFilePath);
            }
        }

        private void CopyStringCollectionToStringArray(StringCollection source, string[] destination)
        {
            if (source != null)
            {
                source.CopyTo(destination, 0);
            }
        }

        private void SetupListViewContextMenu()
        {
            listContextMenu = new ContextMenuStrip();
            ToolStripMenuItem exportItem = new ToolStripMenuItem($"Export track{(MIDIListView.SelectedItems.Count == 1 ? "" : "s")}");
            ToolStripMenuItem editItem = new ToolStripMenuItem($"Edit name{(MIDIListView.SelectedItems.Count == 1 ? "" : "s")}");
            ToolStripMenuItem removeItem = new ToolStripMenuItem($"Remove track{(MIDIListView.SelectedItems.Count == 1 ? "" : "s")}");
            ToolStripMenuItem selectAllItems = new ToolStripMenuItem("Select all tracks");
            ToolStripMenuItem deselectAllItems = new ToolStripMenuItem($"Deselect {(MIDIListView.SelectedItems.Count == 1 ? "track" : "all tracks")}");
            ToolStripMenuItem reloadMidi = new ToolStripMenuItem("Reload midi");

            exportItem.Click += ExportItem_Click;
            editItem.Click += EditItem_Click;
            removeItem.Click += (sender, e) => { if (ConfirmationPopup($"Are you sure you want to delete the track{(MIDIListView.SelectedItems.Count == 1 ? "" : "s")}?", $"Remove track{(MIDIListView.SelectedItems.Count == 1 ? "" : "s")}")) { for (int i = MIDIListView.SelectedItems.Count - 1; i >= 0; i--) MIDIListView.Items.Remove(MIDIListView.SelectedItems[i]); } };
            reloadMidi.Click += (sender, e) => ReloadMidiButton_Click(sender, e);
            selectAllItems.Click += (sender, e) => SelectAllItems(true);
            deselectAllItems.Click += (sender, e) => SelectAllItems(false);

            listContextMenu.Items.Add(exportItem);
            listContextMenu.Items.Add(editItem);
            listContextMenu.Items.Add(removeItem);
            listContextMenu.Items.Add("-");
            if (MIDIListView.SelectedItems.Count != MIDIListView.Items.Count) { listContextMenu.Items.Add(selectAllItems); }
            if (MIDIListView.SelectedItems.Count > 0) { listContextMenu.Items.Add(deselectAllItems); }
            listContextMenu.Items.Add("-");
            listContextMenu.Items.Add(reloadMidi);
            MIDIListView.ContextMenuStrip = listContextMenu;
        }

        private bool ConfirmationPopup(String message, String title, Boolean bypass = false, MessageBoxIcon icon = MessageBoxIcon.Question, MessageBoxButtons buttons = MessageBoxButtons.YesNo)
        {
            if (bypass || (MIDIListView != null && MIDIListView.SelectedItems.Count > 0))
            {
                DialogResult result = MessageBox.Show(message, title, buttons, icon);
                return result == DialogResult.Yes;
            }
            return false;
        }

        private void MIDIListView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ListViewItem item = MIDIListView.GetItemAt(e.X, e.Y);
                if (item != null)
                {
                    item.Selected = true;
                    listContextMenu.Show(Cursor.Position);
                }
            }
        }

        private void SelectAllItems(Boolean select)
        {
            if (MIDIListView.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in MIDIListView.Items)
                {
                    item.Selected = select;
                }
            }
        }

        private void ExportItem_Click(object sender, EventArgs e)
        {
            if (MIDIListView.SelectedItems.Count > 0)
            {
                ExportTracks();
            }
        }

        private void EditItem_Click(object sender, EventArgs e)
        {
            if (MIDIListView.SelectedItems.Count > 0)
            {
                bool isEditingFinished = false;

                ListViewItem item = MIDIListView.SelectedItems[0];
                Rectangle rect = item.SubItems[1].Bounds;
                TextBox editBox = new TextBox
                {
                    Bounds = rect,
                    Text = item.SubItems[1].Text,
                    Parent = MIDIListView
                };
                editBox.KeyPress += (s, args) =>
                {
                    if (args.KeyChar == (char)Keys.Enter)
                    {
                        isEditingFinished = true;
                        FinishEditing(editBox);
                    }
                    if (args.KeyChar == (char)Keys.Escape)
                    {
                        isEditingFinished = true;
                        editBox.Dispose();
                    }
                };
                editBox.Leave += (s, args) =>
                {
                    if (!isEditingFinished)
                    {
                        if (ConfirmationPopup($"Do you want to save the name{(MIDIListView.SelectedItems.Count == 1 ? "" : "s")}?", "Save"))
                            FinishEditing(editBox);
                        else
                            editBox.Dispose();
                    }
                };
                MIDIListView.Controls.Add(editBox);
                editBox.Focus();
            }
        }

        private void FinishEditing(TextBox editBox)
        {
            foreach (ListViewItem item in MIDIListView.SelectedItems)
            {
                item.SubItems[1].Text = editBox.Text;
            }
            editBox.Dispose();
            UpdateListViewColors();
        }

        private void LoadMIDIFile(string filePath)
        {
            using (FileStream midiReader = new FileStream(filePath, FileMode.Open))
            {
                var midiParser = new MidiParser.MidiFile(midiReader);

                int trackSizeMax = Settings.Default.MinBytes;

                midiReader.Seek(4, SeekOrigin.Begin);

                byte[] headerSize = new byte[4];
                midiReader.Read(headerSize, 0, headerSize.Length);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(headerSize);
                int headerSizeInt = BitConverter.ToInt32(headerSize, 0);

                byte[] midiFormat = new byte[2];
                midiReader.Read(midiFormat, 0, midiFormat.Length);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(midiFormat);
                ushort formatNum = BitConverter.ToUInt16(midiFormat, 0);

                if (headerSizeInt != 6)
                {
                    MessageBox.Show(this, Path.GetFileName(filePath) + " is not a MIDI file created under the MIDI 1.0 specification, and won't be opened for splitting.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (formatNum != 1)
                {
                    MessageBox.Show(this, Path.GetFileName(filePath) + " is not a Format 1 MIDI file, and won't be opened for splitting.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MIDIListView.Items.Clear();
                    MIDIPathBox.Text = filePath;

                    byte[] totalTracks = new byte[2];
                    midiReader.Read(totalTracks, 0, totalTracks.Length);
                    if (BitConverter.IsLittleEndian)
                        Array.Reverse(totalTracks);
                    ushort totalTracksShort = BitConverter.ToUInt16(totalTracks, 0);

                    midiReader.Seek(2, SeekOrigin.Current);

                    if (Settings.Default.ReadTrackNames)
                    {
                        for (ushort i = 0; i < totalTracksShort; i++)
                        {
                            int trackSizeInt = FindTrackSize(midiReader);

                            List<byte> tempArray = new List<byte>();
                            if (trackSizeInt <= 4096)
                            {
                                byte[] trackNameData = new byte[trackSizeInt];
                                midiReader.Read(trackNameData, 0, trackNameData.Length);
                                tempArray.AddRange(trackNameData);
                            }
                            else
                            {
                                byte[] trackNameData = new byte[4096];
                                midiReader.Read(trackNameData, 0, trackNameData.Length);
                                midiReader.Seek(trackSizeInt - 4096, SeekOrigin.Current);
                                tempArray.AddRange(trackNameData);
                            }

                            byte[] trackData = tempArray.ToArray();

                            byte[] searchPattern = { 0xFF, 0x03 };
                            List<int> trackNameIndex = new List<int>();
                            trackNameIndex = KMPSearch(trackData, searchPattern);

                            string trackNameStr;

                            if (trackNameIndex.Count > 0)
                            {
                                int lengthIndex = trackNameIndex.ElementAt(trackNameIndex.Count - 1) + 2;
                                byte trackNameByteLength = trackData[lengthIndex];

                                byte[] trackNameBytes = new byte[(int)trackNameByteLength];
                                trackNameBytes = SubArray(trackData, lengthIndex + 1, (int)trackNameByteLength);

                                trackNameStr = Encoding.UTF8.GetString(trackNameBytes);
                            }
                            else
                            {
                                trackNameStr = "Track " + (i + 1).ToString();
                            }

                            trackNameStr = SanitizeFileName(trackNameStr);

                            string[] newRow = { (i + 1).ToString(), trackNameStr, BytesToReadableFormat(trackSizeInt) };
                            ListViewItem newItem = new ListViewItem(newRow);
                            if (Settings.Default.RemoveTracks)
                            {
                                if (trackSizeInt > trackSizeMax)
                                {
                                    MIDIListView.Items.Add(newItem);
                                }
                            }
                            else
                            {
                                MIDIListView.Items.Add(newItem);
                            }
                        }
                        UpdateListViewColors();
                    }
                    else if (Settings.Default.ReadTrackInstruments)
                    {
                        var channelInstrumentMap = new Dictionary<int, string>(); // Dictionary to store the instrument for each channel

                        for (int i = 0; i < midiParser.TracksCount; i++)
                        {
                            int trackSizeInt = FindTrackSize(midiReader);
                            midiReader.Seek(trackSizeInt, SeekOrigin.Current);

                            var trackEvents = midiParser.Tracks[i].MidiEvents;
                            string instrumentName = "Unknown";

                            foreach (var midiEvent in trackEvents)
                            {
                                if (midiEvent.MidiEventType == MidiParser.MidiEventType.ProgramChange)
                                {
                                    int channel = midiEvent.Arg1 - 1;
                                    if (channelInstrumentMap.ContainsKey(channel))
                                    {
                                        instrumentName = channelInstrumentMap[channel];
                                        break;
                                    }
                                    else
                                    {
                                        instrumentName = GetInstrumentName(midiEvent.Arg2);
                                        channelInstrumentMap[channel] = instrumentName;
                                        break;
                                    }
                                }
                            }

                            string trackName = instrumentName;
                            string[] listViewRow = { (i + 1).ToString(), trackName, BytesToReadableFormat(trackSizeInt) };
                            ListViewItem listViewItem = new ListViewItem(listViewRow);

                            if (Settings.Default.RemoveTracks)
                            {
                                if (trackSizeInt > trackSizeMax)
                                {
                                    MIDIListView.Items.Add(listViewItem);
                                }
                            }
                            else
                            {
                                MIDIListView.Items.Add(listViewItem);
                            }
                        }
                        UpdateListViewColors();
                    }
                    else
                    {
                        for (ushort i = 0; i < totalTracksShort; i++)
                        {
                            int trackSizeInt = FindTrackSize(midiReader);
                            midiReader.Seek(trackSizeInt, SeekOrigin.Current);

                            string trackNameStr = "Track " + (i + 1).ToString();

                            string[] newRow = { (i + 1).ToString(), trackNameStr, BytesToReadableFormat(trackSizeInt) };
                            ListViewItem newItem = new ListViewItem(newRow);
                            if (Settings.Default.RemoveTracks)
                            {
                                if (trackSizeInt > trackSizeMax)
                                {
                                    MIDIListView.Items.Add(newItem);
                                }
                            }
                            else
                            {
                                MIDIListView.Items.Add(newItem);
                            }
                        }
                        UpdateListViewColors();
                    }
                }
            }
        }

        private int FindTrackSize(FileStream midiReader)
        {
            midiReader.Seek(4, SeekOrigin.Current);

            byte[] trackSize = new byte[4];
            midiReader.Read(trackSize, 0, trackSize.Length);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(trackSize);
            int trackSizeInt = BitConverter.ToInt32(trackSize, 0);
            return trackSizeInt;
        }

        private string BytesToReadableFormat(int bytes)
        {
            double size = bytes;
            string[] units = { "B", "KB", "MB", "GB", "TB" };
            int unitIndex = 0;

            while (size >= 1024 && unitIndex < units.Length - 1)
            {
                size /= 1024;
                unitIndex++;
            }

            return $"{size:0.##} {units[unitIndex]}";
        }


        private string GetInstrumentName(int programNumber)
        {
            string[] instruments = new string[]
            {
                "Acoustic Grand Piano", "Bright Acoustic Piano", "Electric Grand Piano", "Honky-Tonk Piano",
                "Rhodes Piano", "Chorused Piano", "Harpsichord", "Clavinet",
                "Celesta", "Glockenspiel", "Music Box", "Vibraphone",
                "Marimba", "Xylophone", "Tubular Bells", "Dulcimer",
                "Hammond Organ", "Percussive Organ", "Rock Organ", "Church Organ",
                "Reed Organ", "Accordion", "Harmonica", "Tango Accordion",
                "Acoustic Guitar - Nylon", "Acoustic Guitar - Steel", "Electric Guitar - Jazz", "Electric Guitar - Clean",
                "Electric Guitar - Muted", "Overdriven Guitar", "Distortion Guitar", "Guitar Harmonics",
                "Acoustic Bass", "Electric Bass - Finger", "Electric Bass - Pick", "Fretless Bass",
                "Slap Bass 1", "Slap Bass 2", "Synth Bass 1", "Synth Bass 2",
                "Violin", "Viola", "Cello", "Contrabass",
                "Tremolo Strings", "Pizzicato Strings", "Orchestral Harp", "Timpani",
                "String Ensemble 1", "String Ensemble 2", "Synth. Strings 1", "Synth. Strings 2",
                "Choir Aahs", "Voice Oohs", "Synth Voice", "Orchestra Hit",
                "Trumpet", "Trombone", "Tuba", "Muted Trumpet",
                "French Horn", "Brass Section", "Synth. Brass 1", "Synth. Brass 2",
                "Soprano Sax", "Alto Sax", "Tenor Sax", "Baritone Sax",
                "Oboe", "English Horn", "Bassoon", "Clarinet",
                "Piccolo", "Flute", "Recorder", "Pan Flute",
                "Bottle Blow", "Shakuhachi", "Whistle", "Ocarina",
                "Synth Lead 1 - Square", "Synth Lead 2 - Sawtooth", "Synth Lead 3 - Calliope", "Synth Lead 4 - Chiff",
                "Synth Lead 5 - Charang", "Synth Lead 6 - Voice", "Synth Lead 7 - Fifths", "Synth Lead 8 - Brass + Lead",
                "Synth Pad 1 - New Age", "Synth Pad 2 - Warm", "Synth Pad 3 - Polysynth", "Synth Pad 4 - Choir",
                "Synth Pad 5 - Bowed", "Synth Pad 6 - Metallic", "Synth Pad 7 - Halo", "Synth Pad 8 - Sweep",
                "FX 1 - Rain", "FX 2 - Soundtrack", "FX 3 - Crystal", "FX 4 - Atmosphere",
                "FX 5 - Brightness", "FX 6 - Goblins", "FX 7 - Echoes", "FX 8 - Sci-Fi",
                "Sitar", "Banjo", "Shamisen", "Koto",
                "Kalimba", "Bagpipe", "Fiddle", "Shanai",
                "Tinkle Bell", "Agogo", "Steel Drums", "Woodblock",
                "Taiko Drum", "Melodic Tom", "Synth Drum", "Reverse Cymbal",
                "Guitar Fret Noise", "Breath Noise", "Seashore", "Bird Tweet",
                "Telephone Ring", "Helicopter", "Applause", "Gunshot"
            };

            if (programNumber >= 0 && programNumber < instruments.Length)
            {
                return instruments[programNumber];
            }

            return "Unknown";
        }


        // Prevent the system from entering sleep and turning off monitor.
        private void PreventSleepAndMonitorOff()
        {
            NativeMethods.SetThreadExecutionState(NativeMethods.ES_CONTINUOUS | NativeMethods.ES_SYSTEM_REQUIRED | NativeMethods.ES_DISPLAY_REQUIRED);
        }

        // Clear EXECUTION_STATE flags to allow the system to sleep and turn off monitor normally.
        private void AllowSleep()
        {
            NativeMethods.SetThreadExecutionState(NativeMethods.ES_CONTINUOUS);
        }

        internal static class NativeMethods
        {
            // Import SetThreadExecutionState Win32 API and necessary flags.
            [DllImport("kernel32.dll")]
            public static extern uint SetThreadExecutionState(uint esFlags);
            public const uint ES_CONTINUOUS = 0x80000000;
            public const uint ES_SYSTEM_REQUIRED = 0x00000001;
            public const uint ES_DISPLAY_REQUIRED = 0x00000002;
        }

        private static byte[] SubArray(byte[] data, int index, int length)
        {
            byte[] result = new byte[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        // Knuth-Morris-Pratt search algorithm to quickly find occurences of a byte pattern in a larger byte array.
        // References:
        // http://www.geeksforgeeks.org/searching-for-patterns-set-2-kmp-algorithm/
        // https://gist.github.com/Nabid/fde41e7c2b0b681ac674ccc93c1daeb1
        private static List<int> KMPSearch(byte[] text, byte[] pattern)
        {
            int N = text.Length;
            int M = pattern.Length;

            if (N < M) return new List<int>(new int[] { -1 });
            if (N == M && text == pattern) return new List<int>(new int[] { 0 });
            if (M == 0) return new List<int>(new int[] { 0 });

            int[] lpsArray = new int[M];
            List<int> matchedIndex = new List<int>();

            LPSTable(pattern, ref lpsArray);

            int i = 0, j = 0;
            while (i < N)
            {
                if (text[i] == pattern[j])
                {
                    i++;
                    j++;
                }

                if (j == M)
                {
                    matchedIndex.Add(i - j);
                    j = lpsArray[j - 1];
                }
                else if (i < N && text[i] != pattern[j])
                {
                    if (j != 0)
                    {
                        j = lpsArray[j - 1];
                    }
                    else
                    {
                        i++;
                    }
                }
            }
            return matchedIndex;
        }


        private static void LPSTable(byte[] pattern, ref int[] lpsArray)
        {
            int M = pattern.Length;
            int len = 0;
            lpsArray[0] = 0;
            int i = 1;

            while (i < M)
            {
                if (pattern[i] == pattern[len])
                {
                    len++;
                    lpsArray[i] = len;
                    i++;
                }
                else
                {
                    if (len == 0)
                    {
                        lpsArray[i] = 0;
                        i++;
                    }
                    else
                    {
                        len = lpsArray[len - 1];
                    }
                }
            }
        }

        private void BrowseBTN_Click(object sender, EventArgs e)
        {
            if (OpenMIDIDialog.ShowDialog() == DialogResult.OK)
            {
                LoadMIDIFile(OpenMIDIDialog.FileName);
            }
        }

        private string SanitizeFileName(string fileName)
        {
            string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890-_";
            StringBuilder sanitizedFileName = new StringBuilder();
            char lastChar = '\0';

            for (int i = 0; i < fileName.Length; i++)
            {
                char currentChar = fileName[i];
                bool isLastChar = (i == fileName.Length - 1);

                if (validChars.Contains(currentChar))
                {
                    sanitizedFileName.Append(currentChar);
                    lastChar = currentChar;
                }
                else if (!isLastChar)
                {
                    // Replace invalid chars with underscore, but avoid consecutive underscores
                    if (lastChar != '_')
                    {
                        sanitizedFileName.Append('_');
                        lastChar = '_';
                    }
                }
            }
            return sanitizedFileName.ToString();
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsForm optionsFrm = new OptionsForm(this);
            optionsFrm.ShowDialog();
        }

        private void MIDIListView_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A when e.Control:
                    SelectAllItems(true);
                    break;
                case Keys.D when e.Control:
                    SelectAllItems(false);
                    break;
                case Keys.S when e.Control:
                    if (MIDIListView.SelectedItems.Count < 1)
                    {
                        MessageBox.Show(this, "Error: No tracks selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                    else if (ConfirmationPopup("Are you sure you want to export the midi?", "Export Midi"))
                    {
                        ExportTracks();
                    }
                    break;
                case Keys.R when e.Control:
                    if (ConfirmationPopup("Are you sure you want to reload the midi?", "Reload Midi"))
                    {
                        RequestRestart();
                    }
                    break;
                case Keys.Delete:
                    if (ConfirmationPopup($"Are you sure you want to delete the track{(MIDIListView.SelectedItems.Count == 1 ? "" : "s")}?", $"Remove track{(MIDIListView.SelectedItems.Count == 1 ? "" : "s")}"))
                    {
                        for (int i = MIDIListView.SelectedItems.Count - 1; i >= 0; i--)
                        {
                            MIDIListView.Items.Remove(MIDIListView.SelectedItems[i]);
                        }
                    };
                    break;
            }
        }

        private void splitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportTracks();
        }

        public bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        private void ExportTracks()
        {
            if (string.IsNullOrEmpty(MIDIPathBox.Text) || !File.Exists(MIDIPathBox.Text))
            {
                MessageBox.Show(this, "Error: MIDI file does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (string.IsNullOrEmpty(ExportPathBox.Text) || !Directory.Exists(ExportPathBox.Text))
            {
                MessageBox.Show(this, "Error: Output path does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (MIDIListView.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "Error: No tracks have been selected to be exported.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string exportPath = ExportPathBox.Text;
                if (Settings.Default.ExportSub)
                {
                    string midiFileName = Path.GetFileNameWithoutExtension(MIDIPathBox.Text);
                    exportPath = Path.Combine(exportPath, midiFileName);

                    int folderSuffix = 1;
                    string newExportPath = exportPath;

                    while (Directory.Exists(newExportPath) && !IsDirectoryEmpty(newExportPath))
                    {
                        folderSuffix++;
                        newExportPath = $"{exportPath}_{folderSuffix}";
                    }

                    if (!Directory.Exists(newExportPath))
                    {
                        Directory.CreateDirectory(newExportPath);
                    }

                    exportPath = newExportPath;

                    if (Settings.Default.OldExport != null)
                        Settings.Default.OldExport = ExportPathBox.Text;
                    ExportPathBox.Text = exportPath;
                }
                foreach (ListViewItem item in MIDIListView.SelectedItems)
                {
                    trackNumberList.Add(Convert.ToUInt16(item.SubItems[0].Text));
                    trackNamesList.Add(item.SubItems[1].Text);
                }
                goal = trackNumberList.Count();

                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);

                backgroundWorker.RunWorkerAsync();

                PreventSleepAndMonitorOff();

                splitToolStripMenuItem.Enabled = false;
                abortSplittingToolStripMenuItem.Enabled = true;
                optionsToolStripMenuItem.Enabled = false;
                BrowseBTN.Enabled = false;
                ExportBTN.Enabled = false;
            }
        }

        private void ExportBTN_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(ExportPathBox.Text))
            {
                ExportBrowserDialog.SelectedPath = ExportPathBox.Text;
            }

            if (ExportBrowserDialog.ShowDialog() == DialogResult.OK)
                ExportPathBox.Text = ExportBrowserDialog.SelectedPath;
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (FileStream midiReader = new FileStream(MIDIPathBox.Text, FileMode.Open))
                {

                    byte[] header = new byte[10];
                    midiReader.Read(header, 0, header.Length);
                    byte[] totalTracks = new byte[2];
                    midiReader.Read(totalTracks, 0, totalTracks.Length);

                    if (BitConverter.IsLittleEndian)
                        Array.Reverse(totalTracks);
                    ushort totalTracksInt16 = BitConverter.ToUInt16(totalTracks, 0);

                    byte[] division = new byte[2];
                    midiReader.Read(division, 0, division.Length);

                    if (Settings.Default.CopyFirstTrack)
                    {
                        byte[] trackNumConst = BitConverter.GetBytes((ushort)2);
                        if (BitConverter.IsLittleEndian)
                            Array.Reverse(trackNumConst);

                        byte[] primaryTrackHeader = new byte[4];
                        midiReader.Read(primaryTrackHeader, 0, primaryTrackHeader.Length);

                        byte[] primaryTrackLength = new byte[4];
                        midiReader.Read(primaryTrackLength, 0, primaryTrackLength.Length);

                        byte[] primaryTrackLengthDup = new byte[4];
                        Array.Copy(primaryTrackLength, primaryTrackLengthDup, 4);
                        if (BitConverter.IsLittleEndian)
                            Array.Reverse(primaryTrackLengthDup);
                        int primaryTrackLengthInt = BitConverter.ToInt32(primaryTrackLengthDup, 0);

                        byte[] primaryTrackData = new byte[primaryTrackLengthInt];
                        midiReader.Read(primaryTrackData, 0, primaryTrackData.Length);

                        midiReader.Seek(-1 * (primaryTrackLengthInt + 8), SeekOrigin.Current);

                        for (ushort i = 0; i < totalTracksInt16; i++)
                        {
                            if (backgroundWorker.CancellationPending)
                            {
                                e.Cancel = true;
                                break;
                            }

                            if (trackNumberList.Count > 0)
                            {
                                if ((i + 1) == trackNumberList.ElementAt(0))
                                {
                                    MethodInvoker displayCurrentTrack = delegate
                                    {
                                        this.Text = "MIDI Splitter Lite | Splitting '" + trackNamesList.ElementAt(0) + "'";
                                    };
                                    this.Invoke(displayCurrentTrack);

                                    byte[] trackHeader = new byte[4];
                                    midiReader.Read(trackHeader, 0, trackHeader.Length);

                                    byte[] trackLength = new byte[4];
                                    midiReader.Read(trackLength, 0, trackLength.Length);

                                    byte[] trackLengthCopy = new byte[4];
                                    Array.Copy(trackLength, trackLengthCopy, 4);
                                    if (BitConverter.IsLittleEndian)
                                        Array.Reverse(trackLengthCopy);
                                    int trackLengthInt = BitConverter.ToInt32(trackLengthCopy, 0);

                                    byte[] trackData = new byte[trackLengthInt];
                                    midiReader.Read(trackData, 0, trackData.Length);

                                    string baseFileName = Path.GetFileNameWithoutExtension(MIDIPathBox.Text);
                                    string trackName = trackNamesList.ElementAt(0);
                                    string fileNamePrefix = !Settings.Default.FilePrefixBox ? "" : baseFileName + " - ";
                                    string fileName = Path.Combine(ExportPathBox.Text, fileNamePrefix + trackName + ".mid");

                                    if (File.Exists(fileName))
                                    {
                                        int counter = 1;
                                        do
                                        {
                                            fileName = Path.Combine(ExportPathBox.Text, fileNamePrefix + trackName + " (Copy " + counter + ").mid");
                                            counter++;
                                        } while (File.Exists(fileName));
                                    }

                                    using (FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write))
                                    {
                                        using (BinaryWriter midiWriter = new BinaryWriter(fs))
                                        {
                                            midiWriter.Write(header);
                                            midiWriter.Write(trackNumConst);
                                            midiWriter.Write(division);

                                            midiWriter.Write(primaryTrackHeader);
                                            midiWriter.Write(primaryTrackLength);
                                            midiWriter.Write(primaryTrackData);

                                            midiWriter.Write(trackHeader);
                                            midiWriter.Write(trackLength);
                                            midiWriter.Write(trackData);
                                        }
                                    }
                                    trackNumberList.RemoveAt(0);
                                    trackNamesList.RemoveAt(0);

                                    int percentage = (goal - trackNumberList.Count) * 100 / goal;
                                    backgroundWorker.ReportProgress(percentage);
                                }
                                else
                                {
                                    midiReader.Seek(4, SeekOrigin.Current);

                                    byte[] skipTrackLength = new byte[4];
                                    midiReader.Read(skipTrackLength, 0, skipTrackLength.Length);
                                    if (BitConverter.IsLittleEndian)
                                        Array.Reverse(skipTrackLength);
                                    int skipTrackLengthInt = BitConverter.ToInt32(skipTrackLength, 0);

                                    midiReader.Seek(skipTrackLengthInt, SeekOrigin.Current);
                                }
                            }
                        }
                    }
                    else
                    {
                        byte[] trackNumConst = BitConverter.GetBytes((ushort)1);
                        if (BitConverter.IsLittleEndian)
                            Array.Reverse(trackNumConst);

                        for (ushort i = 0; i < totalTracksInt16; i++)
                        {
                            if (backgroundWorker.CancellationPending)
                            {
                                e.Cancel = true;
                                break;
                            }

                            if (trackNumberList.Count > 0)
                            {
                                if ((i + 1) == trackNumberList.ElementAt(0))
                                {
                                    MethodInvoker displayCurrentTrack = delegate
                                    {
                                        this.Text = "MIDI Splitter Lite | Splitting '" + trackNamesList.ElementAt(0) + "'";
                                    };
                                    this.Invoke(displayCurrentTrack);

                                    byte[] trackHeader = new byte[4];
                                    midiReader.Read(trackHeader, 0, trackHeader.Length);

                                    byte[] trackLength = new byte[4];
                                    midiReader.Read(trackLength, 0, trackLength.Length);

                                    byte[] trackLengthCopy = new byte[4];
                                    Array.Copy(trackLength, trackLengthCopy, 4);
                                    if (BitConverter.IsLittleEndian)
                                        Array.Reverse(trackLengthCopy);
                                    int trackLengthInt = BitConverter.ToInt32(trackLengthCopy, 0);

                                    byte[] trackData = new byte[trackLengthInt];
                                    midiReader.Read(trackData, 0, trackData.Length);

                                    string baseFileName = Path.GetFileNameWithoutExtension(MIDIPathBox.Text);
                                    string trackName = trackNamesList.ElementAt(0);
                                    string fileNamePrefix = !Settings.Default.FilePrefixBox ? "" : baseFileName + " - ";
                                    string fileName = Path.Combine(ExportPathBox.Text, fileNamePrefix + trackName + ".mid");

                                    if (File.Exists(fileName))
                                    {
                                        int counter = 1;
                                        do
                                        {
                                            fileName = Path.Combine(ExportPathBox.Text, fileNamePrefix + trackName + " (Copy " + counter + ").mid");
                                            counter++;
                                        } while (File.Exists(fileName));
                                    }

                                    using (FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write))
                                    {
                                        using (BinaryWriter midiWriter = new BinaryWriter(fs))
                                        {
                                            midiWriter.Write(header);
                                            midiWriter.Write(trackNumConst);
                                            midiWriter.Write(division);

                                            midiWriter.Write(trackHeader);
                                            midiWriter.Write(trackLength);
                                            midiWriter.Write(trackData);
                                        }
                                    }
                                    trackNumberList.RemoveAt(0);
                                    trackNamesList.RemoveAt(0);

                                    int percentage = (goal - trackNumberList.Count) * 100 / goal;
                                    backgroundWorker.ReportProgress(percentage);
                                }
                                else
                                {
                                    midiReader.Seek(4, SeekOrigin.Current);

                                    byte[] skipTrackLength = new byte[4];
                                    midiReader.Read(skipTrackLength, 0, skipTrackLength.Length);
                                    if (BitConverter.IsLittleEndian)
                                        Array.Reverse(skipTrackLength);
                                    int skipTrackLengthInt = BitConverter.ToInt32(skipTrackLength, 0);

                                    midiReader.Seek(skipTrackLengthInt, SeekOrigin.Current);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                BGWorkerExMessage = ex.Message;
            }
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            TaskbarManager.Instance.SetProgressValue(e.ProgressPercentage, 100);
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            AllowSleep();

            this.Text = "MIDI Splitter Lite";
            splitToolStripMenuItem.Enabled = true;
            abortSplittingToolStripMenuItem.Enabled = false;
            optionsToolStripMenuItem.Enabled = true;
            BrowseBTN.Enabled = true;
            ExportBTN.Enabled = true;

            trackNumberList.Clear();
            trackNamesList.Clear();
            progressBar.Value = 0;
            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);

            GC.Collect();

            if (BGWorkerExMessage != "")
            {
                MessageBox.Show(this, "Error: " + BGWorkerExMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                BGWorkerExMessage = "";
            }
            else if (e.Cancelled)
            {
                MessageBox.Show(this, "Splitting aborted.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(this, "Successfully split " + goal.ToString() + " track(s).", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            ExportPathBox.Text = Settings.Default.OldExport;
            goal = 0;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                DialogResult confirmation = MessageBox.Show(this, "The program is still splitting tracks.\n\nAre you sure you want to exit?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (confirmation == DialogResult.Yes)
                {
                    Application.Exit();
                }
            }
            else
            {
                if (ConfirmationPopup("Are you sure you want to exit?", "Exit", true))
                {    
                    Application.Exit();
                };
            }
        }

        private void abortSplittingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker.CancelAsync();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aboutFrm = new AboutForm();
            aboutFrm.ShowDialog();
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
            if (!backgroundWorker.IsBusy && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (Path.GetExtension(data[0]) == ".mid" && data.Length == 1)
                {
                    e.Effect = DragDropEffects.All;
                }
            }
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            var inputFileArray = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            string midiFile = inputFileArray[0];

            LoadMIDIFile(midiFile);
        }

        private Color GetRowColorBasedOnInstrument(string instrumentName)
        {
            var categories = new Dictionary<Color, string[]>();

            AddToCategories(Settings.Default.Color1, instruments1);
            AddToCategories(Settings.Default.Color2, instruments2);
            AddToCategories(Settings.Default.Color3, instruments3);
            AddToCategories(Settings.Default.Color4, instruments4);
            AddToCategories(Settings.Default.Color5, instruments5);
            AddToCategories(Settings.Default.Color6, instruments6);
            AddToCategories(Settings.Default.Color7, instruments7);

            void AddToCategories(Color color, string[] instruments)
            {
                if (color != Color.Empty && !categories.ContainsKey(color))
                {
                    categories.Add(color, instruments);
                }
            }

            var bestMatch = (Color: Color.White, Length: 0);

            foreach (var category in categories)
            {
                foreach (var instrument in category.Value)
                {
                    if (instrumentName.Contains(instrument) && instrument.Length > bestMatch.Length)
                    {
                        bestMatch = (category.Key, instrument.Length);
                    }
                }
            }

            return bestMatch.Color;
        }

        private void UpdateListViewColors()
        {
            if (Settings.Default.ManualColors)
            {
                foreach (ListViewItem item in MIDIListView.Items)
                {
                    string instrumentName = item.SubItems[1].Text.ToLower();
                    Color rowColor = GetRowColorBasedOnInstrument(instrumentName);
                    item.BackColor = rowColor;
                    item.ForeColor = IsColorDark(rowColor) ? Color.White : Color.Black;
                }
            }
            else if (Settings.Default.AutomaticColors)
            {
                long minSize = long.MaxValue;
                long maxSize = long.MinValue;

                foreach (ListViewItem item in MIDIListView.Items)
                {
                    long size = ConvertSizeToBytes(item.SubItems[2].Text);
                    if (size < minSize) minSize = size;
                    if (size > maxSize) maxSize = size;
                }


                foreach (ListViewItem item in MIDIListView.Items)
                {
                    long size = ConvertSizeToBytes(item.SubItems[2].Text);
                    Color rowColor = GetGradientColor(size, minSize, maxSize);
                    item.BackColor = rowColor;
                    item.ForeColor = IsColorDark(rowColor) ? Color.White : Color.Black;
                }
            }
            else
            {
                foreach (ListViewItem item in MIDIListView.Items)
                {
                    item.BackColor = Color.White;
                    item.ForeColor = Color.Black;
                }
            }
            AutoSizeColumnList(MIDIListView);
        }

        private bool IsColorDark(Color color)
        {
            // Calculate the luminance of the color (using the formula for perceived brightness)
            double luminance = 0.299 * color.R + 0.587 * color.G + 0.114 * color.B;
            return luminance < 128;
        }

        Color GetGradientColor(long fileSize, long minSize, long maxSize)
        {
            Color minColor = Settings.Default.MinColor;
            Color maxColor = Settings.Default.MaxColor;

            // Scale the fileSize between 0 and 1
            double scale = (double)(fileSize - minSize) / (maxSize - minSize);

            // Interpolate each color component
            int red = InterpolateColorComponent(minColor.R, maxColor.R, scale);
            int green = InterpolateColorComponent(minColor.G, maxColor.G, scale);
            int blue = InterpolateColorComponent(minColor.B, maxColor.B, scale);

            return Color.FromArgb(red, green, blue);
        }

        // Helper method for interpolating a color component
        int InterpolateColorComponent(int minComponent, int maxComponent, double scale)
        {
            return (int)(minComponent + (maxComponent - minComponent) * scale);
        }

        // Method to convert human-readable file sizes to bytes
        public static long ConvertSizeToBytes(string sizeStr)
        {
            string[] sizeParts = sizeStr.Split(' ');
            if (sizeParts.Length != 2)
                return 0;

            sizeParts[0].Replace(',', '.');
            double sizeValue = double.Parse(sizeParts[0]);
            string sizeUnit = sizeParts[1].ToUpper();

            switch (sizeUnit)
            {
                case "KB":
                    return (long)(sizeValue * 1024);
                case "MB":
                    return (long)(sizeValue * 1024 * 1024);
                case "GB":
                    return (long)(sizeValue * 1024 * 1024 * 1024);
                default: // Assuming "B" for bytes
                    return (long)sizeValue;
            }
        }

        private void MIDIListView_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
            e.NewWidth = MIDIListView.Columns[e.ColumnIndex].Width;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            AllowSleep();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.ExportPath = ExportPathBox.Text;
            Settings.Default.Save();
        }

        private void ReloadMidiButton_Click(object sender, EventArgs e)
        {
            if (ConfirmationPopup("Are you sure you want to reload the MIDI file, this will revert any changes you have made", "Reload MIDI", true))
            {
                RequestRestart();
            }
        }

        private void MIDIListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            SetupListViewContextMenu();
            UpdateSplitText();
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateSplitText();
        }

        private void UpdateSplitText()
        {
            if (MIDIListView.SelectedItems.Count > 0)
            {
                splitToolStripMenuItem.Text = $"Split track{(MIDIListView.SelectedItems.Count == 1 ? "" : "s")}";
                splitToolStripMenuItem.Enabled = true;
            }
            else
            {
                splitToolStripMenuItem.Text = "No track selected";
                splitToolStripMenuItem.Enabled = false;
            }
        }

        private void MIDIListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Check if the clicked column is the same as the previously clicked column
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                    lvwColumnSorter.Order = SortOrder.Descending;
                else
                    lvwColumnSorter.Order = SortOrder.Ascending;
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with the new order
            MIDIListView.Sort();
            lvwColumnSorter.UpdateColumnHeader(MIDIListView);
        }

        private void openMidiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenMIDIDialog.ShowDialog() == DialogResult.OK)
            {
                LoadMIDIFile(OpenMIDIDialog.FileName);
            }
        }
    }

    class ListViewItemComparer : IComparer
    {
        public int SortColumn { get; set; }
        public SortOrder Order { get; set; }

        public ListViewItemComparer()
        {
            SortColumn = 0;
            Order = SortOrder.None;
        }

        public int Compare(object x, object y)
        {
            int returnVal;

            if (SortColumn == 2) // Check if it's the third column
            {
                long sizeX = MainForm.ConvertSizeToBytes(((ListViewItem)x).SubItems[SortColumn].Text);
                long sizeY = MainForm.ConvertSizeToBytes(((ListViewItem)y).SubItems[SortColumn].Text);
                returnVal = sizeX.CompareTo(sizeY);
            }
            else if (double.TryParse(((ListViewItem)x).SubItems[SortColumn].Text, out double xVal) &&
                     double.TryParse(((ListViewItem)y).SubItems[SortColumn].Text, out double yVal))
            {
                returnVal = xVal.CompareTo(yVal);
            }
            else
            {
                returnVal = String.Compare(((ListViewItem)x).SubItems[SortColumn].Text, ((ListViewItem)y).SubItems[SortColumn].Text);
            }

            if (Order == SortOrder.Descending)
                returnVal *= -1;

            return returnVal;
        }

        public void UpdateColumnHeader(ListView listView)
        {
            foreach (ColumnHeader column in listView.Columns)
            {
                if (column.Index == SortColumn)
                {
                    column.Text = column.Text.TrimEnd(' ', '↑', '↓')
                                + (Order == SortOrder.Ascending ? " ↑" : Order == SortOrder.Descending ? " ↓" : "");
                }
                else
                {
                    column.Text = column.Text.TrimEnd(' ', '↑', '↓');
                }
            }
        }
    }
}