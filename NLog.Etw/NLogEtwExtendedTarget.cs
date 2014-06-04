using NLog;
using NLog.Targets;
using System;
using System.Diagnostics;
using System.Diagnostics.Tracing;

namespace NLog.Etw
{
    /// <summary>
    /// A NLog target with support for extended ETW tracing. When using perfview or wpr to record the events use *LowLevelDesign-NLogEtwSource
    /// to enable the NLog provider.
    /// 
    /// Sample configuration and usage sample can be found on my blog: http://lowleveldesign.wordpress.com/2014/04/18/etw-providers-for-nlog/
    /// </summary>
    [Target("ExtendedEventTracing")]
    public sealed class NLogEtwExtendedTarget : TargetWithLayout
    {
        [EventSource(Name = "LowLevelDesign-NLogEtwSource")]
        private sealed class EtwLogger : EventSource
        {
            [Event(1, Level = EventLevel.Verbose)]
            public void Verbose(String LoggerName, String Message) {
                WriteEvent(1, LoggerName, Message);
            }

            [Event(2, Level = EventLevel.Informational)]
            public void Info(String LoggerName, String Message) {
                WriteEvent(2, LoggerName, Message);
            }

            [Event(3, Level = EventLevel.Warning)]
            public void Warn(String LoggerName, String Message) {
                WriteEvent(3, LoggerName, Message);
            }

            [Event(4, Level = EventLevel.Error)]
            public void Error(String LoggerName, String Message) {
                WriteEvent(4, LoggerName, Message);
            }

            [Event(5, Level = EventLevel.Critical)]
            public void Critical(String LoggerName, String Message) {
                WriteEvent(5, LoggerName, Message);
            }

            public readonly static EtwLogger Log = new EtwLogger();
        }

        protected override void Write(LogEventInfo logEvent)
        {
            if (!EtwLogger.Log.IsEnabled())
            {
                return;
            }
            if (logEvent.Level == LogLevel.Debug || logEvent.Level == LogLevel.Trace) {
                EtwLogger.Log.Verbose(logEvent.LoggerName, Layout.Render(logEvent));
            } else if (logEvent.Level == LogLevel.Info) {
                EtwLogger.Log.Info(logEvent.LoggerName, Layout.Render(logEvent));
            } else if (logEvent.Level == LogLevel.Warn) {
                EtwLogger.Log.Warn(logEvent.LoggerName, Layout.Render(logEvent));
            } else if (logEvent.Level == LogLevel.Error) {
                EtwLogger.Log.Error(logEvent.LoggerName, Layout.Render(logEvent));
            } else if (logEvent.Level == LogLevel.Fatal) {
                EtwLogger.Log.Critical(logEvent.LoggerName, Layout.Render(logEvent));
            }
        }
    }
}