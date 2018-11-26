using AutoMapper;
using BookShopWithAuthen.Model.Models;
using BookShopWithAuthen.Service.Services;
using BookShopWithAuthen.Web.ViewModel;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BookShopWithAuthen.Web.Controllers
{
    [Authorize(Roles ="User")]
    public class OrdersController : Controller
    {
        private IOrderService orderService;
        private ICartService cartService;
        private IBookService bookService;

        public OrdersController(IOrderService orderService, ICartService cartService, IBookService bookService)
        {
            this.orderService = orderService;
            this.cartService = cartService;
            this.bookService = bookService;
        }

        // GET: Orders
        public ActionResult Index()
        {
            var orders = orderService.GetAll();
            return View(orders);
        }

        // GET: Orders/Details/5
        public ActionResult Details(int id)
        {
            Order order = orderService.GetByID(id);
            if (!order.UserId.Equals(User.Identity.GetUserId()))
            {
                return HttpNotFound();
            }
            ICollection<OrderDetailViewModel> orderDetailViewModels = new List<OrderDetailViewModel>();
            var orderDetails = order.OrderDetails;
            foreach (var item in orderDetails)
            {
                OrderDetailViewModel tmpOrDeViewModel = Mapper.Map<OrderDetailViewModel>(item);
                orderDetailViewModels.Add(tmpOrDeViewModel);
            }
            ViewBag.orderDetailViewModels = orderDetailViewModels;
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        [HttpPost]
        public ActionResult Cancel(int id)
        {
            string userId = User.Identity.GetUserId();
            Order order = orderService.GetByID(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            else
            {
                orderService.ChangeStatus(id, (int)StatusOrder.Canceled);
                foreach (var item in order.OrderDetails)
                {
                    int wareHouseQuantity = (int)bookService.GetByID(item.BookID).Quantity;
                    bookService.UpdateQuantityBook(item.BookID, wareHouseQuantity + item.Quantity);

                }
            }
            return RedirectToAction("Index");
        }









    }
}
