﻿using System.Linq;
using Newtonsoft.Json;
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

            authentication = new Authentication(path);

            reservationController = new ReservationController(path);
            eventsController = new EventsController(path);
            gamesController = new GamesController(path);
            productsController = new ProductsController(path);



            //TODO: make sure it's loaded at the proper time
            //TODO: admin permission
        }

    }
}