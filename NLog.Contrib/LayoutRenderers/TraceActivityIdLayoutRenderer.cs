using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using NLog;
using NLog.LayoutRenderers;
using NLog.Config;
using NLog.Internal;

namespace NLog.Contrib.LayoutRenderers
{
    /// <summary>
    /// A renderer that puts into log a System.Diagnostics trace correlation id.
    /// </summary>
    [LayoutRenderer("activityid")]
    public class TraceActivityIdLayoutRenderer : LayoutRenderer
    {
        /// <summary>
        /// Renders the current trace activity ID.
        /// </summary>
        /// <param name="builder">The <see cref="StringBuilder"/> to append the rendered data to.</param>
        /// <param name="logEvent">Logging event.</param>
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            builder.Append(Guid.Empty.Equals(Trace.CorrelationManager.ActivityId) ? 
                String.Empty : Trace.CorrelationManager.ActivityId.ToString("D", CultureInfo.InvariantCulture));
        }
    }
}
