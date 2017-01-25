<%@ Page Language="C#"%>
<%@ Import Namespace="System.Collections.Generic" %>
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

        string roomsString = Request.Form["AutoAccept"] ?? "null";



        ReservationController.AutoAcceptSettings settings = Newtonsoft.Json.JsonConvert.DeserializeObject<ReservationController.AutoAcceptSettings>(roomsString);

        if (settings == null)
        {
            Response.Write("No settings provided");
            return;
        }

        Application.Lock();

        diceServer.reservationController.autoAcceptSettings = settings;
        Response.Write("success");

        diceServer.setTimestamp("Reservations", DateTime.UtcNow);


    }
    catch (Exception e)
    {
        Response.Write("failed ");
        Response.Write(e);
    }

    Application.UnLock();

%>