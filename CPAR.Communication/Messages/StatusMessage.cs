using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPAR.Communication.Messages
{
    public class StatusMessage :
        Message
    {
        public enum State
        {
            STATE_IDLE = 0,
            STATE_STIMULATING,
            STATE_EMERGENCY
        }

        public enum StopCondition
        {
            STOPCOND_NO_CONDITION = 0,
            STOPCOND_STOP_BUTTON_PRESSED,
            STOPCOND_MAXIMAL_VAS_SCORED,
            STOPCOND_STIMULATION_COMPLETED,
            STOPCOND_MAXIMAL_TIME_EXCEEDED,
            STOPCOND_VASMETER_DISCONNECTED,
            STOPCOND_EMERGENCY_STOP_ACTIVATED,
            STOPCOND_CONTROL_SOFTWARE,
            STOPCOND_OUT_OF_COMPLIANCE,
            STOPCOND_COMM_WATCHDOG,
            STOPCOND_12V_POWER_OFF
        }

        public StatusMessage(Packet response) :
            base(response)
        {
            if (mResponse.Length != 15)
            {
                throw new InvalidMessageException("A received StatusMessage does not have a length of 15");
            }
        }

        public override void Visit(IMessageVisitor visitor)
        {
            visitor.Accept(this);
        }

        #region System state
        #region BYTE 0: System State
        [Category("1 System State")]
        public State SystemState
        {
            get
            {
                return (State)mResponse.GetByte(0);
            }
        }
        #endregion
        #region BYTE 1: System Status (Bitfield)
        [Category("2 System Status")]
        public bool VasConnected
        {
            get
            {
                return (mResponse.GetByte(1) & 0x01) != 0 ? true : false;
            }
        }

        [Category("2 System Status")]
        public bool VasIsLow
        {
            get
            {
                return (mResponse.GetByte(1) & 0x02) != 0 ? true : false;
            }
        }

        [Category("2 System Status")]
        public bool PowerOn
        {
            get
            {
                return (mResponse.GetByte(1) & 0x04) != 0 ? true : false;
            }
        }

        [Category("2 System Status")]
        public bool CompressorRunning
        {
            get
            {
                return (mResponse.GetByte(1) & 0x08) != 0 ? true : false;
            }
        }

        [Category("2 System Status")]
        public bool StartPossible
        {
            get
            {
                return (mResponse.GetByte(1) & 0x10) != 0 ? true : false;
            }
        }

        #endregion
        #region BYTE 2-3: Update Counter
        [Category("3 Waveform Program")]
        public int UpdateCounter
        {
            get
            {
                return mResponse.GetUInt16(2);
            }
        }
        #endregion
        #region BYTE 4: Stop Condition
        [Category("3 Waveform Program")]
        public StopCondition Condition
        {
            get
            {
                return (StopCondition)mResponse.GetByte(4);
            }
        }
        #endregion
        #endregion
        #region VAS Score
        #region BYTE 5: VAS SCORE
        [Category("4 Pain Rating")]
        public double VasScore
        {
            get
            {
                return CPARDevice.BinaryToScore(mResponse.GetByte(5));
            }
        }

        [Category("4 Pain Rating")]
        public byte VasScoreBinary
        {
            get
            {
                return mResponse.GetByte(5);
            }
        }
        #endregion
        #region BYTE 6: FINAL VAS SCORE
        [Category("4 Pain Rating")]
        public double FinalVasScore
        {
            get
            {
                return CPARDevice.BinaryToScore(mResponse.GetByte(6));
            }
        }
        #endregion
        #endregion
        #region Pressure Stimulation
        #region BYTE 7: SUPPLY PRESSURE
        [Category("5 Air Tank")]
        public double SupplyPressure
        {
            get
            {
                return CPARDevice.BinaryToPressure(SupplyPressureBinary, CPARDevice.PressureType.SUPPLY_PRESSURE);
            }
        }

        [Category("5 Air Tank")]
        public byte SupplyPressureBinary
        {
            get
            {
                return mResponse.GetByte(7);
            }
        }
        #endregion
        #region BYTE 8: ACTUAL PRESSURE 01
        [Category("6 Pressure Stimulation")]
        public double ActualPressure01
        {
            get
            {
                return CPARDevice.BinaryToPressure(mResponse.GetByte(8), CPARDevice.PressureType.STIMULATING_PRESSURE);
            }
        }
        [Category("6 Pressure Stimulation")]
        public double ActualPressure01Binary
        {
            get
            {
                return mResponse.GetByte(8);
            }
        }

        #endregion
        #region BYTE 9: ACTUAL PRESSURE 02
        [Category("6 Pressure Stimulation")]
        public double ActualPressure02
        {
            get
            {
                return CPARDevice.BinaryToPressure(mResponse.GetByte(9), CPARDevice.PressureType.STIMULATING_PRESSURE);
            }
        }
        [Category("6 Pressure Stimulation")]
        public double ActualPressure02Binary
        {
            get
            {
                return mResponse.GetByte(9);
            }
        }
        #endregion
        #region BYTE 10: TARGET PRESSURE 01
        [Category("6 Pressure Stimulation")]
        public double TargetPressure01
        {
            get
            {
                return CPARDevice.BinaryToPressure(mResponse.GetByte(10), CPARDevice.PressureType.STIMULATING_PRESSURE);
            }
        }
        [Category("6 Pressure Stimulation")]
        public double TargetPressure01Binary
        {
            get
            {
                return mResponse.GetByte(10);
            }
        }
        #endregion
        #region BYTE 11: TARGET PRESSURE 02
        [Category("6 Pressure Stimulation")]
        public double TargetPressure02
        {
            get
            {
                return CPARDevice.BinaryToPressure(mResponse.GetByte(11), CPARDevice.PressureType.STIMULATING_PRESSURE);
            }
        }
        [Category("6 Pressure Stimulation")]
        public double TargetPressure02Binary
        {
            get
            {
                return mResponse.GetByte(11);
            }
        }
        #endregion
        #region BYTE 12: FINAL PRESSURE 01
        [Category("6 Pressure Stimulation")]
        public double FinalPressure01
        {
            get
            {
                return CPARDevice.BinaryToPressure(mResponse.GetByte(12), CPARDevice.PressureType.STIMULATING_PRESSURE);
            }
        }
        [Category("6 Pressure Stimulation")]
        public double FinalPressure01Binary
        {
            get
            {
                return mResponse.GetByte(12);
            }
        }
        #endregion
        #region BYTE 13: FINAL PRESSURE 02
        [Category("6 Pressure Stimulation")]
        public double FinalPressure02
        {
            get
            {
                return CPARDevice.BinaryToPressure(mResponse.GetByte(13), CPARDevice.PressureType.STIMULATING_PRESSURE);
            }
        }
        [Category("6 Pressure Stimulation")]
        public double FinalPressure02Binary
        {
            get
            {
                return mResponse.GetByte(13);
            }
        }
        #endregion
        #region BYTE 14: STOP PRESSED
        [Category("4 - Pain Rating")]
        public bool StopPressed
        {
            get
            {
                return mResponse.GetByte(14) != 0;
            }
        }

        #endregion
        #endregion
    }
}
