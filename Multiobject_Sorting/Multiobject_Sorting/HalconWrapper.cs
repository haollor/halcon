using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.Drawing;

namespace Multiobject_Sorting
{
    public class HalconWrapper
    {
        private HWindow hWindow;
        private HObject currentImage;
        
        // HSV颜色分割参数
        public class HSVParameters
        {
            public int HueMin { get; set; } = 0;
            public int HueMax { get; set; } = 180;
            public int SaturationMin { get; set; } = 0;
            public int SaturationMax { get; set; } = 255;
            public int ValueMin { get; set; } = 0;
            public int ValueMax { get; set; } = 255;
            public double MinArea { get; set; } = 100;
            public double MaxArea { get; set; } = 100000;
        }

        // 检测结果结构
        public class DetectionResult
        {
            public double CenterX { get; set; }
            public double CenterY { get; set; }
            public double Angle { get; set; }
            public double Area { get; set; }
            public string ObjectType { get; set; }
            public double Width { get; set; }
            public double Height { get; set; }
        }

        public HalconWrapper(HWindow window)
        {
            this.hWindow = window;
            HOperatorSet.SetPart(hWindow, 0, 0, -1, -1);
        }

        /// <summary>
        /// 从相机或文件加载图像
        /// </summary>
        /// <param name="imagePath">图像路径，为空则从相机获取</param>
        public void LoadImage(string imagePath = null)
        {
            try
            {
                if (string.IsNullOrEmpty(imagePath))
                {
                    // 从相机获取图像 (需要配置相机参数)
                    HOperatorSet.OpenFramegrabber("DirectShow", 1, 1, 0, 0, 0, 0, "default", 8, "rgb",
                        -1, "false", "default", "[0] USB Video Device", 0, -1, out HTuple frameGrabber);
                    HOperatorSet.GrabImage(out currentImage, frameGrabber);
                    HOperatorSet.CloseFramegrabber(frameGrabber);
                }
                else
                {
                    // 从文件加载图像
                    HOperatorSet.ReadImage(out currentImage, imagePath);
                }

                // 显示图像
                HOperatorSet.DispObj(currentImage, hWindow);
            }
            catch (Exception ex)
            {
                throw new Exception($"加载图像失败: {ex.Message}");
            }
        }

