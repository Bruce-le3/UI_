using System;
using System.IO.Ports;

namespace CommPlc
{
	public class ExceptionPlc : Exception
	{
		public SerialPort _port = null;

		public ExceptionPlc(SerialPort port, string msg) : base(msg)
		{
			this._port = port;
		}
	}
}
