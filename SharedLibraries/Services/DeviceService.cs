using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using SharedLibraries.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MAD = Microsoft.Azure.Devices;

namespace SharedLibraries.Services
{
    public static class DeviceService
    {

        private static readonly string _connweat = "http://api.weatherstack.com/current?access_key=e699857f79960f12fc50911e6203d374&query=Koping";
        private static HttpClient _client = new HttpClient();
      
        public static async Task SendMessageAsync(DeviceClient deviceClient)
        {
            while (true)
            {
                try
                {
                    var respone = await _client.GetAsync(_connweat);
                    if (respone.IsSuccessStatusCode)
                    {
                        WeatherModel weather = JsonConvert.DeserializeObject<WeatherModel>(await respone.Content.ReadAsStringAsync());

                        var data = new Current
                        {
                            temperature = weather.current.temperature,
                            humidity = weather.current.humidity
                        };
                  
                        var json = JsonConvert.SerializeObject(data);

                        var payload = new Message(Encoding.UTF8.GetBytes(json));
                        await deviceClient.SendEventAsync(payload);

                        Console.WriteLine($"Sent Message: {json}");

                        await Task.Delay(10 * 1000);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }
            }
        }

        public static async Task ReceiveMessageAsync(DeviceClient deviceClient)
        {
            while (true)
            {
                var payload = await deviceClient.ReceiveAsync();
                if (payload == null)
                    continue;
                Console.WriteLine($"Received the following message: {Encoding.UTF8.GetString(payload.GetBytes())}");
                await deviceClient.CompleteAsync(payload);
            }
        }

        public static async Task SendMessageToDeviceAsync(MAD.ServiceClient serviceClient, string targetDeviceId, string message)
        {
            var payload = new MAD.Message(Encoding.UTF8.GetBytes(message));
            await serviceClient.SendAsync(targetDeviceId, payload);

        }

    }
}
