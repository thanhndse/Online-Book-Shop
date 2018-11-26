using AutoMapper;
using BookShopWithAuthen.Data;
using BookShopWithAuthen.Model.Models;
using BookShopWithAuthen.Service.Services;
using BookShopWithAuthen.Web.ViewModel;
using BookShopWithAuthen.Web.Helpers.EmailTemplate;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web.Mvc;


namespace BookShopWithAuthen.Web.Controllers
{
    [Authorize(Roles = "User")]
    public class CartController : Controller
    {
        private ICartService cartService;
        private IBookService bookService;
        private IOrderService orderService;
        private IOrderDetailService orderDetailService;
        private UserManager<ApplicationUser> userManager;
        public CartController(ICartService cartService,
                                IBookService bookService,
                                IOrderService orderService,
                                IOrderDetailService orderDetailService,
                                UserManager<ApplicationUser> userManager)
        {
            this.orderDetailService = orderDetailService;
            this.orderService = orderService;
            this.cartService = cartService;
            this.bookService = bookService;
            this.userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        }
        // GET: Cart
        public ActionResult Index()
        {

            if (TempData["errorMessage"] != null)
            {
                ViewBag.errorMessage = TempData["errorMessage"].ToString();
            }

            // get cart
            string userID = User.Identity.GetUserId();
            IEnumerable<CartItemViewModel> listCartItems = from cartDetail in cartService.GetByUserID(userID)
                                                           join book in bookService.GetAll() on cartDetail.BookID equals book.ID
                                                           where cartDetail.Quantity > 0
                                                           select new CartItemViewModel()
                                                           {
                                                               BookId = book.ID,
                                                               BookName = book.Name,
                                                               Price = book.Price,
                                                               Image = book.Image,
                                                               Quantity = cartDetail.Quantity
                                                           };

            ViewBag.totalMoney = cartService.GetTotalMoney(userID);
            ViewBag.listCartItems = listCartItems.ToList();

            // get user infor
            ApplicationUser user = userManager.FindByName(User.Identity.Name);
            var shippingDetailViewModel = Mapper.Map<ShippingDetailViewModel>(user);
            return View(shippingDetailViewModel);
        }
        [HttpPost]
        public ActionResult Checkout(ShippingDetailViewModel shippingDetailViewModel)
        {
            if (ModelState.IsValid)
            {
                string userId = User.Identity.GetUserId();
                IEnumerable<CartDetail> cartDetails = cartService.GetByUserID(userId);
                IEnumerable<Book> books = bookService.GetAll();
                // Kiem tra tinh hop le cua gio hang
                bool flagValid = true;
                foreach (var cartDetail in cartDetails)
                {
                    Book tmpBook = cartDetail.Book;
                    int wareHouseQuantity = (int)bookService.GetByID(tmpBook.ID).Quantity;
                    if (cartDetail.Quantity > wareHouseQuantity)
                    {
                        cartDetail.Quantity = wareHouseQuantity;
                        cartService.UpdateCartDetail(cartDetail);
                        flagValid = false;
                    }
                }
                if (flagValid == false)
                {
                    TempData["errorMessage"] = "Một số sách bạn đặt có số lượng không đủ, chúng tôi đã cập nhật " +
                        " lại số lượng sách của vài sản phẩm trong giỏ hàng, mời bạn xem và đặt hàng lại";
                    return RedirectToAction("Index");
                }
                // Neu hop le
                // create order and orderDetail and update book quantity
                int totalMoney = cartService.GetTotalMoney(User.Identity.GetUserId());
                Order order = Mapper.Map<Order>(shippingDetailViewModel);
                order.UserId = userId;
                order.Status = (int)StatusOrder.New;
                order.OrderDate = DateTime.Today;
                order.Amount = totalMoney;
                orderService.CreateOrder(order);




                // send mail to customer
                string subject = "Notification about your order at DTBook";
                ICollection<OrderDetailViewModel> orderDetailViewModels = new List<OrderDetailViewModel>();
                var orderDetails = order.OrderDetails;
                foreach (var item in orderDetails)
                {
                    OrderDetailViewModel tmpOrDeViewModel = Mapper.Map<OrderDetailViewModel>(item);
                    orderDetailViewModels.Add(tmpOrDeViewModel);
                }
                var model = new OrderConfirmEmailModel()
                {
                    Order = order,
                    OrderDetailViewModels = orderDetailViewModels
                };
                var path = Path.Combine(Server.MapPath("/Helpers/EmailTemplate"), "OrderConfirmTemplate.cshtml");
                var templateSerivce = new TemplateService();
                try
                {
                    string emailHtmlBody = templateSerivce.Parse(System.IO.File.ReadAllText(path), model, null, null);
                    OtherService.SendMail(shippingDetailViewModel.Email, subject, emailHtmlBody);
                }
                catch (Exception ex)
                {
                    Debug.Write("This is inner exeption: " + ex.Message);
                    Debug.Write("This is inner exeption: " + ex.InnerException);
                }
                return View();
            }
            else
            {
                string errorMessage = "";
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        errorMessage += error.ErrorMessage + "<br />";
                    }
                }
                TempData["errorMessage"] = errorMessage;

                return RedirectToAction("Index");
            }
            
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult AddToCart(int bookID)
        {
            bool result = true;
            string redirect = "";
            bool maxQuantity = true;
            if (!User.Identity.IsAuthenticated)
            {
                result = false;
                redirect = "/Account/Login?returnUrl=/Cart";
            }
            else
            {
                int quantity = cartService.GetQuanity(User.Identity.GetUserId(), bookID);
                if (quantity <= 19)
                {
                    string userID = User.Identity.GetUserId();
                    cartService.AddItemToCart(userID, bookID);
                    maxQuantity = false;
                }
                
            }
            return Json(new { result = result, redirect = redirect, maxQuantity = maxQuantity });
            
        }

        [HttpPost]
        public JsonResult changeQuantity(int bookID, int quantity)
        {

            string userID = User.Identity.GetUserId();
            cartService.UpdateCartDetail(new CartDetail() { BookID = bookID, UserID = userID, Quantity = quantity });
            int totalPrice = bookService.GetByID(bookID).Price * quantity;
            int totalMoney = cartService.GetTotalMoney(userID);
            return Json(new { totalPrice = totalPrice, totalMoney = totalMoney }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult deleteItem(int bookID)
        {
            string userID = User.Identity.GetUserId();
            cartService.RemoveCartDetail(userID, bookID);
            int totalMoney = cartService.GetTotalMoney(userID);
            return Json(new { totalMoney = totalMoney }, JsonRequestBehavior.AllowGet);
        }
    }
}