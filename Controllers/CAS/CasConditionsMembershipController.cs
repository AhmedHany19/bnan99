using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RentCar.Models;

namespace RentCar.Controllers.CAS
{
    public class CasConditionsMembershipController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: CasConditionsMembership
        public ActionResult Index()
        {
            var LessorCode = "";
            var UserLogin = "";
            try
            {
                LessorCode = Session["LessorCode"].ToString();
                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                if (UserLogin == null || LessorCode == null)
                {
                    RedirectToAction("Account", "Login");
                }
            }
            catch
            {
                return RedirectToAction("Login", "Account");
            }
            var CR_Cas_Sup_Membership_Conditions = db.CR_Cas_Sup_Membership_Conditions.Where(x=>x.CR_Cas_Sup_Membership_Conditions_Lessor_Code==LessorCode 
            && x.CR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Status!="D")
                .Include(c => c.CR_Mas_Com_Lessor);

            

            //var cond2 = new SelectList(new[] {
            //                                  new{ID=1,Name="و"},
            //                                  new{ID=2,Name="أو"},
            //                                  new{ID=3,Name="بدون"},
            //                                  }, "ID", "Name");
            //ViewData["Conditions2"] = cond2;

            
            return View(CR_Cas_Sup_Membership_Conditions.ToList());
        }


        public JsonResult GetMembershipGroup(string No)
        {
            var LessorCode = "";
            var UserLogin = "";
            var BranchCode = "";
            try
            {
                LessorCode = Session["LessorCode"].ToString();
                BranchCode = Session["BranchCode"].ToString();

                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                if (UserLogin == null || LessorCode == null || BranchCode == null)
                {
                    RedirectToAction("Account", "Login");
                }
            }
            catch
            {
                RedirectToAction("Login", "Account");
            }
            var group = db.CR_Mas_Sup_Membership_Group.FirstOrDefault(g=>g.CR_Mas_Sup_Membership_Group_Code==No);
            if (group != null)
            {
                return Json(group.CR_Mas_Sup_Membership_Group_Id, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(" ", JsonRequestBehavior.AllowGet);
            }
            
        }

        // GET: CasConditionsMembership/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Sup_Membership_Conditions CR_Cas_Sup_Membership_Conditions = db.CR_Cas_Sup_Membership_Conditions.Find(id);
            if (CR_Cas_Sup_Membership_Conditions == null)
            {
                return HttpNotFound();
            }
            return View(CR_Cas_Sup_Membership_Conditions);
        }
        public CR_Cas_Administrative_Procedures GetLastRecordTracing(string ProcedureCode, string sector)
        {

            DateTime year = DateTime.Now;
            var y = year.ToString("yy");
            var LessorCode = Session["LessorCode"].ToString();
            var Lrecord = db.CR_Cas_Administrative_Procedures.Where(x => x.CR_Cas_Administrative_Procedures_Lessor == LessorCode &&
                x.CR_Cas_Administrative_Procedures_Code == ProcedureCode
                && x.CR_Cas_Administrative_Procedures_Sector == sector
                && x.CR_Cas_Administrative_Procedures_Year == y)
                .Max(x => x.CR_Cas_Administrative_Procedures_No.Substring(x.CR_Cas_Administrative_Procedures_No.Length - 7, 7));

            CR_Cas_Administrative_Procedures T = new CR_Cas_Administrative_Procedures();
            if (Lrecord != null)
            {
                Int64 val = Int64.Parse(Lrecord) + 1;
                T.CR_Cas_Administrative_Procedures_No = val.ToString("0000000");
            }
            else
            {
                T.CR_Cas_Administrative_Procedures_No = "0000001";
            }




            return T;
        }


