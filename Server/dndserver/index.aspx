<%@ Page Language="C#"%>
<%@ Import Namespace="System.IO" %>

<%
Response.Write("<title>test</title>");
Response.Write("Test");
Response.Write(Directory.GetCurrentDirectory());
Response.Write(Server.MapPath("~"));

%>

<%= typeof(Server.DiceServer)%>