<%@ Page Language="C#"%>
<%@ Import Namespace="System.IO" %>

<%

Response.Write("Blame Hem hvis det går galt");
Response.Write(Directory.GetCurrentDirectory());
Response.Write(Server.MapPath("~"));
try
{
    Server.Server server = new Server.Server();
    Response.Write(server.getReservations());
}
catch (Exception e)
{
    Response.Write(e);
}

%>

<%= typeof(Server.Server)%>