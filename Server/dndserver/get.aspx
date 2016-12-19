<%@ Page Language="C#"%>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="Newtonsoft.Json" %>
<%@ Import Namespace="Shared" %>

<%
    Server.DiceServer diceServer = (Server.DiceServer)Application["DiceServer"];

    string adminKey = Request.Form["AdminKey"];
    bool isAdmin = adminKey != null && diceServer.authentication.authenticate(adminKey);

    string type = Request.Form["Type"] ?? "";

    switch (type)
    {
        case "timestamps":
            Response.Write(JsonConvert.SerializeObject(diceServer.timestamps));
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
                if (!isAdmin)
                {
                    return;
                }
                Response.Write(JsonConvert.SerializeObject(new Tuple<List<Room>, List<Shared.CalendarDay>>(
                    diceServer.reservationController.rooms,
                    diceServer.reservationController.reservationsCalendar)
                ));

            }
        break;

        case "Fullness":
            try
            {
                DateTime day = DateTime.Parse(Request.Form["Day"] ?? "");
                Shared.CalendarDay cd = diceServer.reservationController.reservationsCalendar.First(d => d.theDay.Date == day.Date);

                if (cd.isLocked)
                {
                    Response.Write((double)100);
                }
                else
                {
                    Response.Write(((double)cd.reservedSeats + cd.reservedSeatsPending)/diceServer.reservationController.totalSeats * 100);
                }
            }
            catch (Exception e)
            {
                Response.Write("failed ");
                Response.Write(e);
            }

            break;

        default:
            Response.Write("invalid request");
            break;
    }

%>