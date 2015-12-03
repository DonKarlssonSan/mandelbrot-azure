using Microsoft.VisualStudio.TestTools.UnitTesting;
using MandelbrotAzureWebJob;
using System.IO;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (var imageFileStream = new FileStream("mandelbrot.png", FileMode.Create))
            {
                Functions.Mandelbrot("dummy", imageFileStream);
            }
        }
    }
}
