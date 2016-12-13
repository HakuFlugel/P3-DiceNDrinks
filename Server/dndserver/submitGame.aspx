<%@ Page Language="C#"%>

<%
    Server.DiceServer diceServer = (Server.DiceServer)Application["DiceServer"];

    string adminKey = Request.Form["AdminKey"];
    bool isAdmin = adminKey != null && diceServer.authentication.authenticate(adminKey);
        if (isAdmin)
        {
            Response.Write("No permission");
            return;
            //throw new HttpException(404, "Not Found");

            //Response.Clear();
            //Response.StatusCode = 403;
            //Response.End();
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
            diceServer.gamesController.removeGame(game);
            Response.Write("deleted");
            break;
        case "add":
            game.timestamp = DateTime.UtcNow;
            diceServer.gamesController.addGame(game);
            Response.Write("added " + game.id);
            break;
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
            break;
        default:
            Response.Write("invalid action");
            break;
    }

    diceServer.setTimestamp("Game", DateTime.UtcNow);

    Application.UnLock();

%>