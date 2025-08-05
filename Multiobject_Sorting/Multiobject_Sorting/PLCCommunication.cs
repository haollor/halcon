using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.IO.Ports;
using System.Threading;
using Modbus.Device;
using Modbus.Serial;

namespace Multiobject_Sorting
{
    public class PLCCommunication
    {
        // PLC通信类型枚举
        public enum CommunicationType
        {
            TCP_IP,
            Modbus_RTU,
            Modbus_TCP
        }

        // 坐标数据结构
        public struct CoordinateData
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float Angle { get; set; }
            public int ObjectType { get; set; } // 目标类型编号
            public bool IsValid { get; set; }

            public CoordinateData(float x, float y, float angle, int objectType = 0)
            {
                X = x;
                Y = y;
                Angle = angle;
                ObjectType = objectType;
                IsValid = true;
            }
        }

        // TCP/IP通信类
        public class TCPIPCommunication
        {
            private TcpClient tcpClient;
            private NetworkStream stream;
            private string ipAddress;
            private int port;
            private bool isConnected = false;
            private object lockObject = new object();

            public event Action<string> OnStatusChanged;
            public event Action<string> OnDataSent;
            public event Action<string> OnError;

            public TCPIPCommunication(string ip, int portNumber)
            {
                ipAddress = ip;
                port = portNumber;
            }

            /// <summary>
            /// 连接到PLC
            /// </summary>
            public async Task<bool> ConnectAsync()
            {
                try
                {
                    tcpClient = new TcpClient();
                    await tcpClient.ConnectAsync(ipAddress, port);
                    stream = tcpClient.GetStream();
                    isConnected = true;
                    OnStatusChanged?.Invoke($"已连接到PLC: {ipAddress}:{port}");
                    return true;
                }
                catch (Exception ex)
                {
                    OnError?.Invoke($"连接失败: {ex.Message}");
                    return false;
                }
            }

            /// <summary>
            /// 断开连接
            /// </summary>
            public void Disconnect()
            {
                lock (lockObject)
                {
                    try
                    {
                        isConnected = false;
                        stream?.Close();
                        tcpClient?.Close();
                        OnStatusChanged?.Invoke("已断开连接");
                    }
                    catch (Exception ex)
                    {
                        OnError?.Invoke($"断开连接时出错: {ex.Message}");
                    }
                }
            }

            /// <summary>
            /// 发送坐标数据
            /// </summary>
            public async Task<bool> SendCoordinatesAsync(List<CoordinateData> coordinates)
            {
                if (!isConnected || stream == null)
                {
                    OnError?.Invoke("未连接到PLC");
                    return false;
                }

                try
                {
                    lock (lockObject)
                    {
                        // 构建数据包
                        var dataPacket = BuildDataPacket(coordinates);
                        stream.Write(dataPacket, 0, dataPacket.Length);
                        stream.Flush();
                    }

                    OnDataSent?.Invoke($"已发送{coordinates.Count}个坐标数据");
                    return true;
                }
                catch (Exception ex)
                {
                    OnError?.Invoke($"发送数据失败: {ex.Message}");
                    return false;
                }
            }

            /// <summary>
            /// 构建数据包
            /// </summary>
            private byte[] BuildDataPacket(List<CoordinateData> coordinates)
            {
                var packet = new List<byte>();
                
                // 包头
                packet.AddRange(BitConverter.GetBytes((ushort)0xABCD)); // 固定包头
                packet.AddRange(BitConverter.GetBytes((ushort)coordinates.Count)); // 数据数量

                // 坐标数据
                foreach (var coord in coordinates)
                {
                    packet.AddRange(BitConverter.GetBytes(coord.X));
                    packet.AddRange(BitConverter.GetBytes(coord.Y));
                    packet.AddRange(BitConverter.GetBytes(coord.Angle));
                    packet.AddRange(BitConverter.GetBytes(coord.ObjectType));
                    packet.Add((byte)(coord.IsValid ? 1 : 0));
                }

                // 校验和
                byte checksum = 0;
                for (int i = 4; i < packet.Count; i++) // 跳过包头
                {
                    checksum ^= packet[i];
                }
                packet.Add(checksum);

                return packet.ToArray();
            }

            public bool IsConnected => isConnected;
        }

        // Modbus通信类
        public class ModbusCommunication
        {
            private IModbusMaster modbusMaster;
            private SerialPort serialPort;
            private TcpClient tcpClient;
            private byte slaveId;
            private CommunicationType commType;
            private bool isConnected = false;

            public event Action<string> OnStatusChanged;
            public event Action<string> OnDataSent;
            public event Action<string> OnError;

            public ModbusCommunication(CommunicationType type, byte slaveAddress = 1)
            {
                commType = type;
                slaveId = slaveAddress;
            }

