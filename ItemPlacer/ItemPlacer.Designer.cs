namespace ItemPlacer
{
    partial class ItemPlacer
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
            this.comboBoxItems = new System.Windows.Forms.ComboBox();
            this.comboBoxLocations = new System.Windows.Forms.ComboBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.addButton = new System.Windows.Forms.Button();
            this.removeButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.itemLabel = new System.Windows.Forms.Label();
            this.locationLabel = new System.Windows.Forms.Label();
            this.costUpDown = new System.Windows.Forms.NumericUpDown();
            this.labelCost = new System.Windows.Forms.Label();
            this.checkBoxHideUsedLocations = new System.Windows.Forms.CheckBox();
            this.hidePlacedItems = new System.Windows.Forms.CheckBox();
            this.comboBoxDefaultShopItems = new System.Windows.Forms.ComboBox();
            this.defaultShopLabel = new System.Windows.Forms.Label();
            this.multiItemCounter = new System.Windows.Forms.Label();
            this.resetButton = new System.Windows.Forms.Button();
            this.loadButton = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.titleBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Placements = new System.Windows.Forms.TabPage();
            this.Settings = new System.Windows.Forms.TabPage();
            this.changeStartCheckBox = new System.Windows.Forms.CheckBox();
            this.startSceneNameBox = new System.Windows.Forms.TextBox();
            this.forceUseFirstKeyOnWaterways = new System.Windows.Forms.CheckBox();
            this.unlockAllColosseumTrials = new System.Windows.Forms.CheckBox();
            this.reusableCityCrestNoHardSave = new System.Windows.Forms.CheckBox();
            this.openLowerBeastsDenThroughShortcut = new System.Windows.Forms.CheckBox();
            this.RemoveBeastsDenHardSave = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDownX = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownY = new System.Windows.Forms.NumericUpDown();
            this.SkipDreamerTextBeforeDreamNail = new System.Windows.Forms.CheckBox();
            this.blockLegEaterDeath = new System.Windows.Forms.CheckBox();
            this.oneBlueHPLifebloodCoreDoor = new System.Windows.Forms.CheckBox();
            this.colo1Prompt = new System.Windows.Forms.CheckBox();
            this.colo2Prompt = new System.Windows.Forms.CheckBox();
            this.flowerQuestPrompt = new System.Windows.Forms.CheckBox();
            this.whitePalacePrompt = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.costUpDown)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.Placements.SuspendLayout();
            this.Settings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownY)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxItems
            // 
            this.comboBoxItems.FormattingEnabled = true;
            this.comboBoxItems.Location = new System.Drawing.Point(21, 32);
            this.comboBoxItems.Name = "comboBoxItems";
            this.comboBoxItems.Size = new System.Drawing.Size(121, 21);
            this.comboBoxItems.TabIndex = 0;
            // 
            // comboBoxLocations
            // 
            this.comboBoxLocations.FormattingEnabled = true;
            this.comboBoxLocations.Location = new System.Drawing.Point(168, 32);
            this.comboBoxLocations.Name = "comboBoxLocations";
            this.comboBoxLocations.Size = new System.Drawing.Size(121, 21);
            this.comboBoxLocations.TabIndex = 1;
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.Location = new System.Drawing.Point(463, 3);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(337, 450);
            this.listBox1.TabIndex = 2;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(115, 59);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 3;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // removeButton
            // 
            this.removeButton.Location = new System.Drawing.Point(115, 88);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(75, 23);
            this.removeButton.TabIndex = 4;
            this.removeButton.Text = "Remove";
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(340, 412);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(94, 23);
            this.saveButton.TabIndex = 5;
            this.saveButton.Text = "Save as XML";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // itemLabel
            // 
            this.itemLabel.AutoSize = true;
            this.itemLabel.Location = new System.Drawing.Point(21, 12);
            this.itemLabel.Name = "itemLabel";
            this.itemLabel.Size = new System.Drawing.Size(27, 13);
            this.itemLabel.TabIndex = 6;
            this.itemLabel.Text = "Item";
            // 
            // locationLabel
            // 
            this.locationLabel.AutoSize = true;
            this.locationLabel.Location = new System.Drawing.Point(168, 12);
            this.locationLabel.Name = "locationLabel";
            this.locationLabel.Size = new System.Drawing.Size(48, 13);
            this.locationLabel.TabIndex = 7;
            this.locationLabel.Text = "Location";
            // 
            // costUpDown
            // 
            this.costUpDown.Location = new System.Drawing.Point(312, 32);
            this.costUpDown.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.costUpDown.Name = "costUpDown";
            this.costUpDown.Size = new System.Drawing.Size(120, 20);
            this.costUpDown.TabIndex = 8;
            // 
            // labelCost
            // 
            this.labelCost.AutoSize = true;
            this.labelCost.Location = new System.Drawing.Point(309, 12);
            this.labelCost.Name = "labelCost";
            this.labelCost.Size = new System.Drawing.Size(125, 13);
            this.labelCost.TabIndex = 9;
            this.labelCost.Text = "Geo/Grub/Essence Cost";
            // 
            // checkBoxHideUsedLocations
            // 
            this.checkBoxHideUsedLocations.AutoSize = true;
            this.checkBoxHideUsedLocations.Checked = true;
            this.checkBoxHideUsedLocations.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxHideUsedLocations.Location = new System.Drawing.Point(21, 190);
            this.checkBoxHideUsedLocations.Name = "checkBoxHideUsedLocations";
            this.checkBoxHideUsedLocations.Size = new System.Drawing.Size(117, 17);
            this.checkBoxHideUsedLocations.TabIndex = 10;
            this.checkBoxHideUsedLocations.Text = "Hide filled locations";
            this.checkBoxHideUsedLocations.UseVisualStyleBackColor = true;
            this.checkBoxHideUsedLocations.CheckedChanged += new System.EventHandler(this.checkBoxHideUsedLocations_CheckedChanged);
            // 
            // hidePlacedItems
            // 
            this.hidePlacedItems.AutoSize = true;
            this.hidePlacedItems.Checked = true;
            this.hidePlacedItems.CheckState = System.Windows.Forms.CheckState.Checked;
            this.hidePlacedItems.Location = new System.Drawing.Point(21, 214);
            this.hidePlacedItems.Name = "hidePlacedItems";
            this.hidePlacedItems.Size = new System.Drawing.Size(110, 17);
            this.hidePlacedItems.TabIndex = 11;
            this.hidePlacedItems.Text = "Hide placed items";
            this.hidePlacedItems.UseVisualStyleBackColor = true;
            this.hidePlacedItems.CheckedChanged += new System.EventHandler(this.hidePlacedItems_CheckedChanged);
            // 
            // comboBoxDefaultShopItems
            // 
            this.comboBoxDefaultShopItems.FormattingEnabled = true;
            this.comboBoxDefaultShopItems.Location = new System.Drawing.Point(21, 414);
            this.comboBoxDefaultShopItems.Name = "comboBoxDefaultShopItems";
            this.comboBoxDefaultShopItems.Size = new System.Drawing.Size(121, 21);
            this.comboBoxDefaultShopItems.TabIndex = 12;
            this.comboBoxDefaultShopItems.SelectedIndexChanged += new System.EventHandler(this.comboBoxDefaultShopItems_SelectedIndexChanged);
            // 
            // defaultShopLabel
            // 
            this.defaultShopLabel.AutoSize = true;
            this.defaultShopLabel.Location = new System.Drawing.Point(5, 398);
            this.defaultShopLabel.Name = "defaultShopLabel";
            this.defaultShopLabel.Size = new System.Drawing.Size(163, 13);
            this.defaultShopLabel.TabIndex = 13;
            this.defaultShopLabel.Text = "Vanilla Shop Items (select preset)";
            // 
            // multiItemCounter
            // 
            this.multiItemCounter.AutoSize = true;
            this.multiItemCounter.Location = new System.Drawing.Point(180, 190);
            this.multiItemCounter.Name = "multiItemCounter";
            this.multiItemCounter.Size = new System.Drawing.Size(62, 13);
            this.multiItemCounter.TabIndex = 14;
            this.multiItemCounter.Text = "placeholder";
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(340, 354);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(94, 23);
            this.resetButton.TabIndex = 15;
            this.resetButton.Text = "Reset All";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(340, 383);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(94, 23);
            this.loadButton.TabIndex = 16;
            this.loadButton.Text = "Load from XML";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "XML files|*.xml";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "XML files|*.xml";
            // 
            // titleBox
            // 
            this.titleBox.CausesValidation = false;
            this.titleBox.Location = new System.Drawing.Point(21, 357);
            this.titleBox.Name = "titleBox";
            this.titleBox.Size = new System.Drawing.Size(121, 20);
            this.titleBox.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 340);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Title (shows ingame as version)";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.Placements);
            this.tabControl1.Controls.Add(this.Settings);
            this.tabControl1.Location = new System.Drawing.Point(12, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(811, 482);
            this.tabControl1.TabIndex = 19;
            // 
            // Placements
            // 
            this.Placements.Controls.Add(this.listBox1);
            this.Placements.Controls.Add(this.resetButton);
            this.Placements.Controls.Add(this.label1);
            this.Placements.Controls.Add(this.comboBoxItems);
            this.Placements.Controls.Add(this.titleBox);
            this.Placements.Controls.Add(this.comboBoxLocations);
            this.Placements.Controls.Add(this.loadButton);
            this.Placements.Controls.Add(this.addButton);
            this.Placements.Controls.Add(this.removeButton);
            this.Placements.Controls.Add(this.multiItemCounter);
            this.Placements.Controls.Add(this.saveButton);
            this.Placements.Controls.Add(this.defaultShopLabel);
            this.Placements.Controls.Add(this.itemLabel);
            this.Placements.Controls.Add(this.comboBoxDefaultShopItems);
            this.Placements.Controls.Add(this.locationLabel);
            this.Placements.Controls.Add(this.hidePlacedItems);
            this.Placements.Controls.Add(this.costUpDown);
            this.Placements.Controls.Add(this.checkBoxHideUsedLocations);
            this.Placements.Controls.Add(this.labelCost);
            this.Placements.Location = new System.Drawing.Point(4, 22);
            this.Placements.Name = "Placements";
            this.Placements.Padding = new System.Windows.Forms.Padding(3);
            this.Placements.Size = new System.Drawing.Size(803, 456);
            this.Placements.TabIndex = 0;
            this.Placements.Text = "Placements";
            this.Placements.UseVisualStyleBackColor = true;
            // 
            // Settings
            // 
            this.Settings.Controls.Add(this.label5);
            this.Settings.Controls.Add(this.whitePalacePrompt);
            this.Settings.Controls.Add(this.flowerQuestPrompt);
            this.Settings.Controls.Add(this.colo2Prompt);
            this.Settings.Controls.Add(this.colo1Prompt);
            this.Settings.Controls.Add(this.oneBlueHPLifebloodCoreDoor);
            this.Settings.Controls.Add(this.blockLegEaterDeath);
            this.Settings.Controls.Add(this.SkipDreamerTextBeforeDreamNail);
            this.Settings.Controls.Add(this.numericUpDownY);
            this.Settings.Controls.Add(this.numericUpDownX);
            this.Settings.Controls.Add(this.label4);
            this.Settings.Controls.Add(this.label3);
            this.Settings.Controls.Add(this.label2);
            this.Settings.Controls.Add(this.RemoveBeastsDenHardSave);
            this.Settings.Controls.Add(this.openLowerBeastsDenThroughShortcut);
            this.Settings.Controls.Add(this.reusableCityCrestNoHardSave);
            this.Settings.Controls.Add(this.unlockAllColosseumTrials);
            this.Settings.Controls.Add(this.forceUseFirstKeyOnWaterways);
            this.Settings.Controls.Add(this.startSceneNameBox);
            this.Settings.Controls.Add(this.changeStartCheckBox);
            this.Settings.Location = new System.Drawing.Point(4, 22);
            this.Settings.Name = "Settings";
            this.Settings.Padding = new System.Windows.Forms.Padding(3);
            this.Settings.Size = new System.Drawing.Size(803, 456);
            this.Settings.TabIndex = 1;
            this.Settings.Text = "Settings";
            this.Settings.UseVisualStyleBackColor = true;
            // 
            // changeStartCheckBox
            // 
            this.changeStartCheckBox.AutoSize = true;
            this.changeStartCheckBox.Location = new System.Drawing.Point(6, 6);
            this.changeStartCheckBox.Name = "changeStartCheckBox";
            this.changeStartCheckBox.Size = new System.Drawing.Size(113, 17);
            this.changeStartCheckBox.TabIndex = 0;
            this.changeStartCheckBox.Text = "Edit Start Location";
            this.changeStartCheckBox.UseVisualStyleBackColor = true;
            this.changeStartCheckBox.CheckedChanged += new System.EventHandler(this.changeStart_CheckedChanged);
            // 
            // startSceneNameBox
            // 
            this.startSceneNameBox.Enabled = false;
            this.startSceneNameBox.Location = new System.Drawing.Point(82, 31);
            this.startSceneNameBox.Name = "startSceneNameBox";
            this.startSceneNameBox.Size = new System.Drawing.Size(100, 20);
            this.startSceneNameBox.TabIndex = 1;
            // 
            // forceUseFirstKeyOnWaterways
            // 
            this.forceUseFirstKeyOnWaterways.AutoSize = true;
            this.forceUseFirstKeyOnWaterways.Checked = true;
            this.forceUseFirstKeyOnWaterways.CheckState = System.Windows.Forms.CheckState.Checked;
            this.forceUseFirstKeyOnWaterways.Location = new System.Drawing.Point(339, 30);
            this.forceUseFirstKeyOnWaterways.Name = "forceUseFirstKeyOnWaterways";
            this.forceUseFirstKeyOnWaterways.Size = new System.Drawing.Size(183, 17);
            this.forceUseFirstKeyOnWaterways.TabIndex = 4;
            this.forceUseFirstKeyOnWaterways.Text = "Force use first key on Waterways";
            this.forceUseFirstKeyOnWaterways.UseVisualStyleBackColor = true;
            // 
            // unlockAllColosseumTrials
            // 
            this.unlockAllColosseumTrials.AutoSize = true;
            this.unlockAllColosseumTrials.Checked = true;
            this.unlockAllColosseumTrials.CheckState = System.Windows.Forms.CheckState.Checked;
            this.unlockAllColosseumTrials.Location = new System.Drawing.Point(339, 54);
            this.unlockAllColosseumTrials.Name = "unlockAllColosseumTrials";
            this.unlockAllColosseumTrials.Size = new System.Drawing.Size(173, 17);
            this.unlockAllColosseumTrials.TabIndex = 5;
            this.unlockAllColosseumTrials.Text = "Auto-unlock all colosseum trials";
            this.unlockAllColosseumTrials.UseVisualStyleBackColor = true;
            // 
            // reusableCityCrestNoHardSave
            // 
            this.reusableCityCrestNoHardSave.AutoSize = true;
            this.reusableCityCrestNoHardSave.Checked = true;
            this.reusableCityCrestNoHardSave.CheckState = System.Windows.Forms.CheckState.Checked;
            this.reusableCityCrestNoHardSave.Location = new System.Drawing.Point(339, 78);
            this.reusableCityCrestNoHardSave.Name = "reusableCityCrestNoHardSave";
            this.reusableCityCrestNoHardSave.Size = new System.Drawing.Size(198, 17);
            this.reusableCityCrestNoHardSave.TabIndex = 6;
            this.reusableCityCrestNoHardSave.Text = "Reusable City Crest + No Hard Save";
            this.reusableCityCrestNoHardSave.UseVisualStyleBackColor = true;
            // 
            // openLowerBeastsDenThroughShortcut
            // 
            this.openLowerBeastsDenThroughShortcut.AutoSize = true;
            this.openLowerBeastsDenThroughShortcut.Checked = true;
            this.openLowerBeastsDenThroughShortcut.CheckState = System.Windows.Forms.CheckState.Checked;
            this.openLowerBeastsDenThroughShortcut.Location = new System.Drawing.Point(339, 102);
            this.openLowerBeastsDenThroughShortcut.Name = "openLowerBeastsDenThroughShortcut";
            this.openLowerBeastsDenThroughShortcut.Size = new System.Drawing.Size(220, 17);
            this.openLowerBeastsDenThroughShortcut.TabIndex = 7;
            this.openLowerBeastsDenThroughShortcut.Text = "Open lower Beast\'s Den through shortcut";
            this.openLowerBeastsDenThroughShortcut.UseVisualStyleBackColor = true;
            // 
            // RemoveBeastsDenHardSave
            // 
            this.RemoveBeastsDenHardSave.AutoSize = true;
            this.RemoveBeastsDenHardSave.Checked = true;
            this.RemoveBeastsDenHardSave.CheckState = System.Windows.Forms.CheckState.Checked;
            this.RemoveBeastsDenHardSave.Location = new System.Drawing.Point(339, 126);
            this.RemoveBeastsDenHardSave.Name = "RemoveBeastsDenHardSave";
            this.RemoveBeastsDenHardSave.Size = new System.Drawing.Size(176, 17);
            this.RemoveBeastsDenHardSave.TabIndex = 8;
            this.RemoveBeastsDenHardSave.Text = "Remove Beast\'s Den hard save";
            this.RemoveBeastsDenHardSave.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Scene name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(61, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(15, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "x:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(61, 126);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(15, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "y:";
            // 
            // numericUpDownX
            // 
            this.numericUpDownX.DecimalPlaces = 1;
            this.numericUpDownX.Enabled = false;
            this.numericUpDownX.Location = new System.Drawing.Point(82, 79);
            this.numericUpDownX.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numericUpDownX.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numericUpDownX.Name = "numericUpDownX";
            this.numericUpDownX.Size = new System.Drawing.Size(99, 20);
            this.numericUpDownX.TabIndex = 12;
            // 
            // numericUpDownY
            // 
            this.numericUpDownY.DecimalPlaces = 1;
            this.numericUpDownY.Enabled = false;
            this.numericUpDownY.Location = new System.Drawing.Point(82, 124);
            this.numericUpDownY.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numericUpDownY.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numericUpDownY.Name = "numericUpDownY";
            this.numericUpDownY.Size = new System.Drawing.Size(100, 20);
            this.numericUpDownY.TabIndex = 13;
            // 
            // SkipDreamerTextBeforeDreamNail
            // 
            this.SkipDreamerTextBeforeDreamNail.AutoSize = true;
            this.SkipDreamerTextBeforeDreamNail.Checked = true;
            this.SkipDreamerTextBeforeDreamNail.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SkipDreamerTextBeforeDreamNail.Location = new System.Drawing.Point(339, 150);
            this.SkipDreamerTextBeforeDreamNail.Name = "SkipDreamerTextBeforeDreamNail";
            this.SkipDreamerTextBeforeDreamNail.Size = new System.Drawing.Size(196, 17);
            this.SkipDreamerTextBeforeDreamNail.TabIndex = 14;
            this.SkipDreamerTextBeforeDreamNail.Text = "Skip dreamer text before Dream Nail";
            this.SkipDreamerTextBeforeDreamNail.UseVisualStyleBackColor = true;
            // 
            // blockLegEaterDeath
            // 
            this.blockLegEaterDeath.AutoSize = true;
            this.blockLegEaterDeath.Checked = true;
            this.blockLegEaterDeath.CheckState = System.Windows.Forms.CheckState.Checked;
            this.blockLegEaterDeath.Location = new System.Drawing.Point(339, 174);
            this.blockLegEaterDeath.Name = "blockLegEaterDeath";
            this.blockLegEaterDeath.Size = new System.Drawing.Size(162, 17);
            this.blockLegEaterDeath.TabIndex = 15;
            this.blockLegEaterDeath.Text = "Block Leg Eater death event";
            this.blockLegEaterDeath.UseVisualStyleBackColor = true;
            // 
            // oneBlueHPLifebloodCoreDoor
            // 
            this.oneBlueHPLifebloodCoreDoor.AutoSize = true;
            this.oneBlueHPLifebloodCoreDoor.Checked = true;
            this.oneBlueHPLifebloodCoreDoor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.oneBlueHPLifebloodCoreDoor.Location = new System.Drawing.Point(339, 198);
            this.oneBlueHPLifebloodCoreDoor.Name = "oneBlueHPLifebloodCoreDoor";
            this.oneBlueHPLifebloodCoreDoor.Size = new System.Drawing.Size(221, 17);
            this.oneBlueHPLifebloodCoreDoor.TabIndex = 16;
            this.oneBlueHPLifebloodCoreDoor.Text = "One blue HP to open Lifeblood Core door";
            this.oneBlueHPLifebloodCoreDoor.UseVisualStyleBackColor = true;
            // 
            // colo1Prompt
            // 
            this.colo1Prompt.AutoSize = true;
            this.colo1Prompt.Checked = true;
            this.colo1Prompt.CheckState = System.Windows.Forms.CheckState.Checked;
            this.colo1Prompt.Location = new System.Drawing.Point(339, 254);
            this.colo1Prompt.Name = "colo1Prompt";
            this.colo1Prompt.Size = new System.Drawing.Size(252, 17);
            this.colo1Prompt.TabIndex = 17;
            this.colo1Prompt.Text = "Show item name on prompt to start Colosseum 1";
            this.colo1Prompt.UseVisualStyleBackColor = true;
            // 
            // colo2Prompt
            // 
            this.colo2Prompt.AutoSize = true;
            this.colo2Prompt.Checked = true;
            this.colo2Prompt.CheckState = System.Windows.Forms.CheckState.Checked;
            this.colo2Prompt.Location = new System.Drawing.Point(339, 277);
            this.colo2Prompt.Name = "colo2Prompt";
            this.colo2Prompt.Size = new System.Drawing.Size(252, 17);
            this.colo2Prompt.TabIndex = 18;
            this.colo2Prompt.Text = "Show item name on prompt to start Colosseum 2";
            this.colo2Prompt.UseVisualStyleBackColor = true;
            // 
            // flowerQuestPrompt
            // 
            this.flowerQuestPrompt.AutoSize = true;
            this.flowerQuestPrompt.Checked = true;
            this.flowerQuestPrompt.CheckState = System.Windows.Forms.CheckState.Checked;
            this.flowerQuestPrompt.Location = new System.Drawing.Point(339, 300);
            this.flowerQuestPrompt.Name = "flowerQuestPrompt";
            this.flowerQuestPrompt.Size = new System.Drawing.Size(254, 17);
            this.flowerQuestPrompt.TabIndex = 19;
            this.flowerQuestPrompt.Text = "Show item name on prompt to start Flower Quest";
            this.flowerQuestPrompt.UseVisualStyleBackColor = true;
            // 
            // whitePalacePrompt
            // 
            this.whitePalacePrompt.AutoSize = true;
            this.whitePalacePrompt.Checked = true;
            this.whitePalacePrompt.CheckState = System.Windows.Forms.CheckState.Checked;
            this.whitePalacePrompt.Location = new System.Drawing.Point(339, 323);
            this.whitePalacePrompt.Name = "whitePalacePrompt";
            this.whitePalacePrompt.Size = new System.Drawing.Size(303, 17);
            this.whitePalacePrompt.TabIndex = 20;
            this.whitePalacePrompt.Text = "Show item name on inspect Kingsmould in Palace Grounds";
            this.whitePalacePrompt.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 437);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(588, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Note: the settings of this page will be saved to your xml, but they will not be r" +
    "eloaded if you later load the xml to this program.";
            // 
            // ItemPlacer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(835, 494);
            this.Controls.Add(this.tabControl1);
            this.Name = "ItemPlacer";
            this.Text = "ItemPlacer";
            ((System.ComponentModel.ISupportInitialize)(this.costUpDown)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.Placements.ResumeLayout(false);
            this.Placements.PerformLayout();
            this.Settings.ResumeLayout(false);
            this.Settings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownY)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxItems;
        private System.Windows.Forms.ComboBox comboBoxLocations;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Label itemLabel;
        private System.Windows.Forms.Label locationLabel;
        private System.Windows.Forms.NumericUpDown costUpDown;
        private System.Windows.Forms.Label labelCost;
        private System.Windows.Forms.CheckBox checkBoxHideUsedLocations;
        private System.Windows.Forms.CheckBox hidePlacedItems;
        private System.Windows.Forms.ComboBox comboBoxDefaultShopItems;
        private System.Windows.Forms.Label defaultShopLabel;
        private System.Windows.Forms.Label multiItemCounter;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TextBox titleBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage Placements;
        private System.Windows.Forms.TabPage Settings;
        private System.Windows.Forms.CheckBox whitePalacePrompt;
        private System.Windows.Forms.CheckBox flowerQuestPrompt;
        private System.Windows.Forms.CheckBox colo2Prompt;
        private System.Windows.Forms.CheckBox colo1Prompt;
        private System.Windows.Forms.CheckBox oneBlueHPLifebloodCoreDoor;
        private System.Windows.Forms.CheckBox blockLegEaterDeath;
        private System.Windows.Forms.CheckBox SkipDreamerTextBeforeDreamNail;
        private System.Windows.Forms.NumericUpDown numericUpDownY;
        private System.Windows.Forms.NumericUpDown numericUpDownX;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox RemoveBeastsDenHardSave;
        private System.Windows.Forms.CheckBox openLowerBeastsDenThroughShortcut;
        private System.Windows.Forms.CheckBox reusableCityCrestNoHardSave;
        private System.Windows.Forms.CheckBox unlockAllColosseumTrials;
        private System.Windows.Forms.CheckBox forceUseFirstKeyOnWaterways;
        private System.Windows.Forms.TextBox startSceneNameBox;
        private System.Windows.Forms.CheckBox changeStartCheckBox;
        private System.Windows.Forms.Label label5;
    }
}

