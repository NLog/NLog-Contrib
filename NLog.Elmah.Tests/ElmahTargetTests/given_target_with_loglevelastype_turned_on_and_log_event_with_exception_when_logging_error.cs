using System;

using NUnit.Framework;

namespace NLog.Elmah.Tests.ElmahTargetTests
{
	[TestFixture]
	public class given_target_with_loglevelastype_turned_on_and_log_event_with_exception_when_logging_error : given_target_with_loglevelastype_turned_on
	{
		private Exception _exception;
        
		[SetUp]
		public void SetUp()
		{
			ErrorLog.Clear();
			var logger = LogManager.GetLogger("Test");

			_exception = new ArgumentException {Source = "SetUp"};

			try
			{
				new Thrower().Throw(_exception);
			}
			catch (Exception ex)
			{
				logger.ErrorException("This is an error message.", ex);
			}

		}

		public class Thrower
		{
			public void Throw(Exception ex)
			{
				throw ex;
			}
		}

		[Test]
		public void should_set_message_to_rendered_message()
		{
			var error = ErrorLog.GetFirstError();
			Assert.That(error.Message, Is.EqualTo("Error-This is an error message."));
		}

		[Test]
		public void should_set_log_type_to_full_name_of_exception_type()
		{
			var error = ErrorLog.GetFirstError();
			Assert.That(error.Type, Is.EqualTo("System.ArgumentException"));
		}

		[Test]
		public void should_set_exception()
		{
			var error = ErrorLog.GetFirstError();
			Assert.That(error.Exception, Is.EqualTo(_exception));
		}

		[Test]
		public void should_set_detail_to_stack_trace()
		{
			var error = ErrorLog.GetFirstError();
			Assert.That(error.Detail, Is.EqualTo(_exception.StackTrace));
		}

		[Test]
		public void should_set_source()
		{
			var error = ErrorLog.GetFirstError();
			Assert.That(error.Source, Is.EqualTo(_exception.Source));
		}

		[Test]
		public void should_set_time_to_now()
		{
			var error = ErrorLog.GetFirstError();
			Assert.That(error.Time, Is.EqualTo(Now));
		}

		[Test]
		public void should_set_host_name_to_machine_name()
		{
			var error = ErrorLog.GetFirstError();
			Assert.That(error.HostName, Is.EqualTo(Environment.MachineName));
		}
	}
}