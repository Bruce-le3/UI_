using System;
using System.Collections;
using System.Text;
using System.Windows.Forms;

namespace CommPlc
{
	public class OmronHostLink : ComLink
	{
		private static ArrayList _instances = new ArrayList();

		private OmronHostLink(string strCom) : base(strCom)
		{
			StringBuilder strEnd = new StringBuilder("*");
			strEnd.Append('\r');
			this._strReadTo = strEnd.ToString();
		}

		public static OmronHostLink GetInstance(string strCom)
		{
			int i;
			OmronHostLink result;
			for (i = 0; i < OmronHostLink._instances.Count; i++)
			{
				if (((OmronHostLink)OmronHostLink._instances[i])._port.PortName.Equals(strCom))
				{
					result = (OmronHostLink)OmronHostLink._instances[i];
					return result;
				}
			}
			if (i == OmronHostLink._instances.Count)
			{
				OmronHostLink link = new OmronHostLink(strCom);
				OmronHostLink._instances.Add(link);
				result = link;
				return result;
			}
			result = null;
			return result;
		}

		public override int SetBit(ComLink.BitArea ba, int addrWord, int addrBit)
		{
			return this.WriteBit(ba, addrWord, addrBit, true);
		}

		public override int ResetBit(ComLink.BitArea ba, int addrWord, int addrBit)
		{
			return this.WriteBit(ba, addrWord, addrBit, false);
		}

		public int WriteBit(ComLink.BitArea ba, int addrWord, int addrBit, bool bSet)
		{
			int result;
			if (ba >= (ComLink.BitArea)10)
			{
				result = 1;
			}
			else
			{
				string strMemory;
				switch (ba)
				{
				case ComLink.BitArea.CIO:
					strMemory = "30";
					break;
				case ComLink.BitArea.WR:
					strMemory = "31";
					break;
				case ComLink.BitArea.HR:
					strMemory = "32";
					break;
				case ComLink.BitArea.AR:
					strMemory = "33";
					break;
				case ComLink.BitArea.TM:
				case ComLink.BitArea.CNT:
					strMemory = "09";
					break;
				default:
					result = 1;
					return result;
				}
				StringBuilder strCommand = new StringBuilder("@");
				strCommand.Append(this._strUnitNo);
				strCommand.Append("FA10000000001");
				strCommand.Append("02");
				strCommand.Append(strMemory);
				string strWord = Convert.ToString(addrWord, 16);
				if (strWord.Length < 4)
				{
					string strTemp = "0000";
					strTemp = strTemp.Substring(0, 4 - strWord.Length);
					strWord = strTemp + strWord;
				}
				strCommand.Append(strWord);
				strCommand.Append("0");
				strCommand.Append(Convert.ToString(addrBit, 16));
				strCommand.Append("0001");
				if (bSet)
				{
					strCommand.Append("01");
				}
				else
				{
					strCommand.Append("00");
				}
				string strFcs = this.GetFcs(strCommand.ToString());
				strCommand.Append(strFcs);
				strCommand.Append("*");
				strCommand.Append('\r');
				string strRead = null;
				try
				{
					lock (this._port)
					{
						this._port.Write(strCommand.ToString());
						strRead = this._port.ReadTo(this._strReadTo);
					}
				}
				catch (Exception ex)
				{
					throw new Exception(ex.Message);
				}
				if (strRead.Length > 24 && strRead.Substring(19, 4).Equals("0000"))
				{
					result = 0;
				}
				else
				{
					result = 1;
				}
			}
			return result;
		}

