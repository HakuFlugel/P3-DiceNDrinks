<%@ Page Language="C#"%>
<%@ Import Namespace="System.Collections.Generic" %>

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
        string dayString = Request.Form["Date"] ?? "null";


        List<Shared.Room> rooms = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Shared.Room>>(roomsString);
        DateTime day = Newtonsoft.Json.JsonConvert.DeserializeObject<DateTime>(dayString);

        if (rooms == null)
        {
            Response.Write("No rooms provided");
            return;
        }

        Application.Lock();

        try
        {
            diceServer.reservationController.submitReservedRooms(rooms, day.Date);

            diceServer.setTimestamp("Reservations", DateTime.UtcNow);


            Response.Write("success");
        }
        catch (Exception e)
        {

            Response.Write("failed " + e);
        }




    }
    catch (Exception e)
    {
        Response.Write("failed ");
        Response.Write(e);
    }

    Application.UnLock();

%>