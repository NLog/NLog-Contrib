using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace NLogContrib
{
    /// <summary>
    /// Mapped Diagnostics Logical Context - a logical context structure that keeps a dictionary
    /// of strings and provides methods to output them in layouts.
    /// Mostly for compatibility with log4net (log4net.ThreadLogicalContext).
    /// </summary>
    public static class MappedDiagnosticsLogicalContext
    {
        private const string LogicalContextDictKey = "NLog.MappedDiagnosticsLogicalContext";

        private static IDictionary<string, string> LogicalContextDict
        {
            get
            {
                var dict = CallContext.LogicalGetData(LogicalContextDictKey) as ConcurrentDictionary<string, string>;
                if (dict == null)
                {
                    dict = new ConcurrentDictionary<string, string>();
                    CallContext.LogicalSetData(LogicalContextDictKey, dict);
                }
                return dict;
            }
        }

        /// <summary>
        /// Sets the current logical context item to the specified value.
        /// </summary>
        /// <param name="item">Item name.</param>
        /// <param name="value">Item value.</param>
        public static void Set(string item, string value)
        {
            LogicalContextDict[item] = value;
        }

        /// <summary>
        /// Gets the current logical context named item.
        /// </summary>
        /// <param name="item">Item name.</param>
        /// <returns>The item value of string.Empty if the value is not present.</returns>
        public static string Get(string item)
        {
            string s;

            if (!LogicalContextDict.TryGetValue(item, out s))
            {
                s = string.Empty;
            }

            return s;
        }

        /// <summary>
        /// Checks whether the specified item exists in current logical context.
        /// </summary>
        /// <param name="item">Item name.</param>
        /// <returns>A boolean indicating whether the specified item exists in current thread MDC.</returns>
        public static bool Contains(string item)
        {
            return LogicalContextDict.ContainsKey(item);
        }

        /// <summary>
        /// Removes the specified item from current logical context.
        /// </summary>
        /// <param name="item">Item name.</param>
        public static void Remove(string item)
        {
            LogicalContextDict.Remove(item);
        }

        /// <summary>
        /// Clears the content of current logical context.
        /// </summary>
        public static void Clear()
        {
            LogicalContextDict.Clear();
        }
    }
}
