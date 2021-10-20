using System;
using System.Globalization;
using System.Linq;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Cocycle.Models;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNet.Identity.EntityFramework;
using System.IO;

namespace Cocycle.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public readonly ApplicationDbContext DbContext ;
        public AccountController()
        {
            DbContext = new ApplicationDbContext();
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [AllowAnonymous]
        public ActionResult MyProfile()
        {

            var roleManager = new RoleManager<IdentityRole, string>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var userManager = new UserManager<ApplicationUser, string>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            ViewBag.States = DbContext.States.ToList();
            ViewBag.Areas = DbContext.Areas.ToList();
            ViewBag.Users = DbContext.Users.ToList();
            ProfileViewModel pvm = new ProfileViewModel();
            var currentUser = User.Identity.GetUserId();
            var user = userManager.FindById(currentUser);
            var role = roleManager.FindById(user.Roles.FirstOrDefault().RoleId);
            pvm.RoleName = role.Name;
            pvm.User = DbContext.Users.Where(x => x.Id == currentUser).Include(x=>x.PostCode).FirstOrDefault();
            pvm.routes = DbContext.Routes.Include(x => x.RouteSchedule).Include(x => x.FeedBack).Where(x => x.CreatedBy == currentUser).ToList();
            pvm.routeGroups = DbContext.RouteGroup.Include(x => x.routes).Include(x => x.routes.FeedBack).Where(x => x.RequestBy == currentUser).ToList();
            pvm.arrangeds = DbContext.Arranged.Include(x => x.FeedBack).Where(x => x.ApprovedBy == currentUser).ToList();
            pvm.MyRequests = DbContext.RouteGroup.Where(x => x.RequestBy == currentUser).Include(x => x.routes.FeedBack).ToList();
            pvm.RequestedRides = DbContext.Arranged.Include(x => x.FeedBack).Where(x => x.RequestBy == currentUser).ToList();
            pvm.RideFeedbacks = DbContext.FeedBacks.Where(x => x.UserId == currentUser).ToList();
            return View(pvm);
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    TempData["message"] = "login";
                    return RedirectToAction("Myprofile");
                    //return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        public void fillstates()
        {
            List<StateList> liststate = new List<StateList>();
            var lststate = DbContext.States.ToList();
            foreach (var s in lststate)
            {
                StateList stateList = new StateList
                {
                    StateId = Convert.ToInt32(s.Id),
                    StateName = s.StateName
                };
                liststate.Add(stateList);
            }
            ViewBag.States = liststate;
        }
        public void fillareas()
        {
            List<Area> Areas = new List<Area>();
            Areas = (from c in DbContext.Areas select c).ToList();
            ViewBag.Areas = Areas;
        }

        public void fillusertypes()
        {
            List<UserType> UserTypes = new List<UserType>()
                    {
                    new UserType() {UserTypeId = 0, UserTypeText="I Want To Help" },
                    new UserType() {UserTypeId = 1, UserTypeText="I Need Help" }
                    };
            ViewBag.UserTypes = UserTypes;
        }


        [HttpPost]
        public JsonResult Upload()
        {
            try
            {
                if (Request.Files.Count != 0)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var file = Request.Files[i];
                        var fileName = Path.GetFileName(file.FileName);

                        var guid = Guid.NewGuid().ToString();
                        var path = Path.Combine(Server.MapPath("~/Content/ProfilePic"), guid + fileName);
                        file.SaveAs(path);
                        var currentUser = User.Identity.GetUserId();
                        ApplicationUser model = UserManager.FindById(User.Identity.GetUserId());
                        model.ImageAddress = "../Content/ProfilePic/" + guid + fileName;
                        IdentityResult result = UserManager.Update(model);
                        if (result.Succeeded)
                        {
                            return Json(new { message = "Image Uplaoded Succesfully" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    return Json(new { message = "Cannot Uplaod Image" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { message = "File Not Found" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ee)
            {
                return Json(new { message = ee.Message }, JsonRequestBehavior.AllowGet);
                // throw;
            }
           
           
        }

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            fillstates();
            fillareas();
            fillusertypes();

            List<PostCode> postCodes = new List<PostCode>();
            postCodes = (from c in DbContext.postCodes select c).ToList();
            ViewBag.postCodes = postCodes;
         
            return View();
        }


        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            fillstates();
            fillareas();
            fillusertypes();

            List<PostCode>  postCodes = new List<PostCode>();
            postCodes = (from c in DbContext.postCodes select c).ToList();
            ViewBag.postCodes = postCodes;

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.Name,
                    Address = model.Address,
                    AreaId = model.AreaId,
                    StateId = model.StateId,
                    IsAvailable = true,
                    PhoneNumber = model.PhoneNumber,
                    Created= DateTime.Now,
                    Description= model.Description,
                    PostCodeId= model.PostCodeId
                };

                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    string[] usertypearr = { "cyclist", "learner" };
                    IdentityResult roleresult = await UserManager.AddToRoleAsync(user.Id, usertypearr[Convert.ToInt32(model.UserType)]);
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                

                    //// For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    //   SendMail(model.Email);
                    TempData["message"] = "registered";
                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }
        public void SendMail(string email)
        {
            var emailaccounts = DbContext.EmailAccount.FirstOrDefault();
            var message = new MailMessage();
            message.From = new MailAddress(emailaccounts.Email);
            message.To.Add(new MailAddress(email));
            message.Subject = "Registration Succesful";
            message.Body = "Welcome to Cocycle Group ";
            message.IsBodyHtml = true;
            using (var smtp = new SmtpClient())
            {
                smtp.EnableSsl = emailaccounts.EnableSSL;
                smtp.UseDefaultCredentials = emailaccounts.UseDefaultCredentials;
                var credential = new NetworkCredential
                {
                    UserName = emailaccounts.Email,  // replace with valid value
                    Password = emailaccounts.Password // replace with valid value
                };
                smtp.Credentials = credential;
                smtp.Host = emailaccounts.Host;
                smtp.Port = Convert.ToInt32(emailaccounts.Port);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            //return View(model);
        }
            
            // GET: /Account/ConfirmEmail
            [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                //var emailaccounts = DbContext.EmailAccount.FirstOrDefault();
                //var message = new MailMessage();
                //message.From = new MailAddress(emailaccounts.Email);
                //message.To.Add(new MailAddress(model.Email));
                //message.Subject = "Forgot Password";
                //message.Body = "Dear User<br/> Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a> ";
                //message.IsBodyHtml = true;
                //using (var smtp = new SmtpClient())
                //{
                //    smtp.EnableSsl = emailaccounts.EnableSSL;
                //    smtp.UseDefaultCredentials = emailaccounts.UseDefaultCredentials;
                //    var credential = new NetworkCredential
                //    {
                //        UserName = emailaccounts.Email,  // replace with valid value
                //        Password = emailaccounts.Password // replace with valid value
                //    };
                //    smtp.Credentials = credential;
                //    smtp.Host = emailaccounts.Host;
                //    smtp.Port = Convert.ToInt32(emailaccounts.Port);
                //    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                //     smtp.Send(message);


                    //await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }
        public async Task<ActionResult> EditUser(UserEditView useredit)
        {
            if (useredit.Id == null )
            {
                return View("Error");
            }
            ApplicationUser model = UserManager.FindById(User.Identity.GetUserId());
            model.Name = useredit.Name;
            model.PhoneNumber = useredit.PhoneNumber;
            model.Address = useredit.Address;
            model.StateId = useredit.StateId;
            model.AreaId = useredit.AreaId;
            model.Description = useredit.Description;
            model.PostCodeId = useredit.PostCodeId;
            model.IsAvailable = useredit.IsAvailable;
            IdentityResult result = await UserManager.UpdateAsync(model);
            if (result.Succeeded)
            {
                return RedirectToAction("MyProfile", "Account");
            }
            return View("Error");
        }





        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}