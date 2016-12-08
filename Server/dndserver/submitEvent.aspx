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
    string eventString = Request.Form["Event"] ?? "null";

    Shared.Event @event = Newtonsoft.Json.JsonConvert.DeserializeObject<Shared.Event>(eventString);

    if (@event == null || action == null)
    {
        Response.Write("No event provided");
        return;
    }

    //TODO: make sure it handles both correct and incorrect input...
    Application.Lock();
    switch (action)
    {
        case "delete":
            diceServer.eventsController.removeEvent(@event);
            Response.Write("deleted");
            break;
        case "add":
            @event.timestamp = DateTime.UtcNow;
            diceServer.eventsController.addEvent(@event);
            Response.Write("added " + @event.id);
            break;
        case "update":
            try
            {
                @event.timestamp = DateTime.UtcNow; //TODO: fix merge
                diceServer.eventsController.updateEvent(@event);
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
    Application.UnLock();

%>