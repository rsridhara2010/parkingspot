using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;
using System.Runtime;
using System.Device.Location;
namespace WebApplication1.Controllers
{
    public class ParkingSpotsController : ApiController
    {
        // GET: api/ParkingSpot
        [Route("api/v1/parkingspots/{lat}/{lon}/{rad}/")]
        [HttpGet]
        public IEnumerable<ParkingSpot> Get(double lat, double lon, double rad)
        {
            List<ParkingSpot> list = new List<ParkingSpot>();

            for (int i = WebApiConfig.IdMax; i < WebApiConfig.IdMax + 5; i++)
            {

                ParkingSpot ps = new ParkingSpot();
                ps.Id = i+string.Empty;
                GeoCoordinate geo = GenerateParkingSpots(lat, lon, rad);
                ps.lat = geo.Latitude;
                ps.lon = geo.Longitude;
                ps.reserved = false;
                list.Add(ps);
                WebApiConfig.Favorites.Add(ps.Id, ps);
            }
            WebApiConfig.IdMax += 5;
            return list;
        }

       

        [Route("api/v1/parkingspots/{id}/")]
        [HttpPut]
        // PUT: api/ParkingSpot/5
        public IEnumerable<ParkingSpot> Put(int id, ParkingSpot value)
        {
            //return id+value.reserved.ToString();
            ParkingSpot ps = new ParkingSpot();
            WebApiConfig.Favorites.TryGetValue(id + string.Empty,out ps);
            if (ps != null)
            {
                ps.reserved = true;
                //WebApiConfig.Favorites.Add(id + string.Empty, ps);
            }
            List<ParkingSpot> list = new List<ParkingSpot>();
            list.Add(ps);
            return list;
        }
        [HttpGet]
        public IEnumerable<ParkingSpot> Get()
        {
            //
            List<ParkingSpot> psList = new List<ParkingSpot>();
            foreach(ParkingSpot ps in WebApiConfig.Favorites.Values)
            {
                psList.Add(ps);
            }
            return psList;
        }

        public void TestGeoCord()
        {
            GenerateParkingSpots(37.5, -122.4, 5);
            //Console.WriteLine(result); ;
        }

        public static GeoCoordinate GenerateParkingSpots(double lat, double lon, double rad)
        {
            Random rnd = new Random();
            double x = rnd.NextDouble() * (2 * rad) + lat - rad;
            double maxY = Math.Floor(Math.Sqrt(rad * rad - x * x));

            double y = rnd.NextDouble() * (2 * rad) + lon - rad;

            GeoCoordinate geo = new GeoCoordinate(x, y);
            if (geo.IsUnknown)
                return GenerateParkingSpots(lat, lon, rad);

            return geo;
        }


        
    }

    /*
     * 
     * /*
         * earth_radius = 3960.0
degrees_to_radians = math.pi/180.0
radians_to_degrees = 180.0/math.pi

def change_in_latitude(miles):
    "Given a distance north, return the change in latitude."
    return (miles/earth_radius)*radians_to_degrees

def change_in_longitude(latitude, miles):
    "Given a latitude and a distance west, return the change in longitude."
    # Find the radius of a circle around the earth at given latitude.
    r = earth_radius*math.cos(latitude*degrees_to_radians)
    return (miles/r)*radians_to_degrees
         * /
     */
}
