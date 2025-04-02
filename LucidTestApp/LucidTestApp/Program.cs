// See https://aka.ms/new-console-template for more information
using GigeVision.Core.Enums;
using GigeVision.Core.Services;
using ImageMagick;
using SkiaSharp;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

Console.WriteLine("Hello, World!");


var camera = new Camera();

Console.WriteLine($"Finding cameras");
GigeVision.Core.NetworkService.AllowAppThroughFirewall();
var listOfDevices = await camera.Gvcp.GetAllGigeDevicesInNetworkAsnyc();

foreach (var cam in listOfDevices)
{
    Console.WriteLine($"CAM: {cam.IP} {cam.MAC} {cam.SerialNumber} {cam.ManufacturerName}");
}


var cam1 = new Camera(new Gvcp("10.10.0.130"));
await cam1.SyncParameters();
await cam1.SetResolutionAsync(1772, 1010);
//if (!await cam1.SetCameraParameter("PixelFormat", (long)PixelFormat.RGB8Packed))
//{
//Console.WriteLine("Failed to set pixel format");
//}
//await cam1.Gvcp.SaveXmlFileFromCamera(@"c:\temp\");
int count = 0;
cam1.Payload = 5000;
cam1.FrameReady += FrameReady;
cam1.Updates += Updates;
//Console.WriteLine($"Pixel Format: {cam1.PixelFormat}");
//await Task.Delay(10000);
if (!await cam1.StartStreamAsync("10.10.0.1"))
{
    Console.WriteLine("Start stream failed");
}
Console.WriteLine("Streaming started!");
await Task.Delay(50000);

await cam1.StopStream();

void FrameReady(object sender, byte[] f)
{
    //using var image = new MagickImage(f, new MagickReadSettings() { Width = 500, Height = 500, Format = MagickFormat.Bayer });

    //++count;
    //image.TransformColorSpace(ColorProfile.ColorMatchRGB);
    //image.Write($@"C:\temp\a{count}.jpg", MagickFormat.Jpeg);
    /*
    //GCHandle pinnedArray = GCHandle.Alloc(f, GCHandleType.Pinned);
    //IntPtr pointer = pinnedArray.AddrOfPinnedObject();
    // Do your stuff...
    SKBitmap bitmap = new SKBitmap(500, 500, SKColorType.Rgba8888, SKAlphaType.Opaque);
    var pixels = bitmap.GetPixels();
    unsafe
    {
        var unsafePtr = (byte*)pixels.ToPointer();
        for (int i = 0, j = 0; i < f.Length - 3 && j < bitmap.ByteCount - 4; i = i + 3, j = j + 4)
        {
            unsafePtr[j] = f[i];
            unsafePtr[j + 1] = f[i + 1];
            unsafePtr[j + 2] = f[i + 2];
            unsafePtr[j + 3] = 255;
        }
    }
        //bitmap.InstallPixels(new SKImageInfo(500, 500, SKColorType.Rgb888x, SKAlphaType.Opaque), pointer);

    var bytes = bitmap.Encode(SKEncodedImageFormat.Jpeg, 90);
    
    ++count;
    File.WriteAllBytes($@"C:\temp\a{count}.jpg", bytes.ToArray());

    //pinnedArray.Free();*/
    File.WriteAllBytes($@"C:\temp\a{count}.raw", f);
    Console.WriteLine("Got frame!");

}

void Updates(object sender, String m)
{
    Console.WriteLine($"UPDATE: {m}");
}