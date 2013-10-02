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
using Elmah;
using NLog.Targets;

namespace NLog.Elmah
{
    [Target("Elmah")]
    public sealed class ElmahTarget : TargetWithLayout
    {
        private readonly ErrorLog _errorLog;

        public ElmahTarget()
            : this (ErrorLog.GetDefault(null))
        {}

        public ElmahTarget(ErrorLog errorLog)
        {
            _errorLog = errorLog;
        }

        protected override void Write(LogEventInfo logEvent)
        {
            var logMessage = Layout.Render(logEvent);

            var error = logEvent.Exception == null ? new Error() : new Error(logEvent.Exception);
            error.Message = logMessage;
            error.Type = logEvent.Level.ToString();
            error.Time = logEvent.TimeStamp;

            _errorLog.Log(error);
        }
    }
}
