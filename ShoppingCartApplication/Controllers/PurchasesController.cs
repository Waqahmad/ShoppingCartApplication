using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShoppingCartApplication.Models;

namespace ShoppingCartApplication.Controllers
{
    public class PurchasesController : Controller
    {
        private ShopingCartDBEntities db = new ShopingCartDBEntities();

        // GET: Purchases
        public async Task<ActionResult> Index()
        {
         
            var purchases = db.Purchases.Include(p => p.Product).Include(p => p.Vendor);
            return View(await purchases.ToListAsync());
        }

        // GET: Purchases/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchase purchase = await db.Purchases.FindAsync(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            return View(purchase);
        }

        // GET: Purchases/Create
        public ActionResult Create()
        {
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name");
            ViewBag.VendorID = new SelectList(db.Vendors, "ID", "Name");
            return View();
        }

        // POST: Purchases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,VendorID,ProductID,Purchase_Qty,Price,Createdon,CreatedBy")] Purchase purchase)
        {
            if (ModelState.IsValid)
            {
                using (var dbcontextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        
                        db.Purchases.Add(purchase);
                        await db.SaveChangesAsync();
                       // db.News_Entries.OrderByDescending(n => n.News_Date).Take(4).ToList()

                    var id= db.Purchases.OrderByDescending(n => n.ID).Take(1).FirstOrDefault().ID;
                        
                          
                        Stock stk = new Stock
                        {
                            ProductID = purchase.ProductID,
                            PurchaseID = purchase.ID,
                            StockQty = purchase.Purchase_Qty,
                            CreatedBy = purchase.CreatedBy,
                            Createdon = DateTime.Now
                        };

                        db.Stocks.Add(stk);
                        await db.SaveChangesAsync();
                        dbcontextTransaction.Commit();
                        return RedirectToAction("Index");
                    }
                    catch (Exception)
                    {
                        dbcontextTransaction.Rollback();
                        throw;
                    }
                 
                    
                }
               

            }

            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", purchase.ProductID);
            ViewBag.VendorID = new SelectList(db.Vendors, "ID", "Name", purchase.VendorID);
            return View(purchase);
        }

        // GET: Purchases/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchase purchase = await db.Purchases.FindAsync(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", purchase.ProductID);
            ViewBag.VendorID = new SelectList(db.Vendors, "ID", "Name", purchase.VendorID);
            return View(purchase);
        }

        // POST: Purchases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,VendorID,ProductID,Purchase_Qty,Price,Createdon,CreatedBy")] Purchase purchase)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchase).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", purchase.ProductID);
            ViewBag.VendorID = new SelectList(db.Vendors, "ID", "Name", purchase.VendorID);
            return View(purchase);
        }

        // GET: Purchases/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchase purchase = await db.Purchases.FindAsync(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            return View(purchase);
        }

        // POST: Purchases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Purchase purchase = await db.Purchases.FindAsync(id);
            db.Purchases.Remove(purchase);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