        private void SaveTracing(string LessorCode, string ProcedureType)
        {
            CR_Cas_Administrative_Procedures Ad = new CR_Cas_Administrative_Procedures();
            DateTime year = DateTime.Now;
            var y = year.ToString("yy");
            var sector = "1";
            var ProcedureCode = "55";
            var autoInc = GetLastRecordTracing(ProcedureCode, "1");
            Ad.CR_Cas_Administrative_Procedures_No = y + "-" + sector + "-" + ProcedureCode + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No;
            Ad.CR_Cas_Administrative_Procedures_Date = DateTime.Now;
            string currentTime = DateTime.Now.ToString("HH:mm:ss");
            Ad.CR_Cas_Administrative_Procedures_Time = TimeSpan.Parse(currentTime);
            Ad.CR_Cas_Administrative_Procedures_Year = y;
            Ad.CR_Cas_Administrative_Procedures_Sector = sector;
            Ad.CR_Cas_Administrative_Procedures_Code = ProcedureCode;
            Ad.CR_Cas_Administrative_Int_Procedures_Code = int.Parse(ProcedureCode);
            Ad.CR_Cas_Administrative_Procedures_Lessor = LessorCode;
            Ad.CR_Cas_Administrative_Procedures_Targeted_Action = null;
            Ad.CR_Cas_Administrative_Procedures_User_Insert = Session["UserLogin"].ToString();
            Ad.CR_Cas_Administrative_Procedures_Type = ProcedureType;
            Ad.CR_Cas_Administrative_Procedures_Action = true;
            Ad.CR_Cas_Administrative_Procedures_Doc_Date = null;
            Ad.CR_Cas_Administrative_Procedures_Doc_Start_Date = null;
            Ad.CR_Cas_Administrative_Procedures_Doc_End_Date = null;
            Ad.CR_Cas_Administrative_Procedures_Doc_No = "";
            
            db.CR_Cas_Administrative_Procedures.Add(Ad);
        }




        // GET: CasConditionsMembership/Create
        public ActionResult Create()
        {
            ViewBag.CR_Cas_Sup_Membership_Conditions_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo");
            return View();
        }

