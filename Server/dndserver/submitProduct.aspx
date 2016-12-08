<%@ Page Language="C#"%>

<%
    Server.DiceServer diceServer = (Server.DiceServer)Application["DiceServer"];

    string adminKey = Request.Form["AdminKey"];
    bool isAdmin = adminKey != null && diceServer.authentication.authenticate(adminKey);
        if (isAdmin)
        {
            Response.Write("No permission");
            return;
            //throw new HttpException(404, "Not Found");

            //Response.Clear();
            //Response.StatusCode = 403;
            //Response.End();
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
            diceServer.productsController.removeProduct(product);
            Response.Write("deleted");
            break;
        case "add":
            product.timestamp = DateTime.UtcNow;
            diceServer.productsController.addProduct(product);
            Response.Write("added " + product.id);
            break;
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
            break;
        default:
            Response.Write("invalid action");
            break;
    }
    Application.UnLock();

%>