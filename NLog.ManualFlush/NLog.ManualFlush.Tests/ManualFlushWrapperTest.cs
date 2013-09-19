using System.Diagnostics;
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
