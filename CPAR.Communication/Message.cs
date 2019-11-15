using CPAR.Communication.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPAR.Communication
{
    public abstract class Message
    {
        const byte CODE_STATUS_MESSAGE = 0x80;
        const byte CODE_EVENT_MESSAGE = 0x81;

        protected Message(Packet response)
        {
            mResponse = response;
        }

        public static Message CreateMessage(Packet packet)
        {
            Message retValue = null;

            switch (packet.Code)
            {
                case CODE_STATUS_MESSAGE:
                    retValue = new StatusMessage(packet);
                    break;
                case CODE_EVENT_MESSAGE:
                    retValue = new EventMessage(packet);
                    break;
                default:
                    throw new UnknownMessageReceivedException(String.Format("Unkown message [ 0x{0:X} ]", packet.Code));
            }

            return retValue;
        } 

        public abstract void Visit(IMessageVisitor visitor);

        protected Packet mResponse;
    }
}
