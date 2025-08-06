namespace Multiobject_Sorting
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.listViewResults = new System.Windows.Forms.ListView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblPLCStatus = new System.Windows.Forms.Label();
            this.btnPLCConnect = new System.Windows.Forms.Button();
            this.btnPLCConfig = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnLoadCalib = new System.Windows.Forms.Button();
            this.btnSaveCalib = new System.Windows.Forms.Button();
            this.btnPerformCalib = new System.Windows.Forms.Button();
            this.btnStartCalib = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAutoMode = new System.Windows.Forms.Button();
            this.btnDetect = new System.Windows.Forms.Button();
            this.btnHSVParams = new System.Windows.Forms.Button();
            this.btnGrabCamera = new System.Windows.Forms.Button();
            this.btnLoadImage = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hWindowControl1.BackColor = System.Drawing.Color.Gray;
            this.hWindowControl1.BorderColor = System.Drawing.Color.Gray;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.Location = new System.Drawing.Point(12, 12);
            this.hWindowControl1.Name = "hWindowControl1";
            this.hWindowControl1.Size = new System.Drawing.Size(640, 532);
            this.hWindowControl1.TabIndex = 0;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(640, 532);
            this.hWindowControl1.HMouseDown += new HalconDotNet.HMouseEventHandler(this.hWindowControl1_HMouseDown);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(6, 20);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.Size = new System.Drawing.Size(450, 200);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Location = new System.Drawing.Point(658, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(474, 480);
            this.panel1.TabIndex = 2;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.listViewResults);
            this.groupBox4.Location = new System.Drawing.Point(3, 421);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(468, 130);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "检测结果";
            // 
            // listViewResults
            // 
            this.listViewResults.FullRowSelect = true;
            this.listViewResults.GridLines = true;
            this.listViewResults.HideSelection = false;
            this.listViewResults.Location = new System.Drawing.Point(6, 20);
            this.listViewResults.Name = "listViewResults";
            this.listViewResults.Size = new System.Drawing.Size(450, 91);
            this.listViewResults.TabIndex = 3;
            this.listViewResults.UseCompatibleStateImageBehavior = false;
            this.listViewResults.View = System.Windows.Forms.View.Details;
            this.listViewResults.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lblPLCStatus);
            this.groupBox3.Controls.Add(this.btnPLCConnect);
            this.groupBox3.Controls.Add(this.btnPLCConfig);
            this.groupBox3.Location = new System.Drawing.Point(3, 355);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(468, 60);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "PLC通信";
            // 
            // lblPLCStatus
            // 
            this.lblPLCStatus.AutoSize = true;
            this.lblPLCStatus.Location = new System.Drawing.Point(200, 29);
            this.lblPLCStatus.Name = "lblPLCStatus";
            this.lblPLCStatus.Size = new System.Drawing.Size(97, 15);
            this.lblPLCStatus.TabIndex = 2;
            this.lblPLCStatus.Text = "状态：未连接";
            // 
            // btnPLCConnect
            // 
            this.btnPLCConnect.Location = new System.Drawing.Point(97, 20);
            this.btnPLCConnect.Name = "btnPLCConnect";
            this.btnPLCConnect.Size = new System.Drawing.Size(85, 30);
            this.btnPLCConnect.TabIndex = 1;
            this.btnPLCConnect.Text = "连接/断开";
            this.btnPLCConnect.UseVisualStyleBackColor = true;
            this.btnPLCConnect.Click += new System.EventHandler(this.btnPLCConnect_Click);
            // 
            // btnPLCConfig
            // 
            this.btnPLCConfig.Location = new System.Drawing.Point(6, 20);
            this.btnPLCConfig.Name = "btnPLCConfig";
            this.btnPLCConfig.Size = new System.Drawing.Size(85, 30);
            this.btnPLCConfig.TabIndex = 0;
            this.btnPLCConfig.Text = "通信配置";
            this.btnPLCConfig.UseVisualStyleBackColor = true;
            this.btnPLCConfig.Click += new System.EventHandler(this.btnPLCConfig_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataGridView1);
            this.groupBox2.Controls.Add(this.btnLoadCalib);
            this.groupBox2.Controls.Add(this.btnSaveCalib);
            this.groupBox2.Controls.Add(this.btnPerformCalib);
            this.groupBox2.Controls.Add(this.btnStartCalib);
            this.groupBox2.Location = new System.Drawing.Point(3, 89);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(468, 260);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "坐标标定";
            // 
            // btnLoadCalib
            // 
            this.btnLoadCalib.Location = new System.Drawing.Point(339, 226);
            this.btnLoadCalib.Name = "btnLoadCalib";
            this.btnLoadCalib.Size = new System.Drawing.Size(105, 28);
            this.btnLoadCalib.TabIndex = 5;
            this.btnLoadCalib.Text = "加载标定";
            this.btnLoadCalib.UseVisualStyleBackColor = true;
            this.btnLoadCalib.Click += new System.EventHandler(this.btnLoadCalib_Click);
            // 
            // btnSaveCalib
            // 
            this.btnSaveCalib.Location = new System.Drawing.Point(228, 226);
            this.btnSaveCalib.Name = "btnSaveCalib";
            this.btnSaveCalib.Size = new System.Drawing.Size(105, 28);
            this.btnSaveCalib.TabIndex = 4;
            this.btnSaveCalib.Text = "保存标定";
            this.btnSaveCalib.UseVisualStyleBackColor = true;
            this.btnSaveCalib.Click += new System.EventHandler(this.btnSaveCalib_Click);
            // 
            // btnPerformCalib
            // 
            this.btnPerformCalib.Location = new System.Drawing.Point(117, 226);
            this.btnPerformCalib.Name = "btnPerformCalib";
            this.btnPerformCalib.Size = new System.Drawing.Size(105, 28);
            this.btnPerformCalib.TabIndex = 3;
            this.btnPerformCalib.Text = "执行标定";
            this.btnPerformCalib.UseVisualStyleBackColor = true;
            this.btnPerformCalib.Click += new System.EventHandler(this.btnPerformCalib_Click);
            // 
            // btnStartCalib
            // 
            this.btnStartCalib.Location = new System.Drawing.Point(6, 226);
            this.btnStartCalib.Name = "btnStartCalib";
            this.btnStartCalib.Size = new System.Drawing.Size(105, 28);
            this.btnStartCalib.TabIndex = 2;
            this.btnStartCalib.Text = "开始标定";
            this.btnStartCalib.UseVisualStyleBackColor = true;
            this.btnStartCalib.Click += new System.EventHandler(this.btnStartCalib_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnAutoMode);
            this.groupBox1.Controls.Add(this.btnDetect);
            this.groupBox1.Controls.Add(this.btnHSVParams);
            this.groupBox1.Controls.Add(this.btnGrabCamera);
            this.groupBox1.Controls.Add(this.btnLoadImage);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(468, 80);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "图像获取与检测";
            // 
            // btnAutoMode
            // 
            this.btnAutoMode.Location = new System.Drawing.Point(370, 20);
            this.btnAutoMode.Name = "btnAutoMode";
            this.btnAutoMode.Size = new System.Drawing.Size(85, 50);
            this.btnAutoMode.TabIndex = 4;
            this.btnAutoMode.Text = "自动模式";
            this.btnAutoMode.UseVisualStyleBackColor = true;
            this.btnAutoMode.Click += new System.EventHandler(this.btnAutoMode_Click);
            // 
            // btnDetect
            // 
            this.btnDetect.Location = new System.Drawing.Point(279, 20);
            this.btnDetect.Name = "btnDetect";
            this.btnDetect.Size = new System.Drawing.Size(85, 50);
            this.btnDetect.TabIndex = 3;
            this.btnDetect.Text = "单次检测";
            this.btnDetect.UseVisualStyleBackColor = true;
            this.btnDetect.Click += new System.EventHandler(this.btnDetect_Click);
            // 
            // btnHSVParams
            // 
            this.btnHSVParams.Location = new System.Drawing.Point(188, 20);
            this.btnHSVParams.Name = "btnHSVParams";
            this.btnHSVParams.Size = new System.Drawing.Size(85, 50);
            this.btnHSVParams.TabIndex = 2;
            this.btnHSVParams.Text = "HSV参数";
            this.btnHSVParams.UseVisualStyleBackColor = true;
            this.btnHSVParams.Click += new System.EventHandler(this.btnHSVParams_Click);
            // 
            // btnGrabCamera
            // 
            this.btnGrabCamera.Location = new System.Drawing.Point(97, 20);
            this.btnGrabCamera.Name = "btnGrabCamera";
            this.btnGrabCamera.Size = new System.Drawing.Size(85, 50);
            this.btnGrabCamera.TabIndex = 1;
            this.btnGrabCamera.Text = "相机采集";
            this.btnGrabCamera.UseVisualStyleBackColor = true;
            this.btnGrabCamera.Click += new System.EventHandler(this.btnGrabCamera_Click);
            // 
            // btnLoadImage
            // 
            this.btnLoadImage.Location = new System.Drawing.Point(6, 20);
            this.btnLoadImage.Name = "btnLoadImage";
            this.btnLoadImage.Size = new System.Drawing.Size(85, 50);
            this.btnLoadImage.TabIndex = 0;
            this.btnLoadImage.Text = "加载图像";
            this.btnLoadImage.UseVisualStyleBackColor = true;
            this.btnLoadImage.Click += new System.EventHandler(this.btnLoadImage_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 581);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1144, 25);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            this.statusStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.statusStrip1_ItemClicked);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(69, 20);
            this.toolStripStatusLabel1.Text = "系统就绪";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1144, 606);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.hWindowControl1);
            this.Name = "Form1";
            this.Text = "多目标检测与分拣系统";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HalconDotNet.HWindowControl hWindowControl1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnAutoMode;
        private System.Windows.Forms.Button btnDetect;
        private System.Windows.Forms.Button btnHSVParams;
        private System.Windows.Forms.Button btnGrabCamera;
        private System.Windows.Forms.Button btnLoadImage;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnLoadCalib;
        private System.Windows.Forms.Button btnSaveCalib;
        private System.Windows.Forms.Button btnPerformCalib;
        private System.Windows.Forms.Button btnStartCalib;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lblPLCStatus;
        private System.Windows.Forms.Button btnPLCConnect;
        private System.Windows.Forms.Button btnPLCConfig;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ListView listViewResults;
    }
}

