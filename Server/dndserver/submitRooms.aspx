<%@ Page Language="C#"%>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="Newtonsoft.Json" %>
<%@ Import Namespace="Shared" %>

<%


    Server.DiceServer diceServer = (Server.DiceServer)Application["DiceServer"];

    string adminKey = Request.Form["AdminKey"];
    bool isAdmin = adminKey != null && diceServer.authentication.authenticate(adminKey);

    if (!isAdmin)
    {
        Response.Write("Permission denied");
        Response.StatusCode = 403;
        return;
    }

    try
    {

        string roomsString = Request.Form["Rooms"] ?? "null";



        List<Shared.Room> rooms = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Shared.Room>>(roomsString);

        if (rooms == null)
        {
            Response.Write("No rooms provided");
            return;
        }

        Application.Lock();

        diceServer.reservationController.submitRooms(rooms);
        //TODO: need to update days maybe?
        Response.Write("success");

        diceServer.setTimestamp("ReservationS", DateTime.UtcNow);


    }
    catch (Exception e)
    {
        Response.Write("failed ");
        //TODO: remove
        Response.Write(e);
    }

    Application.UnLock();

%>