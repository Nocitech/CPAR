using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPAR.Communication
{
    internal class SerialPortLayer
    {
        public void Open()
        {
            if (port != null)
                Close();

            if (port == null)
            {
                try
                {
                    port = new SerialPort(PortName)
                    {
                        BaudRate = BaudRate,
                        Parity = Parity.None,
                        StopBits = StopBits.One,
                        DataBits = 8,
                        Handshake = Handshake.None,
                        DtrEnable = true
                    };
                    destuffer.Reset();
                    port.DataReceived += new SerialDataReceivedEventHandler(ReceiveData);
                    
                    port.Open();
                }
                catch (Exception e)
                {
                    Logging.Log.Debug("Could not open port: " + e.Message);
                }
            }
        }

        public void Close()
        {
            if (port != null)
            {
                if (port.IsOpen)
                {
                    port.Close();
                    port.Dispose();
                    port = null;
                }
                else
                {
                    port = null;
                }
            }
        }

        private void ReceiveData(object sender, SerialDataReceivedEventArgs args)
        {
            int bytesPending = port.BytesToRead;
            byte[] buffer = new byte[bytesPending];
            port.Read(buffer, 0, bytesPending);

            foreach (byte b in buffer)
            {
                Destuffer.Add(b);
            }
        }

        public void Transmit(byte[] frame)
        {
            if (port != null)
            {
                if (port.IsOpen)
                {
                    port.Write(frame, 0, frame.Length);
                }
            }
        }

        public bool IsOpen
        {
            get
            {
                bool retValue = false;

                if (port != null)
                    retValue = port.IsOpen;

                return retValue;
            }
        }

        public string PortName { get; set; }
        public int BaudRate { get; set; }

        public Destuffer Destuffer {  get { return destuffer; } }

        private SerialPort port = null;
        private Destuffer destuffer = new Destuffer();
    }
}
