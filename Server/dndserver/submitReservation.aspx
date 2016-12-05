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

    if (reservation == null || action == null)
    {
        Response.Write("<p>No reservation provided</p>");
        return;
    }

    //TODO: make sure it handles both correct and incorrect input...
    Application.Lock();
    if (action == "delete")
    {
        diceServer.reservationController.removeReservation(reservation);
        Response.Write("deleted");
    }
    else if (action == "add")
    {
        reservation.timestamp = DateTime.UtcNow;
        diceServer.reservationController.addReservation(reservation);
        Response.Write("added " +reservation.id);
    }
    else if (action == "update")
    {
        try
        {
            diceServer.reservationController.updateReservation(reservation);
            Response.Write("updated");
        }
        catch (ArgumentNullException)
        {
            Response.Write("failed");
        }
        reservation.timestamp = DateTime.UtcNow; //TODO: fix merge
        diceServer.reservationController.updateReservation(reservation);
    }
    Application.UnLock();



    if (reservation == null)
    {

        Response.Write("<p>No reservation provided</p>");


    }
    else
    {
        Response.Write("<p>invalid action</p>");
    }
    Application.UnLock();

%>