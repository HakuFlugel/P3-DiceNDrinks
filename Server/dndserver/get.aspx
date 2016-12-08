<%@ Page Language="C#"%>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="Newtonsoft.Json" %>
<%@ Import Namespace="Shared" %>

<%
    Server.DiceServer diceServer = (Server.DiceServer)Application["DiceServer"];

    string type = Request.Form["Type"];

    switch (type)
    {
        case "revid":
            Response.Write("5;7;3;14");
            break;
        case "Games":
            Response.Write(JsonConvert.SerializeObject(diceServer.gamesController.games));
            break;

        case "Products":
            Response.Write(JsonConvert.SerializeObject(new Tuple<List<ProductCategory>, List<Product>>(
                diceServer.productsController.categories,
                diceServer.productsController.products)
            ));
        break;

        case "Events":
            Response.Write(JsonConvert.SerializeObject(diceServer.eventsController.events));
            break;

        case "Reservations":
            string resid = Request.Form["ReservationID"];
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
                Response.Write(JsonConvert.SerializeObject(new Tuple<List<Room>, List<Shared.CalendarDay>>(
                    diceServer.reservationController.rooms,
                    diceServer.reservationController.reservationsCalendar)
                ));
            }
        break;
        default:
            Response.Write("invalid request");
            break;
    }

%>