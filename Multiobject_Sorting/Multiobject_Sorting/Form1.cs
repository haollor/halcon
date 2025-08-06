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

                // 添加窗口大小变化事件处理
                this.Resize += Form1_Resize;
                hWindowControl1.Resize += HWindowControl1_Resize;

                UpdateStatus("系统初始化完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"系统初始化失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            // 当主窗体大小变化时，刷新图像显示
            if (halconWrapper != null)
            {
                try
                {
                    halconWrapper.RefreshDisplay();
                }
                catch { }
            }
        }

        private void HWindowControl1_Resize(object sender, EventArgs e)
        {
            // 当HWindowControl大小变化时，刷新图像显示
            if (halconWrapper != null)
            {
                try
                {
                    // 延迟刷新，确保控件完全调整大小后再显示
                    var timer = new System.Windows.Forms.Timer();
                    timer.Interval = 100;
                    timer.Tick += (s, args) =>
                    {
                        halconWrapper.RefreshDisplay();
                        timer.Stop();
                        timer.Dispose();
                    };
                    timer.Start();
                }
                catch { }
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
            listViewResults.Columns.Add("序号", 50);
            listViewResults.Columns.Add("X坐标", 75);
            listViewResults.Columns.Add("Y坐标", 75);
            listViewResults.Columns.Add("角度", 60);
            listViewResults.Columns.Add("面积", 70);
            listViewResults.Columns.Add("类型", 80);
            
            // 设置ListView的详细属性
            listViewResults.FullRowSelect = true;
            listViewResults.GridLines = true;
            listViewResults.MultiSelect = false;
            listViewResults.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            listViewResults.Scrollable = true;
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
            var hsvForm = new HSVParameterForm(hsvParams, halconWrapper);
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
            try
            {
                if (isCalibrationMode)
                {
                    // 在图像上标记点击位置
                    if (halconWrapper != null)
                    {
                        // 绘制一个小十字标记选择的点
                        var hWindow = hWindowControl1.HalconWindow;
                        HOperatorSet.SetColor(hWindow, "red");
                        HOperatorSet.SetLineWidth(hWindow, 2);
                        HOperatorSet.DispCross(hWindow, e.Y, e.X, 12, 0);
                        
                        // 显示坐标信息
                        string coordText = $"({e.X:F1},{e.Y:F1})";
                        HOperatorSet.SetColor(hWindow, "yellow");
                        HOperatorSet.DispText(hWindow, coordText, "window", e.Y + 15, e.X - 30, "black", "box", "false");
                    }
                    
                    AddCalibrationPoint(e.X, e.Y);
                    UpdateStatus($"选择标定点: 图像坐标({e.X:F1}, {e.Y:F1})");
                }
                else
                {
                    // 非标定模式下，显示点击位置的坐标
                    UpdateStatus($"图像坐标: ({e.X:F1}, {e.Y:F1})");
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"鼠标点击处理出错: {ex.Message}");
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
            // 当选择检测结果列表中的项目时，在图像中高亮显示对应的检测对象
            if (listViewResults.SelectedItems.Count > 0)
            {
                try
                {
                    int selectedIndex = listViewResults.SelectedItems[0].Index;
                    if (selectedIndex >= 0 && selectedIndex < lastDetectionResults.Count)
                    {
                        var selectedResult = lastDetectionResults[selectedIndex];
                        
                        // 在状态栏显示选中对象的详细信息
                        UpdateStatus($"选中对象 #{selectedIndex + 1}: " +
                                   $"位置({selectedResult.CenterX:F2}, {selectedResult.CenterY:F2}), " +
                                   $"角度{selectedResult.Angle:F2}°, " +
                                   $"面积{selectedResult.Area:F0}, " +
                                   $"类型{selectedResult.ObjectType}");
                        
                        // 如果有Halcon显示窗口，可以在图像中高亮显示选中的对象
                        if (halconWrapper != null)
                        {
                            halconWrapper.HighlightDetectedObject(selectedResult);
                        }
                    }
                }
                catch (Exception ex)
                {
                    UpdateStatus($"选择项目时出错: {ex.Message}");
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // 处理标定数据网格的单元格点击事件
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                try
                {
                    var selectedRow = dataGridView1.Rows[e.RowIndex];
                    
                    // 获取当前行的标定点数据
                    if (selectedRow.Cells.Count >= 4)
                    {
                        double imageX = double.Parse(selectedRow.Cells[0].Value?.ToString() ?? "0");
                        double imageY = double.Parse(selectedRow.Cells[1].Value?.ToString() ?? "0");
                        double realX = double.Parse(selectedRow.Cells[2].Value?.ToString() ?? "0");
                        double realY = double.Parse(selectedRow.Cells[3].Value?.ToString() ?? "0");
                        
                        UpdateStatus($"选中标定点: 图像坐标({imageX:F2}, {imageY:F2}) -> 真实坐标({realX:F2}, {realY:F2})");
                        
                        // 显示上下文菜单，允许删除或编辑标定点
                        var result = MessageBox.Show(
                            $"标定点操作：\n图像坐标: ({imageX:F2}, {imageY:F2})\n真实坐标: ({realX:F2}, {realY:F2})\n\n点击'是'删除，'否'编辑，'取消'退出",
                            "标定点操作",
                            MessageBoxButtons.YesNoCancel,
                            MessageBoxIcon.Question);
                            
                        if (result == DialogResult.Yes)
                        {
                            // 删除标定点
                            calibration.RemoveCalibrationPoint(e.RowIndex);
                            UpdateCalibrationGrid();
                            UpdateStatus($"已删除标定点 #{e.RowIndex + 1}");
                        }
                        else if (result == DialogResult.No)
                        {
                            // 编辑标定点
                            var form = new CalibrationPointForm(realX, realY);
                            if (form.ShowDialog() == DialogResult.OK)
                            {
                                calibration.UpdateCalibrationPoint(e.RowIndex, imageX, imageY, form.RealX, form.RealY);
                                UpdateCalibrationGrid();
                                UpdateStatus($"已更新标定点 #{e.RowIndex + 1}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    UpdateStatus($"处理标定点时出错: {ex.Message}");
                }
            }
        }
    }

    // HSV参数设置窗体
    public partial class HSVParameterForm : Form
    {
        public HalconWrapper.HSVParameters HSVParameters { get; private set; }

        private TrackBar trackBarHueMin, trackBarHueMax;
        private TrackBar trackBarSatMin, trackBarSatMax;
        private TrackBar trackBarValMin, trackBarValMax;
        private TextBox textBoxMinArea, textBoxMaxArea;
        private Label lblHueMin, lblHueMax, lblSatMin, lblSatMax, lblValMin, lblValMax;
        private Button btnOK, btnCancel, btnPreview, btnReset;
        private HalconWrapper parentHalconWrapper;  // 引用父窗口的HalconWrapper

        public HSVParameterForm(HalconWrapper.HSVParameters currentParams, HalconWrapper halconWrapper = null)
        {
            HSVParameters = new HalconWrapper.HSVParameters
            {
                HueMin = currentParams.HueMin,
                HueMax = currentParams.HueMax,
                SaturationMin = currentParams.SaturationMin,
                SaturationMax = currentParams.SaturationMax,
                ValueMin = currentParams.ValueMin,
                ValueMax = currentParams.ValueMax,
                MinArea = currentParams.MinArea,
                MaxArea = currentParams.MaxArea
            };
            parentHalconWrapper = halconWrapper;
            InitializeComponent();
            LoadParameters();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // 设置窗体属性
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 400);
            this.Name = "HSVParameterForm";
            this.Text = "HSV参数设置";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            int yPos = 20;
            int labelWidth = 80;
            int trackBarWidth = 200;
            int valueWidth = 60;

            // Hue参数
            var lblHue = new Label { Text = "色调(H):", Location = new Point(20, yPos), Size = new Size(labelWidth, 20) };
            this.Controls.Add(lblHue);

            lblHueMin = new Label { Text = "0", Location = new Point(20 + labelWidth + 10, yPos + 25), Size = new Size(valueWidth, 20) };
            trackBarHueMin = new TrackBar { Location = new Point(20 + labelWidth + 10 + valueWidth, yPos + 20), Size = new Size(trackBarWidth, 30), Minimum = 0, Maximum = 180, TickFrequency = 20 };
            lblHueMax = new Label { Text = "180", Location = new Point(20 + labelWidth + 10 + valueWidth + trackBarWidth + 10, yPos + 25), Size = new Size(valueWidth, 20) };
            trackBarHueMax = new TrackBar { Location = new Point(20 + labelWidth + 10, yPos + 50), Size = new Size(trackBarWidth + valueWidth, 30), Minimum = 0, Maximum = 180, TickFrequency = 20 };

            trackBarHueMin.ValueChanged += (s, e) => { lblHueMin.Text = trackBarHueMin.Value.ToString(); HSVParameters.HueMin = trackBarHueMin.Value; };
            trackBarHueMax.ValueChanged += (s, e) => { lblHueMax.Text = trackBarHueMax.Value.ToString(); HSVParameters.HueMax = trackBarHueMax.Value; };

            this.Controls.AddRange(new Control[] { lblHueMin, trackBarHueMin, lblHueMax, trackBarHueMax });
            yPos += 90;

            // Saturation参数
            var lblSat = new Label { Text = "饱和度(S):", Location = new Point(20, yPos), Size = new Size(labelWidth, 20) };
            this.Controls.Add(lblSat);

            lblSatMin = new Label { Text = "0", Location = new Point(20 + labelWidth + 10, yPos + 25), Size = new Size(valueWidth, 20) };
            trackBarSatMin = new TrackBar { Location = new Point(20 + labelWidth + 10 + valueWidth, yPos + 20), Size = new Size(trackBarWidth, 30), Minimum = 0, Maximum = 255, TickFrequency = 50 };
            lblSatMax = new Label { Text = "255", Location = new Point(20 + labelWidth + 10 + valueWidth + trackBarWidth + 10, yPos + 25), Size = new Size(valueWidth, 20) };
            trackBarSatMax = new TrackBar { Location = new Point(20 + labelWidth + 10, yPos + 50), Size = new Size(trackBarWidth + valueWidth, 30), Minimum = 0, Maximum = 255, TickFrequency = 50 };

            trackBarSatMin.ValueChanged += (s, e) => { lblSatMin.Text = trackBarSatMin.Value.ToString(); HSVParameters.SaturationMin = trackBarSatMin.Value; };
            trackBarSatMax.ValueChanged += (s, e) => { lblSatMax.Text = trackBarSatMax.Value.ToString(); HSVParameters.SaturationMax = trackBarSatMax.Value; };

            this.Controls.AddRange(new Control[] { lblSatMin, trackBarSatMin, lblSatMax, trackBarSatMax });
            yPos += 90;

            // Value参数
            var lblVal = new Label { Text = "亮度(V):", Location = new Point(20, yPos), Size = new Size(labelWidth, 20) };
            this.Controls.Add(lblVal);

            lblValMin = new Label { Text = "0", Location = new Point(20 + labelWidth + 10, yPos + 25), Size = new Size(valueWidth, 20) };
            trackBarValMin = new TrackBar { Location = new Point(20 + labelWidth + 10 + valueWidth, yPos + 20), Size = new Size(trackBarWidth, 30), Minimum = 0, Maximum = 255, TickFrequency = 50 };
            lblValMax = new Label { Text = "255", Location = new Point(20 + labelWidth + 10 + valueWidth + trackBarWidth + 10, yPos + 25), Size = new Size(valueWidth, 20) };
            trackBarValMax = new TrackBar { Location = new Point(20 + labelWidth + 10, yPos + 50), Size = new Size(trackBarWidth + valueWidth, 30), Minimum = 0, Maximum = 255, TickFrequency = 50 };

            trackBarValMin.ValueChanged += (s, e) => { lblValMin.Text = trackBarValMin.Value.ToString(); HSVParameters.ValueMin = trackBarValMin.Value; };
            trackBarValMax.ValueChanged += (s, e) => { lblValMax.Text = trackBarValMax.Value.ToString(); HSVParameters.ValueMax = trackBarValMax.Value; };

            this.Controls.AddRange(new Control[] { lblValMin, trackBarValMin, lblValMax, trackBarValMax });
            yPos += 90;

            // 面积参数
            var lblMinArea = new Label { Text = "最小面积:", Location = new Point(20, yPos), Size = new Size(labelWidth, 20) };
            textBoxMinArea = new TextBox { Location = new Point(20 + labelWidth + 10, yPos), Size = new Size(100, 20) };
            textBoxMinArea.TextChanged += (s, e) => { if (double.TryParse(textBoxMinArea.Text, out double val)) HSVParameters.MinArea = val; };

            var lblMaxArea = new Label { Text = "最大面积:", Location = new Point(250, yPos), Size = new Size(labelWidth, 20) };
            textBoxMaxArea = new TextBox { Location = new Point(250 + labelWidth + 10, yPos), Size = new Size(100, 20) };
            textBoxMaxArea.TextChanged += (s, e) => { if (double.TryParse(textBoxMaxArea.Text, out double val)) HSVParameters.MaxArea = val; };

            this.Controls.AddRange(new Control[] { lblMinArea, textBoxMinArea, lblMaxArea, textBoxMaxArea });
            yPos += 40;

            // 按钮
            btnPreview = new Button { Text = "预览", Location = new Point(80, yPos), Size = new Size(75, 30) };
            btnReset = new Button { Text = "重置显示", Location = new Point(165, yPos), Size = new Size(75, 30) };
            btnOK = new Button { Text = "确定", Location = new Point(250, yPos), Size = new Size(75, 30), DialogResult = DialogResult.OK };
            btnCancel = new Button { Text = "取消", Location = new Point(335, yPos), Size = new Size(75, 30), DialogResult = DialogResult.Cancel };

            btnOK.Click += BtnOK_Click;
            btnCancel.Click += BtnCancel_Click;
            btnPreview.Click += BtnPreview_Click;
            btnReset.Click += BtnReset_Click;

            this.Controls.AddRange(new Control[] { btnPreview, btnReset, btnOK, btnCancel });

            this.ResumeLayout(false);
        }

        private void LoadParameters()
        {
            trackBarHueMin.Value = HSVParameters.HueMin;
            trackBarHueMax.Value = HSVParameters.HueMax;
            trackBarSatMin.Value = HSVParameters.SaturationMin;
            trackBarSatMax.Value = HSVParameters.SaturationMax;
            trackBarValMin.Value = HSVParameters.ValueMin;
            trackBarValMax.Value = HSVParameters.ValueMax;
            textBoxMinArea.Text = HSVParameters.MinArea.ToString();
            textBoxMaxArea.Text = HSVParameters.MaxArea.ToString();

            // 更新标签显示
            lblHueMin.Text = HSVParameters.HueMin.ToString();
            lblHueMax.Text = HSVParameters.HueMax.ToString();
            lblSatMin.Text = HSVParameters.SaturationMin.ToString();
            lblSatMax.Text = HSVParameters.SaturationMax.ToString();
            lblValMin.Text = HSVParameters.ValueMin.ToString();
            lblValMax.Text = HSVParameters.ValueMax.ToString();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            SaveParameters();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void BtnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                if (parentHalconWrapper == null)
                {
                    MessageBox.Show("无法预览：未提供图像显示窗口", "预览失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 保存当前参数
                SaveParameters();

                // 使用当前参数进行HSV颜色分割预览
                var regions = parentHalconWrapper.HSVPreview(HSVParameters);
                
                // 更新状态
                btnPreview.Text = "已预览";
                btnPreview.BackColor = Color.LightGreen;
                
                // 1秒后恢复按钮状态
                var timer = new System.Windows.Forms.Timer();
                timer.Interval = 1000;
                timer.Tick += (s, args) =>
                {
                    btnPreview.Text = "预览";
                    btnPreview.BackColor = SystemColors.Control;
                    timer.Stop();
                    timer.Dispose();
                };
                timer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"预览失败: {ex.Message}", "预览错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                // 恢复按钮状态
                btnPreview.Text = "预览";
                btnPreview.BackColor = SystemColors.Control;
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            try
            {
                if (parentHalconWrapper == null)
                {
                    MessageBox.Show("无法重置：未提供图像显示窗口", "重置失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 刷新显示原始图像
                parentHalconWrapper.RefreshDisplay();
                
                // 更新状态
                btnReset.Text = "已重置";
                btnReset.BackColor = Color.LightBlue;
                
                // 1秒后恢复按钮状态
                var timer = new System.Windows.Forms.Timer();
                timer.Interval = 1000;
                timer.Tick += (s, args) =>
                {
                    btnReset.Text = "重置显示";
                    btnReset.BackColor = SystemColors.Control;
                    timer.Stop();
                    timer.Dispose();
                };
                timer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"重置失败: {ex.Message}", "重置错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                // 恢复按钮状态
                btnReset.Text = "重置显示";
                btnReset.BackColor = SystemColors.Control;
            }
        }

        private void SaveParameters()
        {
            // 参数已经在TrackBar的ValueChanged事件中实时更新了
            // 这里可以进行最终的验证和调整
            if (HSVParameters.HueMin > HSVParameters.HueMax)
            {
                int temp = HSVParameters.HueMin;
                HSVParameters.HueMin = HSVParameters.HueMax;
                HSVParameters.HueMax = temp;
            }

            if (HSVParameters.SaturationMin > HSVParameters.SaturationMax)
            {
                int temp = HSVParameters.SaturationMin;
                HSVParameters.SaturationMin = HSVParameters.SaturationMax;
                HSVParameters.SaturationMax = temp;
            }

            if (HSVParameters.ValueMin > HSVParameters.ValueMax)
            {
                int temp = HSVParameters.ValueMin;
                HSVParameters.ValueMin = HSVParameters.ValueMax;
                HSVParameters.ValueMax = temp;
            }

            if (HSVParameters.MinArea > HSVParameters.MaxArea)
            {
                double temp = HSVParameters.MinArea;
                HSVParameters.MinArea = HSVParameters.MaxArea;
                HSVParameters.MaxArea = temp;
            }
        }
    }

    // 标定点输入窗体
    public partial class CalibrationPointForm : Form
    {
        public double RealX { get; private set; }
        public double RealY { get; private set; }

        private TextBox textBoxRealX, textBoxRealY;
        private Label lblRealX, lblRealY, lblInstruction;
        private Button btnOK, btnCancel;

        public CalibrationPointForm()
        {
            InitializeComponent();
        }

        public CalibrationPointForm(double currentRealX, double currentRealY) : this()
        {
            RealX = currentRealX;
            RealY = currentRealY;
            LoadExistingValues();
        }

        private void LoadExistingValues()
        {
            if (textBoxRealX != null && textBoxRealY != null)
            {
                textBoxRealX.Text = RealX.ToString("F2");
                textBoxRealY.Text = RealY.ToString("F2");
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // 设置窗体属性
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 180);
            this.Name = "CalibrationPointForm";
            this.Text = "标定点输入";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            // 说明标签
            lblInstruction = new Label
            {
                Text = "请输入此标定点对应的真实世界坐标(单位:mm):",
                Location = new Point(20, 20),
                Size = new Size(300, 30),
                AutoSize = false
            };
            this.Controls.Add(lblInstruction);

            // X坐标输入
            lblRealX = new Label
            {
                Text = "真实X坐标:",
                Location = new Point(20, 60),
                Size = new Size(100, 20)
            };
            textBoxRealX = new TextBox
            {
                Location = new Point(130, 58),
                Size = new Size(100, 23),
                Text = "0.00"
            };
            var lblXUnit = new Label
            {
                Text = "mm",
                Location = new Point(240, 60),
                Size = new Size(30, 20)
            };

            // Y坐标输入
            lblRealY = new Label
            {
                Text = "真实Y坐标:",
                Location = new Point(20, 90),
                Size = new Size(100, 20)
            };
            textBoxRealY = new TextBox
            {
                Location = new Point(130, 88),
                Size = new Size(100, 23),
                Text = "0.00"
            };
            var lblYUnit = new Label
            {
                Text = "mm",
                Location = new Point(240, 90),
                Size = new Size(30, 20)
            };

            // 按钮
            btnOK = new Button
            {
                Text = "确定",
                Location = new Point(130, 130),
                Size = new Size(75, 30),
                DialogResult = DialogResult.OK
            };
            btnCancel = new Button
            {
                Text = "取消",
                Location = new Point(220, 130),
                Size = new Size(75, 30),
                DialogResult = DialogResult.Cancel
            };

            btnOK.Click += OKButton_Click;

            // 添加所有控件
            this.Controls.AddRange(new Control[] {
                lblRealX, textBoxRealX, lblXUnit,
                lblRealY, textBoxRealY, lblYUnit,
                btnOK, btnCancel
            });

            // 设置默认按钮和取消按钮
            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;

            this.ResumeLayout(false);
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            try
            {
                // 验证并获取真实世界坐标
                if (!double.TryParse(textBoxRealX.Text, out double realX))
                {
                    MessageBox.Show("请输入有效的X坐标数值", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxRealX.Focus();
                    return;
                }

                if (!double.TryParse(textBoxRealY.Text, out double realY))
                {
                    MessageBox.Show("请输入有效的Y坐标数值", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxRealY.Focus();
                    return;
                }

                RealX = realX;
                RealY = realY;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"输入处理出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // PLCConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 300);
            this.Name = "PLCConfigForm";
            this.Text = "PLC配置";
            this.ResumeLayout(false);
        }

        private async void ConnectButton_Click(object sender, EventArgs e)
        {
            // 根据选择的通信类型连接PLC
        }
    }
}
