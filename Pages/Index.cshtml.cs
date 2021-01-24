﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Threading.Tasks;
using Project.Models;
using Project.Models.DB;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Project.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        //public IndexModel(ILogger<IndexModel> logger)
        //{
        //    _logger = logger;
        //}

        //public void OnGet()
        //{

        //}

        #region Private Properties.  

        /// <summary>  
        /// Database Manager property.  
        /// </summary>  
        private readonly db_coreloginContext databaseManager;

        #endregion

        #region Default Constructor method.  

        /// <summary>  
        /// Initializes a new instance of the <see cref="IndexModel"/> class.  
        /// </summary>  
        /// <param name="databaseManagerContext">Database manager context parameter</param>  
        public IndexModel(db_coreloginContext databaseManagerContext)
        {
            try
            {
                // Settings.  
                this.databaseManager = databaseManagerContext;
            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);
            }
        }

        #endregion

        #region Public Properties  

        /// <summary>  
        /// Gets or sets login model property.  
        /// </summary>  
        [BindProperty]
        public LoginViewModel LoginModel { get; set; }

        #endregion

        #region On Get method.  

        /// <summary>  
        /// GET: /Index  
        /// </summary>  
        /// <returns>Returns - Appropriate page </returns>  
        public IActionResult OnGet()
        {
            try
            {
                // Verification.  
                if (this.User.Identity.IsAuthenticated)
                {
                    // Home Page.  
                    return this.RedirectToPage("/Home/Index");
                }
            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);
            }

            // Info.  
            return this.Page();
        }

        #endregion

        #region On Post Login method.  

        /// <summary>  
        /// POST: /Index/LogIn  
        /// </summary>  
        /// <returns>Returns - Appropriate page </returns>  
        public async Task<IActionResult> OnPostLogIn()
        {
            try
            {
                // Verification.  
                if (ModelState.IsValid)
                {
                    // Initialization.  
                    var loginInfo = await this.databaseManager.LoginByUsernamePasswordMethodAsync(this.LoginModel.Username, this.LoginModel.Password);

                    // Verification.  
                    if (loginInfo != null && loginInfo.Count() > 0)
                    {
                        // Initialization.  
                        var logindetails = loginInfo.First();

                        // Login In.  
                        await this.SignInUser(logindetails.Username, false);

                        // Info.  
                        return this.RedirectToPage("/Home/Index");
                    }
                    else
                    {
                        // Setting.  
                        ModelState.AddModelError(string.Empty, "Invalid username or password.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);
            }

            // Info.  
            return this.Page();
        }

        #endregion

        #region Helpers  

        #region Sign In method.  

        /// <summary>  
        /// Sign In User method.  
        /// </summary>  
        /// <param name="username">Username parameter.</param>  
        /// <param name="isPersistent">Is persistent parameter.</param>  
        /// <returns>Returns - await task</returns>  
        private async Task SignInUser(string username, bool isPersistent)
        {
            // Initialization.  
            var claims = new List<Claim>();

            try
            {
                // Setting  
                claims.Add(new Claim(ClaimTypes.Name, username));
                var claimIdenties = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimPrincipal = new ClaimsPrincipal(claimIdenties);
                var authenticationManager = Request.HttpContext;

                // Sign In.  
                await authenticationManager.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal, new AuthenticationProperties() { IsPersistent = isPersistent });
            }
            catch (Exception ex)
            {
                // Info  
                throw ex;
            }
        }

        #endregion

        #endregion
    }
}
