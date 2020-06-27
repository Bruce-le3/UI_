using System;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;
using Justech;

namespace CommPlc
{
	public abstract class ComLink
	{
		public enum PLCStatus
		{
			PROGRAM,
			MONITOR,
			RUN
		}

		public enum BitArea
		{
			NONE,
			CIO,
			WR,
			HR,
			AR,
			TM,
			CNT,
			X = 20,
			Y,
			R,
			T,
			C,
			L
		}
        
		public enum WordArea
		{
			NONE,
			CIO,
			WR,
			HR,
			AR,
			TM,
			CNT,
			DM,
			EM,
			DT = 20,
			LD,
			WX,
			WY,
			WRR,
			WL,
			WT,
			WC
		}

		public SerialPort _port = null;

		public string _strReadTo = null;

    //    private IniFile _ini = new IniFile(Application.StartupPath + "\\Config" + "\\config.ini");
        
		private static readonly object syncRoot = new object();

		public string _strUnitNo = "00";

		public string strUnitNo
		{
			get
			{
				return this._strUnitNo;
			}
			set
			{
				this._strUnitNo = value;
			}
		}

		public bool IsPortOpen
		{
			get
			{
				return this._port.IsOpen;
			}
		}

		public abstract int SetBit(ComLink.BitArea ba, int addrWord, int addrBit);

		public abstract int ResetBit(ComLink.BitArea ba, int addrWord, int addrBit);

		public abstract int ReadBit(ComLink.BitArea ba, int addrWord, int addrBit, ref bool status);

		public abstract int SetWordValue(ComLink.WordArea wa, int addrWord, string strValue);

		public abstract int SetWordValue(ComLink.WordArea wa, int addrWord, int value);

		public abstract int SetDoubleWordValue(ComLink.WordArea wa, int addrWord, int value);

		public abstract int ReadWordValue(ComLink.WordArea wa, int addrWord, ref string iValue);

		public abstract int ReadWordValue(ComLink.WordArea wa, int addrWord, ref int value);

		public abstract int ReadDoubleWord(ComLink.WordArea wa, int addrWord, ref int value);

		public abstract int SetMultiWordValue(ComLink.WordArea wa, int addrWord, int length, string strValue);

		public abstract int ReadMultiWordValue(ComLink.WordArea wa, int addrWord, int length, ref string strValue);

		public abstract int GetPlcModel(ref string strModel);

		public abstract int GetPlcStatus(ref ComLink.PLCStatus status);

		public ComLink(string strCom)
		{
			try
			{
				if (this._port == null)
				{
					this._port = new SerialPort(strCom);
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public SerialPort GetPort(string strCom)
		{
			if (this._port == null)
			{
				lock (ComLink.syncRoot)
				{
					if (this._port == null)
					{
						this._port = new SerialPort(strCom);
					}
				}
			}
			return this._port;
		}

		public void OpenPort(SerialPort port)
		{
			try
			{
				port.Close();
                port.BaudRate = int.Parse(GClsConfigFile.ReadConfig("config.ini", port.PortName, "Baudrate", "115200"));
				port.DataBits = int.Parse(GClsConfigFile.ReadConfig("config.ini",port.PortName, "DataBit", "8"));
				port.StopBits = StopBits.One;
				switch (int.Parse(GClsConfigFile.ReadConfig("config.ini",port.PortName, "StopBit", "1")))
				{
				case 1:
					port.StopBits = StopBits.One;
					break;
				case 2:
					port.StopBits = StopBits.Two;
					break;
				}
				port.Parity = Parity.None;
				string @string = GClsConfigFile.ReadConfig("config.ini",port.PortName, "Parity", "None(无)");
				if (@string != null)
				{
					if (!(@string == "None(无)"))
					{
						if (!(@string == "Odd(奇)"))
						{
							if (@string == "Even(偶)")
							{
								port.Parity = Parity.Even;
							}
						}
						else
						{
							port.Parity = Parity.Odd;
						}
					}
					else
					{
						port.Parity = Parity.None;
					}
				}
				port.WriteTimeout = 3000;
				port.ReadTimeout = 3000;
				Thread.Sleep(200);
				port.Open();
			}
			catch (Exception ex)
			{
				MessageBox.Show("OpenPort error!" + ex.Message);
			}
		}

		public void OpenPort(int Baudrate, Parity parity, StopBits stopbits, int databits)
		{
			try
			{
				if (this._port != null)
				{
					this._port.WriteTimeout = 1000;
					this._port.ReadTimeout = 1000;
					if (this._port.BaudRate != Baudrate || this._port.Parity != parity || this._port.StopBits != stopbits || this._port.DataBits != databits)
					{
						if (this._port.IsOpen)
						{
							this._port.Close();
						}
						this._port.BaudRate = Baudrate;
						this._port.Parity = parity;
						this._port.StopBits = stopbits;
						this._port.DataBits = databits;
					}
					if (!this._port.IsOpen)
					{
						this._port.Open();
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public void OpenPort(int Baudrate, string strparity, int istopbits, int databits)
		{
			strparity = strparity.ToUpper();
			string text = strparity;
			Parity parity;
			if (text != null)
			{
				if (text == "EVEN")
				{
					parity = Parity.Even;
					goto IL_3A;
				}
				if (text == "ODD")
				{
					parity = Parity.Odd;
					goto IL_3A;
				}
			}
			parity = Parity.None;
			IL_3A:
			StopBits stopbits;
			switch (istopbits)
			{
			case 1:
				stopbits = StopBits.One;
				break;
			case 2:
				stopbits = StopBits.Two;
				break;
			default:
				stopbits = StopBits.None;
				break;
			}
			this.OpenPort(Baudrate, parity, stopbits, databits);
		}

		public void OpenPort(int Baudrate)
		{
			this.OpenPort(Baudrate, Parity.None, StopBits.One, 8);
		}

		public void OpenPort()
		{
			this.OpenPort(9600, Parity.None, StopBits.One, 8);
		}

		public void ClosePort()
		{
			this._port.Close();
			if (this._port.IsOpen)
			{
				Thread.Sleep(1000);
				this._port.Close();
			}
		}
	}
}
