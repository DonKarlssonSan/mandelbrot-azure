using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.ServiceBus.Messaging;

namespace MandelbrotAzureWebJob
{
    public class Functions
    {
        public static void Mandelbrot(
            [ServiceBusTrigger("jobs")] string msgIn,
            [Blob("images/mandelbrot.png", FileAccess.Write)] Stream output)
        {
            Console.WriteLine("Job received from queue. Starting...");
            var width = 1024;
            var height = 768;

            using (Bitmap b = new Bitmap(width, height))
            {
                using (Graphics g = Graphics.FromImage(b))
                {
                    var xmin = -2.3;
                    var ymin = -1.0;
                    var xmax = 0.8;
                    var ymax = 1.0;
                    double xDelta;
                    double yDelta;
                    var x0 = xmin;
                    xDelta = (xmax - xmin) / width;
                    yDelta = (ymax - ymin) / height;
                    double x;
                    double y;
                    int iteration;
                    int max_iteration = 256;
                    double xtemp;
                    int color;
                    for (var Px = 0; Px < width; Px++)
                    {
                        x0 += xDelta;
                        var y0 = ymin;
                        for (var Py = 0; Py < height; Py++)
                        {
                            y0 += yDelta;
                            x = 0.0;
                            y = 0.0;
                            iteration = 0;
                            while (x * x + y * y < 4 && iteration < max_iteration)
                            {
                                xtemp = x * x - y * y + x0;
                                y = 2 * x * y + y0;
                                x = xtemp;
                                iteration = iteration + 1;
                            }
                            color = 256 - iteration;
                            b.SetPixel(Px, Py, Color.FromArgb(255, color, color, color));
                        }
                    }
                    Console.WriteLine("xmin: " + xmin + " xmax: " + xmax + " ymin: " + ymin + " ymax: " + ymax);
                }
                b.Save(output, ImageFormat.Png);
            }
            Console.WriteLine("Done");
        }
    }
}
