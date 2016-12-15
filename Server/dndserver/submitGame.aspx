<%@ Page Language="C#"%>

<%
    Server.DiceServer diceServer = (Server.DiceServer)Application["DiceServer"];

    string adminKey = Request.Form["AdminKey"];
    bool isAdmin = adminKey != null && diceServer.authentication.authenticate(adminKey);
    if (!isAdmin)
    {
        Response.Write("No permission");
        Response.StatusCode = 403;
        return;
    }

    string action = Request.Form["Action"];
    string gameString = Request.Form["Game"] ?? "null";

    Shared.Game game = Newtonsoft.Json.JsonConvert.DeserializeObject<Shared.Game>(gameString);

    if (game == null || action == null)
    {
        Response.Write("No game provided");
        return;
    }

    //TODO: make sure it handles both correct and incorrect input...
    Application.Lock();
    switch (action)
    {
        case "delete":
            if (game.imageName != null)
            {
                try
                {
                    System.IO.File.Delete(diceServer.path + "images/games/" + game.imageName);

                }
                catch (Exception)
                {
                    Response.Write("imgfailed");
                    return;
                }
            }

            diceServer.gamesController.removeGame(game);


            Response.Write("deleted");
            break;
        case "add":
            game.timestamp = DateTime.UtcNow;
            diceServer.gamesController.addGame(game);
            Response.Write("added " + game.id);
            goto doafter;
        case "update":
            try
            {
                game.timestamp = DateTime.UtcNow; //TODO: fix merge
                diceServer.gamesController.updateGame(game);
                Response.Write("updated");
            }
            catch (Exception)
            {
                Response.Write("failed");
            }
            //goto doafter;

        doafter:
            string imgstring = Request.Form["Image"];
            if (imgstring != null)
            {
                try
                {
                    byte[] data = System.Text.Encoding.UTF8.GetBytes(imgstring);
                        //new byte[imgstring.Length];
                    //int i = 0;
                    //foreach (var _char in imgstring)
                    //{
                    //    data[i++] = Convert.ToByte(_char);
                    //}

                    System.Drawing.Image image = Shared.ImageHelper.byteArrayToImage(data);

                    image.Save(diceServer.path + "images/games/" + game.imageName);
                }
                catch (Exception)
                {
                    Response.Write("imgfailed");
                    return;
                }
            }

            break;

        default:
            Response.Write("invalid action");
            break;
    }

    diceServer.setTimestamp("Games", DateTime.UtcNow);

    Application.UnLock();

%>