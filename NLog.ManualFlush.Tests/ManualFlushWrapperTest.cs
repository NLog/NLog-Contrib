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
using NLog.Config;
using NLog.Targets;
using Xunit;

namespace NLog.ManualFlush.Tests
{
    public class ManualFlushWrapperTest
    {
        private DebugTarget debugTarget;
        private ManualFlushWrapper manualFlushTarget;

        public ManualFlushWrapperTest()
        {
            var loggingConfiguration = new LoggingConfiguration();
            debugTarget = new DebugTarget();
            manualFlushTarget = new ManualFlushWrapper
            {
                WrappedTarget = debugTarget
            };
            loggingConfiguration.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, manualFlushTarget));
            loggingConfiguration.AddTarget("Manual", manualFlushTarget);
            LogManager.Configuration = loggingConfiguration;
        }

        [Fact]
        public void Writing_To_Manual_Target_Wrapper_Does_Not_Write_To_Wrapped_Target()
        {
            var logger = LogManager.GetLogger("A");

            logger.Debug("Test");

            Assert.Equal(0, debugTarget.Counter);
        }

        [Fact]
        public void Flushing_Manual_Target_Wrapper_Writes_To_Wrapped_Target()
        {
            var logger = LogManager.GetLogger("A");
            logger.Debug("Test");

            manualFlushTarget.Flush(exception => { });

            Assert.Equal(1, debugTarget.Counter);
        }

        [Fact]
        public void Flushing_Using_LogManager_Writes_To_Wrapped_Target()
        {
            var logger = LogManager.GetLogger("A");
            logger.Debug("Test");

            LogManager.Flush();

            Assert.Equal(1, debugTarget.Counter);
        }

        [Fact]
        public void Flushing_Writes_All_Messages_To_The_Wrapped_Target()
        {
            var logger = LogManager.GetLogger("A");
            logger.Debug("Test");
            logger.Debug("Test2");

            LogManager.Flush();

            Assert.Equal(2, debugTarget.Counter);
        }

        [Fact]
        public void Flushing_Multiple_Times_Only_Writes_To_Wrapped_Target_Once()
        {
            var logger = LogManager.GetLogger("A");
            logger.Debug("Test");
            logger.Debug("Test2");
            LogManager.Flush();
            
            LogManager.Flush();

            Assert.Equal(2, debugTarget.Counter);
        }

        [Fact]
        public void Flushing_Multiple_Time_Only_Writes_New_Log_Entries_To_Wrapped_Target_Once()
        {
            var logger = LogManager.GetLogger("A");
            logger.Debug("Test");
            LogManager.Flush();
            Assert.Equal(1, debugTarget.Counter);
            logger.Debug("Test2");

            LogManager.Flush();

            Assert.Equal(2, debugTarget.Counter);
        }
    }
}
