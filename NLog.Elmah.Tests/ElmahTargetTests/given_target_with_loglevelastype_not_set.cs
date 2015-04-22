using System;

using Elmah;

using NLog.Config;
using NLog.Layouts;

using NUnit.Framework;

namespace NLog.Elmah.Tests.ElmahTargetTests
{
	public abstract class given_target_with_loglevelastype_not_set
	{
		protected ErrorLog ErrorLog;
		protected readonly DateTime _now = new DateTime(2013, 10, 8, 19, 5, 0);

		[TestFixtureSetUp]
		public void Init()
		{
			ErrorLog = new MemoryErrorLog(1);
			var loggingConfiguration = new LoggingConfiguration();
			var target = new ElmahTarget(ErrorLog)
			{
				Layout = new SimpleLayout("${level}-${message}"),
				GetCurrentDateTime = () => _now
			};

			loggingConfiguration.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, target));
			loggingConfiguration.AddTarget("Elmah", target);
			LogManager.Configuration = loggingConfiguration;
		}
	}
}