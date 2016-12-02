<%--  --%>

<%@ Application Language="C#" %>

<script runat="server">
    static string _pagePath;

    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
        Application["DiceServer"] = new Server.DiceServer(Server.MapPath("~"));

        _pagePath = Server.MapPath("~/Folder/Page.aspx");
    }


</script>