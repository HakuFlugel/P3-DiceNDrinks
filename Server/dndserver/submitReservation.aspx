<%@ Page Language="C#"%>

<%
    Server.DiceServer diceServer = (Server.DiceServer)Application["DiceServer"];

    string action = Request.Form["Action"];
    string reservationString = Request.Form["Reservation"] ?? "null";

    Shared.Reservation reservation = Newtonsoft.Json.JsonConvert.DeserializeObject<Shared.Reservation>(reservationString);

    if (reservation == null || action == null)
    {
        Response.Write("No reservation provided");
        return;
    }

    //TODO: make sure it handles both correct and incorrect input...
    Application.Lock();
    switch (action)
    {
        case "delete":
            diceServer.reservationController.removeReservation(reservation);
            Response.Write("deleted");
            break;
        case "add":
            reservation.timestamp = DateTime.UtcNow;
            diceServer.reservationController.addReservation(reservation);
            Response.Write("added " + reservation.id);
            break;
        case "update":
            try
            {
                reservation.timestamp = DateTime.UtcNow; //TODO: fix merge
                diceServer.reservationController.updateReservation(reservation);
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