<%@ Page Language="C#"%>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="Newtonsoft.Json" %>

<%
    Server.DiceServer diceServer = (Server.DiceServer)Application["DiceServer"];

    string type = Request.Form["Type"];

    switch (type)
    {
        case "Games":
        Response.Write(JsonConvert.SerializeObject(diceServer.gamesController.games));
        break;

        case "Products":
        Response.Write(JsonConvert.SerializeObject(diceServer.productsController.categories));
        Response.Write(JsonConvert.SerializeObject(diceServer.productsController.products));
        break;

        case "Events":
        Response.Write(JsonConvert.SerializeObject(diceServer.eventsController.events));
        break;

        case "Reservations":
        string resid = Request.Form["Type"];
        if (resid != null)
        {

            Response.Write(JsonConvert.SerializeObject(diceServer.reservationController.reservationsCalendar
                .SelectMany(cd => cd.reservations)
                .FirstOrDefault(r => r.id == Convert.ToInt32(resid))
            ));
        }
        else
        {
            // TODO: is admin?
            // TODO: day fullness
            Response.Write(JsonConvert.SerializeObject(diceServer.reservationController.rooms));
            Response.Write(JsonConvert.SerializeObject(diceServer.reservationController.reservationsCalendar));
        }
        break;
    }

%>