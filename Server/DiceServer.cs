using System.Linq;
using Newtonsoft.Json;
using Shared;

// In case we use Application['string'] to store loaded data, remember to lock and unlock before modifying it...


namespace Server
{
    public class DiceServer
    {

        private string path;

        //public Authentication authentication;

        public ReservationController reservationController;
        public EventsController eventsController;
        public GamesController gamesController;
        public ProductsController productsController;

        public DiceServer(string path)
        {
            this.path = path;

            //authentication = new Authentication(path);

            reservationController = new ReservationController(path);
            eventsController = new EventsController(path);
            gamesController = new GamesController(path);
            productsController = new ProductsController(path);



            //TODO: make sure it's loaded at the proper time
            //TODO: admin permission
        }


//        public string getProducts()
//        {
//            Tuple<List<ProductCategory>, List<Product>> results = new Tuple<List<ProductCategory>, List<Product>>(categories, products);
//
//            return JsonConvert.SerializeObject(results);
//        }
//
//        private void submitProduct(string json)
//        {
//            //TODO: new category+section maybe
//            Product submittedProduct = JsonConvert.DeserializeObject<Product>(json);
//            int targetindex = products.FindIndex(o => o.id == submittedProduct.id);
//
//            if (targetindex == -1)
//            {
//                products.Add(submittedProduct);
//            }
//            else
//            {
//                products[targetindex] = submittedProduct;
//            }
//
//            jsonSerializer.Serialize(new JsonTextWriter(new StreamWriter("categories.json")), categories);
//            jsonSerializer.Serialize(new JsonTextWriter(new StreamWriter("products.json")), products);
//
//        }
//
//        private void deleteProduct(int id)
//        {
//            products.RemoveAll(o => o.id == id);
//            // TODO: remove empty sections+categories
//            jsonSerializer.Serialize(new JsonTextWriter(new StreamWriter("categories.json")), categories);
//            jsonSerializer.Serialize(new JsonTextWriter(new StreamWriter("products.json")), products);
//        }
//
//
//        private string getGames()
//        {
//            return JsonConvert.SerializeObject(games);
//        }
//
//        private void submitGame(string json)
//        {
//            Game submittedGame = JsonConvert.DeserializeObject<Game>(json);
//            int targetindex = games.FindIndex(o => o.id == submittedGame.id);
//
//            if (targetindex == -1)
//            {
//                games.Add(submittedGame);
//            }
//            else
//            {
//                games[targetindex] = submittedGame;
//            }
//
//            jsonSerializer.Serialize(new JsonTextWriter(new StreamWriter("games.json")), games);
//
//
//        }
//
//        private void deleteGame(int id)
//        {
//            games.RemoveAll(o => o.id == id);
//            // TODO: remove empty genres? maybe?
//            jsonSerializer.Serialize(new JsonTextWriter(new StreamWriter("games.json")), games);
//
//        }
//
//
////TODO: expired events?
//        private string getEvents()
//        {
//            return JsonConvert.SerializeObject(events);
//        }
//
//        private void submitEvent(string json)
//        {
//            Event submittedEvent = JsonConvert.DeserializeObject<Event>(json);
//            int targetindex = events.FindIndex(o => o.id == submittedEvent.id);
//
//            if (targetindex == -1)
//            {
//                events.Add(submittedEvent);
//            }
//            else
//            {
//                events[targetindex] = submittedEvent;
//            }
//
//            jsonSerializer.Serialize(new JsonTextWriter(new StreamWriter("events.json")), events);
//
//
//        }
//
//        private void deleteEvent(int id)
//        {
//            events.RemoveAll(o => o.id == id);
//            jsonSerializer.Serialize(new JsonTextWriter(new StreamWriter("events.json")), games);
//
//        }

        public string getReservations()
        {
            return JsonConvert.SerializeObject(reservationController.reservationsCalendar);
        }

        public string getReservation(int id)
        {
            //Tuple<List<ProductCategory>, List<Product>> results = new Tuple<List<ProductCategory>, List<Product>>(categories, products);

            return JsonConvert.SerializeObject(reservationController.reservationsCalendar.SelectMany(o => o.reservations).First(o => o.id == id));
        }

//        private void submitReservation(string json)
//        {
//            //TODO: has to validate somewhere serverside in case it's from a user
//            //TODO: maybe a seperate one for customer side
//            Reservation submittedReservation = JsonConvert.DeserializeObject<Reservation>(json);
//            int targetindex = reservations.FindIndex(o => o.id == submittedReservation.id);
//
//            if (targetindex == -1)
//            {
//                reservations.Add(submittedReservation);
//            }
//            else
//            {
//                reservations[targetindex] = submittedReservation;
//            }
//
//            jsonSerializer.Serialize(new JsonTextWriter(new StreamWriter("reservations.json")), reservations);
//
//
//        }
//
//        private void deleteReservation(int id)
//        {
//            reservations.RemoveAll(o => o.id == id);
//            // TODO: effect on calendarday
//            // TODO: any other effects
//            jsonSerializer.Serialize(new JsonTextWriter(new StreamWriter("events.json")), games);
//
//        }

    }
}