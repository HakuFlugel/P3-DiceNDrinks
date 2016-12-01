<%@ Page Language="C#"%>
<%@ Import Namespace="Server" %>

<%
    DiceServer diceServer = (DiceServer)Application["DiceServer"];

    if (!diceServer.authentication.authenticate(Request.Form["AdminKey"]))
    {
        Response.Clear();
        Response.StatusCode = 404;
        Response.End();
    }



%>