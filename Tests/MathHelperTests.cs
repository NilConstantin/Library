using Library;
using NUnit.Framework;

namespace Library.Tests
{
    public class MathHelperTests
    {
        [Test]
        public void MathHelperApproximately()
        {
            Assert.IsTrue(MathHelper.Approximately(0.0f, 0.0f));

            Assert.IsTrue(MathHelper.Approximately(0.00001f, 0.0f));
            Assert.IsTrue(MathHelper.Approximately(0.00001f, 0.00002f));

            Assert.IsTrue(MathHelper.Approximately(-0.00001f, 0.0f));
            Assert.IsTrue(MathHelper.Approximately(-0.00001f, 0.00002f));

            Assert.IsTrue(MathHelper.Approximately(0.00001f, -0.0f));
            Assert.IsTrue(MathHelper.Approximately(0.00001f, -0.00002f));

            Assert.IsFalse(MathHelper.Approximately(0.001f, 0.0f));
            Assert.IsFalse(MathHelper.Approximately(-0.001f, 0.0f));
            Assert.IsFalse(MathHelper.Approximately(-0.001f, 0.001f));
            Assert.IsFalse(MathHelper.Approximately(0.001f, -0.001f));
        }
    }
}
