<%--  --%>

<%@ Application Language="C#" %>
<%@ Import Namespace="Server" %>

<script runat="server">

    static string _pagePath;

    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
        Application["DiceServer"] = new DiceServer(Server.MapPath("~"));

        _pagePath = Server.MapPath("~/Folder/Page.aspx");
    }


</script>