
using DWS.Framework.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DWS.Web.UI.Controllers;

// Handles pages and actions available to customers.
[Authorize(Roles = UserRoles.Customer)]
public class CustomerController : Controller
{
    // Displays the customer dashboard.
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    // Displays the products registered by the logged-in customer.
    [HttpGet]
    public IActionResult MyProducts()
    {
        return View();
    }
}
