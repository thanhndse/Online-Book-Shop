using AutoMapper;
using BookShopWithAuthen.Model.Models;
using BookShopWithAuthen.Web.ViewModel;
using BookShopWithAuthen.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BookShopWithAuthen.Web.Controllers
{
    [Authorize(Roles ="User")]
    public class UserManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public UserManageController()
        {
        }

        public UserManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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
        // GET: UserManage
        public ActionResult Index(string Message)
        {
            ViewBag.Message = Message;
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            var userViewModel = Mapper.Map<UserViewModel>(user);
            return View(userViewModel);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(UserViewModel userViewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.updated = false;
                return View("Index",userViewModel);
            }
            var currentUser = UserManager.FindByEmail(userViewModel.Email);
            currentUser.PhoneNumber = userViewModel.PhoneNumber;
            currentUser.Name = userViewModel.Name;
            currentUser.Address = userViewModel.Address;
            await UserManager.UpdateAsync(currentUser);
            return RedirectToAction("Index", new { Message = "Cập nhật tài khoản thành công" });
        }
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        //POST: /Manage/ChangePassword
       [HttpPost]
       [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = "Thay đổi mật khẩu thành công" });
            }
            AddErrors(result);
            return View(model);
        }



        #region Helpers
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        #endregion
    }
}