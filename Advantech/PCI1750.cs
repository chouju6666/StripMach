using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Automation.BDaq;
using System.Windows.Forms;

namespace RT
{
    /// <summary>
    /// PCI-1750 DI 16-ch
    /// </summary>
    class PCI1750
    {
        #region "变量定义"
        /// <summary>
        /// IO状态开/关枚举
        /// </summary>
        public enum IO
        {
            /// <summary>
            /// 信号开
            /// </summary>
            On = 0,

            /// <summary>
            /// 信号关
            /// </summary>
            Off = 1
        }

        public string ErrorMessage = "";
        private bool SuccessBuiltNew = false, PasswordIsCorrect = true;

        private ErrorCode ErrCode;
        private BDaqDevice TargetDevice = null;
        private BDaqDio TargetDIOCard = null;
        private InstantDiCtrl TargetPCI1750InCard = null;
        private InstantDoCtrl TargetPCI1750OutCard = null;

        /// <summary>
        /// 是否需要窗体控件，根据实例化时的条件进行判断
        /// </summary>
        private bool NeedFormControlFlag = false;

        /// <summary>
        /// 输入位结构
        /// </summary>
        public unsafe struct Bits
        {
            /// <summary>
            /// 64个输出位标志数组【0~63】
            /// </summary>
            public fixed bool InBits[16];
            public fixed bool OutBits[16];
        }

        private bool[] ReadInStatus = new bool[16];
        private bool[] ReadOutStatus = new bool[16];

        /// <summary>
        /// 读取当前IO输入的状态
        /// </summary>
        public bool[] ReadCurrentInputStatus
        {
            get
            {
                //判断扫描线程是否工作，没有工作就执行函数
                if (UpdateInputSignal != null)
                {
                    if (UpdateInputSignal.IsAlive == true)
                    {
                        return ReadInStatus;
                    }
                }
                GetInputStatus();
                return ReadInStatus;
            }
        }

        /// <summary>
        /// 是否成功实例化
        /// </summary>
        public bool SuccessBuilt
        {
            get { return SuccessBuiltNew; }
        }

        private int TempDeviceNumber = 0;
        private byte InPortData = 0;
        private byte OutPortData = 0;
        private Thread UpdateInputSignal = null;

        /// <summary>
        /// 当前打开的PCI1750卡设备号
        /// </summary>
        public int DeviceNumber
        {
            get
            {
                if (NeedFormControlFlag == true)
                {
                    return TargetPCI1750InCard.SelectedDevice.DeviceNumber;
                }
                else
                {
                    return TempDeviceNumber;
                }
            }
        }

        #endregion

        #region "函数代码"

