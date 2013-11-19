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

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

namespace NLogContrib
{
    /// <summary>
    /// Async version of Mapped Diagnostics Context - a logical context structure that keeps a dictionary
    /// of strings and provides methods to output them in layouts.  Allows for maintaining state across
    /// asynchronous tasks and call contexts.
    /// Mostly for compatibility with log4net (log4net.ThreadLogicalContext).
    /// </summary>
    /// <remarks>
    /// Ideally, these changes should be incorporated as a new version of the MappedDiagnosticsContext class in the original
    /// NLog library so that state can be maintained for multiple threads in asynchronous situations.
    /// </remarks>
    public static class MappedDiagnosticsLogicalContext
    {
        private const string LogicalThreadDictionaryKey = "NLog.AsyncableMappedDiagnosticsContext";

        private static IDictionary<string, string> LogicalThreadDictionary
        {
            get
            {
                var dictionary = CallContext.LogicalGetData(LogicalThreadDictionaryKey) as ConcurrentDictionary<string, string>;
                if (dictionary == null)
                {
                    dictionary = new ConcurrentDictionary<string, string>();
                    CallContext.LogicalSetData(LogicalThreadDictionaryKey, dictionary);
                }
                return dictionary;
            }
        }

        /// <summary>
        /// Gets the current logical context named item.
        /// </summary>
        /// <param name="item">Item name.</param>
        /// <returns>The item value of string.Empty if the value is not present.</returns>
        public static string Get(string item)
        {
            string s;

            if (!LogicalThreadDictionary.TryGetValue(item, out s))
            {
                s = string.Empty;
            }

            return s;
        }

        /// <summary>
        /// Sets the current logical context item to the specified value.
        /// </summary>
        /// <param name="item">Item name.</param>
        /// <param name="value">Item value.</param>
        public static void Set(string item, string value)
        {
            LogicalThreadDictionary[item] = value;
        }

        /// <summary>
        /// Checks whether the specified item exists in current logical context.
        /// </summary>
        /// <param name="item">Item name.</param>
        /// <returns>A boolean indicating whether the specified item exists in current thread MDC.</returns>
        public static bool Contains(string item)
        {
            return LogicalThreadDictionary.ContainsKey(item);
        }

        /// <summary>
        /// Removes the specified item from current logical context.
        /// </summary>
        /// <param name="item">Item name.</param>
        public static void Remove(string item)
        {
            LogicalThreadDictionary.Remove(item);
        }

        /// <summary>
        /// Clears the content of current logical context.
        /// </summary>
        public static void Clear()
        {
            LogicalThreadDictionary.Clear();
        }
    }
}