        /// <summary>
        /// HSV颜色分割
        /// </summary>
        /// <param name="parameters">HSV参数</param>
        /// <returns>分割后的区域</returns>
        public HObject HSVColorSegmentation(HSVParameters parameters)
        {
            if (currentImage == null)
                throw new Exception("请先加载图像");

            try
            {
                // 转换为HSV颜色空间
                HOperatorSet.Rgb1ToHsv(currentImage, out HObject hueImage, out HObject saturationImage, out HObject valueImage);

                // HSV通道分割
                HOperatorSet.Threshold(hueImage, out HObject hueRegion, parameters.HueMin, parameters.HueMax);
                HOperatorSet.Threshold(saturationImage, out HObject saturationRegion, parameters.SaturationMin, parameters.SaturationMax);
                HOperatorSet.Threshold(valueImage, out HObject valueRegion, parameters.ValueMin, parameters.ValueMax);

                // 合并三个通道的结果
                HOperatorSet.Intersection(hueRegion, saturationRegion, out HObject tempRegion);
                HOperatorSet.Intersection(tempRegion, valueRegion, out HObject colorRegion);

                // 形态学处理 - 开运算去除小噪点
                HOperatorSet.OpeningCircle(colorRegion, out HObject openedRegion, 3.5);
                
                // 闭运算填充空洞
                HOperatorSet.ClosingCircle(openedRegion, out HObject closedRegion, 5.5);

                // 连通域分析
                HOperatorSet.Connection(closedRegion, out HObject connectedRegions);

                // 根据面积过滤
                HOperatorSet.SelectShape(connectedRegions, out HObject selectedRegions, "area", "and", parameters.MinArea, parameters.MaxArea);

                // 显示结果
                HOperatorSet.SetDraw(hWindow, "margin");
                HOperatorSet.SetColor(hWindow, "red");
                HOperatorSet.DispObj(selectedRegions, hWindow);

                return selectedRegions;
            }
            catch (Exception ex)
            {
                throw new Exception($"HSV颜色分割失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 多目标检测和位置角度计算
        /// </summary>
        /// <param name="regions">分割后的区域</param>
        /// <returns>检测结果列表</returns>
        public List<DetectionResult> DetectMultipleObjects(HObject regions)
        {
            var results = new List<DetectionResult>();

            try
            {
                // 获取区域数量
                HOperatorSet.CountObj(regions, out HTuple numRegions);

                for (int i = 1; i <= numRegions.I; i++)
                {
                    // 选择单个区域
                    HOperatorSet.SelectObj(regions, out HObject singleRegion, i);

                    var result = new DetectionResult();

                    // 计算区域中心
                    HOperatorSet.AreaCenter(singleRegion, out HTuple area, out HTuple centerY, out HTuple centerX);
                    result.CenterX = centerX.D;
                    result.CenterY = centerY.D;
                    result.Area = area.D;

                    // 计算最小外接矩形来获取角度
                    HOperatorSet.SmallestRectangle2(singleRegion, out HTuple row, out HTuple column, 
                        out HTuple phi, out HTuple length1, out HTuple length2);
                    
                    result.Angle = phi.D * 180.0 / Math.PI; // 转换为度
                    result.Width = length1.D * 2;
                    result.Height = length2.D * 2;

                    // 根据形状特征判断目标类型
                    double aspectRatio = result.Width / result.Height;
                    if (aspectRatio > 0.8 && aspectRatio < 1.2)
                        result.ObjectType = "圆形";
                    else if (aspectRatio > 1.5)
                        result.ObjectType = "长方形";
                    else
                        result.ObjectType = "其他";

                    results.Add(result);

                    // 显示检测结果
                    HOperatorSet.SetColor(hWindow, "green");
                    HOperatorSet.DispCross(hWindow, result.CenterY, result.CenterX, 6, result.Angle * Math.PI / 180.0);
                    
                    // 显示文本信息
                    string info = $"({result.CenterX:F1},{result.CenterY:F1}) {result.Angle:F1}°";
                    HOperatorSet.SetTposition(hWindow, (int)result.CenterY - 20, (int)result.CenterX - 30);
                    HOperatorSet.WriteString(hWindow, info);
                }

                return results;
            }
            catch (Exception ex)
            {
                throw new Exception($"多目标检测失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 边缘检测辅助角度计算
        /// </summary>
        /// <param name="region">单个区域</param>
        /// <returns>精确角度</returns>
        public double CalculatePreciseAngle(HObject region)
        {
            try
            {
                // 获取区域轮廓
                HOperatorSet.GenContourRegionXld(region, out HObject contour, "border");
                
                // 拟合椭圆
                HOperatorSet.FitEllipseContourXld(contour, "fitzgibbon", -1, 0, 0, 200, 3, 2,
                    out HTuple row, out HTuple column, out HTuple phi, out HTuple ra, out HTuple rb, 
                    out HTuple startPhi, out HTuple endPhi, out HTuple pointOrder);

                return phi.D * 180.0 / Math.PI;
            }
            catch
            {
                // 如果椭圆拟合失败，使用最小外接矩形的角度
                HOperatorSet.SmallestRectangle2(region, out HTuple row, out HTuple column, 
                    out HTuple phi, out HTuple length1, out HTuple length2);
                return phi.D * 180.0 / Math.PI;
            }
        }

        /// <summary>
        /// 模板匹配检测
        /// </summary>
        /// <param name="templatePath">模板图像路径</param>
        /// <param name="matchThreshold">匹配阈值</param>
        /// <returns>匹配结果</returns>
        public List<DetectionResult> TemplateMatching(string templatePath, double matchThreshold = 0.7)
        {
            var results = new List<DetectionResult>();

            try
            {
                // 加载模板
                HOperatorSet.ReadImage(out HObject template, templatePath);
                
                // 创建形状模型
                HOperatorSet.CreateShapeModel(template, 1, 0, 6.28, 0.0175, "none", "use_polarity", 
                    30, 10, out HTuple modelID);

                // 在当前图像中查找模板
                HOperatorSet.FindShapeModel(currentImage, modelID, 0, 6.28, matchThreshold, 0, 0.5,
                    "least_squares", 0, 0.9, out HTuple row, out HTuple column, out HTuple angle, out HTuple score);

                // 处理匹配结果
                for (int i = 0; i < row.Length; i++)
                {
                    var result = new DetectionResult
                    {
                        CenterX = column[i].D,
                        CenterY = row[i].D,
                        Angle = angle[i].D * 180.0 / Math.PI,
                        ObjectType = "模板匹配"
                    };
                    results.Add(result);
                }

                // 清理模型
                HOperatorSet.ClearShapeModel(modelID);

                return results;
            }
            catch (Exception ex)
            {
                throw new Exception($"模板匹配失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            currentImage?.Dispose();
        }
    }
}
