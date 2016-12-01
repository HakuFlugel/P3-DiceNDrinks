<%@ Page Language="C#"%>
<%@ Import Namespace="System.IO" %>

<%
Response.Write("<title>test</title>");
Response.Write("Blame Hem hvis det går galt");
Response.Write(Directory.GetCurrentDirectory());
Response.Write(Server.MapPath("~"));
try
{
    Server.DiceServer diceServer = new Server.DiceServer();
    Application.Lock();
    Response.Write(diceServer.getReservations());
}
catch (Exception e)
{
    Response.Write(e);
}

%>

<%= typeof(Server.DiceServer)%>