            /// <summary>
            /// 连接Modbus RTU
            /// </summary>
            public bool ConnectRTU(string portName, int baudRate = 9600, System.IO.Ports.Parity parity = System.IO.Ports.Parity.None)
            {
                try
                {
                    serialPort = new SerialPort(portName, baudRate, parity);
                    serialPort.Open();
                    modbusMaster = ModbusSerialMaster.CreateRtu(serialPort);
                    isConnected = true;
                    OnStatusChanged?.Invoke($"Modbus RTU已连接: {portName}");
                    return true;
                }
                catch (Exception ex)
                {
                    OnError?.Invoke($"Modbus RTU连接失败: {ex.Message}");
                    return false;
                }
            }

            /// <summary>
            /// 连接Modbus TCP
            /// </summary>
            public async Task<bool> ConnectTCPAsync(string ipAddress, int port = 502)
            {
                try
                {
                    tcpClient = new TcpClient();
                    await tcpClient.ConnectAsync(ipAddress, port);
                    modbusMaster = ModbusMaster.CreateTcp(tcpClient);
                    isConnected = true;
                    OnStatusChanged?.Invoke($"Modbus TCP已连接: {ipAddress}:{port}");
                    return true;
                }
                catch (Exception ex)
                {
                    OnError?.Invoke($"Modbus TCP连接失败: {ex.Message}");
                    return false;
                }
            }

            /// <summary>
            /// 断开连接
            /// </summary>
            public void Disconnect()
            {
                try
                {
                    isConnected = false;
                    modbusMaster?.Dispose();
                    serialPort?.Close();
                    tcpClient?.Close();
                    OnStatusChanged?.Invoke("Modbus连接已断开");
                }
                catch (Exception ex)
                {
                    OnError?.Invoke($"断开Modbus连接时出错: {ex.Message}");
                }
            }

            /// <summary>
            /// 发送坐标数据到保持寄存器
            /// </summary>
            public async Task<bool> SendCoordinatesAsync(List<CoordinateData> coordinates, ushort startAddress = 0)
            {
                if (!isConnected || modbusMaster == null)
                {
                    OnError?.Invoke("Modbus未连接");
                    return false;
                }

                try
                {
                    var registerValues = new List<ushort>();
                    
                    // 添加数据数量
                    registerValues.Add((ushort)coordinates.Count);

                    // 转换坐标数据为寄存器值
                    foreach (var coord in coordinates)
                    {
                        // X坐标（分成两个16位寄存器）
                        var xBytes = BitConverter.GetBytes(coord.X);
                        registerValues.Add(BitConverter.ToUInt16(xBytes, 0));
                        registerValues.Add(BitConverter.ToUInt16(xBytes, 2));

                        // Y坐标（分成两个16位寄存器）
                        var yBytes = BitConverter.GetBytes(coord.Y);
                        registerValues.Add(BitConverter.ToUInt16(yBytes, 0));
                        registerValues.Add(BitConverter.ToUInt16(yBytes, 2));

                        // 角度（分成两个16位寄存器）
                        var angleBytes = BitConverter.GetBytes(coord.Angle);
                        registerValues.Add(BitConverter.ToUInt16(angleBytes, 0));
                        registerValues.Add(BitConverter.ToUInt16(angleBytes, 2));

                        // 目标类型和有效性
                        registerValues.Add((ushort)coord.ObjectType);
                        registerValues.Add((ushort)(coord.IsValid ? 1 : 0));
                    }

                    // 写入保持寄存器
                    await modbusMaster.WriteMultipleRegistersAsync(slaveId, startAddress, registerValues.ToArray());
                    
                    OnDataSent?.Invoke($"Modbus已发送{coordinates.Count}个坐标数据到地址{startAddress}");
                    return true;
                }
                catch (Exception ex)
                {
                    OnError?.Invoke($"Modbus发送数据失败: {ex.Message}");
                    return false;
                }
            }

            /// <summary>
            /// 读取保持寄存器
            /// </summary>
            public async Task<ushort[]> ReadHoldingRegistersAsync(ushort startAddress, ushort numberOfPoints)
            {
                if (!isConnected || modbusMaster == null)
                    throw new Exception("Modbus未连接");

                return await modbusMaster.ReadHoldingRegistersAsync(slaveId, startAddress, numberOfPoints);
            }

            /// <summary>
            /// 写入单个线圈
            /// </summary>
            public async Task WriteSingleCoilAsync(ushort coilAddress, bool value)
            {
                if (!isConnected || modbusMaster == null)
                    throw new Exception("Modbus未连接");

                await modbusMaster.WriteSingleCoilAsync(slaveId, coilAddress, value);
            }

            public bool IsConnected => isConnected;
        }

        // 主通信管理器
        private TCPIPCommunication tcpComm;
        private ModbusCommunication modbusComm;
        private CommunicationType currentCommType;
        private Timer heartbeatTimer;

