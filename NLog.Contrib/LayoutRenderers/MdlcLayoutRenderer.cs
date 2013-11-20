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

using System.Text;
using NLog.Config;
using NLog.LayoutRenderers;

namespace NLog.Contrib.LayoutRenderers
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
            var message = MappedDiagnosticsLogicalContext.Get(Item);
            builder.Append(message);
        }
    }

}