        //创建PCI1750更新输入类的实例
        /// <summary>
        /// 创建PCI1754更新输入类的实例
        /// </summary>
        /// <param name="TargetDeviceNumber">目标PCI1750设备卡号</param>
        /// <param name="DLLPassword">使用此DLL的密码</param>
        public PCI1750(int TargetDeviceNumber)
        {
            SuccessBuiltNew = false;
            try
            {
                if (TargetDeviceNumber < 0)
                {
                    MessageBox.Show("'TargetDeviceNumber'设备卡号不能小于0，请改为正确参数。",
                        "参数错误");
                    return;
                }

                ErrCode = BDaqDevice.Open(TargetDeviceNumber,
                    AccessMode.ModeWriteWithReset, out TargetDevice);

                if (ErrCode == ErrorCode.Success)
                {
                    ErrCode = TargetDevice.GetModule(0, out TargetDIOCard);
                    if (ErrCode == ErrorCode.Success)
                    {
                        TempDeviceNumber = TargetDeviceNumber;
                        SuccessBuiltNew = true;
                        NeedFormControlFlag = false;
                    }
                    else
                    {
                        SuccessBuiltNew = false;
                        return;
                    }
                }
                else
                {
                    SuccessBuiltNew = false;
                    return;
                }
            }
            catch (Exception ex)
            {
                SuccessBuiltNew = false;
                MessageBox.Show("创建类的实例时出现错误！\r\n" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        //【重载】创建PCI1750更新输入类的实例
        /// <summary>
        /// 【重载】创建PCI1750更新输入类的实例
        /// </summary>
        /// <param name="TargetCard">目标PCI1750卡【窗体控件的形式】</param>
        public PCI1750(ref Automation.BDaq.InstantDiCtrl TargetCard)
        {
            SuccessBuiltNew = false;
            PasswordIsCorrect = false;
            try
            {
                TargetPCI1750InCard = TargetCard;
                if (TargetPCI1750InCard.Initialized == true)
                {
                    NeedFormControlFlag = true;
                    SuccessBuiltNew = true;
                }
                else
                {
                    MessageBox.Show("参数'TargetCard'传递的PCI1750控件初始化失败，没有选择设备或者是设备打开失败，请检查具体原因。", "错误");
                    SuccessBuiltNew = false;
                    return;
                }
            }
            catch (Exception ex)
            {
                SuccessBuiltNew = false;
                MessageBox.Show("创建类的实例时出现错误！\r\n" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }


        //研华输出卡PCI1750刷新输出
        /// <summary>
        /// 研华输出卡PCI1750刷新输出
        /// </summary>
        /// <param name="Out">输出的IO数组，长度必须等于64，对应64位输出位</param>
        /// <returns>是否执行成功</returns>
        public bool SetOutput(IO[] Out)
        {
            if (PasswordIsCorrect == false || SuccessBuiltNew == false)
            {
                return false;
            }
            try
            {
                if (Out.Length != 16)
                {
                    ErrorMessage = "函数SetOutput的参数'Out'数组长度不等于16";
                    return false;
                }
                //思路：传入的参数数组总计64个，0~7为端口0，依次类推，直到端口7
                for (int Port = 0; Port <= 7; Port++)
                {
                    int PortOutputStatus = 0;
                    int TempByte = 0;
                    for (int a = 7; a >= 0; a--)
                    {
                        if (Out[Port * 8 + a] == IO.On)
                        {
                            TempByte = 1;
                        }
                        else
                        {
                            TempByte = 0;
                        }
                        PortOutputStatus = (PortOutputStatus << 1) | TempByte & 0x1;
                    }

                    //需要从窗体控件传入引用进行实例化
                    if (NeedFormControlFlag == true)
                    {
                        TargetPCI1750OutCard.Write(Port, (byte)PortOutputStatus);
                    }
                    else
                    {
                        //直接用设备编号进行实例化
                        TargetDIOCard.DoWrite(Port, (byte)PortOutputStatus);
                    }
                }

                return true;
            }
            catch (Exception)// ex)
            {
                //ErrorMessage = ex.Message;
                return false;
            }
        }

        //【重载】研华输出卡PCI1752刷新输出，在实例化后调用此函数的函数前面要加 unsafe
        /// <summary>
        /// 【重载】研华输出卡PCI1752刷新输出，在实例化后调用此函数的函数前面要加 unsafe
        /// </summary>
        /// <param name="Out">输出位数据结构</param>
        /// <returns>是否执行成功</returns>
        public unsafe bool SetOutput(Bits Out)
        {
            if (PasswordIsCorrect == false || SuccessBuiltNew == false)
            {
                return false;
            }
            try
            {
                //思路：传入的参数数组总计64个，0~7为端口0，依次类推，直到端口7
                for (int Port = 0; Port <= 7; Port++)
                {
                    int PortOutputStatus = 0;
                    int TempByte = 0;
                    for (int a = 7; a >= 0; a--)
                    {
                        if (Out.OutBits[Port * 8 + a] == true)
                        {
                            TempByte = 1;
                        }
                        else
                        {
                            TempByte = 0;
                        }
                        PortOutputStatus = (PortOutputStatus << 1) | TempByte & 0x1;
                    }

                    //需要从窗体控件传入引用进行实例化
                    if (NeedFormControlFlag == true)
                    {
                        TargetPCI1750OutCard.Write(Port, (byte)PortOutputStatus);
                    }
                    else
                    {
                        //直接用设备编号进行实例化
                        TargetDIOCard.DoWrite(Port, (byte)PortOutputStatus);
                    }
                }

                return true;
            }
            catch (Exception)// ex)
            {
                //ErrorMessage = ex.Message;
                return false;
            }
        }

        //研华输入卡PCI1750刷新输入
        /// <summary>
        /// 研华输入卡PCI1750刷新输入
        /// </summary>
        /// <returns>返回输入信号数组【数组长度64】：true - ON; false - OFF</returns>
        public bool[] GetInputStatus()
        {
            //initial the return value
            for (int a = 0; a < 16; a++)
            {
                ReadInStatus[a] = false;
            }

            if (SuccessBuiltNew == false)
            {
                ErrorMessage = "未成功建立类的新实例，无法开启线程进行输入信号扫描";
                return ReadInStatus;
            }

            try
            {
                for (int Port = 0; Port <= 7; Port++)
                {
                    TargetDIOCard.DiRead(Port, out InPortData);

                    for (int Bit = 0; Bit <= 7; Bit++)
                    {
                        ReadInStatus[Port * 8 + Bit] = ((InPortData >> Bit) == 1) ? true : false;
                    }
                }

                return ReadInStatus;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return ReadInStatus;
            }
        }

        //开启线程监控PCI1754输入信号并实时更新
        /// <summary>
        /// 开启线程监控PCI1754输入信号并实时更新
        /// </summary>
        public void StartMonitor()
        {
            try
            {
                if (SuccessBuiltNew == false)
                {
                    //MessageBox.Show("未成功建立类的新实例，无法开启线程进行输入信号扫描");
                    ErrorMessage = "未成功建立类的新实例，无法开启线程进行输入信号扫描";
                    return;
                }

                if (UpdateInputSignal != null)
                {
                    return;
                }

                UpdateInputSignal = new Thread(ScanningInputStatus);
                UpdateInputSignal.IsBackground = true;
                UpdateInputSignal.Start();

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        //研华输入卡PCI1754刷新输入
        /// <summary>
        /// 研华输入卡PCI1754刷新输入
        /// </summary>
        private void ScanningInputStatus()
        {
            while (true)
            {
                try
                {
                    for (int Port = 0; Port <= 7; Port++)
                    {
                        TargetDIOCard.DiRead(Port, out InPortData);

                        for (int Bit = 0; Bit <= 7; Bit++)
                        {
                            ReadInStatus[Port * 8 + Bit] = ((InPortData >> Bit) == 1) ? true : false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                }
            }
        }

        public bool[] GetOutputStatusNew()
        {
            try
            {
                //Initial the value;
                for (int a = 0; a < 64; a++)
                {
                    ReadOutStatus[a] = false;
                }

                if (PasswordIsCorrect == false || SuccessBuiltNew == false)
                {
                    return ReadOutStatus;
                }

                //Read the output status first for each port, total 8 ports
                byte ReadPortData = 0;
                for (int Port = 0; Port <= 7; Port++)
                {
                    if (NeedFormControlFlag == true)
                    {
                        TargetPCI1750OutCard.Read(Port, out ReadPortData);
                    }
                    else
                    {
                        TargetDIOCard.DoRead(Port, out ReadPortData);
                    }

                    //Judge the status and change the return value for each bit of port
                    for (int Bit = 0; Bit <= 7; Bit++)
                    {
                        //Once the bit is 1, then set the return value bit as true
                        if ((ReadPortData >> Bit & 0x1) == 1)
                        {
                            ReadOutStatus[Port * 8 + Bit] = true;
                        }
                        else
                        {
                            ReadOutStatus[Port * 8 + Bit] = false;
                        }
                    }
                }
                return ReadOutStatus;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return ReadOutStatus;
            }
        }

        //设置某个输出位的状态【ON/OFF】：true - ON ; false - OFF
        /// <summary>
        /// 设置某个输出位的状态【ON/OFF】：true - ON ; false - OFF
        /// </summary>
        /// <param name="TargetBit">目标输出位【1~64】</param>
        /// <param name="SetOn">需要设置的状态：true - ON ; false - OFF</param>
        /// <returns>是否执行成功</returns>
        public bool SetBit(int TargetBit, bool SetOn)
        {
            if (PasswordIsCorrect == false || SuccessBuiltNew == false)
            {
                return false;
            }

            if (TargetBit < 1 || TargetBit > 64)
            {
                ErrorMessage = " 设置某个输出位的状态【ON/OFF】函数SetBit的参数'TargetBit'超出有效范围：1~64";
                return false;
            }
            int TempTargetBit = TargetBit;
            int Port = TempTargetBit / 8;
            int Bit = TempTargetBit % 8;
            byte ReadPortData = 0;
            try
            {
                //Select different device to read the current output status
                if (NeedFormControlFlag == true)
                {
                    if (TargetPCI1750OutCard.Read(Port, out ReadPortData) != Automation.BDaq.ErrorCode.Success)
                    {
                        return false;
                    }
                }
                else
                {
                    if (TargetDIOCard.DoRead(Port, out ReadPortData) != ErrorCode.Success)
                    {
                        return false;
                    }
                }

                //Set target bit ON or OFF
                if (SetOn == true)
                {
                    ReadPortData = (byte)(ReadPortData | (1 << Bit));
                }
                else
                {
                    ReadPortData = (byte)(ReadPortData & (~(1 << Bit)));
                }

                ErrorCode TempErr;
                if (NeedFormControlFlag == true)
                {
                    TempErr = (ErrorCode)TargetPCI1750OutCard.Write(Port, ReadPortData);
                }
                else
                {
                    TempErr = TargetDIOCard.DoWrite(Port, ReadPortData);
                }

                if (TempErr == ErrorCode.Success)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
        }

        //释放相关资源
        /// <summary>
        /// 释放相关资源
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (NeedFormControlFlag == true)
                {
                    TargetPCI1750OutCard.Dispose();
                    TargetPCI1750OutCard = null;
                }

                if (UpdateInputSignal != null)
                {
                    UpdateInputSignal.Abort();
                    UpdateInputSignal = null;
                }

                TargetDIOCard = null;
                TargetDevice.Close();
                TargetDevice.Dispose();

            }
            catch (Exception)// ex)
            {
                //ErrorMessage = ex.Message;;
            }
        }
        #endregion
    }
}