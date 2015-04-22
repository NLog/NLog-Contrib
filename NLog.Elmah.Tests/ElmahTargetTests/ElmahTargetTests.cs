using System;
using System.IO;
using System.Web;

using NUnit.Framework;

namespace NLog.Elmah.Tests.ElmahTargetTests
{
	public class ElmahTargetTests : given_target_with_loglevelastype_turned_on
	{
		private string _host;
		private Logger _logger = LogManager.GetLogger("Test");



		[Test]
		public void given_no_httpcontext_when_logging_info_should_not_set_up_server_variables_from_context()
		{
			HttpContext.Current = null;
			_logger.Info("This is an info message.");

			var error = ErrorLog.GetFirstError();
			Assert.That(error.ServerVariables, Is.Empty);
		}

		[Test]
		public void given_httpcontext_when_logging_info_should_not_set_up_server_variables_from_context()
		{
			HttpContext.Current = GetContext();
			_logger.Info("This is an info message.");

			var error = ErrorLog.GetFirstError();
			Assert.That(error.ServerVariables, Is.Empty);
		}

		[Test]
		public void given_httpcontext_when_logging_error_should_set_up_server_variables_from_context()
		{
			HttpContext.Current = GetContext();
			var exception = new ArgumentException { Source = "SetUp" };
			_logger.ErrorException("This is an error message.", exception);

			var error = ErrorLog.GetFirstError();
			Assert.That(error.ServerVariables, Is.Not.Empty);
		}

		[Test]
		public void given_no_httpcontext_when_logging_error_should_not_set_up_server_variables_from_context()
		{
			HttpContext.Current = null;

			var exception = new ArgumentException { Source = "SetUp" };
			_logger.ErrorException("This is an error message.", exception);

			var error = ErrorLog.GetFirstError();
			Assert.That(error.ServerVariables, Is.Empty);
		}

		private HttpContext GetContext()
		{
			const string appVirtualDir = "/";
			const string appPhysicalDir = @"c:\\projects\\SubtextSystem\\Subtext.Web\\";
			const string page = "application/default.aspx";
			_host = Environment.MachineName;
			var query = string.Empty;
			TextWriter output = null;

			var workerRequest = new SimulatedHttpRequest(appVirtualDir, appPhysicalDir, page, query, output, _host);
			return new HttpContext(workerRequest);
		}
	}
}