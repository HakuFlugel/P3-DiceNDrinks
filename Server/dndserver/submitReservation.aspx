<%@ Page Language="C#"%>

<%
    Server.DiceServer diceServer = (Server.DiceServer)Application["DiceServer"];

    //    if (!diceServer.authentication.authenticate(Request.Form["AdminKey"]))
    //    {
    //        //throw new HttpException(404, "Not Found");
    //        Response.Clear();
    //        Response.StatusCode = 403;
    //        Response.End();
    //    }

    string action = Request.Form["Action"];
    string reservationString = Request.Form["Reservation"] ?? "null";

    Shared.Reservation reservation = Newtonsoft.Json.JsonConvert.DeserializeObject<Shared.Reservation>(reservationString);

    Application.Lock();
    if (action == "delete")
    {
        diceServer.reservationController.removeReservation(reservation);
    }
    else if (action == "add")
    {
        reservation.timestamp = DateTime.UtcNow;
        diceServer.reservationController.addReservation(reservation);
    }
    else if (action == "update")
    {
        reservation.timestamp = DateTime.UtcNow;
        diceServer.reservationController.updateReservation(reservation);
    }
    Application.UnLock();



    if (reservation == null)
    {

        Response.Write("<p>No reservation provided</p>");


    }
    else
    {
        Application.Lock();
        //TODO: check if add, change or remove; maybe earlier
        diceServer.reservationController.addReservation(reservation);
        Response.Write("...");
        Application.UnLock();
    }


%>