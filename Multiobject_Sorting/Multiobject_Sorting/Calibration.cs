using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;

namespace Multiobject_Sorting
{
    public class Calibration
    {
        // 标定点结构
        [Serializable]
        public class CalibrationPoint
        {
            public double ImageX { get; set; }
            public double ImageY { get; set; }
            public double RealX { get; set; }
            public double RealY { get; set; }
            
            public CalibrationPoint() { }
            public CalibrationPoint(double imgX, double imgY, double realX, double realY)
            {
                ImageX = imgX;
                ImageY = imgY;
                RealX = realX;
                RealY = realY;
            }
        }

        // 变换矩阵参数
        [Serializable]
        public class TransformationMatrix
        {
            public double A11 { get; set; }
            public double A12 { get; set; }
            public double A13 { get; set; }
            public double A21 { get; set; }
            public double A22 { get; set; }
            public double A23 { get; set; }
            public double A31 { get; set; }
            public double A32 { get; set; }
            public double A33 { get; set; } = 1.0;

            public TransformationMatrix()
            {
                // 初始化为单位矩阵
                A11 = 1.0; A12 = 0.0; A13 = 0.0;
                A21 = 0.0; A22 = 1.0; A23 = 0.0;
                A31 = 0.0; A32 = 0.0; A33 = 1.0;
            }
        }

        private List<CalibrationPoint> calibrationPoints;
        private TransformationMatrix transformMatrix;
        private bool isCalibrated = false;

        public Calibration()
        {
            calibrationPoints = new List<CalibrationPoint>();
            transformMatrix = new TransformationMatrix();
        }

        /// <summary>
        /// 添加标定点
        /// </summary>
        /// <param name="imageX">图像X坐标</param>
        /// <param name="imageY">图像Y坐标</param>
        /// <param name="realX">真实世界X坐标</param>
        /// <param name="realY">真实世界Y坐标</param>
        public void AddCalibrationPoint(double imageX, double imageY, double realX, double realY)
        {
            if (calibrationPoints.Count >= 9)
            {
                throw new Exception("已达到最大标定点数量(9个)");
            }

            calibrationPoints.Add(new CalibrationPoint(imageX, imageY, realX, realY));
        }

        /// <summary>
        /// 清除所有标定点
        /// </summary>
        public void ClearCalibrationPoints()
        {
            calibrationPoints.Clear();
            isCalibrated = false;
        }

        /// <summary>
        /// 获取标定点数量
        /// </summary>
        public int GetCalibrationPointCount()
        {
            return calibrationPoints.Count;
        }

        /// <summary>
        /// 获取标定点列表
        /// </summary>
        public List<CalibrationPoint> GetCalibrationPoints()
        {
            return new List<CalibrationPoint>(calibrationPoints);
        }

