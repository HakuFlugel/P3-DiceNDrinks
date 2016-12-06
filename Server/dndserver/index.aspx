<%@ Page Language="C#"%>
<%@ Import Namespace="System.IO" %>

<%
Response.Write("<title>test</title>");
Response.Write("Blame Hem hvis det går galt");
Response.Write(Directory.GetCurrentDirectory());
Response.Write(Server.MapPath("~"));

%>

<%= typeof(Server.DiceServer)%>