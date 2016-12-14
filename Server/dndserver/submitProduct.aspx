<%@ Page Language="C#"%>

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

    //TODO: make sure it handles both correct and incorrect input...
    Application.Lock();
    switch (action)
    {
        case "delete":
            if (product.image != null)
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
                product.timestamp = DateTime.UtcNow; //TODO: fix merge
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
            if (imgstring != null)
            {
                try
                {
                    byte[] data = new byte[imgstring.Length];
                    int i = 0;
                    foreach (var _char in imgstring)
                    {
                        data[i++] = Convert.ToByte(_char);
                    }

                    System.Drawing.Image image = Shared.ImageHelper.byteArrayToImage(data);

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