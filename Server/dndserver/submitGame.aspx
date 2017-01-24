<%@ Page Language="C#"%>
<%@ Import Namespace="System.Drawing.Imaging" %>

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

    Application.Lock();
    switch (action)
    {
        case "delete":
            if (!string.IsNullOrEmpty(game.imageName))
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
                game.timestamp = DateTime.UtcNow;
                diceServer.gamesController.updateGame(game);
                Response.Write("updated");
            }
            catch (Exception e)
            {
                Response.Write("failed ");
                Response.Write(e);
            }
            //goto doafter;

        doafter:
            string imgstring = Request.Form["Image"];
            if (!string.IsNullOrEmpty(imgstring))
            {
                try
                {
                    System.Drawing.Image image = Shared.ImageHelper.byteArrayToImage(imgstring);

                    image.Save(diceServer.path + "images/games/" + game.imageName);
                }
                catch (Exception e)
                {
                    Response.Write(" imgfailed " + e);
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