        // POST: CasConditionsMembership/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Cas_Sup_Membership_Conditions_Lessor_Code," +
            "CR_Cas_Sup_Membership_Conditions_Code,CR_Cas_Sup_Membership_Conditions_Insert,CR_Cas_Sup_Membership_Conditions_KM," +
            "CR_Cas_Sup_Membership_Conditions_Link_1,CR_Cas_Sup_Membership_Conditions_Amount,CR_Cas_Sup_Membership_Conditions_Link_2," +
            "CR_Cas_Sup_Membership_Conditions_Contract_No,CR_Cas_Sup_Membership_Conditions_Link_3," +
            "CR_Cas_Sup_Membership_Conditions_Day_No")] CR_Cas_Sup_Membership_Conditions CR_Cas_Sup_Membership_Conditions,FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                var LessorCode = "";
                var UserLogin = "";
                var BranchCode = "";
                try
                {
                    LessorCode = Session["LessorCode"].ToString();
                    BranchCode = Session["BranchCode"].ToString();

                    UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                    if (UserLogin == null || LessorCode == null || BranchCode == null)
                    {
                        RedirectToAction("Account", "Login");
                    }
                }
                catch
                {
                    RedirectToAction("Login", "Account");
                }
                //using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                //{
                //    try
                //    {
                ///////////////////////////////Tracing//////////////////////////////////////
                SaveTracing(LessorCode, "U");
                foreach (string item in collection.AllKeys)
                        {
                            if (item.StartsWith("CR_Cas_Sup_Membership_Conditions_Amount-"))
                            {
                                var code = item.Replace("CR_Cas_Sup_Membership_Conditions_Amount-", "");
                                if (code != null && code != "")
                                {
                                    var path = "";
                                    var imgpath = "";
                                    var Amount = collection["CR_Cas_Sup_Membership_Conditions_Amount-" + code];
                                    var cond1 = collection["Conditions1_" + code];
                                    var KM = collection["CR_Cas_Sup_Membership_Conditions_KM-" + code];
                                    var cond2 = collection["Conditions2_" + code];
                                    var NoContract = collection["CR_Cas_Sup_Membership_Conditions_Contract_No-" + code];
                                    var result = collection["result-" + code];
                                    var Group = collection["Group-" + code];
                                    var ConditionInsert = collection["CR_Cas_Sup_Membership_Conditions_Insert-" + code];
                                    var insert = false;
                                    if (ConditionInsert == "on")
                                    {
                                        insert = true;
                                    }

                                    if (Request.Files.Count > 0)
                                    {
                                        HttpPostedFileBase postedFile = Request.Files["Img-" + code];

                                        path = Server.MapPath(string.Format("~/{0}/", "~/images/Company/"+ LessorCode +"/Membership/" + code + "/"));

                                        if (!Directory.Exists(path))
                                        {
                                            Directory.CreateDirectory(path);
                                        }
                                        if (postedFile.ContentLength > 0)
                                        {

                                            imgpath  = "~/images/Company/"+LessorCode+"/Membership/" + code + "/" + Path.GetFileName(postedFile.FileName);
                                            postedFile.SaveAs(HttpContext.Server.MapPath(imgpath));
                                        }
                                    }
                                    var MembershipCond = db.CR_Cas_Sup_Membership_Conditions.FirstOrDefault(c => c.CR_Cas_Sup_Membership_Conditions_Code == code
                                    && c.CR_Cas_Sup_Membership_Conditions_Lessor_Code == LessorCode);
                                    MembershipCond.CR_Cas_Sup_Membership_Conditions_Amount = decimal.Parse(Amount);
                                    MembershipCond.CR_Cas_Sup_Membership_Conditions_Link_1 = int.Parse(cond1);
                                    MembershipCond.CR_Cas_Sup_Membership_Conditions_KM = int.Parse(KM);
                                    MembershipCond.CR_Cas_Sup_Membership_Conditions_Link_2 = int.Parse(cond2);
                                    MembershipCond.CR_Cas_Sup_Membership_Conditions_Contract_No = int.Parse(NoContract);
                                    MembershipCond.CR_Cas_Sup_Membership_Conditions_Insert_Code = result;
                                    MembershipCond.CR_Cas_Sup_Membership_Conditions_Insert_Group = Group;
                                    MembershipCond.CR_Cas_Sup_Membership_Conditions_Insert = insert;
                                    if(imgpath!=null && imgpath != "")
                                    {
                                        MembershipCond.CR_Cas_Sup_Membership_Conditions_Picture = imgpath;
                                    }
                                    
                                    db.Entry(MembershipCond).State = EntityState.Modified;


                                    
                                    ////////////////////////////////////Documentation////////////////////////////////////////
                                    db.SaveChanges();
                                    //dbTran.Commit();
                                    TempData["TempModif"] = "Saved";
                                }
                            }
                        }
                    //}
                    //catch (DbEntityValidationException ex)
                    //{
                    //    dbTran.Rollback();
                    //    throw ex;
                    //}
                //}
                return RedirectToAction("Index");
            }

            ViewBag.CR_Cas_Sup_Membership_Conditions_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo", CR_Cas_Sup_Membership_Conditions.CR_Cas_Sup_Membership_Conditions_Lessor_Code);
            return View(CR_Cas_Sup_Membership_Conditions);
        }

        // GET: CasConditionsMembership/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Sup_Membership_Conditions CR_Cas_Sup_Membership_Conditions = db.CR_Cas_Sup_Membership_Conditions.Find(id);
            if (CR_Cas_Sup_Membership_Conditions == null)
            {
                return HttpNotFound();
            }
            ViewBag.CR_Cas_Sup_Membership_Conditions_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo", CR_Cas_Sup_Membership_Conditions.CR_Cas_Sup_Membership_Conditions_Lessor_Code);
            return View(CR_Cas_Sup_Membership_Conditions);
        }

        // POST: CasConditionsMembership/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Cas_Sup_Membership_Conditions_Lessor_Code,CR_Cas_Sup_Membership_Conditions_Code,CR_Cas_Sup_Membership_Conditions_Insert,CR_Cas_Sup_Membership_Conditions_KM,CR_Cas_Sup_Membership_Conditions_Link_1,CR_Cas_Sup_Membership_Conditions_Amount,CR_Cas_Sup_Membership_Conditions_Link_2,CR_Cas_Sup_Membership_Conditions_Contract_No,CR_Cas_Sup_Membership_Conditions_Link_3,CR_Cas_Sup_Membership_Conditions_Day_No")] CR_Cas_Sup_Membership_Conditions CR_Cas_Sup_Membership_Conditions)
        {
            if (ModelState.IsValid)
            {
                db.Entry(CR_Cas_Sup_Membership_Conditions).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CR_Cas_Sup_Membership_Conditions_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo", CR_Cas_Sup_Membership_Conditions.CR_Cas_Sup_Membership_Conditions_Lessor_Code);
            return View(CR_Cas_Sup_Membership_Conditions);
        }

        // GET: CasConditionsMembership/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Sup_Membership_Conditions CR_Cas_Sup_Membership_Conditions = db.CR_Cas_Sup_Membership_Conditions.Find(id);
            if (CR_Cas_Sup_Membership_Conditions == null)
            {
                return HttpNotFound();
            }
            return View(CR_Cas_Sup_Membership_Conditions);
        }

        // POST: CasConditionsMembership/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CR_Cas_Sup_Membership_Conditions CR_Cas_Sup_Membership_Conditions = db.CR_Cas_Sup_Membership_Conditions.Find(id);
            db.CR_Cas_Sup_Membership_Conditions.Remove(CR_Cas_Sup_Membership_Conditions);
            db.SaveChanges();
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
