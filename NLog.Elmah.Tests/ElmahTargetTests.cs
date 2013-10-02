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
using System;
using Elmah;
using NLog.Config;
using NLog.Layouts;
using NUnit.Framework;

namespace NLog.Elmah.Tests
{
    [TestFixture]
    public class given_log_event_with_no_exception_when_logging_info
    {
        private ErrorLog _errorLog;

        [TestFixtureSetUp]
        public void Init()
        {
            _errorLog = new MemoryErrorLog(1);
            var loggingConfiguration = new LoggingConfiguration();
            var target = new ElmahTarget(_errorLog) {Layout = new SimpleLayout("${level}-${message}")};

            loggingConfiguration.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, target));
            loggingConfiguration.AddTarget("Elmah", target);
            LogManager.Configuration = loggingConfiguration;
        }

        [SetUp]
        public void SetUp()
        {
            _errorLog.Clear();
            var logger = LogManager.GetLogger("Test");
            logger.Info("This is an info message.");
        }

        [Test]
        public void should_set_message_to_rendered_message()
        {
            var error = _errorLog.GetFirstError();
            Assert.That(error.Message, Is.EqualTo("Info-This is an info message."));
        }

        [Test]
        public void should_set_log_type()
        {
            var error = _errorLog.GetFirstError();
            Assert.That(error.Type, Is.EqualTo("Info"));
        }

        [Test]
        public void should_not_set_exception()
        {
            var error = _errorLog.GetFirstError();
            Assert.That(error.Exception, Is.Null);
        }

        [Test]
        public void should_not_set_detail()
        {
            var error = _errorLog.GetFirstError();
            Assert.That(error.Detail, Is.Empty);
        }

        [Test]
        public void should_not_set_source()
        {
            var error = _errorLog.GetFirstError();
            Assert.That(error.Source, Is.Empty);
        }

        [Test]
        public void should_set_time_to_now()
        {
            var error = _errorLog.GetFirstError();
            Assert.That(error.Time, Is.EqualTo(DateTime.Now));
        }
    }

    [TestFixture]
    public class given_log_event_with_exception_when_logging_error
    {
        private ErrorLog _errorLog;
        private Exception _exception;

        [TestFixtureSetUp]
        public void Init()
        {
            _errorLog = new MemoryErrorLog(1);
            var loggingConfiguration = new LoggingConfiguration();
            var target = new ElmahTarget(_errorLog) { Layout = new SimpleLayout("${level}-${message}") };

            loggingConfiguration.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, target));
            loggingConfiguration.AddTarget("Elmah", target);
            LogManager.Configuration = loggingConfiguration;
        }

        [SetUp]
        public void SetUp()
        {
            _errorLog.Clear();
            var logger = LogManager.GetLogger("Test");

            _exception = new ArgumentException();
            _exception.Source = "SetUp";
            logger.ErrorException("This is an error message.", _exception);
        }

        [Test]
        public void should_set_message_to_rendered_message()
        {
            var error = _errorLog.GetFirstError();
            Assert.That(error.Message, Is.EqualTo("Error-This is an error message."));
        }

        [Test]
        public void should_set_log_type()
        {
            var error = _errorLog.GetFirstError();
            Assert.That(error.Type, Is.EqualTo("Error"));
        }

        [Test]
        public void should_set_exception()
        {
            var error = _errorLog.GetFirstError();
            Assert.That(error.Exception, Is.EqualTo(_exception));
        }

        [Test]
        public void should_set_detail()
        {
            var error = _errorLog.GetFirstError();
            Assert.That(error.Detail, Is.EqualTo(_exception.ToString()));
        }

        [Test]
        public void should_set_source()
        {
            var error = _errorLog.GetFirstError();
            Assert.That(error.Source, Is.EqualTo(_exception.Source));
        }

        [Test]
        public void should_set_time_to_now()
        {
            var error = _errorLog.GetFirstError();
            Assert.That(error.Time, Is.EqualTo(DateTime.Now));
        }
    }
}
