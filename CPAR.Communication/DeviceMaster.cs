using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CPAR.Logging;

namespace CPAR.Communication
{
    public class DeviceMaster
    {
        private enum CommState
        {
            IDLE,
            WAITING,
            COMPLETED,
            ERROR
        };

        public DeviceMaster()
        {
            connection = new SerialPortLayer()
            {
                PortName = "COM1",
                BaudRate = 38400
            };
            connection.Destuffer.OnReceive += HandleIncommingFrame;

            Timeout = 500;
        }

        public void Open(string portName)
        {
            connection.PortName = portName;
            connection.Open();
        }

        public void Open()
        {
            connection.Open();
        }

        public void Close()
        {
            connection.Close();
        }

        public bool IsOpen
        {
            get
            {
                return connection.IsOpen;
            }
        }

        public void Execute(Function function)
        {
            Initiate(function);

            while (!IsCompleted());

            state = CommState.WAITING;

            if (currentException != null)
                throw currentException;
        }

        private bool IsCompleted()
        {
            bool retValue = false;

            if (stopwatch.ElapsedMilliseconds < Timeout)
            {
                lock (lockObject)
                {
                    if (state != CommState.WAITING)
                    {
                        retValue = true;
                    }
                }
            }
            else
            {
                currentException = new SlaveNotRespondingException("No response from the slave");
                retValue = true;
            }

            return retValue;
        }

        private void Initiate(Function function)
        {
            var bytes = function.GetRequest();

            lock (lockObject)
            {
                stopwatch.Restart();
                current = function;
                state = CommState.WAITING;
                currentException = null;
            }

            connection.Transmit(Frame.Encode(bytes));
        }


        private void HandleIncommingFrame(Destuffer caller, byte[] frame)
        {
            try
            {
                var response = new Packet(frame);

                if (response.Code != 0x00)
                {
                    if (response.IsFunction)
                    {
                        lock (lockObject)
                        {
                            if (current != null)
                                current.SetResponse(response);

                            state = CommState.COMPLETED;
                        }
                    }
                    else
                    {
                        try
                        {
                            lock (lockObject)
                            {
                                messages.Enqueue(response);
                            }
                        }
                        catch (Exception e)
                        {
                            Log.Error(e.Message);
                        }
                    }
                }
                else
                {
                    
                    lock (lockObject)
                    {
                        currentException = new UnknownFunctionCallException(String.Format("Error: {0}", response.GetByte(0)));
                        state = CommState.ERROR;
                    }
                }
            }
            catch {}
        }

        public long Timeout { get; set; }

        public string PortName
        {
            get
            {
                return connection.PortName;
            }
            set
            {
                connection.PortName = value;
            }
        }

        public int BaudRate
        {
            get
            {
                return connection.BaudRate;
            }
            set
            {
                connection.BaudRate = value;
            }
        }

        public bool IsMessagePending
        {
            get
            {
                lock (lockObject)
                {
                    return messages.Count > 0;
                }
            }
        }

        public Packet GetMessage()
        {
            Packet retValue = null;

            if (IsMessagePending)
            {
                lock (lockObject)
                {
                    retValue = messages.Dequeue();
                }
            }

            return retValue;
        }

        private SerialPortLayer connection;
        private Function current = null;
        private readonly object lockObject = new object();
        private Exception currentException = null;
        private Stopwatch stopwatch = new Stopwatch();
        private CommState state = CommState.WAITING;
        private Queue<Packet> messages = new Queue<Packet>();
    }
}
