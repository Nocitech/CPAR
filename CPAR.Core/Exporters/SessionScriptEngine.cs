using System.Collections.Generic;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace CPAR.Core.Exporters
{
    public class SessionScriptEngine
    {
        public SessionScriptEngine(Dictionary<string, Result> results)
        {
            engine = Python.CreateEngine();
            scope = engine.CreateScope(new Dictionary<string, object>()
            {
                { "R", results }
            });
        }

        public ScriptEngine Engine
        {
            get
            {
                return engine;
            }
        }

        public ScriptScope Scope
        {
            get
            {
                return scope;
            }
        }

        private ScriptEngine engine;
        private ScriptScope scope; 

    }
}
