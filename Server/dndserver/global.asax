<%--  --%>

<%@ Application Language="C#" %>

<script runat="server">

    public void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
        Application["DiceServer"] = new Server.DiceServer(Server.MapPath("~"));

    }


</script>