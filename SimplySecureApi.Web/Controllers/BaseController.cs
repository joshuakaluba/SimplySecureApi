﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimplySecureApi.Data.DataAccessLayer.Boots;
using SimplySecureApi.Data.DataAccessLayer.Modules;
using SimplySecureApi.Data.DataAccessLayer.ModuleEvents;
using SimplySecureApi.Data.Models.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using SimplySecureApi.Data.DataAccessLayer.Locations;

namespace SimplySecureApi.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly UserManager<ApplicationUser> UserManager;

        protected BaseController(UserManager<ApplicationUser> userManager)
        {
            this.UserManager = userManager;
        }

        protected string GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return userId;
        }

        protected async Task<ApplicationUser> GetUser()
        {
            var user = await UserManager.FindByIdAsync(GetUserId());

            return user;
        }
    }
}