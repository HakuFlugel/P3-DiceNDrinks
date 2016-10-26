/* Search functioner vi skal bruge i koden. (til hjælp)
        private List<Game> tempGameList = new List<Game>();


        public SearchGames() {

        }

        public List<Game> OrderByDate(List<Game> list) {
            tempGameList = list.OrderBy(o => o.publishedYear).ToList();
            return tempGameList;
        }

        public List<Game> OrderByName(List<Game> list) {
            tempGameList = list.OrderBy(o => o.name);
            return tempGameList;
        }

        public List<Game> OrderByDifficulty(List<Game> list) {
            tempGameList = list.OrderBy(o => o.difficulity).ToList();
            return tempGameList;
        }

        public List<Game> OrderByPlayerAmount(List<Game> list) {
            
            return tempGameList;
        }

        public List<Game> OrderByPlayTime(List<Game> list) {
            
            return tempGameList;
        }

        public List<Game> PopularGames(List<Game> list) {
            
            return tempGameList;
        }

        public List<Game> FilterName(List<Game> list, string keyword) {
            list.All((game) => { return game.name.Contains(keyword); });
            return tempGameList;
        }
    }
}
