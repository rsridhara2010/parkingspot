using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebApplication1.Models;
using System.Device.Location;
namespace WebApplication1.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]

        public void TestGeoCord()
        {
            GenerateParkingSpots(37.5,-122.4,5);
            //Console.WriteLine(result); ;
        }

        public static GeoCoordinate GenerateParkingSpots(double lat, double lon, double rad)
        {
            Random rnd = new Random();
            double x = rnd.NextDouble() * (-1 * rad) + lat - rad;
            double maxY = Math.Floor(Math.Sqrt(rad * rad - x * x));

            double y = rnd.NextDouble() * (-1 * rad) + lon - rad;
            y += lon;
            x += lat;

            GeoCoordinate geo = new GeoCoordinate(x, y);
            if (geo.IsUnknown)
                return GenerateParkingSpots(lat, lon, rad);

            return geo;
        }


        public static string local_host_address = "http://localhost:61842/";
        [TestMethod]
        public void TestGetMethod()
        {
            var result= Get(local_host_address+"api/v1/parkingspots/100/100/5/");
            foreach(ParkingSpot ps in result)
            {
                Console.WriteLine(ps.Id + "," + ps.lat + "," + ps.lon + "," + ps.reserved);
            }
            //Console.WriteLine(result); ;
        }

        [TestMethod]
        public void TestPutMethod()
        {
            ParkingSpot ps = new ParkingSpot();
            ps.reserved = true;
            var result = Put<ParkingSpot>(local_host_address + "api/v1/parkingspots/1/",ps);
            
        }
        public string Put<T>(string path, T data)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0, 0, 5);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var json = JsonConvert.SerializeObject(data);

                var response_message = client.PutAsync(local_host_address + path, new StringContent(json, Encoding.UTF8, "application/json"));

                if (response_message.Result != null)
                {
                    if (!response_message.Result.IsSuccessStatusCode)
                        throw new Exception("request failed");
                }
               
            }
            return "200";
        }
        public static List<ParkingSpot> Get(string path)
        {
            List<ParkingSpot> root;
            using (var client = new HttpClient())
            {
                // client.Timeout = new TimeSpan(0, 0, 5);
                client.BaseAddress = new Uri(local_host_address);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
                client.DefaultRequestHeaders.Add("User-Agent", "Ridecell Project Reporter");

                var msg = client.GetStringAsync(path);
                var result1 = msg.Result;
                //var response_message =  client.GetAsync(local_host_address + path );

                //var response = await response_message.Content.ReadAsStringAsync();
                root = JsonConvert.DeserializeObject<List<ParkingSpot>>(result1, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            }
            return root;
        }
    }
}
