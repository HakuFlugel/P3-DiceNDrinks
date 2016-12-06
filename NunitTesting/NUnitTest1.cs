using System;
using NUnit.Framework;

namespace NunitTesting {
    [TestFixture]
    public class NUnitTest1 {
        [Test]
        public void TestMethod1() {
            Assert.True(1 == 1);
        }
    }
}