using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPAR.Communication.Messages
{
    public class EventMessage :
        Message
    {
        public EventMessage(Packet response) :
            base(response)
        {
            if (mResponse.Length != 1)
            {
                throw new InvalidMessageException("A received EventMessage does not have a length of 1");
            }
        }

        public override void Visit(IMessageVisitor visitor)
        {
            visitor.Accept(this);
        }

        public EventID Event
        {
            get
            {
                return (EventID) mResponse.GetByte(0);
            }
        }
    }
}
