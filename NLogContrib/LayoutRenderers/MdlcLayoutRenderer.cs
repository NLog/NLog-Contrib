using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.LayoutRenderers;

namespace NLogContrib.LayoutRenderers
{
    /// <summary>
    /// Mapped Diagnostic Logical Context item (based on CallContext).
    /// </summary>
    [LayoutRenderer("mdlc")]
    public class MdlcLayoutRenderer : LayoutRenderer
    {
        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        /// <docgen category='Rendering Options' order='10' />
        [RequiredParameter]
        [DefaultParameter]
        public string Item { get; set; }

        /// <summary>
        /// Renders the specified MDLC item and appends it to the specified <see cref="StringBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="StringBuilder"/> to append the rendered data to.</param>
        /// <param name="logEvent">Logging event.</param>
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            string msg = MappedDiagnosticsLogicalContext.Get(this.Item);
            builder.Append(msg);
        }
    }

}
