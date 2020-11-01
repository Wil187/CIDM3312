using System;
using System.IO;
using System.Linq;

using VatsimLibrary.VatsimClient;
using VatsimLibrary.VatsimDb;

namespace hw
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine($"{VatsimDbHepler.DATA_DIR}");

            using(var db = new VatsimDbContext())
            {

                var _pilots = db.Pilots.Select(p => p).ToList();
                Console.WriteLine($"The number of pilots records is: {_pilots.Count} ");

                //1238470                
                //UAL2865
                //20201013162413
                var _pilot = db.Pilots.Find("1238470", "UAL2865", "20201013162413");                
                if(_pilot != null){
                    Console.WriteLine($"Pilot found: {_pilot.Realname}");
                } else {
                    Console.WriteLine("Pilot not found");
                }                

                //1385451
                //N130JM
                //20201021233811
                _pilot = db.Pilots.Find("1385451", "N130JM", "20201021233811");
                if(_pilot != null){
                    Console.WriteLine($"Pilot found: {_pilot.Realname}");
                } else {
                    Console.WriteLine("Pilot not found");
                }

                //1484591
                //PAL922
                //20201028132105
                _pilot = db.Pilots.Find("1484591", "PAL922", "20201028132105");
                if(_pilot != null){
                    Console.WriteLine($"Pilot found: {_pilot.Realname}");
                } else {
                    Console.WriteLine("Pilot not found");
                }

                var _longest = db.Pilots.Min(x => x.TimeLogon);
                System.Console.WriteLine("\n------\nPilot/Pilots logged in the longest\n------");
                foreach (var v in db.Pilots)
                {
                    if (v.TimeLogon == _longest)
                    {
                        System.Console.WriteLine($"{v} - {v.TimeLogon}");
                    }
                }

                var _longestControlles = db.Controllers.Min(x => x.TimeLogon);
                System.Console.WriteLine("\n------\nController/Controller logged in the longest\n------");
                foreach (var v in db.Controllers)
                {
                    if (v.TimeLogon == _longestControlles)
                    {
                        System.Console.WriteLine($"{v} - {v.Realname} - {v.TimeLogon}");
                    }
                }

                var _AirportDeparts = db.Flights.Select(x => x.PlannedDepairport).ToList();
                
                var _mostDeprated = 
                    _AirportDeparts.GroupBy(x => x)
                    .OrderByDescending(x => x.Count()).ThenBy(x => x.Key)
                    .Select(x => x.Key)
                    .FirstOrDefault();

                System.Console.WriteLine($"\n------\nMost Departed Airport - {_mostDeprated}\n------");


                var _AirportArrivals = db.Flights.Select(x => x.PlannedDestairport).ToList();
                var _mostArrivals = 
                    _AirportArrivals.GroupBy(x => x)
                    .OrderByDescending(x => x.Count()).ThenBy(x => x.Key)
                    .Select(x => x.Key)
                    .FirstOrDefault();

                System.Console.WriteLine($"\n------\nMost Arrived at Airport - {_mostArrivals}\n------");


                var _highestAltitude = db.Flights.Max(x => x.PlannedAltitude);

                System.Console.WriteLine("\n------\nPilot/Pilots with the highest planned altitude\n------");
                foreach (var v in db.Flights)
                {
                    if (v.PlannedAltitude == _highestAltitude)
                    {
                        System.Console.WriteLine($"{v.Realname} has a planned altitude of {v.PlannedAltitude} in a {v.PlannedAircraft}");
                    }
                }


                var _slowSpeed = db.Positions.Select(x => x.Groundspeed).ToList();
                var _slowest = 
                    _slowSpeed
                    .Where(x => x != "0")
                    .Min();
                System.Console.WriteLine($"\n------\nPilot/Pilots with the slowest speed\n------");
                foreach (var v in db.Positions)
                {
                    if (v.Groundspeed == _slowest)
                    {
                        System.Console.WriteLine($"{v.Realname} is flying the slowest with a speed of {_slowest}");
                    }
                }


                var _planes = db.Flights.Select(x => x.PlannedAircraft).ToList();
                var _mostUsedPlane = 
                    _planes.GroupBy(x => x)
                    .OrderByDescending(x => x.Count()).ThenBy(x => x.Key)
                    .Select(x => x.Key)
                    .FirstOrDefault();
                System.Console.WriteLine($"\n------\nMost used plane - {_mostUsedPlane}\n------");



                var _fastSpeed = Convert.ToInt32(db.Positions.Max(x => x.Groundspeed));
                System.Console.WriteLine($"\n------\nPilot/Pilots with the fastest speed\n------");
                foreach (var v in db.Positions)
                {
                    if (Convert.ToInt32(v.Groundspeed) == _fastSpeed)
                    {
                        System.Console.WriteLine($"{v.Realname} is flying the fastest with a speed of {_fastSpeed}");
                    }
                }


                var _headings = db.Positions.Select(x => x.Heading).ToList();
                int _northernHeading = 
                    (from record in _headings
                    where Convert.ToInt32(record) >= 90 && Convert.ToInt32(record) <= 270
                    select record)
                    .Count();
                System.Console.WriteLine($"\n------\nNumber of pilots flying North - {_northernHeading}\n------");


                var _remarks = db.Flights.Select(x => x.PlannedRemarks).ToList();
                var _longestRemark = 
                    _remarks
                    .Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur);
                foreach (var v in db.Flights)
                {
                    if (v.PlannedRemarks == _longestRemark)
                    {
                        System.Console.WriteLine($"\n------\nPilot {v.Realname} has the longest remark section\n------");
                    }
                }
                

    
            }            
        }
    } 
}
