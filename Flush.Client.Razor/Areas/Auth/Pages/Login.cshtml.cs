using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Flush.Authentication;
using Flush.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Flush.Client.Razor.Areas.Auth.Pages
{
    /// <summary>
    /// A model for the login page, supporting registration, token procurement
    /// and redirection.
    /// </summary>
    public class LoginModel : PageModel
    {
        [Required]
        [Display(Name = "Room Name")]
        [BindProperty]
        public string InputRoom { get; set; }

        [Required]
        [Display(Name = "What's Your Name?")]
        [BindProperty]
        public string InputName { get; set; }

        /// <summary>
        /// An error message returned during post, if applicable.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// A logger for use by this model.
        /// </summary>
        private readonly ILogger<LoginModel> logger;

        /// <summary>
        /// Construct an instance of the LoginModel.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public LoginModel(ILogger<LoginModel> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public async Task<IActionResult> OnGetAsync(string r, string n)
        {
            ViewData["Title"] = "Authorisation";

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            /*
             * We can populate these even if r/n are null. They'll just appear
             * empty to the user.
             */
            InputRoom = r;
            InputName = n;

            await Task.CompletedTask;
            return Page();
        }

        /// <summary>
        /// Peforms guest user registration and generates a JWT.
        /// </summary>
        /// <returns>A JSON response containing a JWT.</returns>
        public async Task<IActionResult> OnPostAcquireTokenAsync()
        {
            logger.LogDebug($"Entering {nameof(OnPostAcquireTokenAsync)}.");

            if (ModelState.IsValid)
            {
                logger.LogDebug($"A user is attempting to log in.");

                // Normalise the inputs to lowercase wthout spacs.
                var inputNameNormalised = InputName.ToLowerInvariant().Replace(" ", string.Empty);
                var inputRoomNormalised = InputRoom.ToLowerInvariant().Replace(" ", string.Empty);

                // Construct the authentication fields.
                var displayName = InputName;
                var userName = $@"{inputNameNormalised}+{inputRoomNormalised}";
                var emailAddress = $@"{inputNameNormalised}@{inputRoomNormalised}.guests.scrumpoker.club";

                var services = HttpContext.RequestServices;
                var authenticationServiceProxy =
                    services.GetService(typeof(IAuthenticationServiceProxy)) as IAuthenticationServiceProxy;

                var userInfo = await authenticationServiceProxy.GetUserByEmail(emailAddress, true);
                if (userInfo is null)
                {
                    // Need to register a new user as they don't already exist
                    var registrationData = new RegistrationData
                    {
                        Name = displayName,
                        Username = userName,
                        Email = emailAddress
                    };

                    // if userinfo is still null after registration, we fail out.
                    userInfo = await authenticationServiceProxy.RegisterUser(registrationData);
                    if (userInfo is null)
                    {
                        ModelState.AddModelError(string.Empty, "We were unable to register you.");
                        return Page();
                    }
                }

                // Assuming we get this far, let's double check the user can sign in.
                var canSignInResult = await authenticationServiceProxy.CanSignIn(userInfo);
                switch (canSignInResult)
                {
                    case CanSignInResult.SignedIn:
                        ModelState.AddModelError(string.Empty, "It looks like you're already signed in!");
                        return Page();
                    case CanSignInResult.LockedOut:
                        ModelState.AddModelError(string.Empty, "Unfortunately, the account has been locked out.");
                        return Page();
                    case CanSignInResult.Success:
                        break;
                }

                // If we got this far, we can sign in!
                var signInCredentials = new SignInCredentials
                {
                    Room = InputRoom,
                    Username = userName
                };
                var userInfo2 = await authenticationServiceProxy.SignIn(signInCredentials);

                // last step, the userinfo should not be null ...
                if (userInfo2 is null)
                {
                    ModelState.AddModelError(string.Empty, "Failed to sign in.");
                    return Page();
                }

                // ... and should match the earlier one.
                if (!userInfo2.Equals(userInfo))
                {
                    await authenticationServiceProxy.SignOut();
                    //await authenticationServiceProxy.RemoveUser(userInfo);
                    ModelState.AddModelError(string.Empty, "The signed in user didn't match who we expected.");
                    return Page();
                }

                // Everything good? Let's go!
                return RedirectToPage("/Standard", new { area = "Play" });
            }

            // If we got this far, something failed, redisplay form
            logger.LogError("The model state was not valid.");
            logger.LogDebug($"Exiting {nameof(OnPostAcquireTokenAsync)}.");
            return Page();
        }
    }
}
