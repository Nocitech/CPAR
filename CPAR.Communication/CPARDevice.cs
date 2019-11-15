using CPAR.Communication.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPAR.Communication
{
    public static class CPARDevice
    {
        public enum PressureType
        {
            SUPPLY_PRESSURE = 0,
            STIMULATING_PRESSURE
        }

        public static readonly int UPDATE_RATE = 20;
        public static readonly double MAX_PRESSURE = 100;
        public static readonly double MAX_SUPPLY_PRESSURE = 1000;
        public static readonly double MAX_SCORE = 10;
        
        public static int TimeToRate(double time)
        {
            return (int) Math.Round(time * UPDATE_RATE);
        }

        public static double BinaryToPressure(byte x, PressureType type = PressureType.STIMULATING_PRESSURE)
        {
            if (type == PressureType.SUPPLY_PRESSURE)
            {
                return MAX_SUPPLY_PRESSURE * ((double)x) / byte.MaxValue;
            }
            else
            {
                return MAX_PRESSURE * ((double)x) / byte.MaxValue;
            }
        }

        public static double BinaryToScore(byte x)
        {
            return MAX_SCORE * ((double)x) / byte.MaxValue;
        }

        public static double PressureToBinary(double pressure)
        {
            return (255.0 / 100.0) * pressure;
        }

        public static double DeltaPressureToBinary(double delta)
        {
            return (255.0 / 100.0) * (delta/UPDATE_RATE);
        }

        public static double CountToTime(int count)
        {
            return ((double)count) / UPDATE_RATE;
        }

        public static ushort TimeToCount(double time)
        {
            return (ushort)Math.Ceiling(time * UPDATE_RATE);
        }

        public static SetWaveformProgram CreateEmptyProgram(byte channel)
        {
            return new SetWaveformProgram()
            {
                Channel = channel,
                Instructions = new SetWaveformProgram.Instruction[] { },
                Repeat = 1
            };
        }

        public static SetWaveformProgram CreateRampProgram(byte channel, double delta, double limit)
        {
            return new SetWaveformProgram()
            {
                Channel = channel,
                Instructions = new SetWaveformProgram.Instruction[]
                {
                    SetWaveformProgram.CreateIncrementInstr(delta, limit/delta)
                },
                Repeat = 1
            };

        }

        public static SetWaveformProgram CreateConditioningProgram(byte channel, 
                                                                   double DELTA_COND_PRESSURE, 
                                                                   double CONDITIONING_PRESSURE,
                                                                   double DELTA_PRESSURE,
                                                                   double PRESSURE_LIMIT)
        {
            return new SetWaveformProgram()
            {
                Channel = channel,
                Instructions = new SetWaveformProgram.Instruction[]
                {
                    new SetWaveformProgram.Instruction()
                    {
                        InstructionType = SetWaveformProgram.InstructionType.INC,
                        Argument = DeltaPressureToBinary(DELTA_COND_PRESSURE),
                        Steps = TimeToCount(CONDITIONING_PRESSURE/DELTA_COND_PRESSURE)
                    },
                    new SetWaveformProgram.Instruction()
                    {
                        InstructionType = SetWaveformProgram.InstructionType.STEP,
                        Argument = PressureToBinary(CONDITIONING_PRESSURE),
                        Steps = TimeToCount(PRESSURE_LIMIT/DELTA_PRESSURE)
                    }
                },
            };
        }

        public static SetWaveformProgram CreateDelayedRampProgram(byte channel, 
                                                                  double DELTA_COND_PRESSURE,
                                                                  double CONDITIONING_PRESSURE,
                                                                  double DELTA_PRESSURE,
                                                                  double PRESSURE_LIMIT)
        {
            return new SetWaveformProgram()
            {
                Channel = channel,
                Instructions = new SetWaveformProgram.Instruction[]
                {
                    new SetWaveformProgram.Instruction()
                    {
                        InstructionType = SetWaveformProgram.InstructionType.STEP,
                        Argument = 0,
                        Steps = TimeToCount(CONDITIONING_PRESSURE/DELTA_COND_PRESSURE)
                    },
                    new SetWaveformProgram.Instruction()
                    {
                        InstructionType = SetWaveformProgram.InstructionType.INC,
                        Argument = DeltaPressureToBinary(DELTA_PRESSURE),
                        Steps = TimeToCount(PRESSURE_LIMIT/DELTA_PRESSURE)
                    }
                },
                Repeat = 1
            };
        }

        public static SetWaveformProgram CreatePulseProgram(byte channel,
                                                            byte NO_OF_STIMULI,
                                                            double T_ON,
                                                            double T_OFF,
                                                            double P_STIMULATE,
                                                            double P_STATIC)
        {
            return new SetWaveformProgram()
            {
                Channel = channel,
                Instructions = new SetWaveformProgram.Instruction[]
                {
                    new SetWaveformProgram.Instruction()
                    {
                        InstructionType = SetWaveformProgram.InstructionType.STEP,
                        Argument = PressureToBinary(P_STIMULATE),
                        Steps = TimeToCount(T_ON)
                    },
                    new SetWaveformProgram.Instruction()
                    {
                        InstructionType = SetWaveformProgram.InstructionType.STEP,
                        Argument = PressureToBinary(P_STATIC),
                        Steps = TimeToCount(T_OFF)
                    }
                },
                Repeat = NO_OF_STIMULI
            };
        }

        public static SetWaveformProgram CreateStaticProgram(byte channel,                                                            
                                                            double t,
                                                            double p)
        {
            return new SetWaveformProgram()
            {
                Channel = channel,
                Instructions = new SetWaveformProgram.Instruction[]
                {
                    new SetWaveformProgram.Instruction()
                    {
                        InstructionType = SetWaveformProgram.InstructionType.STEP,
                        Argument = PressureToBinary(p),
                        Steps = TimeToCount(t)
                    }
                },
                Repeat = 1
            };
        }

        public static SetWaveformProgram CreateNormalProbe(byte channel,
                                                           double P_STIMULATE,
                                                           double T_ON,
                                                           double T_RESPONSE)
        {
            return new SetWaveformProgram()
            {
                Channel = channel,
                Instructions = new SetWaveformProgram.Instruction[]
                {
                    new SetWaveformProgram.Instruction()
                    {
                        InstructionType = SetWaveformProgram.InstructionType.STEP,
                        Argument = PressureToBinary(P_STIMULATE),
                        Steps = TimeToCount(T_ON)
                    },
                    new SetWaveformProgram.Instruction(SetWaveformProgram.InstructionType.STEP)
                    {
                        Argument = 0,
                        Steps = TimeToCount(T_RESPONSE)
                    }
                },
                Repeat = 1
            };
        }

        public static SetWaveformProgram CreateStartleProbe(byte channel,
                                                            double P_STIMULATE,
                                                            double P_STARTLE,
                                                            double T_ON,
                                                            double T_DELAY,
                                                            double T_STARTLE,
                                                            double T_RESPONSE)
        {
            List<SetWaveformProgram.Instruction> instr = new List<SetWaveformProgram.Instruction>();
            ushort N_ON = TimeToCount(T_ON);
            ushort N_DELAY = TimeToCount(T_DELAY);
            ushort N_STARTLE = TimeToCount(T_STARTLE);

            if ((N_DELAY > 0) && (N_DELAY + N_STARTLE < N_ON))
            {
                instr.Add(new SetWaveformProgram.Instruction(SetWaveformProgram.InstructionType.STEP)
                {
                    Argument = PressureToBinary(P_STIMULATE),
                    Steps = N_DELAY
                });
                instr.Add(new SetWaveformProgram.Instruction(SetWaveformProgram.InstructionType.STEP)
                {
                    Argument = PressureToBinary(P_STARTLE),
                    Steps = N_STARTLE
                });
                instr.Add(new SetWaveformProgram.Instruction(SetWaveformProgram.InstructionType.STEP)
                {
                    Argument = PressureToBinary(P_STIMULATE),
                    Steps = (ushort) (N_ON - N_DELAY - N_STARTLE)
                });
            }
            else if ((N_DELAY > 0) && (N_DELAY + N_STARTLE >= N_ON))
            {
                instr.Add(new SetWaveformProgram.Instruction(SetWaveformProgram.InstructionType.STEP)
                {
                    Argument = PressureToBinary(P_STIMULATE),
                    Steps = N_DELAY
                });
                instr.Add(new SetWaveformProgram.Instruction(SetWaveformProgram.InstructionType.STEP)
                {
                    Argument = PressureToBinary(P_STARTLE),
                    Steps = (ushort) (N_ON - N_DELAY)
                });
            }
            else // ((N_DELAY == 0)
            {
                instr.Add(new SetWaveformProgram.Instruction(SetWaveformProgram.InstructionType.STEP)
                {
                    Argument = PressureToBinary(P_STARTLE),
                    Steps = N_STARTLE
                });
                instr.Add(new SetWaveformProgram.Instruction(SetWaveformProgram.InstructionType.STEP)
                {
                    Argument = PressureToBinary(P_STIMULATE),
                    Steps = (ushort)(N_ON - N_STARTLE)
                });
            }

            instr.Add(new SetWaveformProgram.Instruction(SetWaveformProgram.InstructionType.STEP)
            {
                Argument = 0,
                Steps = TimeToCount(T_RESPONSE)
            });

            return new SetWaveformProgram()
            {
                Channel = channel,
                Instructions = instr.ToArray(),
                Repeat = 1
            };
        }
    }
}
