# Mandelbrot Azure
A Mandelbrot renderer hosted in Azure. Consists of a command queue and a fractal still image renderer.


### Done

 * A WebJob that listens to commands on an Azure Service Bus Queue
 * Draws a Mandelbrot fractal for the zoom level received in the command
 * Writes the Mandelbrot image to Azure Blob Storage

### TODO

 * A WebJob that takes all the still images and turn them into an animation (short movie).
