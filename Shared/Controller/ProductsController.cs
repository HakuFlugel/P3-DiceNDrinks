using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Shared
{
    public class ProductsController
    {


//
//        public override void save()
//        {
//            Directory.CreateDirectory("data");
//
//            using (StreamWriter streamWriter = new StreamWriter("data/reservationsCalendar.json"))
//            using (JsonTextWriter jsonTextWriter = new JsonTextWriter(streamWriter))
//            {
//                jsonSerializer.Serialize(jsonTextWriter, reservationsCalendar);
//            }
//            using (StreamWriter streamWriter = new StreamWriter("data/rooms.json"))
//            using (JsonTextWriter jsonTextWriter = new JsonTextWriter(streamWriter))
//            {
//                jsonSerializer.Serialize(jsonTextWriter, rooms);
//            }
//        }
//
//
//
//        public override void load()
//        {
//            //TODO: create a function for these(consider Entity System thing)
//            Directory.CreateDirectory("data");
//            try
//            {
//                using (StreamReader streamReader = new StreamReader("data/reservationsCalendar.json"))
//                using (JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
//                {
//                    reservationsCalendar = jsonSerializer.Deserialize<List<CalendarDay>>(jsonTextReader);
//
//                }
//                using (StreamReader streamReader = new StreamReader("data/rooms.json"))
//                using (JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
//                {
//                    rooms = jsonSerializer.Deserialize<List<Room>>(jsonTextReader);
//
//                }
//            }
//            catch (FileNotFoundException)
//            {
//                Console.WriteLine("reservationsCalendar.json or rooms.json not found"); // TODO: put this stuff inside some function
//            }
//
//            if (reservationsCalendar == null)
//            {
//                Console.WriteLine("reservationsCalendar was null after loading... setting it to new list");
//                reservationsCalendar = new List<CalendarDay>();
//            }
//
//            if (rooms == null)
//            {
//                Console.WriteLine("rooms was null after loading... setting it to new list");
//                rooms = new List<Room>();
//            }
//
//        }
    }
}