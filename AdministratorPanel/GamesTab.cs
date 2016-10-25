﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AdministratorPanel {
    public class GamesTab : TabPage {
        private Form form;
        private List<Games> games;




        public GamesTab() {
            Text = "Games";

            // For testing purpose only
            games = new List<Games>();
            games.Add(new Games("AHZ2xB", "Secret Hitler", "Horror", "A game about gaming", 2014, 5, 10, 30, 60, "TosetPictureInFuture"));
            games.Add(new Games("J8AkkS", "Dominion", "Horror", "A game about gaming", 2014, 5, 10, 30, 60, "TosetPictureInFuture"));
            games.Add(new Games("5ExgGS", "Small Worlds", "Horror", "A game about gaming", 2014, 5, 10, 30, 60, "TosetPictureInFuture"));
            games.Add(new Games("TYE3sj", "Enter The Gundion", "Horror", "A game about gaming", 2014, 5, 10, 30, 60, "TosetPictureInFuture"));


            displayGames();

        }



        private void displayGames() {

            string listOfGames = "";
            Font font = new Font("Microsoft Sans Serif", 12);
            foreach (var item in games)
                listOfGames += item.ToString() + Environment.NewLine;

            TextBox allGames = new TextBox();
            allGames.Multiline = true;
            allGames.ScrollBars = ScrollBars.Both;
            allGames.Dock = DockStyle.Fill;
            allGames.Font = font;
            allGames.Text = listOfGames;
            allGames.ReadOnly = true;

            Controls.Add(allGames);

        }
    }
    class Games {
        public string id;

        public string name;
        public string genre;
        public string description;
        public int publishedYear;

        public int minPlayers, maxPlayers;
        public int minPlayTime, maxPlayTime;

        public string thumbnail, image;

        public Games(string id, string name, string genre, string description, int publishedYear, int minPlayers, int maxPlayers, int minPlayTime, int maxPlayTime, string thumbnail) {
            this.id = id;
            this.name = name;
            this.genre = genre;
            this.description = description;
            this.publishedYear = publishedYear;
            this.minPlayers = minPlayers;
            this.maxPlayers = maxPlayers;
            this.minPlayTime = minPlayTime;
            this.maxPlayTime = maxPlayTime;
            this.thumbnail = thumbnail;
        }
        public override string ToString() {

            return id + " " + name;
        }
    }
}
