using NLog.Common;
using NLog.Targets.Wrappers;
using System.Collections.Generic;

namespace NLog.ManualFlush
{
    public class ManualFlushWrapper : WrapperTargetBase
    {
        private readonly IList<AsyncLogEventInfo> logs = new List<AsyncLogEventInfo>();

        protected override void Write(AsyncLogEventInfo logEvent)
        {
            logs.Add(logEvent);
        }

        protected override void FlushAsync(AsyncContinuation asyncContinuation)
        {
            foreach (var log in logs)
            {
                WrappedTarget.WriteAsyncLogEvent(log);
            }

            logs.Clear();
            base.FlushAsync(asyncContinuation);
        }
    }
}