        public event Action<string> OnStatusChanged;
        public event Action<string> OnDataSent;
        public event Action<string> OnError;

        public PLCCommunication()
        {
            // 初始化心跳定时器
            heartbeatTimer = new Timer(SendHeartbeat, null, Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// 初始化TCP/IP通信
        /// </summary>
        public void InitializeTCPIP(string ipAddress, int port)
        {
            currentCommType = CommunicationType.TCP_IP;
            tcpComm = new TCPIPCommunication(ipAddress, port);
            tcpComm.OnStatusChanged += (msg) => OnStatusChanged?.Invoke(msg);
            tcpComm.OnDataSent += (msg) => OnDataSent?.Invoke(msg);
            tcpComm.OnError += (msg) => OnError?.Invoke(msg);
        }

        /// <summary>
        /// 初始化Modbus通信
        /// </summary>
        public void InitializeModbus(CommunicationType type, byte slaveId = 1)
        {
            currentCommType = type;
            modbusComm = new ModbusCommunication(type, slaveId);
            modbusComm.OnStatusChanged += (msg) => OnStatusChanged?.Invoke(msg);
            modbusComm.OnDataSent += (msg) => OnDataSent?.Invoke(msg);
            modbusComm.OnError += (msg) => OnError?.Invoke(msg);
        }

        /// <summary>
        /// 连接到PLC
        /// </summary>
        public async Task<bool> ConnectAsync(params object[] parameters)
        {
            try
            {
                switch (currentCommType)
                {
                    case CommunicationType.TCP_IP:
                        return await tcpComm.ConnectAsync();

                    case CommunicationType.Modbus_RTU:
                        if (parameters.Length >= 2)
                        {
                            string portName = parameters[0].ToString();
                            int baudRate = (int)parameters[1];
                            return modbusComm.ConnectRTU(portName, baudRate);
                        }
                        break;

                    case CommunicationType.Modbus_TCP:
                        if (parameters.Length >= 2)
                        {
                            string ip = parameters[0].ToString();
                            int port = (int)parameters[1];
                            return await modbusComm.ConnectTCPAsync(ip, port);
                        }
                        break;
                }
                return false;
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"连接失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
            heartbeatTimer.Change(Timeout.Infinite, Timeout.Infinite);
            
            switch (currentCommType)
            {
                case CommunicationType.TCP_IP:
                    tcpComm?.Disconnect();
                    break;
                case CommunicationType.Modbus_RTU:
                case CommunicationType.Modbus_TCP:
                    modbusComm?.Disconnect();
                    break;
            }
        }

        /// <summary>
        /// 发送检测结果到PLC
        /// </summary>
        public async Task<bool> SendDetectionResultsAsync(List<HalconWrapper.DetectionResult> results)
        {
            var coordinates = results.Select(r => new CoordinateData(
                (float)r.CenterX,
                (float)r.CenterY,
                (float)r.Angle,
                GetObjectTypeCode(r.ObjectType)
            )).ToList();

            return await SendCoordinatesAsync(coordinates);
        }

        /// <summary>
        /// 发送坐标数据
        /// </summary>
        public async Task<bool> SendCoordinatesAsync(List<CoordinateData> coordinates)
        {
            try
            {
                switch (currentCommType)
                {
                    case CommunicationType.TCP_IP:
                        return await tcpComm.SendCoordinatesAsync(coordinates);
                    case CommunicationType.Modbus_RTU:
                    case CommunicationType.Modbus_TCP:
                        return await modbusComm.SendCoordinatesAsync(coordinates);
                    default:
                        return false;
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"发送坐标失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 启动心跳
        /// </summary>
        public void StartHeartbeat(int intervalMs = 5000)
        {
            heartbeatTimer.Change(intervalMs, intervalMs);
        }

        /// <summary>
        /// 发送心跳
        /// </summary>
        private async void SendHeartbeat(object state)
        {
            try
            {
                if (IsConnected)
                {
                    var heartbeat = new List<CoordinateData>
                    {
                        new CoordinateData(999, 999, 0, 255) // 心跳标识
                    };
                    await SendCoordinatesAsync(heartbeat);
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"心跳发送失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取目标类型编码
        /// </summary>
        private int GetObjectTypeCode(string objectType)
        {
            switch (objectType?.ToLower())
            {
                case "圆形": return 1;
                case "长方形": return 2;
                case "模板匹配": return 3;
                default: return 0;
            }
        }

        /// <summary>
        /// 检查连接状态
        /// </summary>
        public bool IsConnected
        {
            get
            {
                switch (currentCommType)
                {
                    case CommunicationType.TCP_IP:
                        return tcpComm?.IsConnected ?? false;
                    case CommunicationType.Modbus_RTU:
                    case CommunicationType.Modbus_TCP:
                        return modbusComm?.IsConnected ?? false;
                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            heartbeatTimer?.Dispose();
            Disconnect();
        }
    }
}