		public override int ReadBit(ComLink.BitArea ba, int addrWord, int addrBit, ref bool status)
		{
			string strMemory;
			int result;
			switch (ba)
			{
			case ComLink.BitArea.CIO:
				strMemory = "30";
				break;
			case ComLink.BitArea.WR:
				strMemory = "31";
				break;
			case ComLink.BitArea.HR:
				strMemory = "32";
				break;
			case ComLink.BitArea.AR:
				strMemory = "33";
				break;
			case ComLink.BitArea.TM:
			case ComLink.BitArea.CNT:
				strMemory = "09";
				break;
			default:
				result = 1;
				return result;
			}
			StringBuilder strCommand = new StringBuilder("@");
			strCommand.Append(this._strUnitNo);
			strCommand.Append("FA1000000000101");
			strCommand.Append(strMemory);
			string strWord = Convert.ToString(addrWord, 16);
			if (strWord.Length < 4)
			{
				string strTemp = "0000";
				strTemp = strTemp.Substring(0, 4 - strWord.Length);
				strWord = strTemp + strWord;
			}
			strCommand.Append(strWord);
			strCommand.Append("0");
			strCommand.Append(Convert.ToString(addrBit, 16));
			strCommand.Append("0001");
			string strFcs = this.GetFcs(strCommand.ToString());
			strCommand.Append(strFcs);
			strCommand.Append("*");
			strCommand.Append('\r');
			string strRead = null;
			try
			{
				lock (this._port)
				{
					this._port.Write(strCommand.ToString());
					strRead = this._port.ReadTo(this._strReadTo);
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			if (strRead.Length > 24 && strRead.Substring(19, 4).Equals("0000"))
			{
				string str = strRead.Substring(23, 2);
				if (strRead.Substring(23, 2).Equals("00"))
				{
					status = false;
				}
				else
				{
					status = true;
				}
				result = 0;
			}
			else
			{
				result = 1;
			}
			return result;
		}

		public override int SetWordValue(ComLink.WordArea wa, int addrWord, string strValue)
		{
			int result;
			if (wa >= (ComLink.WordArea)10)
			{
				result = 1;
			}
			else
			{
				string strMemory = null;
				switch (wa)
				{
                        
				case ComLink.WordArea.CIO:
					strMemory = "B0";
					break;
                      
				case ComLink.WordArea.WR:
					strMemory = "B1";
					break;
				case ComLink.WordArea.HR:
					strMemory = "B2";
					break;
				case ComLink.WordArea.AR:
					strMemory = "B3";
					break;
				case ComLink.WordArea.TM:
				case ComLink.WordArea.CNT:
					strMemory = "89";
					break;
				case ComLink.WordArea.DM:
					strMemory = "82";
					break;
				}
				StringBuilder strCommand = new StringBuilder("@");
				strCommand.Append(this._strUnitNo);
				strCommand.Append("FA10000000001");
				strCommand.Append("02");
				strCommand.Append(strMemory);
				string strWord = Convert.ToString(addrWord, 16);
				if (strWord.Length < 4)
				{
					string strTemp = "0000";
					strTemp = strTemp.Substring(0, 4 - strWord.Length);
					strWord = strTemp + strWord;
				}
				strCommand.Append(strWord);
				strCommand.Append("000001");
				if (strValue.Length > 4)
				{
					strValue = strValue.Substring(strValue.Length - 4, 4);
				}
				else if (strValue.Length < 4)
				{
					string strTemp = "0000";
					strTemp = strTemp.Substring(0, 4 - strValue.Length);
					strValue = strTemp + strValue;
				}
				strCommand.Append(strValue);
				string strFcs = this.GetFcs(strCommand.ToString());
				strCommand.Append(strFcs);
				strCommand.Append("*");
				strCommand.Append('\r');
				string strRead = null;
				try
				{
					lock (this._port)
					{
						DateTime dt = DateTime.Now;
						this._port.Write(strCommand.ToString());
						double elapse = (DateTime.Now - dt).TotalMilliseconds;
						dt = DateTime.Now;
						strRead = this._port.ReadTo(this._strReadTo);
						elapse = (DateTime.Now - dt).TotalMilliseconds;
					}
				}
				catch (Exception ex)
				{
					throw new Exception(ex.Message);
				}
				if (strRead.Length > 24 && strRead.Substring(19, 4).Equals("0000"))
				{
					result = 0;
				}
				else
				{
					result = 1;
				}
			}
			return result;
		}

		public override int SetWordValue(ComLink.WordArea wa, int addrWord, int value)
		{
			string strValue = Convert.ToString(value, 16);
			if (strValue.Length > 4)
			{
				strValue = strValue.Substring(strValue.Length - 4, 4);
			}
			else if (strValue.Length < 4)
			{
				string strTemp = "0000";
				strTemp = strTemp.Substring(0, 4 - strValue.Length);
				strValue = strTemp + strValue;
			}
			return this.SetWordValue(wa, addrWord, strValue);
		}

		public override int ReadWordValue(ComLink.WordArea wa, int addrWord, ref string strValue)
		{
			int result;
			if (wa >= (ComLink.WordArea)10)
			{
				result = 1;
			}
			else
			{
				string strMemory = null;
				switch (wa)
				{
				case ComLink.WordArea.CIO:
					strMemory = "B0";
					break;
				case ComLink.WordArea.WR:
					strMemory = "B1";
					break;
				case ComLink.WordArea.HR:
					strMemory = "B2";
					break;
				case ComLink.WordArea.AR:
					strMemory = "B3";
					break;
				case ComLink.WordArea.TM:
				case ComLink.WordArea.CNT:
					strMemory = "89";
					break;
				case ComLink.WordArea.DM:
					strMemory = "82";
					break;
				}
				StringBuilder strCommand = new StringBuilder("@");
				strCommand.Append(this._strUnitNo);
				strCommand.Append("FA00000000001");
				strCommand.Append("01");
				strCommand.Append(strMemory);
				string strWord = Convert.ToString(addrWord, 16);
				if (strWord.Length < 4)
				{
					string strTemp = "0000";
					strTemp = strTemp.Substring(0, 4 - strWord.Length);
					strWord = strTemp + strWord;
				}
				strCommand.Append(strWord);
				strCommand.Append("000001");
				string strFcs = this.GetFcs(strCommand.ToString());
				strCommand.Append(strFcs);
				strCommand.Append("*");
				strCommand.Append('\r');
				string strRead = null;
				try
				{
					lock (this._port)
					{
						this._port.Write(strCommand.ToString());
						strRead = this._port.ReadTo(this._strReadTo);
					}
				}
				catch (Exception ex)
				{
					throw new Exception(ex.Message);
				}
				if (strRead.Length > 28 && strRead.Substring(19, 4).Equals("0000"))
				{
					strValue = strRead.Substring(23, 4);
					result = 0;
				}
				else
				{
					result = 1;
				}
			}
			return result;
		}

		public override int ReadWordValue(ComLink.WordArea wa, int addrWord, ref int value)
		{
			string str = string.Empty;
			int ret = this.ReadWordValue(wa, addrWord, ref str);
			if (ret == 0)
			{
				value = (int)Convert.ToInt16(str, 16);
			}
			return ret;
		}

		public override int SetMultiWordValue(ComLink.WordArea wa, int addrWord, int length, string strValue)
		{
			int result;
			if (wa >= (ComLink.WordArea)10)
			{
				result = 1;
			}
			else if (length > 100)
			{
				result = 1;
			}
			else
			{
				string strMemory = null;
				switch (wa)
				{
				case ComLink.WordArea.CIO:
					strMemory = "B0";
					break;
				case ComLink.WordArea.WR:
					strMemory = "B1";
					break;
				case ComLink.WordArea.HR:
					strMemory = "B2";
					break;
				case ComLink.WordArea.AR:
					strMemory = "B3";
					break;
				case ComLink.WordArea.TM:
				case ComLink.WordArea.CNT:
					strMemory = "89";
					break;
				case ComLink.WordArea.DM:
					strMemory = "82";
					break;
				}
				StringBuilder strCommand = new StringBuilder("@");
				strCommand.Append(this._strUnitNo);
				strCommand.Append("FA10000000001");
				strCommand.Append("02");
				strCommand.Append(strMemory);
				string strWord = Convert.ToString(addrWord, 16);
				if (strWord.Length < 4)
				{
					string strTemp = "0000";
					strTemp = strTemp.Substring(0, 4 - strWord.Length);
					strWord = strTemp + strWord;
				}
				strCommand.Append(strWord);
				strCommand.Append("00");
				string strlength = Convert.ToString(length, 16);
				if (strlength.Length < 4)
				{
					string strTemp = "0000";
					strTemp = strTemp.Substring(0, 4 - strlength.Length);
					strlength = strTemp + strlength;
				}
				strCommand.Append(strlength);
				if (strValue.Length > 4 * length)
				{
					strValue = strValue.Substring(strValue.Length - 4 * length, 4 * length);
				}
				else if (strValue.Length < 4 * length)
				{
					StringBuilder sb = new StringBuilder(strValue);
					for (int i = strValue.Length; i < 4 * length; i++)
					{
						sb.Append("0");
					}
					strValue = sb.ToString();
				}
				strCommand.Append(strValue);
				string strFcs = this.GetFcs(strCommand.ToString());
				strCommand.Append(strFcs);
				strCommand.Append("*");
				strCommand.Append('\r');
				string strRead = null;
				try
				{
					lock (this._port)
					{
						this._port.Write(strCommand.ToString());
						strRead = this._port.ReadTo(this._strReadTo);
					}
				}
				catch (Exception ex)
				{
					throw new Exception(ex.Message);
				}
				if (strRead.Length > 24 && strRead.Substring(19, 4).Equals("0000"))
				{
					result = 0;
				}
				else
				{
					result = 1;
				}
			}
			return result;
		}

		public override int SetDoubleWordValue(ComLink.WordArea wa, int addrWord, int value)
		{
			string strValue = Convert.ToString(value, 16);
			StringBuilder sb = new StringBuilder();
			string str = "0000";
			if (strValue.Length <= 4)
			{
				sb.Append(str.Substring(0, 4 - strValue.Length));
				sb.Append(strValue);
				sb.Append(str);
			}
			else
			{
				sb.Append(strValue.Substring(strValue.Length - 4, 4));
				sb.Append(str.Substring(0, 8 - strValue.Length));
				sb.Append(strValue.Substring(0, strValue.Length - 4));
			}
			return this.SetMultiWordValue(wa, addrWord, 2, sb.ToString());
		}

		public override int ReadMultiWordValue(ComLink.WordArea wa, int addrWord, int length, ref string strValue)
		{
			int result;
			if (wa >= (ComLink.WordArea)10)
			{
				result = 1;
			}
			else if (length > 100)
			{
				result = 1;
			}
			else
			{
				string strMemory;
				switch (wa)
				{
				case ComLink.WordArea.CIO:
					strMemory = "B0";
					break;
				case ComLink.WordArea.WR:
					strMemory = "B1";
					break;
				case ComLink.WordArea.HR:
					strMemory = "B2";
					break;
				case ComLink.WordArea.AR:
					strMemory = "B3";
					break;
				case ComLink.WordArea.TM:
				case ComLink.WordArea.CNT:
					strMemory = "89";
					break;
				case ComLink.WordArea.DM:
					strMemory = "82";
					break;
				default:
					result = 1;
					return result;
				}
				StringBuilder strCommand = new StringBuilder("@");
				strCommand.Append(this._strUnitNo);
				strCommand.Append("FA00000000001");
				strCommand.Append("01");
				strCommand.Append(strMemory);
				string strWord = Convert.ToString(addrWord, 16);
				if (strWord.Length < 4)
				{
					string strTemp = "0000";
					strTemp = strTemp.Substring(0, 4 - strWord.Length);
					strWord = strTemp + strWord;
				}
				strCommand.Append(strWord);
				strCommand.Append("00");
				string strlength = Convert.ToString(length, 16);
				if (strlength.Length < 4)
				{
					string strTemp = "0000";
					strTemp = strTemp.Substring(0, 4 - strlength.Length);
					strlength = strTemp + strlength;
				}
				strCommand.Append(strlength);
				string strFcs = this.GetFcs(strCommand.ToString());
				strCommand.Append(strFcs);
				strCommand.Append("*");
				strCommand.Append('\r');
				string strRead = null;
				try
				{
					lock (this._port)
					{
						this._port.Write(strCommand.ToString());
						strRead = this._port.ReadTo(this._strReadTo);
					}
				}
				catch (Exception ex)
				{
					throw new Exception(ex.Message);
				}
				if (strRead.Length > 28 && strRead.Substring(19, 4).Equals("0000"))
				{
					strValue = strRead.Substring(23, 4 * length);
					result = 0;
				}
				else
				{
					result = 1;
				}
			}
			return result;
		}

		public override int ReadDoubleWord(ComLink.WordArea wa, int addrWord, ref int rtnvalue)
		{
			string strvalue = string.Empty;
			int ret = this.ReadMultiWordValue(wa, addrWord, 2, ref strvalue);
			if (ret == 0)
			{
				StringBuilder sb = new StringBuilder(strvalue.Substring(4, 4));
				sb.Append(strvalue.Substring(0, 4));
				rtnvalue = Convert.ToInt32(sb.ToString(), 16);
			}
			return ret;
		}

		public override int GetPlcModel(ref string strModel)
		{
			StringBuilder strCommand = new StringBuilder("@");
			strCommand.Append(this._strUnitNo);
			strCommand.Append("FA100000000");
			strCommand.Append("050100");
			string strFcs = this.GetFcs(strCommand.ToString());
			strCommand.Append(strFcs);
			strCommand.Append("*");
			strCommand.Append('\r');
			int result;
			try
			{
				string strRead = null;
				lock (this._port)
				{
					this._port.Write(strCommand.ToString());
					strRead = this._port.ReadTo(this._strReadTo);
				}
				StringBuilder strM = new StringBuilder();
				if (strRead.Length <= 28 || !strRead.Substring(19, 4).Equals("0000"))
				{
					result = 1;
					return result;
				}
				int iFind = strRead.IndexOf("20");
				string strTemp = strRead.Substring(23, iFind - 23);
				for (int i = 23; i < iFind; i += 2)
				{
					string str = strRead.Substring(i, 2);
					int ichar = Convert.ToInt32(str, 16);
					strM.Append((char)ichar);
				}
				strModel = strM.ToString();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			result = 0;
			return result;
		}

		public override int GetPlcStatus(ref ComLink.PLCStatus status)
		{
			StringBuilder strCommand = new StringBuilder("@");
			strCommand.Append(this._strUnitNo);
			strCommand.Append("FA100000000");
			strCommand.Append("0601");
			string strFcs = this.GetFcs(strCommand.ToString());
			strCommand.Append(strFcs);
			strCommand.Append("*");
			strCommand.Append('\r');
			int result;
			try
			{
				string strRead = null;
				lock (this._port)
				{
					this._port.Write(strCommand.ToString());
					strRead = this._port.ReadTo(this._strReadTo);
				}
				if (strRead.Substring(19, 4).Equals("0000"))
				{
					string strStatus = strRead.Substring(25, 2);
					string text = strStatus;
					if (text != null)
					{
						if (!(text == "00"))
						{
							if (!(text == "02"))
							{
								if (text == "04")
								{
									status = ComLink.PLCStatus.RUN;
								}
							}
							else
							{
								status = ComLink.PLCStatus.MONITOR;
							}
						}
						else
						{
							status = ComLink.PLCStatus.PROGRAM;
						}
					}
					result = 0;
					return result;
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			result = 1;
			return result;
		}

		private string GetFcs(string strCommand)
		{
			char fcs = '@';
			for (int i = 1; i < strCommand.Length; i++)
			{
				fcs ^= strCommand[i];
			}
			string strFcs = Convert.ToString((int)fcs, 16);
			if (strFcs.Length < 2)
			{
				strFcs = "0" + strFcs;
			}
			return strFcs.ToUpper();
		}

		public static ComLink.WordArea GetWordArea(string strArea)
		{
			strArea = strArea.Trim();
			ComLink.WordArea result;
			foreach (ComLink.WordArea wa in Enum.GetValues(typeof(ComLink.WordArea)))
			{
				if (wa.ToString().Equals(strArea))
				{
					result = wa;
					return result;
				}
			}
			result = ComLink.WordArea.NONE;
			return result;
		}

		public static ComLink.BitArea GetBitdArea(string strArea)
		{
			strArea = strArea.Trim();
			ComLink.BitArea result;
			foreach (ComLink.BitArea ba in Enum.GetValues(typeof(ComLink.BitArea)))
			{
				if (ba.ToString().Equals(strArea))
				{
					result = ba;
					return result;
				}
			}
			result = ComLink.BitArea.NONE;
			return result;
		}
	}
}
