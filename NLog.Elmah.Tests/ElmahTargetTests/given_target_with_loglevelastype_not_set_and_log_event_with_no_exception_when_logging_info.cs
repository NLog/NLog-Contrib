using System;

using NUnit.Framework;

namespace NLog.Elmah.Tests.ElmahTargetTests
{
	[TestFixture]
	public class given_target_with_loglevelastype_not_set_and_log_event_with_no_exception_when_logging_info : given_target_with_loglevelastype_not_set
	{
		[SetUp]
		public void SetUp()
		{
			ErrorLog.Clear();
			var logger = LogManager.GetLogger("Test");
			logger.Info("This is an info message.");
		}

		[Test]
		public void should_set_message_to_rendered_message()
		{
			var error = ErrorLog.GetFirstError();
			Assert.That(error.Message, Is.EqualTo("Info-This is an info message."));
		}

		[Test]
		public void should_set_detail_to_rendered_message()
		{
			var error = ErrorLog.GetFirstError();
			Assert.That(error.Detail, Is.EqualTo("Info-This is an info message."));
		}

		[Test]
		public void should_set_log_type_to_empty_string()
		{
			var error = ErrorLog.GetFirstError();
			Assert.That(error.Type, Is.Empty);
		}

		[Test]
		public void should_not_set_exception()
		{
			var error = ErrorLog.GetFirstError();
			Assert.That(error.Exception, Is.Null);
		}

		[Test]
		public void should_not_set_source()
		{
			var error = ErrorLog.GetFirstError();
			Assert.That(error.Source, Is.Empty);
		}

		[Test]
		public void should_set_time_to_now()
		{
			var error = ErrorLog.GetFirstError();
			Assert.That(error.Time, Is.EqualTo(_now));
		}

		[Test]
		public void should_set_host_name_to_machine_name()
		{
			var error = ErrorLog.GetFirstError();
			Assert.That(error.HostName, Is.EqualTo(Environment.MachineName));
		}
	}
}