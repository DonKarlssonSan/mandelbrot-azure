using Microsoft.VisualStudio.TestTools.UnitTesting;
using MandelbrotAzureWebJob;
using System.IO;
using Microsoft.ServiceBus.Messaging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using MandelbrotAzure.Common;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void SingleImage()
        {
            using (var imageFileStream = new FileStream("mandelbrot.png", FileMode.Create))
            {
                var zoom = new Zoom()
                {
                    xmin = -2.3,
                    ymin = -1.0,
                    xmax = 0.8,
                    ymax = 1.0
                };
                var message = new BrokeredMessage(zoom);
                Functions.Mandelbrot(message, imageFileStream);
            }
        }

        [TestMethod]
        public void TwoImages()
        {
            var zooms = new List<Zoom>()
            {
                new Zoom()
                {
                    xmin = -2.3,
                    ymin = -1.0,
                    xmax = 0.8,
                    ymax = 1.0
                },
                new Zoom()
                {
                    xmin = -2.0,
                    ymin = -0.8,
                    xmax = 0.5,
                    ymax = 0.8
                }
            };
            long counter = 0;
            foreach (var zoom in zooms)
            {
                using (var imageFileStream = new FileStream(++counter + ".png", FileMode.Create))
                {
                    var message = new BrokeredMessage(zoom);
                    Functions.Mandelbrot(message, imageFileStream);
                }
            }
        }

        [TestMethod]
        public void SeveralImages()
        {
            var xmin = -2.3;
            var ymin = -1.0;
            var xmax = 0.8;
            var ymax = 1.0;

            var x = -0.789374599271466936740382412558;
            var y = 0.163089252677526719026415054868;

            long iterations = 15;
            var xdelta = (x - (xmin-xmax)/2) / iterations;
            var ydelta = (y - (ymin-ymax)/2) / iterations;

            //-0.789374599271466936740382412558 +0.163089252677526719026415054868i
            for (long i = 0; i < iterations; i++)
            {
                using (var imageFileStream = new FileStream(i + ".png", FileMode.Create))
                {
                    var zoom = new Zoom()
                    {
                        xmin = xmin,
                        ymin = ymin,
                        xmax = xmax,
                        ymax = ymax
                    };
                    var message = new BrokeredMessage(zoom);
                    Functions.Mandelbrot(message, imageFileStream);
                }

                xmin += xdelta;
                xmax -= xdelta;
                ymin += ydelta;
                ymax -= ydelta;

            }
        }

        [TestMethod]
        public void SerializeZoom()
        {
            var zoom = new Zoom()
            {
                xmin = 1,
                xmax = 2,
                ymin = 3,
                ymax = 4
            };
            var serializer = new DataContractSerializer(typeof(Zoom));
            MemoryStream memoryStream = new MemoryStream();
            serializer.WriteObject(memoryStream, zoom);
            Console.WriteLine(Encoding.UTF8.GetString(memoryStream.GetBuffer()));

            memoryStream.Seek(0, SeekOrigin.Begin);
            var reader = XmlDictionaryReader.CreateTextReader(memoryStream, new XmlDictionaryReaderQuotas());

            // Deserialize the data and read it from the instance.
            var zoom2 = (Zoom)serializer.ReadObject(reader, true);

            Assert.AreEqual(zoom.xmin, zoom2.xmin);
        }
    }
}
