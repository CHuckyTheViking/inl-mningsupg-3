using Microsoft.Azure.Devices.Client;
using SharedLibraries.Services;
using System;

namespace IotDevice
{
    class Program
    {
        private static readonly string _connect = "HostName=WIN20-iothub.azure-devices.net;DeviceId=IotDevice;SharedAccessKey=3dgzEhGS+Ba7bBZhCpiFuGj8Wn2yE2yjoVBM63LT+XI=";
        private static readonly DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(_connect, TransportType.Mqtt);



        static void Main(string[] args)
        {
            DeviceService.SendMessageAsync(deviceClient).GetAwaiter();
            DeviceService.ReceiveMessageAsync(deviceClient).GetAwaiter();
            Console.ReadKey();
        }
    }
}
