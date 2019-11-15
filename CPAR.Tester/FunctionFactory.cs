using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPAR.Communication;
using CPAR.Communication.Functions;

namespace CPAR.Tester
{
    public static class FunctionFactory
    {
        private static Dictionary<Type, Function> functions = new Dictionary<Type, Function>();

        static FunctionFactory()
        {
            functions.Add(typeof(DeviceIdentification), new DeviceIdentification());
            functions.Add(typeof(KickWatchdog), new KickWatchdog());
            functions.Add(typeof(ReadCalibration), new ReadCalibration());
            functions.Add(typeof(ReadHalStatus), new ReadHalStatus());
            functions.Add(typeof(ResetDevice), new ResetDevice());
            functions.Add(typeof(SetAnalogVoltage), new SetAnalogVoltage());
            functions.Add(typeof(SetDigitalPin), new SetDigitalPin());
            functions.Add(typeof(SetWaveformProgram), new SetWaveformProgram());
            functions.Add(typeof(StartStimulation), new StartStimulation());
            functions.Add(typeof(StopStimulation), new StopStimulation());
            functions.Add(typeof(WriteCalibration), new WriteCalibration());
            functions.Add(typeof(WriteSerialNumber), new WriteSerialNumber());
            functions.Add(typeof(ForceStartStimulation), new ForceStartStimulation());
        }

        public static Function[] GetFunctions(DeviceID id)
        {
            List<Function> retValue = new List<Function>();

            switch (id)
            {
                case DeviceID.CPAR:
                    retValue.Add(functions[typeof(DeviceIdentification)]);
                    retValue.Add(functions[typeof(SetWaveformProgram)]);
                    retValue.Add(functions[typeof(StartStimulation)]);
                    retValue.Add(functions[typeof(StopStimulation)]);
                    retValue.Add(functions[typeof(WriteSerialNumber)]);
                    retValue.Add(functions[typeof(WriteCalibration)]);
                    retValue.Add(functions[typeof(ReadCalibration)]);
                    retValue.Add(functions[typeof(KickWatchdog)]);
                    retValue.Add(functions[typeof(ResetDevice)]);
                    retValue.Add(functions[typeof(ForceStartStimulation)]);
                    break;
                case DeviceID.HW_TESTER:
                    retValue.Add(functions[typeof(DeviceIdentification)]);
                    retValue.Add(functions[typeof(WriteSerialNumber)]);
                    retValue.Add(functions[typeof(WriteCalibration)]);
                    retValue.Add(functions[typeof(ReadCalibration)]);
                    retValue.Add(functions[typeof(SetDigitalPin)]);
                    retValue.Add(functions[typeof(SetAnalogVoltage)]);
                    retValue.Add(functions[typeof(ReadHalStatus)]);
                    break;
                default:
                    break;
            }

            return retValue.ToArray();
        }

        public static Function GetFunction(Type type)
        {
            if (functions.ContainsKey(type))
            {
                return functions[type];
            }
            else
            {
                throw new ArgumentException("Incorrect message type");
            }
        }
    }
}
