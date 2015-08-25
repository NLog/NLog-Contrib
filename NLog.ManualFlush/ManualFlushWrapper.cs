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
using NLog.Common;
using NLog.Targets;
using NLog.Targets.Wrappers;
using System.Collections.Generic;

namespace NLog.ManualFlush
{
    [Target("ManualFlush")]
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
        
        public void EmptyLogs()
		{
			logs.Clear();
		}
    }
}
