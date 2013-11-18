using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NLog;
using NLog.Config;
using NLogContrib;
using NLogContrib.LayoutRenderers;
using NUnit.Framework;

namespace NLogContrib.Tests.LayoutRenderers
{
    [TestFixture]
    public class MDLCTests
    {
        [SetUp]
        public static void SetUp()
        {
            NLog.Config.ConfigurationItemFactory.Default.LayoutRenderers.RegisterDefinition("mdlc", typeof(MdlcLayoutRenderer));

            string cfgStr = @"
            <nlog>
                <targets><target name='debug' type='Debug' layout='${mdlc:item=myitem} ${message}' /></targets>
                <rules>
                    <logger name='*' minlevel='Debug' writeTo='debug' />
                </rules>
            </nlog>";

            XElement el = XElement.Parse(cfgStr);
            XmlLoggingConfiguration cfg = new XmlLoggingConfiguration(el.CreateReader(), null);
            LogManager.Configuration = cfg;
        }


        [Test]
        public void MDLCSingleThreadTest()
        {
            var dbgTarget = (NLog.Targets.DebugTarget)LogManager.Configuration.FindTargetByName("debug");

            MappedDiagnosticsLogicalContext.Clear();
            MappedDiagnosticsLogicalContext.Set("myitem", "myvalue");
            LogManager.GetLogger("A").Debug("a");
            Assert.AreEqual("myvalue a", dbgTarget.LastMessage);

            MappedDiagnosticsLogicalContext.Set("myitem", "value2");
            LogManager.GetLogger("A").Debug("b");
            Assert.AreEqual("value2 b", dbgTarget.LastMessage);


            MappedDiagnosticsLogicalContext.Remove("myitem");
            LogManager.GetLogger("A").Debug("c");
            Assert.AreEqual(" c", dbgTarget.LastMessage);
        }

        [Test]
        public void MDLCMultiThreadTest()
        {
            var dbgTarget = (NLog.Targets.DebugTarget)LogManager.Configuration.FindTargetByName("debug");

            MappedDiagnosticsLogicalContext.Clear();
            MappedDiagnosticsLogicalContext.Set("myitem", "myvalue");
            LogManager.GetLogger("A").Debug("a");
            Assert.AreEqual("myvalue a", dbgTarget.LastMessage);


            var task1 = Task<string>.Run(() =>
                {
                    LogManager.GetLogger("A").Debug("b");
                    return dbgTarget.LastMessage;
                });

            task1.Wait();
            Assert.AreEqual("myvalue b", dbgTarget.LastMessage);
        }
    }
}
