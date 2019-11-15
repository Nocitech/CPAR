using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CPAR.Communication;
using CPAR.Logging;

namespace CPAR.Tester
{
   public class Logger : ILogger
   {
      public delegate void InvokeDelegate();

      public void Add(DateTime time, LogCategory category, LogLevel level, string str)
      {
         if (mBox != null)
         {
            if (mBox.InvokeRequired)
            {
               mBox.BeginInvoke(new InvokeDelegate(() => mBox.AppendText(level.ToString() + ": " + str + System.Environment.NewLine)));
            }
            else
               mBox.AppendText(level.ToString() + ": " + str + System.Environment.NewLine);
         }
      }

      public TextBox Box
      {
         get
         {
            return mBox;
         }
         set
         {
            mBox = value;
         }
      }

      TextBox mBox = null;
   }
}
