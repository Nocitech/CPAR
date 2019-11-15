using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPAR.Communication.Messages;

namespace CPAR.Communication
{
    public interface IMessageVisitor
    {
        void Accept(StatusMessage message);
        void Accept(EventMessage message);
    }
}
