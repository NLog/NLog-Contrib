using NLog;
using NLog.Targets;
using System;
using System.Diagnostics;
using System.Diagnostics.Eventing;

namespace Nlog.Etw
{
    /// <summary>
    /// A NLog target with support for basic ETW tracing. When using this provider you should specify providerId in NLog configuration 
    /// and use this GUID for further event collection, eg.:
    /// 
    /// &lt;target name="etw" type="contribetw.EventTracing" providerId="ff1d574a-58a1-45f1-ae5e-040cf8d3fae2" layout="${longdate}|${uppercase:${level}}|${message}${onexception:|Exception occurred\:${exception:format=tostring}}" /&gt;
    /// 
    /// Sample configuration and usage sample can be found also on my blog: http://lowleveldesign.wordpress.com/2014/04/18/etw-providers-for-nlog/
    /// </summary>
    [Target("EventTracing")]
    public sealed class NLogEtwTarget : TargetWithLayout
    {
        private EventProvider provider;
        private Guid providerId = Guid.NewGuid();

        /// <summary>
        /// A provider guid that will be used in ETW tracing.
        /// </summary>
        public String ProviderId {
            get { return providerId.ToString(); }
            set {
                providerId = Guid.Parse(value);
            }
        }

        protected override void InitializeTarget() {
            base.InitializeTarget();

            // we will create an EventProvider for ETW
            try {
                provider = new EventProvider(providerId);
            } catch (PlatformNotSupportedException) {
                // sorry :(
            }
        }

        protected override void Write(LogEventInfo logEvent) {
            if (provider == null || !provider.IsEnabled()) {
                return;
            }
            byte t;
            if (logEvent.Level == LogLevel.Debug || logEvent.Level == LogLevel.Trace) {
                t = 5;
            } else if (logEvent.Level == LogLevel.Info) {
                t = 4;
            } else if (logEvent.Level == LogLevel.Warn) {
                t = 3;
            } else if (logEvent.Level == LogLevel.Error) {
                t = 2;
            } else if (logEvent.Level == LogLevel.Fatal) {
                t = 1;
            } else {
                t = 5; // let it be verbose
            }
            provider.WriteMessageEvent(this.Layout.Render(logEvent), t, 0);
        }

        protected override void CloseTarget() {
            base.CloseTarget();

            provider.Dispose();
        }
    }
}