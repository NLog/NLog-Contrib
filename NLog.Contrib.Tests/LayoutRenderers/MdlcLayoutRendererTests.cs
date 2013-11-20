// Copyright 2013 Kim Christensen, Todd Meinershagen, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.

using System.Xml.Linq;
using NLog.Config;
using NLog.Contrib.LayoutRenderers;
using NLog.Targets;
using NUnit.Framework;

namespace NLog.Contrib.Tests.LayoutRenderers
{
    [TestFixture]
    public class MdlcLayoutRendererTests
    {
        private static DebugTarget _target;

        [SetUp]
        public static void SetUp()
        {
            ConfigurationItemFactory.Default.LayoutRenderers.RegisterDefinition("mdlc", typeof(MdlcLayoutRenderer));

            const string configXml = @"
            <nlog>
                <targets><target name='debug' type='Debug' layout='${mdlc:Item=myitem}${message}' /></targets>
                <rules>
                    <logger name='*' minlevel='Debug' writeTo='debug' />
                </rules>
            </nlog>";

            var element = XElement.Parse(configXml);
            var config = new XmlLoggingConfiguration(element.CreateReader(), null);
            LogManager.Configuration = config;

            _target = LogManager.Configuration.FindTargetByName("debug") as DebugTarget;

            MappedDiagnosticsLogicalContext.Clear();
        }

        [Test]
        public void given_item_does_not_exist_when_rendering_item_and_message_should_render_only_message()
        {
            const string message = "message";
            LogManager.GetLogger("A").Debug(message);
            Assert.That(_target.LastMessage, Is.EqualTo(message));
        }

        [Test]
        public void given_item_exists_when_rendering_item_and_message_should_render_item_and_message()
        {
            const string message = "message";
            const string key = "myitem";
            const string item = "item";

            MappedDiagnosticsLogicalContext.Set(key, item);
            LogManager.GetLogger("A").Debug(message);

            Assert.That(_target.LastMessage, Is.EqualTo(item + message));
        }
    }
}
