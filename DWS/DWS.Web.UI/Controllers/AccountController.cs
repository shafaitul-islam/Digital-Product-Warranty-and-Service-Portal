
using DWS.Entities.Models;
using DWS.Framework.Constants;
using DWS.Web.UI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DWS.Web.UI.Controllers;

// Handles customer registration, login, logout, and access-denied requests.
public class AccountController : Controller
{
    // Manages user creation, updates, and role assignment.
    private readonly UserManager<ApplicationUser> _userManager;

    // Manages user sign-in and sign-out operations.
    private readonly SignInManager<ApplicationUser> _signInManager;

    // Constructor: Receives Identity services through dependency injection.
    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    // Displays the customer registration form.
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register()
    {
        return View();
    }

    // Processes the customer registration form.
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        // Checks whether the submitted form data is valid.
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Creates a new application user from the registration information.
        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FullName = model.FullName,
            PhoneNumber = model.PhoneNumber,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // Creates the user account and securely stores the password.
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            // Assigns the Customer role to the newly registered user.
            var roleResult = await _userManager.AddToRoleAsync(
                user,
                UserRoles.Customer);

            // Removes the account if role assignment fails.
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);

                foreach (var error in roleResult.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description);
                }

                return View(model);
            }

            // Signs in the customer after successful registration.
            await _signInManager.SignInAsync(user, isPersistent: false);

            return RedirectToAction("Index", "Home");
        }

        // Displays Identity validation errors on the registration form.
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    // Displays the login form.
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        // Keeps the requested destination after successful login.
        ViewData["ReturnUrl"] = returnUrl;

        return View();
    }

    // Processes the customer login form.
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(
        LoginViewModel model,
        string? returnUrl = null)
    {
        // Preserves the return URL when validation fails.
        ViewData["ReturnUrl"] = returnUrl;

        // Checks whether the submitted login information is valid.
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Checks the email and password against ASP.NET Core Identity.
        var result = await _signInManager.PasswordSignInAsync(
            model.Email,
            model.Password,
            model.RememberMe,
            lockoutOnFailure: false);

        if (result.Succeeded)
        {
            // Redirects to the requested local page after successful login.
            if (!string.IsNullOrWhiteSpace(returnUrl)
                && Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            // Redirects to the home page when no return URL is provided.
            return RedirectToAction("Index", "Home");
        }

        // Displays an error when the login information is incorrect.
        ModelState.AddModelError(
            string.Empty,
            "Invalid email or password.");

        return View(model);
    }

    // Signs out the currently authenticated user.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return RedirectToAction("Index", "Home");
    }

    // Displays the access-denied page.
    [HttpGet]
    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return View();
    }
}
