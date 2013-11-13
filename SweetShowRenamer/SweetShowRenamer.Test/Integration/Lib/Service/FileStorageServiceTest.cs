using System;
using NUnit.Framework;
using SweetShowRenamer.Lib.Service.Abstract;
using SweetShowRenamer.Lib.Service;

namespace SweetShowRenamer.Test.Integration.Lib.Service
{
    public class TestObject
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
    
    [TestFixture]
    public class FileStorageServiceTest
    {
        [Test]
        public void Get_Success()
        {

            var meh = new FileStorageService<TestObject>();

            var testObject = new TestObject
            {
                Title = "This is a test title",
                Description = "This is a test description"
            };
            meh.Set(testObject);
            var result = meh.Get();

            Assert.AreEqual(result.Title, testObject.Title);
            Assert.AreEqual(result.Description, testObject.Description);
        }
    }
}
