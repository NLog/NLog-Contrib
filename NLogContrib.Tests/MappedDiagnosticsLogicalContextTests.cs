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

using System.Threading.Tasks;
using NUnit.Framework;

namespace NLogContrib.Tests
{
    [TestFixture]
    public class MappedDiagnosticsLogicalContextTests
    {
        [SetUp]
        public void SetUp()
        {
            MappedDiagnosticsLogicalContext.Clear();
        }

        [Test]
        public void given_no_item_exists_when_getting_item_should_return_empty_string()
        {
            Assert.That(MappedDiagnosticsLogicalContext.Get("itemThatShouldNotExist"), Is.Empty);
        }

        [Test]
        public void given_item_exists_when_getting_item_should_return_item()
        {
            const string key = "Key";
            const string item = "Item";
            MappedDiagnosticsLogicalContext.Set(key, item);

            Assert.That(MappedDiagnosticsLogicalContext.Get(key), Is.EqualTo(item));
        }

        [Test]
        public void given_item_does_not_exist_when_setting_item_should_contain_item()
        {
            const string key = "Key";
            const string item = "Item";

            MappedDiagnosticsLogicalContext.Set(key, item);

            Assert.That(MappedDiagnosticsLogicalContext.Contains(key), Is.True);
        }

        [Test]
        public void given_item_exists_when_setting_item_should_not_throw()
        {
            const string key = "Key";
            const string item = "Item";
            MappedDiagnosticsLogicalContext.Set(key, item);

            Assert.DoesNotThrow(() => MappedDiagnosticsLogicalContext.Set(key, item));
        }

        [Test]
        public void given_item_exists_when_setting_item_should_update_item()
        {
            const string key = "Key";
            const string item = "Item";
            const string newItem = "NewItem";
            MappedDiagnosticsLogicalContext.Set(key, item);

            MappedDiagnosticsLogicalContext.Set(key, newItem);

            Assert.That(MappedDiagnosticsLogicalContext.Get(key), Is.EqualTo(newItem));
        }

        [Test]
        public void given_item_does_not_exist_when_checking_if_context_contains_should_return_false()
        {
            Assert.That(MappedDiagnosticsLogicalContext.Contains("keyForItemThatDoesNotExist"), Is.False);
        }

        [Test]
        public void given_item_exists_when_checking_if_context_contains_should_return_true()
        {
            const string key = "Key";
            MappedDiagnosticsLogicalContext.Set(key, "Item");

            Assert.That(MappedDiagnosticsLogicalContext.Contains(key), Is.True);
        }

        [Test]
        public void given_item_exists_when_removing_item_should_not_contain_item()
        {
            const string keyForItemThatShouldExist = "Key";
            const string itemThatShouldExist = "Item";
            MappedDiagnosticsLogicalContext.Set(keyForItemThatShouldExist, itemThatShouldExist);

            MappedDiagnosticsLogicalContext.Remove(keyForItemThatShouldExist);

            Assert.That(MappedDiagnosticsLogicalContext.Contains(keyForItemThatShouldExist), Is.False);
        }

        [Test]
        public void given_item_does_not_exist_when_removing_item_should_not_throw()
        {
            const string keyForItemThatShouldExist = "Key";
            Assert.DoesNotThrow(() => MappedDiagnosticsLogicalContext.Remove(keyForItemThatShouldExist));
        }

        [Test]
        public void given_item_does_not_exist_when_clearing_should_not_throw()
        {
            Assert.DoesNotThrow(MappedDiagnosticsLogicalContext.Clear);
        }

        [Test]
        public void given_item_exists_when_clearing_should_not_contain_item()
        {
            const string key = "Key";
            MappedDiagnosticsLogicalContext.Set(key, "Item");
            
            MappedDiagnosticsLogicalContext.Clear();
            
            Assert.That(MappedDiagnosticsLogicalContext.Contains(key), Is.False);
        }

        [Test]
        public void given_multiple_threads_running_asynchronously_when_setting_and_getting_values_should_return_thread_specific_values()
        {
            const string key = "Key";
            const string valueForLogicalThread1 = "ValueForTask1";
            const string valueForLogicalThread2 = "ValueForTask2";
            const string valueForLogicalThread3 = "ValueForTask3";

            var task1 = Task.Run(() =>
                {
                    MappedDiagnosticsLogicalContext.Set(key, valueForLogicalThread1);
                    return MappedDiagnosticsLogicalContext.Get(key);
                });

            var task2 = Task.Run(() =>
                {
                    MappedDiagnosticsLogicalContext.Set(key, valueForLogicalThread2);
                    return MappedDiagnosticsLogicalContext.Get(key);
                });

            var task3 = Task.Run(() =>
            {
                MappedDiagnosticsLogicalContext.Set(key, valueForLogicalThread3);
                return MappedDiagnosticsLogicalContext.Get(key);
            });

            Task.WaitAll();

            Assert.That(task1.Result, Is.EqualTo(valueForLogicalThread1));
            Assert.That(task2.Result, Is.EqualTo(valueForLogicalThread2));
            Assert.That(task3.Result, Is.EqualTo(valueForLogicalThread3));
        }
    }
}
