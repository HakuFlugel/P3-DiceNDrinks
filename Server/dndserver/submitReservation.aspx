<%@ Page Language="C#"%>

<%
    Server.DiceServer diceServer = (Server.DiceServer)Application["DiceServer"];

    //string adminKey = Request.Form["AdminKey"];
    //bool isAdmin = adminKey != null && diceServer.authentication.authenticate(adminKey);
    //    if (!diceServer.authentication.authenticate())
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
            diceServer.reservationController.updateReservation(reservation);
            break;
        default:
            Response.Write("invalid action");
            break;
    }
    Application.UnLock();

%>