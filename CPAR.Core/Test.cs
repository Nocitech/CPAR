using CPAR.Communication;
using CPAR.Communication.Functions;
using CPAR.Communication.Messages;
using CPAR.Core.Results;
using CPAR.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CPAR.Core
{
    public abstract class Test :
        IArrayIndex
    {
        public const int UPDATE_RATE = 20; // 20Hz

        public static event EventHandler<TestState> StateChanged;

        public enum TestState
        {
            BLOCKED = 0,
            READY,
            RUNNING,
            PENDING,
            COMPLETED
        }

        private enum InternalState
        {
            IDLE = 0,
            RUNNING,
            PENDING
        }

        private InternalState state = InternalState.IDLE;
        private bool hasFocus = false;

        [XmlAttribute(AttributeName = "ID")]
        public string ID { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlElement("instruction")]
        public string Instruction { get; set; }
        [XmlIgnore]
        public int Index { get; set; }

        public static void Initialize(Test[] tests)
        {
            foreach (var test in tests)
            {
                testList.Add(test);
                DeviceManager.StatusReceived += test.OnStatusReceived;
            }
        }

        private void OnStatusReceived(object sender, StatusMessage msg)
        {
            if (state == InternalState.RUNNING)
            {
                Process(msg);
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public abstract bool IsBlocked();

        public virtual bool RequireTimer
        {
            get
            {
                return false;
            }
        }

        public virtual int Period
        {
            get
            {
                return 1000;
            }
        }

        public virtual void Run()
        {

        }

        protected abstract Result GetResult();

        protected abstract bool StartTest();

        protected abstract void Process(StatusMessage msg);

        protected abstract void InitializeChart();


        [XmlIgnore]
        public abstract CalculatedParameter[] ExternalParameters { get; }

        [XmlIgnore]
        public abstract int NumberOfExternalParameters { get; }

        [XmlIgnore]
        public abstract Test[] Dependencies { get; }
        
        public Test[] Dependents
        {
            get
            {
                List<Test> retValue = new List<Test>();

                foreach (var test in Experiment.Active.Protocol.Tests)
                {
                    if (test.ID != ID)
                    {
                        if (test.State == TestState.COMPLETED)
                        {
                            if (test.Dependencies.Count((d) => d.ID == ID) > 0)
                            {
                                retValue.Add(test);
                            }
                        }
                    }
                }

                return retValue.ToArray();
            }
        }

        [XmlIgnore]
        public TestState State
        {
            get
            {
                TestState retValue = TestState.BLOCKED;

                switch (state)
                {
                    case InternalState.IDLE:
                        if (IsBlocked())
                        {
                            if (!Session.Active.Complete(ID))
                            {
                                retValue = TestState.BLOCKED;
                            }
                            else
                            {
                                retValue = TestState.COMPLETED;
                            }
                        }
                        else
                        {
                            if (Session.Active.Complete(ID))
                            {
                                retValue = TestState.COMPLETED;
                            }
                            else
                            {
                                retValue = TestState.READY;
                            }
                        }
                        break;
                    case InternalState.RUNNING:
                        retValue = TestState.RUNNING;
                        break;
                    case InternalState.PENDING:
                        retValue = TestState.PENDING;
                        break;
                }

                return retValue;
            }
        }

        public void Focus()
        {
            testList.ForEach((t) => t.OnFocusChanged(ID));
        }

        public bool Focused { get { return hasFocus; } }

        private void OnFocusChanged(string id)
        {
            if (id == ID)
            {
                if (!hasFocus)
                {
                    Visualizer.Title = Name;
                    InitializeChart();

                    var result = Session.Active.GetResult(ID);

                    if (result is NullResult)
                    {
                        Visualizer.Initialize(new double[] { 0 },
                                              new double[] { 0 },
                                              new double[] { 0 });
                    }
                    else
                    {
                        Visualizer.Initialize(result.StimulationPressure,
                                              result.ConditioningPressure,
                                              result.VAS);
                    }

                    hasFocus = true;
                }
            }
            else
            {
                hasFocus = false;
            }
        }

        public void Start()
        {
            if ((state == InternalState.IDLE) && !IsBlocked())
            {
                if (StartTest())
                {
                    Visualizer.Initialize();
                    state = InternalState.RUNNING;
                    Log.Status("Test [{0}] started", Name);
                    StateChanged?.Invoke(this, State);
                }
            }
            else
            {
                Log.Debug("Attempted to start test while it was in {0} state", state.ToString());
            }
        }

        public void Abort()
        {
            if ((state == InternalState.PENDING) || (state == InternalState.RUNNING))
            {
                try
                {
                    if (state == InternalState.RUNNING)
                    {
                        StopDevice();
                    }

                    state = InternalState.IDLE;
                    StateChanged?.Invoke(this, State);
                    Log.Status("Test [{0}] aborted", Name);
                }
                catch (Exception e)
                {
                    Log.Debug(e.Message);
                }
            }
            else
            {
                Log.Debug("Attempted to abort test while it was in {0} state", state.ToString());
            }
        }

        public void Accept()
        {
            if (state == InternalState.PENDING)
            {
                var result = GetResult();

                if (result != null)
                {
                    result.ID = ID;
                    result.Name = Name;
                    result.Index = Index;
                    Session.Active.Add(result);
                    state = InternalState.IDLE;
                    StateChanged?.Invoke(this, State);
                    Log.Status("Test [{0}] completed", Name);
                }
                else
                {
                    throw new InvalidOperationException("result is null");
                }
            }
            else
            {
                Log.Debug("Attempted to accept test while it was in {0} state", state.ToString());
            }

        }

        protected void Pending()
        {
            if (state == InternalState.RUNNING)
            {
                state = InternalState.PENDING;
                StateChanged?.Invoke(this, State);
                Log.Debug("Test [{0}] pending", Name);
            }
            else
            {
                Log.Debug("Attempted to set state to pending while it was in {0} state", state.ToString());
            }
        }

        protected void StartDevice(AlgometerStopCriterion criterion)
        {
            DeviceManager.Execute(new StartStimulation() { Criterion = criterion });
        }

        protected void ForceStartDevice(AlgometerStopCriterion criterion)
        {
            DeviceManager.Execute(new ForceStartStimulation() { Criterion = criterion });
        }

        protected void StopDevice()
        {
            try
            {
                if (DeviceManager.DeviceState == StatusMessage.State.STATE_STIMULATING)
                {
                    DeviceManager.Execute(new StopStimulation());
                }
            }
            catch { }
        }

        private static List<Test> testList = new List<Test>();
    }
}
