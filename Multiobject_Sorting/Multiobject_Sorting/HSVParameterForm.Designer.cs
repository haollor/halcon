namespace Multiobject_Sorting
{
    partial class HSVParameterForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.trackBarHueMax = new System.Windows.Forms.TrackBar();
            this.trackBarHueMin = new System.Windows.Forms.TrackBar();
            this.numericUpDownHueMax = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownHueMin = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.trackBarSatMax = new System.Windows.Forms.TrackBar();
            this.trackBarSatMin = new System.Windows.Forms.TrackBar();
            this.numericUpDownSatMax = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownSatMin = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.trackBarValueMax = new System.Windows.Forms.TrackBar();
            this.trackBarValueMin = new System.Windows.Forms.TrackBar();
            this.numericUpDownValueMax = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownValueMin = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.numericUpDownMaxArea = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownMinArea = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarHueMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarHueMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHueMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHueMin)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSatMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSatMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSatMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSatMin)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarValueMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarValueMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownValueMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownValueMin)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinArea)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.trackBarHueMax);
            this.groupBox1.Controls.Add(this.trackBarHueMin);
            this.groupBox1.Controls.Add(this.numericUpDownHueMax);
            this.groupBox1.Controls.Add(this.numericUpDownHueMin);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(400, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "色调 (Hue)";
            // 
            // trackBarHueMax
            // 
            this.trackBarHueMax.Location = new System.Drawing.Point(120, 55);
            this.trackBarHueMax.Maximum = 180;
            this.trackBarHueMax.Name = "trackBarHueMax";
            this.trackBarHueMax.Size = new System.Drawing.Size(200, 45);
            this.trackBarHueMax.TabIndex = 5;
            this.trackBarHueMax.TickFrequency = 20;
            this.trackBarHueMax.Value = 180;
            this.trackBarHueMax.Scroll += new System.EventHandler(this.trackBarHueMax_Scroll);
            // 
            // trackBarHueMin
            // 
            this.trackBarHueMin.Location = new System.Drawing.Point(120, 20);
            this.trackBarHueMin.Maximum = 180;
            this.trackBarHueMin.Name = "trackBarHueMin";
            this.trackBarHueMin.Size = new System.Drawing.Size(200, 45);
            this.trackBarHueMin.TabIndex = 4;
            this.trackBarHueMin.TickFrequency = 20;
            this.trackBarHueMin.Scroll += new System.EventHandler(this.trackBarHueMin_Scroll);
            // 
            // numericUpDownHueMax
            // 
            this.numericUpDownHueMax.Location = new System.Drawing.Point(330, 60);
            this.numericUpDownHueMax.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.numericUpDownHueMax.Name = "numericUpDownHueMax";
            this.numericUpDownHueMax.Size = new System.Drawing.Size(60, 22);
            this.numericUpDownHueMax.TabIndex = 3;
            this.numericUpDownHueMax.Value = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.numericUpDownHueMax.ValueChanged += new System.EventHandler(this.numericUpDownHueMax_ValueChanged);
            // 
            // numericUpDownHueMin
            // 
            this.numericUpDownHueMin.Location = new System.Drawing.Point(330, 25);
            this.numericUpDownHueMin.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.numericUpDownHueMin.Name = "numericUpDownHueMin";
            this.numericUpDownHueMin.Size = new System.Drawing.Size(60, 22);
            this.numericUpDownHueMin.TabIndex = 2;
            this.numericUpDownHueMin.ValueChanged += new System.EventHandler(this.numericUpDownHueMin_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "最大值：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "最小值：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.trackBarSatMax);
            this.groupBox2.Controls.Add(this.trackBarSatMin);
            this.groupBox2.Controls.Add(this.numericUpDownSatMax);
            this.groupBox2.Controls.Add(this.numericUpDownSatMin);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(12, 125);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(400, 100);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "饱和度 (Saturation)";
            // 
            // trackBarSatMax
            // 
            this.trackBarSatMax.Location = new System.Drawing.Point(120, 55);
            this.trackBarSatMax.Maximum = 255;
            this.trackBarSatMax.Name = "trackBarSatMax";
            this.trackBarSatMax.Size = new System.Drawing.Size(200, 45);
            this.trackBarSatMax.TabIndex = 5;
            this.trackBarSatMax.TickFrequency = 25;
            this.trackBarSatMax.Value = 255;
            this.trackBarSatMax.Scroll += new System.EventHandler(this.trackBarSatMax_Scroll);
            // 
            // trackBarSatMin
            // 
            this.trackBarSatMin.Location = new System.Drawing.Point(120, 20);
            this.trackBarSatMin.Maximum = 255;
            this.trackBarSatMin.Name = "trackBarSatMin";
            this.trackBarSatMin.Size = new System.Drawing.Size(200, 45);
            this.trackBarSatMin.TabIndex = 4;
            this.trackBarSatMin.TickFrequency = 25;
            this.trackBarSatMin.Scroll += new System.EventHandler(this.trackBarSatMin_Scroll);
            // 
            // numericUpDownSatMax
            // 
            this.numericUpDownSatMax.Location = new System.Drawing.Point(330, 60);
            this.numericUpDownSatMax.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownSatMax.Name = "numericUpDownSatMax";
            this.numericUpDownSatMax.Size = new System.Drawing.Size(60, 22);
            this.numericUpDownSatMax.TabIndex = 3;
            this.numericUpDownSatMax.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownSatMax.ValueChanged += new System.EventHandler(this.numericUpDownSatMax_ValueChanged);
            // 
            // numericUpDownSatMin
            // 
            this.numericUpDownSatMin.Location = new System.Drawing.Point(330, 25);
            this.numericUpDownSatMin.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownSatMin.Name = "numericUpDownSatMin";
            this.numericUpDownSatMin.Size = new System.Drawing.Size(60, 22);
            this.numericUpDownSatMin.TabIndex = 2;
            this.numericUpDownSatMin.ValueChanged += new System.EventHandler(this.numericUpDownSatMin_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 15);
            this.label3.TabIndex = 1;
            this.label3.Text = "最大值：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 15);
            this.label4.TabIndex = 0;
            this.label4.Text = "最小值：";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.trackBarValueMax);
            this.groupBox3.Controls.Add(this.trackBarValueMin);
            this.groupBox3.Controls.Add(this.numericUpDownValueMax);
            this.groupBox3.Controls.Add(this.numericUpDownValueMin);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Location = new System.Drawing.Point(12, 238);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(400, 100);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "明度 (Value)";
            // 
            // trackBarValueMax
            // 
            this.trackBarValueMax.Location = new System.Drawing.Point(120, 55);
            this.trackBarValueMax.Maximum = 255;
            this.trackBarValueMax.Name = "trackBarValueMax";
            this.trackBarValueMax.Size = new System.Drawing.Size(200, 45);
            this.trackBarValueMax.TabIndex = 5;
            this.trackBarValueMax.TickFrequency = 25;
            this.trackBarValueMax.Value = 255;
            this.trackBarValueMax.Scroll += new System.EventHandler(this.trackBarValueMax_Scroll);
            // 
            // trackBarValueMin
            // 
            this.trackBarValueMin.Location = new System.Drawing.Point(120, 20);
            this.trackBarValueMin.Maximum = 255;
            this.trackBarValueMin.Name = "trackBarValueMin";
            this.trackBarValueMin.Size = new System.Drawing.Size(200, 45);
            this.trackBarValueMin.TabIndex = 4;
            this.trackBarValueMin.TickFrequency = 25;
            this.trackBarValueMin.Scroll += new System.EventHandler(this.trackBarValueMin_Scroll);
            // 
            // numericUpDownValueMax
            // 
            this.numericUpDownValueMax.Location = new System.Drawing.Point(330, 60);
            this.numericUpDownValueMax.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownValueMax.Name = "numericUpDownValueMax";
            this.numericUpDownValueMax.Size = new System.Drawing.Size(60, 22);
            this.numericUpDownValueMax.TabIndex = 3;
            this.numericUpDownValueMax.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownValueMax.ValueChanged += new System.EventHandler(this.numericUpDownValueMax_ValueChanged);
            // 
            // numericUpDownValueMin
            // 
            this.numericUpDownValueMin.Location = new System.Drawing.Point(330, 25);
            this.numericUpDownValueMin.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownValueMin.Name = "numericUpDownValueMin";
            this.numericUpDownValueMin.Size = new System.Drawing.Size(60, 22);
            this.numericUpDownValueMin.TabIndex = 2;
            this.numericUpDownValueMin.ValueChanged += new System.EventHandler(this.numericUpDownValueMin_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 65);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 15);
            this.label5.TabIndex = 1;
            this.label5.Text = "最大值：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 30);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 15);
            this.label6.TabIndex = 0;
            this.label6.Text = "最小值：";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.numericUpDownMaxArea);
            this.groupBox4.Controls.Add(this.numericUpDownMinArea);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Location = new System.Drawing.Point(12, 351);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(400, 80);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "面积过滤";
            // 
            // numericUpDownMaxArea
            // 
            this.numericUpDownMaxArea.Location = new System.Drawing.Point(120, 50);
            this.numericUpDownMaxArea.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDownMaxArea.Name = "numericUpDownMaxArea";
            this.numericUpDownMaxArea.Size = new System.Drawing.Size(100, 22);
            this.numericUpDownMaxArea.TabIndex = 3;
            this.numericUpDownMaxArea.Value = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            // 
            // numericUpDownMinArea
            // 
            this.numericUpDownMinArea.Location = new System.Drawing.Point(120, 25);
            this.numericUpDownMinArea.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDownMinArea.Name = "numericUpDownMinArea";
            this.numericUpDownMinArea.Size = new System.Drawing.Size(100, 22);
            this.numericUpDownMinArea.TabIndex = 2;
            this.numericUpDownMinArea.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 55);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 15);
            this.label7.TabIndex = 1;
            this.label7.Text = "最大面积：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 30);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(82, 15);
            this.label8.TabIndex = 0;
            this.label8.Text = "最小面积：";
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(200, 450);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 30);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(290, 450);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(110, 450);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 30);
            this.btnReset.TabIndex = 6;
            this.btnReset.Text = "重置";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // HSVParameterForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(424, 492);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HSVParameterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "HSV参数设置";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarHueMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarHueMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHueMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHueMin)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSatMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSatMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSatMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSatMin)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarValueMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarValueMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownValueMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownValueMin)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinArea)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TrackBar trackBarHueMax;
        private System.Windows.Forms.TrackBar trackBarHueMin;
        private System.Windows.Forms.NumericUpDown numericUpDownHueMax;
        private System.Windows.Forms.NumericUpDown numericUpDownHueMin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TrackBar trackBarSatMax;
        private System.Windows.Forms.TrackBar trackBarSatMin;
        private System.Windows.Forms.NumericUpDown numericUpDownSatMax;
        private System.Windows.Forms.NumericUpDown numericUpDownSatMin;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TrackBar trackBarValueMax;
        private System.Windows.Forms.TrackBar trackBarValueMin;
        private System.Windows.Forms.NumericUpDown numericUpDownValueMax;
        private System.Windows.Forms.NumericUpDown numericUpDownValueMin;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.NumericUpDown numericUpDownMaxArea;
        private System.Windows.Forms.NumericUpDown numericUpDownMinArea;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnReset;
    }
}