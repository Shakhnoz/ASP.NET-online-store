using DAL.Entities;
using DAL.Repository;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace WAD.Controllers
{
    public class ProductsController : Controller
    {


        private ProductRepository _productRepo = new ProductRepository();
        private CategoryRepository _categoryRepo = new CategoryRepository();
        private UserRepository _userRepo = new UserRepository();
        private static NLog.Logger logger = NLog.LogManager.GetLogger("WebSite");
        // GET: Products


        public ActionResult Products()
        {
            var products = _productRepo.GetAll().Include(p => p.Category).Include(p => p.User).OrderByDescending(p => p.ProductID);
            return View(products.ToList());
        }



        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _productRepo.GetById(id.Value);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(_categoryRepo.GetAll(), "CategoryID", "Title");
            ViewBag.UserID = new SelectList(_userRepo.GetAll(), "UserID", "Username");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {


            product.Photo = "/Content/images/box.png";//initial value of photo

            if (ModelState.IsValid)
            {
                _productRepo.Create(product);

                if (product.PhotoFile != null)
                {
                    var folder = "/Content/images";//directory of the photos uploaded
                    var file = string.Format("{0}.png", product.ProductID);//name of photo matches given product's ID
                    var response = FileHelper.UploadFile.UploadPhoto(product.PhotoFile, folder, file);
                    if (response)
                    {
                        var picture = string.Format("{0}/{1}", folder, file);
                        product.Photo = picture;
                        _productRepo.Update(product);
                    }

                }

                if (product.Price != null)
                {
                    var priceCurrency = " sum";//"sum" suffix
                    product.Price = product.Price + priceCurrency;
                    _productRepo.Update(product);

                }




                return RedirectToAction("Products");
            }

            ViewBag.CategoryID = new SelectList(_categoryRepo.GetAll(), "CategoryID", "Title", product.CategoryID);
            ViewBag.UserID = new SelectList(_userRepo.GetAll(), "UserID", "Username");
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _productRepo.GetById(id.Value);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(_categoryRepo.GetAll(), "CategoryID", "Title", product.CategoryID);
            ViewBag.UserID = new SelectList(_userRepo.GetAll(), "UserID", "Username");
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                var folder = "/Content/images";
                var file = string.Format("{0}.png", product.ProductID);
                var response = FileHelper.UploadFile.UploadPhoto(product.PhotoFile, folder, file);
                if (response)
                {
                    var picture = string.Format("{0}/{1}", folder, file);
                    product.Photo = picture;
                }


                _productRepo.Update(product);
                return RedirectToAction("Products");
            }
            ViewBag.CategoryID = new SelectList(_categoryRepo.GetAll(), "CategoryID", "Title", product.CategoryID);
            ViewBag.UserID = new SelectList(_userRepo.GetAll(), "UserID", "Username");
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _productRepo.GetById(id.Value);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = _productRepo.GetById(id);
            User user = product.User;
            _productRepo.Delete(id);
            logger.Info("User '" + user.Username + "' deleted the product '" + product.Name + "' with ID " + product.ProductID);
            return RedirectToAction("Products");
        }

    }
}
