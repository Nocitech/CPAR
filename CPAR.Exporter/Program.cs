using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CPAR.Core;

namespace CPAR.Exporter
{
    class Program
    {
        /** 
         * \brief Initialize the export
         * This function will initialize the export of data based on the parameters passed to 
         * the program from the command line. There is the following options for initialize the
         * export:
         * 
         * 1. The program has no command line arguments, in this case the current working directory
         *    is used and the program will look for an export definition file in the current working
         *    directory.
         * 2. The program has been passed the path and filename of an export definition file, in which
         *    case it use this export definition to perform the data export.
         *    
         * \param[in] args the command line parameters that has been passed to the program.
         * \return an export definition file
         */
        static CPAR.Core.Exporter Initialize(string[] args)
        {
            CPAR.Core.Exporter retValue = null;

            if (args.Length == 0)
            {
                var workingPath = Directory.GetCurrentDirectory();
                retValue = CPAR.Core.Exporter.LoadFromDirectory(workingPath);
            }
            else
            {
                var filename = args[0];

                if (File.Exists(filename))
                {
                    retValue = CPAR.Core.Exporter.Load(filename);
                }
            }

            return retValue;
        }


        static void Main(string[] args)
        {
            Console.WriteLine("CPAR Exporter, Rev. 001");

            try
            {
                var exporter = Initialize(args);
                exporter?.Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.WriteLine("Press any key to continue . . .");

            while (!Console.KeyAvailable);
        }
    }
}
