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
    public class given_target_with_loglevelastype_not_set_and_log_event_with_no_exception_when_logging_info : given_target_with_loglevelastype_not_set
    {
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
        public void should_set_detail_to_rendered_message()
        {
            var error = _errorLog.GetFirstError();
            Assert.That(error.Detail, Is.EqualTo("Info-This is an info message."));
        }

        [Test]
        public void should_set_log_type_to_empty_string()
        {
            var error = _errorLog.GetFirstError();
            Assert.That(error.Type, Is.Empty);
        }

        [Test]
        public void should_not_set_exception()
        {
            var error = _errorLog.GetFirstError();
            Assert.That(error.Exception, Is.Null);
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
            Assert.That(error.Time, Is.EqualTo(_now));
        }

        [Test]
        public void should_set_host_name_to_machine_name()
        {
            var error = _errorLog.GetFirstError();
            Assert.That(error.HostName, Is.EqualTo(Environment.MachineName));
        }
    }

    [TestFixture]
    public class given_target_with_loglevelastype_not_set_log_event_with_exception_when_logging_error : given_target_with_loglevelastype_not_set
    {
        private Exception _exception;
        
        [SetUp]
        public void SetUp()
        {
            _errorLog.Clear();
            var logger = LogManager.GetLogger("Test");

            _exception = new ArgumentException();
            _exception.Source = "SetUp";

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
            var error = _errorLog.GetFirstError();
            Assert.That(error.Message, Is.EqualTo("Error-This is an error message."));
        }

        [Test]
        public void should_set_log_type_to_full_name_of_exception_type()
        {
            var error = _errorLog.GetFirstError();
            Assert.That(error.Type, Is.EqualTo("System.ArgumentException"));
        }

        [Test]
        public void should_set_exception()
        {
            var error = _errorLog.GetFirstError();
            Assert.That(error.Exception, Is.EqualTo(_exception));
        }

        [Test]
        public void should_set_detail_to_stack_trace()
        {
            var error = _errorLog.GetFirstError();
            Assert.That(error.Detail, Is.EqualTo(_exception.StackTrace));
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
            Assert.That(error.Time, Is.EqualTo(_now));
        }

        [Test]
        public void should_set_host_name_to_machine_name()
        {
            var error = _errorLog.GetFirstError();
            Assert.That(error.HostName, Is.EqualTo(Environment.MachineName));
        }
    }

    public abstract class given_target_with_loglevelastype_not_set
    {
        protected ErrorLog _errorLog;
        protected readonly DateTime _now = new DateTime(2013, 10, 8, 19, 5, 0);

        [TestFixtureSetUp]
        public void Init()
        {
            _errorLog = new MemoryErrorLog(1);
            var loggingConfiguration = new LoggingConfiguration();
            var target = new ElmahTarget(_errorLog)
            {
                Layout = new SimpleLayout("${level}-${message}"),
                GetCurrentDateTime = () => _now
            };

            loggingConfiguration.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, target));
            loggingConfiguration.AddTarget("Elmah", target);
            LogManager.Configuration = loggingConfiguration;
        }
    }

    [TestFixture]
    public class given_target_with_loglevelastype_turned_on_and_log_event_and_no_exception_when_logging_info : given_target_with_loglevelastype_turned_on
    {
        [SetUp]
        public void SetUp()
        {
            _errorLog.Clear();
            var logger = LogManager.GetLogger("Test");
            logger.Info("This is an info message.");
        }


        [Test]
        public void should_set_log_type_to_info()
        {
            var error = _errorLog.GetFirstError();
            Assert.That(error.Type, Is.EqualTo("Info"));
        }
    }

    [TestFixture]
    public class given_target_with_loglevelastype_turned_on_and_log_event_with_exception_when_logging_error : given_target_with_loglevelastype_turned_on
    {
        private Exception _exception;
        
        [SetUp]
        public void SetUp()
        {
            _errorLog.Clear();
            var logger = LogManager.GetLogger("Test");

            _exception = new ArgumentException();
            _exception.Source = "SetUp";

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
            var error = _errorLog.GetFirstError();
            Assert.That(error.Message, Is.EqualTo("Error-This is an error message."));
        }

        [Test]
        public void should_set_log_type_to_full_name_of_exception_type()
        {
            var error = _errorLog.GetFirstError();
            Assert.That(error.Type, Is.EqualTo("System.ArgumentException"));
        }

        [Test]
        public void should_set_exception()
        {
            var error = _errorLog.GetFirstError();
            Assert.That(error.Exception, Is.EqualTo(_exception));
        }

        [Test]
        public void should_set_detail_to_stack_trace()
        {
            var error = _errorLog.GetFirstError();
            Assert.That(error.Detail, Is.EqualTo(_exception.StackTrace));
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
            Assert.That(error.Time, Is.EqualTo(_now));
        }

        [Test]
        public void should_set_host_name_to_machine_name()
        {
            var error = _errorLog.GetFirstError();
            Assert.That(error.HostName, Is.EqualTo(Environment.MachineName));
        }
    }

    public abstract class given_target_with_loglevelastype_turned_on
    {
        protected ErrorLog _errorLog;
        protected readonly DateTime _now = new DateTime(2013, 10, 8, 19, 5, 0);

        [TestFixtureSetUp]
        public void Init()
        {
            _errorLog = new MemoryErrorLog(1);
            var loggingConfiguration = new LoggingConfiguration();
            var target = new ElmahTarget(_errorLog)
            {
                LogLevelAsType = true,
                Layout = new SimpleLayout("${level}-${message}"),
                GetCurrentDateTime = () => _now
            };

            loggingConfiguration.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, target));
            loggingConfiguration.AddTarget("Elmah", target);
            LogManager.Configuration = loggingConfiguration;
        }
    }
}
