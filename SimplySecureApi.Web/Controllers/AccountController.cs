using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimplySecureApi.Data;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using SimplySecureApi.Data.Models.Authentication;
using SimplySecureApi.Data.DataContext;
using SimplySecureApi.Data.Models;
using SimplySecureApi.Data.Models.Response;

namespace SimplySecureApi.Web.Controllers
{
	[Route ( "[controller]/[action]" )]
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private SimplySecureDataContext _context;

		public AccountController (
			UserManager<ApplicationUser> userManager ,
			SignInManager<ApplicationUser> signInManager ,
			SimplySecureDataContext context ,
			ILogger<AccountController> logger )
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_context = context;
		}

		[TempData]
		public string ErrorMessage
		{
			get; set;
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> Login ( string returnUrl = null )
		{
			// Clear the existing external cookie to ensure a clean login process
			await HttpContext.SignOutAsync ( IdentityConstants.ExternalScheme );

			ViewData [ "ReturnUrl" ] = returnUrl;
			return View ( );
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login ( LoginViewModel model , string returnUrl = null )
		{
			ViewData [ "ReturnUrl" ] = returnUrl;
			if ( ModelState.IsValid )
			{
				// This doesn't count login failures towards account lockout
				// To enable password failures to trigger account lockout, set lockoutOnFailure: true
				var result = await _signInManager.PasswordSignInAsync ( model.Email , model.Password , model.RememberMe , lockoutOnFailure: false );
				if ( result.Succeeded )
				{
					return RedirectToLocal ( returnUrl );
				}

				if ( result.IsLockedOut )
				{
					return RedirectToAction ( nameof ( Lockout ) );
				}
				else
				{
					ModelState.AddModelError ( string.Empty , "Invalid login attempt." );
					return View ( model );
				}
			}

			return View ( model );
		}


		[HttpGet]
		[AllowAnonymous]
		public IActionResult Lockout ( )
		{
			return View ( );
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult Register ( string returnUrl = null )
		{
			ViewData [ "ReturnUrl" ] = returnUrl;
			return View ( );
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register ( RegisterViewModel model , string returnUrl = null )
		{
			ViewData [ "ReturnUrl" ] = returnUrl;

			if ( ModelState.IsValid )
			{
				var user = new ApplicationUser { UserName = model.Email , Email = model.Email };

				var result = await _userManager.CreateAsync ( user , model.Password );

				if ( result.Succeeded )
				{
					var code = await _userManager.GenerateEmailConfirmationTokenAsync ( user );					

					var claim = new Claim ( "DefaultUserClaim" , "DefaultUserAuthorization" );

					var addClaimResult = await _userManager.AddClaimAsync ( user , claim );

                    await _signInManager.SignInAsync( user, isPersistent: false );

                    TempData["CustomResponseAlert"] = ViewResponseAlert.GetStringResponse( ViewResponseStatus.Success, "Thank you for registering!" );

                    return RedirectToLocal ( returnUrl );
				}

				AddErrors ( result );
			}

			return View ( model );
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout ( )
		{
			await _signInManager.SignOutAsync ( );
			return RedirectToAction ( nameof ( HomeController.Index ) , "Home" );
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult ForgotPassword ( )
		{
			return View ( );
		}

	
		[HttpGet]
		[AllowAnonymous]
		public IActionResult ResetPasswordConfirmation ( )
		{
			return View ( );
		}

		[HttpGet]
		public IActionResult AccessDenied ( )
		{
			return View ( );
		}

		#region Helpers

		private void AddErrors ( IdentityResult result )
		{
			foreach ( var error in result.Errors )
			{
				ModelState.AddModelError ( string.Empty , error.Description );
			}
		}

		private IActionResult RedirectToLocal ( string returnUrl )
		{
			if ( Url.IsLocalUrl ( returnUrl ) )
			{
				return Redirect ( returnUrl );
			}
			else
			{
				return RedirectToAction ( nameof ( HomeController.Index ) , "Home" );
			}
		}

		#endregion Helpers
	}
}