<%@ Page Language="C#"%>
<%@ Import Namespace="Shared" %>

<%


    Server.DiceServer diceServer = (Server.DiceServer)Application["DiceServer"];

    string adminKey = Request.Form["AdminKey"];
    bool isAdmin = adminKey != null && diceServer.authentication.authenticate(adminKey);


    try
        {

    string action = Request.Form["Action"];
    string reservationString = Request.Form["Reservation"] ?? "null";



        Shared.Reservation reservation = Newtonsoft.Json.JsonConvert.DeserializeObject<Shared.Reservation>(reservationString);

        if (reservation == null || action == null)
        {
            Response.Write("No reservation provided");
            return;
        }

        Application.Lock();

        switch (action)
        {

            case "delete":
                diceServer.reservationController.removeReservation(reservation);
                Response.Write("deleted");
                break;
            case "add":
                reservation.timestamp = DateTime.UtcNow;

                if (!isAdmin)
                {
                    reservation.state = Reservation.State.Pending;
                }

                diceServer.reservationController.addReservation(reservation);
                Response.Write("added " + reservation.id);
                break;
            case "update":

                reservation.timestamp = DateTime.UtcNow;
                diceServer.reservationController.updateReservation(reservation);
                Response.Write("updated");

                break;
            default:
                Response.Write("invalid action");
                break;
        }
    }
    catch (Exception e)
    {
        Response.Write("failed");
        //TODO: remove
        Response.Write(e.Message);
    }

    Application.UnLock();

%>