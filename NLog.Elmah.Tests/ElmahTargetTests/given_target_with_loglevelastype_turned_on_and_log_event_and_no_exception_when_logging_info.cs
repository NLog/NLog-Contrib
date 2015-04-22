using NUnit.Framework;

namespace NLog.Elmah.Tests.ElmahTargetTests
{
	[TestFixture]
	public class given_target_with_loglevelastype_turned_on_and_log_event_and_no_exception_when_logging_info : given_target_with_loglevelastype_turned_on
	{
		[SetUp]
		public void SetUp()
		{
			ErrorLog.Clear();
			var logger = LogManager.GetLogger("Test");
			logger.Info("This is an info message.");
		}


		[Test]
		public void should_set_log_type_to_info()
		{
			var error = ErrorLog.GetFirstError();
			Assert.That(error.Type, Is.EqualTo("Info"));
		}
	}
}