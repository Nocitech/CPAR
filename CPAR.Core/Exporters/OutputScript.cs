using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting;

namespace CPAR.Core.Exporters
{
    public class OutputScript
    {
        public double Execute(SessionScriptEngine engine)
        {
            ThrowIf.Argument.IsNull(engine, "engine");
            ThrowIf.Argument.IsNull(Script, "Script");
            ThrowIf.Argument.IsNull(Name, "Name");

            return engine.Engine.CreateScriptSourceFromString(Script, SourceCodeKind.Expression).Execute(engine.Scope);
        }

        [XmlAttribute("script")]
        public string Script { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }
    }
}