        /// <summary>
        /// 执行九点标定
        /// </summary>
        /// <returns>标定是否成功</returns>
        public bool PerformCalibration()
        {
            if (calibrationPoints.Count < 4)
            {
                throw new Exception("至少需要4个标定点");
            }

            try
            {
                // 使用最小二乘法计算变换矩阵
                var result = CalculateTransformationMatrix();
                if (result)
                {
                    isCalibrated = true;
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"标定失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 计算变换矩阵（仿射变换）
        /// </summary>
        private bool CalculateTransformationMatrix()
        {
            try
            {
                int n = calibrationPoints.Count;
                
                // 构建线性方程组 AX = B
                // 对于仿射变换: X_real = a11*X_img + a12*Y_img + a13
                //              Y_real = a21*X_img + a22*Y_img + a23

                // 构建系数矩阵A (2n x 6)
                double[,] A = new double[2 * n, 6];
                double[] B = new double[2 * n];

                for (int i = 0; i < n; i++)
                {
                    var point = calibrationPoints[i];
                    
                    // X方程
                    A[2 * i, 0] = point.ImageX;     // a11
                    A[2 * i, 1] = point.ImageY;     // a12
                    A[2 * i, 2] = 1.0;              // a13
                    A[2 * i, 3] = 0.0;
                    A[2 * i, 4] = 0.0;
                    A[2 * i, 5] = 0.0;
                    B[2 * i] = point.RealX;

                    // Y方程
                    A[2 * i + 1, 0] = 0.0;
                    A[2 * i + 1, 1] = 0.0;
                    A[2 * i + 1, 2] = 0.0;
                    A[2 * i + 1, 3] = point.ImageX; // a21
                    A[2 * i + 1, 4] = point.ImageY; // a22
                    A[2 * i + 1, 5] = 1.0;          // a23
                    B[2 * i + 1] = point.RealY;
                }

                // 求解最小二乘解
                double[] solution = SolveLeastSquares(A, B);

                // 设置变换矩阵参数
                transformMatrix.A11 = solution[0];
                transformMatrix.A12 = solution[1];
                transformMatrix.A13 = solution[2];
                transformMatrix.A21 = solution[3];
                transformMatrix.A22 = solution[4];
                transformMatrix.A23 = solution[5];
                transformMatrix.A31 = 0.0;
                transformMatrix.A32 = 0.0;
                transformMatrix.A33 = 1.0;

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 最小二乘法求解线性方程组
        /// </summary>
        private double[] SolveLeastSquares(double[,] A, double[] B)
        {
            int rows = A.GetLength(0);
            int cols = A.GetLength(1);

            // 计算 A^T * A 和 A^T * B
            double[,] AtA = new double[cols, cols];
            double[] AtB = new double[cols];

            // A^T * A
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    double sum = 0.0;
                    for (int k = 0; k < rows; k++)
                    {
                        sum += A[k, i] * A[k, j];
                    }
                    AtA[i, j] = sum;
                }
            }

            // A^T * B
            for (int i = 0; i < cols; i++)
            {
                double sum = 0.0;
                for (int k = 0; k < rows; k++)
                {
                    sum += A[k, i] * B[k];
                }
                AtB[i] = sum;
            }

            // 高斯消元法求解 AtA * X = AtB
            return GaussianElimination(AtA, AtB);
        }

        /// <summary>
        /// 高斯消元法
        /// </summary>
        private double[] GaussianElimination(double[,] A, double[] B)
        {
            int n = B.Length;
            double[] solution = new double[n];

            // 前向消元
            for (int i = 0; i < n; i++)
            {
                // 选主元
                int maxRow = i;
                for (int k = i + 1; k < n; k++)
                {
                    if (Math.Abs(A[k, i]) > Math.Abs(A[maxRow, i]))
                        maxRow = k;
                }

                // 交换行
                if (maxRow != i)
                {
                    for (int k = i; k < n; k++)
                    {
                        double temp = A[i, k];
                        A[i, k] = A[maxRow, k];
                        A[maxRow, k] = temp;
                    }
                    double tempB = B[i];
                    B[i] = B[maxRow];
                    B[maxRow] = tempB;
                }

                // 消元
                for (int k = i + 1; k < n; k++)
                {
                    double factor = A[k, i] / A[i, i];
                    for (int j = i; j < n; j++)
                    {
                        A[k, j] -= factor * A[i, j];
                    }
                    B[k] -= factor * B[i];
                }
            }

            // 回代
            for (int i = n - 1; i >= 0; i--)
            {
                solution[i] = B[i];
                for (int j = i + 1; j < n; j++)
                {
                    solution[i] -= A[i, j] * solution[j];
                }
                solution[i] /= A[i, i];
            }

            return solution;
        }

        /// <summary>
        /// 图像坐标转换为真实世界坐标
        /// </summary>
        /// <param name="imageX">图像X坐标</param>
        /// <param name="imageY">图像Y坐标</param>
        /// <returns>真实世界坐标</returns>
        public PointF ImageToReal(double imageX, double imageY)
        {
            if (!isCalibrated)
                throw new Exception("未完成标定，无法进行坐标转换");

            double realX = transformMatrix.A11 * imageX + transformMatrix.A12 * imageY + transformMatrix.A13;
            double realY = transformMatrix.A21 * imageX + transformMatrix.A22 * imageY + transformMatrix.A23;

            return new PointF((float)realX, (float)realY);
        }

        /// <summary>
        /// 真实世界坐标转换为图像坐标
        /// </summary>
        /// <param name="realX">真实X坐标</param>
        /// <param name="realY">真实Y坐标</param>
        /// <returns>图像坐标</returns>
        public PointF RealToImage(double realX, double realY)
        {
            if (!isCalibrated)
                throw new Exception("未完成标定，无法进行坐标转换");

            // 计算逆变换矩阵
            double det = transformMatrix.A11 * transformMatrix.A22 - transformMatrix.A12 * transformMatrix.A21;
            if (Math.Abs(det) < 1e-10)
                throw new Exception("变换矩阵不可逆");

            double invA11 = transformMatrix.A22 / det;
            double invA12 = -transformMatrix.A12 / det;
            double invA21 = -transformMatrix.A21 / det;
            double invA22 = transformMatrix.A11 / det;
            double invA13 = (transformMatrix.A12 * transformMatrix.A23 - transformMatrix.A22 * transformMatrix.A13) / det;
            double invA23 = (transformMatrix.A21 * transformMatrix.A13 - transformMatrix.A11 * transformMatrix.A23) / det;

            double imageX = invA11 * realX + invA12 * realY + invA13;
            double imageY = invA21 * realX + invA22 * realY + invA23;

            return new PointF((float)imageX, (float)imageY);
        }

        /// <summary>
        /// 计算标定精度
        /// </summary>
        /// <returns>平均误差（像素）</returns>
        public double CalculateCalibrationAccuracy()
        {
            if (!isCalibrated)
                return -1;

            double totalError = 0;
            foreach (var point in calibrationPoints)
            {
                var realCoord = ImageToReal(point.ImageX, point.ImageY);
                double errorX = realCoord.X - point.RealX;
                double errorY = realCoord.Y - point.RealY;
                double error = Math.Sqrt(errorX * errorX + errorY * errorY);
                totalError += error;
            }

            return totalError / calibrationPoints.Count;
        }

        /// <summary>
        /// 保存标定数据到XML文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public void SaveCalibrationData(string filePath)
        {
            try
            {
                var data = new CalibrationData
                {
                    CalibrationPoints = calibrationPoints,
                    TransformMatrix = transformMatrix,
                    IsCalibrated = isCalibrated
                };

                XmlSerializer serializer = new XmlSerializer(typeof(CalibrationData));
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    serializer.Serialize(fs, data);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"保存标定数据失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 从XML文件加载标定数据
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public void LoadCalibrationData(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    throw new Exception("标定文件不存在");

                XmlSerializer serializer = new XmlSerializer(typeof(CalibrationData));
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    var data = (CalibrationData)serializer.Deserialize(fs);
                    calibrationPoints = data.CalibrationPoints ?? new List<CalibrationPoint>();
                    transformMatrix = data.TransformMatrix ?? new TransformationMatrix();
                    isCalibrated = data.IsCalibrated;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"加载标定数据失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 检查是否已标定
        /// </summary>
        public bool IsCalibrated()
        {
            return isCalibrated;
        }

        /// <summary>
        /// 获取变换矩阵
        /// </summary>
        public TransformationMatrix GetTransformationMatrix()
        {
            return transformMatrix;
        }

        // 序列化辅助类
        [Serializable]
        public class CalibrationData
        {
            public List<CalibrationPoint> CalibrationPoints { get; set; }
            public TransformationMatrix TransformMatrix { get; set; }
            public bool IsCalibrated { get; set; }
        }
    }
}
