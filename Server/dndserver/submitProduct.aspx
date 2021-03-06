﻿<%@ Page Language="C#"%>
<%@ Import Namespace="System.Drawing.Imaging" %>

<%
    Server.DiceServer diceServer = (Server.DiceServer)Application["DiceServer"];

    string adminKey = Request.Form["AdminKey"];
    bool isAdmin = adminKey != null && diceServer.authentication.authenticate(adminKey);
    if (!isAdmin)
    {
        Response.Write("No permission");
        Response.StatusCode = 403;
        return;
    }

    string action = Request.Form["Action"];
    string productString = Request.Form["Product"] ?? "null";

    Shared.Product product = Newtonsoft.Json.JsonConvert.DeserializeObject<Shared.Product>(productString);

    if (product == null || action == null)
    {
        Response.Write("No product provided");
        return;
    }

    Application.Lock();
    switch (action)
    {
        case "delete":
            if (!string.IsNullOrEmpty(product.image))
            {
                try
                {
                    System.IO.File.Delete(diceServer.path + "images/products/" + product.image);

                }
                catch (Exception)
                {
                    Response.Write("imgfailed");
                    return;
                }
            }
            diceServer.productsController.removeProduct(product);

            Response.Write("deleted");
            break;
        case "add":
            product.timestamp = DateTime.UtcNow;
            diceServer.productsController.addProduct(product);
            Response.Write("added " + product.id);
            goto doafter;

        case "update":
            try
            {
                product.timestamp = DateTime.UtcNow;
                diceServer.productsController.updateProduct(product);
                Response.Write("updated");
            }
            catch (Exception)
            {
                Response.Write("failed");
            }
            //goto doafter;

        doafter:
            string imgstring = Request.Form["Image"];
            if (!string.IsNullOrEmpty(imgstring))
            {
                try
                {
                    System.Drawing.Image image = Shared.ImageHelper.byteArrayToImage(imgstring);

                    image.Save(diceServer.path + "images/products/" + product.image);
                }
                catch (Exception)
                {
                    Response.Write("imgfailed");
                    return;
                }
            }

            break;

        default:
            Response.Write("invalid action");
            break;
    }

    diceServer.setTimestamp("Products", DateTime.UtcNow);

    Application.UnLock();

%>