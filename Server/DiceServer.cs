using Shared;

// In case we use Application['string'] to store loaded data, remember to lock and unlock before modifying it...


namespace Server
{
    public class DiceServer
    {

        private string path;

        public Authentication authentication;

        public ReservationController reservationController;
        public EventsController eventsController;
        public GamesController gamesController;
        public ProductsController productsController;

        public DiceServer(string path)
        {
            this.path = path;

            authentication = new Authentication(path+"data/");

            reservationController = new ReservationController(path+"data/");
            eventsController = new EventsController(path+"data/");
            gamesController = new GamesController(path+"data/");
            productsController = new ProductsController(path+"data/");



            //TODO: make sure it's loaded at the proper time
            //TODO: admin permission
        }

    }
}