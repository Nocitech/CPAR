using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPAR.Communication
{
    public abstract class Function
    {
        public Function()
        {
            request = new Packet(0x00, 0);
        }

        public Function(byte code)
        {
            request = new Packet(code, 0);
        }

        public Function(byte code, byte length)
        {
            request = new Packet(code, length);
        }

        internal byte[] GetRequest()
        {
            return GetRequestPacket().ToArray();
        }

        internal virtual Packet GetRequestPacket()
        {
            return request;
        }

        internal void SetResponse(Packet packet)
        {
            response = packet;

            if (response.Code != GetRequestPacket().Code)
                throw new InvalidSlaveResponseException("Invalid function code");

            if (!IsResponseValid())
                throw new InvalidSlaveResponseException("Response content invalid");
        }

        protected virtual bool IsResponseValid()
        {
            return response.Length == 0;
        }

        public abstract string SerializeResponse();

        protected Packet request;
        protected Packet response = null;
    }
}
