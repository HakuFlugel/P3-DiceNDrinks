<%@ Page Language="C#"%>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="Newtonsoft.Json" %>
<%@ Import Namespace="Shared" %>

<%


    Server.DiceServer diceServer = (Server.DiceServer)Application["DiceServer"];

    string adminKey = Request.Form["AdminKey"];
    bool isAdmin = adminKey != null && diceServer.authentication.authenticate(adminKey);

    if (!isAdmin)
    {
        Response.Write("Permission denied");
        Response.StatusCode = 403;
        return;
    }

    try
    {

        string contactinfoString = Request.Form["ContactInfo"] ?? "null";

        ContactInformation contactInformation = Newtonsoft.Json.JsonConvert.DeserializeObject<ContactInformation>(contactinfoString);

        if (contactInformation == null)
        {
            Response.Write("No contact information provided");
            return;
        }

        Application.Lock();

        diceServer.contactInformation = contactInformation;
        Response.Write("success");

        diceServer.setTimestamp("ContactInfo", DateTime.UtcNow);

    }
    catch (Exception e)
    {
        Response.Write("failed ");
        //TODO: remove
        Response.Write(e);
    }

    Application.UnLock();

%>