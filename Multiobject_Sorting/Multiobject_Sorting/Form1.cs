using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using System.IO;

namespace Multiobject_Sorting
{
    public partial class Form1 : Form
    {
        private HalconWrapper halconWrapper;
        private Calibration calibration;
        private PLCCommunication plcComm;
        private HalconWrapper.HSVParameters hsvParams;
        private List<HalconWrapper.DetectionResult> lastDetectionResults;
        private bool isCalibrationMode = false;
        private bool isAutoMode = false;
        private System.Windows.Forms.Timer autoDetectionTimer;

        public Form1()
        {
            InitializeComponent();
            InitializeSystem();
            InitGridview();
            InitControls();
        }

        private void InitializeSystem()
        {
            try
            {
                // 初始化Halcon包装器
                halconWrapper = new HalconWrapper(hWindowControl1.HalconWindow);
                
                // 初始化标定系统
                calibration = new Calibration();
                
                // 初始化PLC通信
                plcComm = new PLCCommunication();
                plcComm.OnStatusChanged += (msg) => UpdateStatus($"PLC: {msg}");
                plcComm.OnDataSent += (msg) => UpdateStatus($"数据发送: {msg}");
                plcComm.OnError += (msg) => UpdateStatus($"错误: {msg}");

                // 初始化HSV参数
                hsvParams = new HalconWrapper.HSVParameters();
                
                // 初始化检测结果列表
                lastDetectionResults = new List<HalconWrapper.DetectionResult>();

                // 初始化自动检测定时器
                autoDetectionTimer = new System.Windows.Forms.Timer();
                autoDetectionTimer.Interval = 1000; // 1秒检测一次
                autoDetectionTimer.Tick += AutoDetectionTimer_Tick;

                UpdateStatus("系统初始化完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"系统初始化失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitGridview()
        {
            dataGridView1.ColumnCount = 4;  
            dataGridView1.Columns[0].Name = "图像x轴";
            dataGridView1.Columns[1].Name = "图像y轴";
            dataGridView1.Columns[2].Name = "现实x轴";
            dataGridView1.Columns[3].Name = "现实y轴";
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void InitControls()
        {
            // 初始化检测结果ListView
            listViewResults.Columns.Add("序号", 60);
            listViewResults.Columns.Add("X坐标", 80);
            listViewResults.Columns.Add("Y坐标", 80);
            listViewResults.Columns.Add("角度", 60);
            listViewResults.Columns.Add("面积", 80);
            listViewResults.Columns.Add("类型", 80);
        }

        // 按钮事件处理器
        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            LoadImage();
        }

        private void btnGrabCamera_Click(object sender, EventArgs e)
        {
            GrabFromCamera();
        }

        private void btnHSVParams_Click(object sender, EventArgs e)
        {
            SetHSVParameters();
        }

        private async void btnDetect_Click(object sender, EventArgs e)
        {
            await PerformDetection();
        }

        private void btnAutoMode_Click(object sender, EventArgs e)
        {
            ToggleAutoMode();
            btnAutoMode.Text = isAutoMode ? "停止自动" : "自动模式";
            btnAutoMode.BackColor = isAutoMode ? Color.Red : SystemColors.Control;
        }

        private void btnStartCalib_Click(object sender, EventArgs e)
        {
            StartCalibration();
        }

        private void btnPerformCalib_Click(object sender, EventArgs e)
        {
            PerformCalibration();
        }

        private void btnSaveCalib_Click(object sender, EventArgs e)
        {
            SaveCalibration();
        }

        private void btnLoadCalib_Click(object sender, EventArgs e)
        {
            LoadCalibration();
        }

        private void btnPLCConfig_Click(object sender, EventArgs e)
        {
            SetupPLCCommunication();
        }

        private async void btnPLCConnect_Click(object sender, EventArgs e)
        {
            if (plcComm.IsConnected)
            {
                plcComm.Disconnect();
                btnPLCConnect.Text = "连接";
                lblPLCStatus.Text = "状态：未连接";
                lblPLCStatus.ForeColor = Color.Red;
            }
            else
            {
                // 这里应该根据配置连接PLC
                // 示例：await plcComm.ConnectAsync("192.168.1.100", 502);
                btnPLCConnect.Text = "断开";
                lblPLCStatus.Text = "状态：已连接";
                lblPLCStatus.ForeColor = Color.Green;
            }
        }

        private async void AutoDetectionTimer_Tick(object sender, EventArgs e)
        {
            if (isAutoMode && !isCalibrationMode)
            {
                await PerformDetection();
            }
        }

        // 图像加载
        private void LoadImage()
        {
            try
            {
                var openFileDialog = new OpenFileDialog
                {
                    Filter = "图像文件|*.bmp;*.jpg;*.jpeg;*.png;*.tiff|所有文件|*.*",
                    Title = "选择图像文件"
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    halconWrapper.LoadImage(openFileDialog.FileName);
                    UpdateStatus($"已加载图像: {Path.GetFileName(openFileDialog.FileName)}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载图像失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 从相机获取图像
        private void GrabFromCamera()
        {
            try
            {
                halconWrapper.LoadImage(); // 无参数表示从相机获取
                UpdateStatus("已从相机获取图像");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"从相机获取图像失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 设置HSV参数
        private void SetHSVParameters()
        {
            var hsvForm = new HSVParameterForm(hsvParams);
            if (hsvForm.ShowDialog() == DialogResult.OK)
            {
                hsvParams = hsvForm.HSVParameters;
                UpdateStatus("HSV参数已更新");
            }
        }

        // 执行检测
        private async Task PerformDetection()
        {
            try
            {
                // HSV颜色分割
                var regions = halconWrapper.HSVColorSegmentation(hsvParams);
                
                // 多目标检测
                lastDetectionResults = halconWrapper.DetectMultipleObjects(regions);

                // 更新检测结果显示
                UpdateDetectionResults();

                // 如果已标定，转换为真实世界坐标并发送到PLC
                if (calibration.IsCalibrated())
                {
                    var realWorldResults = ConvertToRealWorldCoordinates(lastDetectionResults);
                    await SendToPLC(realWorldResults);
                }

                UpdateStatus($"检测完成，发现{lastDetectionResults.Count}个目标");
            }
            catch (Exception ex)
            {
                UpdateStatus($"检测失败: {ex.Message}");
            }
        }

        // 转换为真实世界坐标
        private List<HalconWrapper.DetectionResult> ConvertToRealWorldCoordinates(List<HalconWrapper.DetectionResult> results)
        {
            var convertedResults = new List<HalconWrapper.DetectionResult>();
            
            foreach (var result in results)
            {
                var realCoord = calibration.ImageToReal(result.CenterX, result.CenterY);
                var convertedResult = new HalconWrapper.DetectionResult
                {
                    CenterX = realCoord.X,
                    CenterY = realCoord.Y,
                    Angle = result.Angle,
                    Area = result.Area,
                    ObjectType = result.ObjectType,
                    Width = result.Width,
                    Height = result.Height
                };
                convertedResults.Add(convertedResult);
            }
            
            return convertedResults;
        }

        // 发送到PLC
        private async Task SendToPLC(List<HalconWrapper.DetectionResult> results)
        {
            try
            {
                if (plcComm.IsConnected)
                {
                    await plcComm.SendDetectionResultsAsync(results);
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"发送到PLC失败: {ex.Message}");
            }
        }

        // 更新检测结果显示
        private void UpdateDetectionResults()
        {
            listViewResults.Items.Clear();
            
            for (int i = 0; i < lastDetectionResults.Count; i++)
            {
                var result = lastDetectionResults[i];
                var item = new ListViewItem((i + 1).ToString());
                item.SubItems.Add(result.CenterX.ToString("F2"));
                item.SubItems.Add(result.CenterY.ToString("F2"));
                item.SubItems.Add(result.Angle.ToString("F2"));
                item.SubItems.Add(result.Area.ToString("F0"));
                item.SubItems.Add(result.ObjectType);
                
                listViewResults.Items.Add(item);
            }
        }

        // 标定相关方法
        private void StartCalibration()
        {
            isCalibrationMode = true;
            calibration.ClearCalibrationPoints();
            UpdateCalibrationGrid();
            UpdateStatus("标定模式已启动，请点击图像中的标定点");
        }

        private void AddCalibrationPoint(double imageX, double imageY)
        {
            if (!isCalibrationMode) return;

            var form = new CalibrationPointForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                calibration.AddCalibrationPoint(imageX, imageY, form.RealX, form.RealY);
                UpdateCalibrationGrid();
                UpdateStatus($"已添加标定点 {calibration.GetCalibrationPointCount()}/9");
            }
        }

        private void PerformCalibration()
        {
            try
            {
                if (calibration.GetCalibrationPointCount() < 4)
                {
                    MessageBox.Show("至少需要4个标定点才能进行标定", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (calibration.PerformCalibration())
                {
                    isCalibrationMode = false;
                    var accuracy = calibration.CalculateCalibrationAccuracy();
                    UpdateStatus($"标定完成，精度: {accuracy:F2}mm");
                    MessageBox.Show($"标定成功！\n精度: {accuracy:F2}mm", "标定结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("标定失败，请检查标定点数据", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"标定过程中出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateCalibrationGrid()
        {
            dataGridView1.Rows.Clear();
            var points = calibration.GetCalibrationPoints();
            
            foreach (var point in points)
            {
                dataGridView1.Rows.Add(point.ImageX.ToString("F2"), point.ImageY.ToString("F2"), 
                                      point.RealX.ToString("F2"), point.RealY.ToString("F2"));
            }
        }

        private void SaveCalibration()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "标定文件|*.xml",
                Title = "保存标定数据"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    calibration.SaveCalibrationData(saveFileDialog.FileName);
                    UpdateStatus("标定数据已保存");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"保存标定数据失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadCalibration()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "标定文件|*.xml",
                Title = "加载标定数据"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    calibration.LoadCalibrationData(openFileDialog.FileName);
                    UpdateCalibrationGrid();
                    UpdateStatus("标定数据已加载");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"加载标定数据失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // PLC通信设置
        private void SetupPLCCommunication()
        {
            var plcForm = new PLCConfigForm(plcComm);
            plcForm.ShowDialog();
        }

        // 自动模式控制
        private void ToggleAutoMode()
        {
            isAutoMode = !isAutoMode;
            if (isAutoMode)
            {
                autoDetectionTimer.Start();
                UpdateStatus("自动检测模式已启动");
            }
            else
            {
                autoDetectionTimer.Stop();
                UpdateStatus("自动检测模式已停止");
            }
        }

        // 状态更新
        private void UpdateStatus(string message)
        {
            toolStripStatusLabel1.Text = $"{DateTime.Now:HH:mm:ss} - {message}";
            this.Text = $"多目标检测与分拣系统 - {message}";
        }

        // 鼠标点击事件处理（用于标定）
        private void hWindowControl1_HMouseDown(object sender, HalconDotNet.HMouseEventArgs e)
        {
            if (isCalibrationMode)
            {
                AddCalibrationPoint(e.X, e.Y);
            }
        }

        // 窗体关闭事件
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                autoDetectionTimer?.Stop();
                plcComm?.Dispose();
                halconWrapper?.Dispose();
            }
            catch (Exception ex)
            {
                // 记录错误但不阻止关闭
                System.Diagnostics.Debug.WriteLine($"关闭程序时出错: {ex.Message}");
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 保留原有的事件处理器
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // 保留原有的事件处理器
        }
    }

    // HSV参数设置窗体
    public partial class HSVParameterForm : Form
    {
        public HalconWrapper.HSVParameters HSVParameters { get; private set; }

        public HSVParameterForm(HalconWrapper.HSVParameters currentParams)
        {
            InitializeComponent();
            HSVParameters = currentParams;
            LoadParameters();
        }

        private void LoadParameters()
        {
            // 这里应该加载参数到界面控件
        }

        private void SaveParameters()
        {
            // 这里应该从界面控件保存参数
        }
    }

    // 标定点输入窗体
    public partial class CalibrationPointForm : Form
    {
        public double RealX { get; private set; }
        public double RealY { get; private set; }

        public CalibrationPointForm()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            // 从文本框获取真实世界坐标
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }

    // PLC配置窗体
    public partial class PLCConfigForm : Form
    {
        private PLCCommunication plcComm;

        public PLCConfigForm(PLCCommunication communication)
        {
            InitializeComponent();
            plcComm = communication;
        }

        private async void ConnectButton_Click(object sender, EventArgs e)
        {
            // 根据选择的通信类型连接PLC
        }
    }
}
