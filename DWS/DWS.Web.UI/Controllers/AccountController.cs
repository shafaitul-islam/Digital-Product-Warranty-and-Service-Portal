
using DWS.Entities.Models;
using DWS.Facade.Interfaces;
using DWS.Framework.Constants;
using DWS.Web.UI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Net;

namespace DWS.Web.UI.Controllers;

// Handles registration, authentication, logout, and password recovery.
public class AccountController : Controller
{
    // Manages user creation, updates, and role assignment.
    private readonly UserManager<ApplicationUser> _userManager;

    // Manages user sign-in and sign-out operations.
    private readonly SignInManager<ApplicationUser> _signInManager;

    // Sends password recovery emails through Gmail SMTP.
    private readonly IEmailService _emailService;

    // Records application errors for developer troubleshooting.
    private readonly ILogger<AccountController> _logger;

    // Receives required services through dependency injection.
    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailService emailService,
        ILogger<AccountController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
        _logger = logger;
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
            ModelState.AddModelError(
                string.Empty,
                error.Description);
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
            // Redirects only to a local URL after successful login.
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

    // Displays the forgot password form.
    [HttpGet]
    [AllowAnonymous]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    // Processes a password recovery request.
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(
        ForgotPasswordViewModel model)
    {
        // Checks whether the submitted email address is valid.
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Looks up the account using the submitted email address.
        var user = await _userManager.FindByEmailAsync(model.Email);

        // Uses the same response for existing and non-existing accounts.
        if (user != null && user.IsActive)
        {
            try
            {
                // Generates a secure password reset token using Identity.
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                // Encodes the token so it can safely travel inside a URL.
                var encodedToken = WebEncoders.Base64UrlEncode(
                    Encoding.UTF8.GetBytes(token));

                // Generates the password reset link.
                var resetLink = Url.Action(
                    nameof(ResetPassword),
                    "Account",
                    new
                    {
                        email = user.Email,
                        token = encodedToken
                    },
                    protocol: Request.Scheme);

                if (!string.IsNullOrWhiteSpace(resetLink))
                {
                    // Encodes user-provided content before inserting it into HTML.
                    var safeName = WebUtility.HtmlEncode(user.FullName);
                    var safeLink = WebUtility.HtmlEncode(resetLink);

                    // Builds the HTML content of the recovery email.
                    var emailBody = $@"
                        <div style='font-family:Arial,sans-serif;max-width:600px;margin:auto;padding:25px;'>
                            <h2 style='color:#2456b5;'>Password Reset Request</h2>
                            <p>Hello {safeName},</p>
                            <p>We received a request to reset your DWS account password.</p>
                            <p>Click the button below to create a new password.</p>
                            <p style='margin:30px 0;'>
                                <a href='{safeLink}'
                                   style='background:#2456b5;color:white;padding:14px 24px;text-decoration:none;border-radius:8px;'>
                                    Reset My Password
                                </a>
                            </p>
                            <p>If you did not request this change, you can safely ignore this email.</p>
                            <p style='color:#777;font-size:12px;'>Digital Warranty & Service Portal</p>
                        </div>";

                    // Sends the password reset email through Gmail SMTP.
                    await _emailService.SendEmailAsync(
                        user.Email!,
                        "Reset Your DWS Account Password",
                        emailBody);
                }
                else
                {
                    // Records an error if the reset URL could not be generated.
                    _logger.LogError(
                        "Password reset URL generation failed.");
                }
            }
            catch (Exception ex)
            {
                // Records the SMTP or token generation error for troubleshooting.
                // Does not expose technical details to the customer.
                _logger.LogError(
                    ex,
                    "Password recovery email processing failed.");
            }
        }

        // Always displays the same message to prevent email enumeration.
        return RedirectToAction(nameof(ForgotPasswordConfirmation));
    }

    // Displays the password recovery confirmation page.
    [HttpGet]
    [AllowAnonymous]
    public IActionResult ForgotPasswordConfirmation()
    {
        return View();
    }

    // Displays the reset password form using the supplied email and token.
    [HttpGet]
    [AllowAnonymous]
    public IActionResult ResetPassword(string? email, string? token)
    {
        // Rejects requests with missing reset information.
        if (string.IsNullOrWhiteSpace(email)
            || string.IsNullOrWhiteSpace(token))
        {
            return View("ResetPasswordInvalid");
        }

        string decodedToken;

        try
        {
            // Decodes the URL-safe token generated earlier.
            decodedToken = Encoding.UTF8.GetString(
                WebEncoders.Base64UrlDecode(token));
        }
        catch (FormatException)
        {
            // Rejects a malformed reset token.
            return View("ResetPasswordInvalid");
        }

        // Creates the reset form model.
        var model = new ResetPasswordViewModel
        {
            Email = email,
            Token = decodedToken
        };

        return View(model);
    }

    // Processes the new password submitted by the customer.
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(
        ResetPasswordViewModel model)
    {
        // Checks whether all submitted form values are valid.
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Finds the account associated with the reset email address.
        var user = await _userManager.FindByEmailAsync(model.Email);

        // Rejects unknown or inactive accounts.
        if (user == null || !user.IsActive)
        {
            return View("ResetPasswordInvalid");
        }

        // Resets the password after Identity validates the token.
        var result = await _userManager.ResetPasswordAsync(
            user,
            model.Token,
            model.NewPassword);
        // Logs Identity password reset validation results.
        foreach (var error in result.Errors)
        {
            _logger.LogWarning(
                "Password reset failed. Error Code: {ErrorCode}",
                error.Code);
        }

        if (result.Succeeded)
        {
            // Displays the successful password reset confirmation.
            return RedirectToAction(nameof(ResetPasswordConfirmation));
        }

        // Checks whether Identity rejected the reset token.
        if (result.Errors.Any(error => error.Code == "InvalidToken"))
        {
            // Displays the invalid or expired reset link page.
            return View("ResetPasswordInvalid");
        }

        // Displays password policy and other validation errors.
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(
                string.Empty,
                error.Description);
        }

        return View(model);
    }

    // Displays a confirmation after successful password reset.
    [HttpGet]
    [AllowAnonymous]
    public IActionResult ResetPasswordConfirmation()
    {
        return View();
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
