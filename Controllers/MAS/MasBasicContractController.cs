using RentCar.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.UI;

namespace RentCar.Controllers
{
    public class MasBasicContractController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();
        public static string TaskCode;
        // GET: MasBasicContract
        public ActionResult Index()
        {
            var cR_Mas_Basic_Contract = db.CR_Mas_Basic_Contract.Where(x => x.CR_Mas_Basic_Contract_Code == "18").Include(c => c.CR_Mas_Com_Lessor).Include(c => c.CR_Mas_Sup_Sector);
            TaskCode = "1401";
            Session["POS"] = "1401";
            if (Session["ST_1401_unhold"].ToString() != "true" || Session["ST_1401_hold"].ToString() != "true" && Session["ST_1401_undelete"].ToString() != "true" || Session["ST_1401_delete"].ToString() != "true")
            {
                var contract = from CR_Mas_Basic_Contract in db.CR_Mas_Basic_Contract
                               where CR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Status != "H" && CR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Status != "D"
                               select CR_Mas_Basic_Contract;
                return View(contract);
            }
            else
                if (Session["ST_1401_unhold"].ToString() != "true" || Session["ST_1401_hold"].ToString() != "true")
            {
                var contract = (List<CR_Mas_Basic_Contract>)db.CR_Mas_Basic_Contract.Where(x => x.CR_Mas_Basic_Contract_Status != "H");
                return View(contract);
            }
            else if (Session["ST_1401_undelete"].ToString() != "true" || Session["ST_1401_delete"].ToString() != "true")
            {
                var contract = db.CR_Mas_Basic_Contract.Where(x => x.CR_Mas_Basic_Contract_Status != "D");
                return View(contract);
            }
            else
            {
                return View(cR_Mas_Basic_Contract.ToList());
            }
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult Index_Post(String lang, String excelCall)
        {
            if (!string.IsNullOrEmpty(lang))
            {
                if (Session["Language"].ToString() == "1")
                {
                    Session["Language"] = "2";
                    Session["Lang"] = "Arabic";
                }
                else
                {
                    if (Session["Language"].ToString() == "2")
                    {
                        Session["Language"] = "1";
                        Session["Lang"] = "English";
                    }
                }
            }

            if (!string.IsNullOrEmpty(excelCall))
            {
                var Service = new System.Data.DataTable("teste");

                Service.Columns.Add("الملاحظات", typeof(string));
                Service.Columns.Add("النوع", typeof(string));
                Service.Columns.Add("الإسم", typeof(string));
                Service.Columns.Add("الرمز", typeof(string));
                var Lrecord = db.CR_Mas_Sup_Virtual_Inspection.ToList();
                //if (Lrecord != null)
                //{
                //    foreach (var i in Lrecord)
                //    {
                //        Service.Rows.Add(i.CR_Mas_Sup_Virtual_Inspection_Reasons, i.CR_Mas_Sup_Virtual_Inspection_Type, i.CR_Mas_Sup_Virtual_Inspection_Ar_Name,
                //                            i.CR_Mas_Sup_Virtual_Inspection_Code);
                //    }
                //}
                var grid = new System.Web.UI.WebControls.GridView();
                grid.DataSource = Service;
                grid.DataBind();

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=Contract.xls");
                Response.ContentType = "application/ms-excel";

                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                grid.RenderControl(htw);

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            return View(db.CR_Mas_Sup_Brand.ToList());
        }



        public CR_Cas_Administrative_Procedures GetLastRecord(string Lessorcode, string ProcedureCode)
        {
            DateTime year = DateTime.Now;
            var y = year.ToString("yy");
            var Lrecord = db.CR_Cas_Administrative_Procedures.Where(x => x.CR_Cas_Administrative_Procedures_Lessor == Lessorcode
                && x.CR_Cas_Administrative_Procedures_Code == ProcedureCode && x.CR_Cas_Administrative_Procedures_Year == y)
                .Max(lr => lr.CR_Cas_Administrative_Procedures_No.Substring(lr.CR_Cas_Administrative_Procedures_No.Length - 7, 7));



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



        // GET: MasBasicContract/Create
        public ActionResult Create()
        {
            DateTime year = DateTime.Now;
            var y = year.ToString("yy");
            var sector = "1";
            var LessorCode = Session["LessorCode"].ToString();
            var autoInc = GetLastRecord(LessorCode, "18");

            CR_Mas_Basic_Contract BC = new CR_Mas_Basic_Contract();
            BC.CR_Mas_Basic_Contract_No = y + "-" + sector + "-" + "18" + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No;
            BC.CR_Mas_Basic_Contract_Code = "18";
            BC.CR_Mas_Basic_Contract_Year = y;
            BC.CR_Mas_Basic_Contract_Sector = "1";
            BC.CR_Mas_Basic_Contract_Com_Code = int.Parse(LessorCode);
            BC.CR_Mas_Basic_Contract_Status = "A";

            ViewBag.DocDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            ViewBag.StartDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            ViewBag.EndDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);

            ViewBag.CR_Mas_Basic_Contract_Lessor = new SelectList(db.CR_Mas_Com_Lessor.Where(x => x.CR_Mas_Com_Lessor_Sector != "9"), "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Ar_Short_Name");
            ViewBag.CR_Mas_Basic_Contract_Sector = new SelectList(db.CR_Mas_Sup_Sector, "CR_Mas_Sup_Sector_Code", "CR_Mas_Sup_Sector_Ar_Name");
            return View(BC);
        }

        // POST: MasBasicContract/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Mas_Basic_Contract_No,CR_Mas_Basic_Contract_Year," +
            "CR_Mas_Basic_Contract_Sector,CR_Mas_Basic_Contract_Code,CR_Mas_Basic_Contract_Lessor,CR_Mas_Basic_Contract_Com_Code," +
            "CR_Mas_Basic_Contract_Date,CR_Mas_Basic_Contract_Start_Date,CR_Mas_Basic_Contract_End_Date,CR_Mas_Basic_Contract_Annual_Fees," +
            "CR_Mas_Basic_Contract_Service_Fees,CR_Mas_Basic_Contract_Discount_Rate,CR_Mas_Basic_Contract_Tax_Rate," +
            "CR_Mas_Basic_Contract_Tamm_User_Id,CR_Mas_Basic_Contract_Tamm_User_PassWord,CR_Mas_Basic_Contract_Status,CR_Mas_Basic_Contract_Reasons")]
            CR_Mas_Basic_Contract cR_Mas_Basic_Contract)
        {

            var CheckExistContract = db.CR_Mas_Basic_Contract.FirstOrDefault(x => x.CR_Mas_Basic_Contract_Com_Code == 1000
            && x.CR_Mas_Basic_Contract_Lessor == cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Lessor && x.CR_Mas_Basic_Contract_Status != "D");


            if (ModelState.IsValid)
            {

                if (cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Date >= Convert.ToDateTime(DateTime.Now.ToShortDateString()) && cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Start_Date >= cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Date
                    && cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_End_Date >= cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Start_Date && cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Date != null &&
                    cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Start_Date != null && cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_End_Date != null && CheckExistContract == null)
                {
                    var LessorCode = Session["LessorCode"].ToString();
                    ///////////////////////////////Tracing//////////////////////////////////////
                    CR_Cas_Administrative_Procedures Ad = new CR_Cas_Administrative_Procedures();
                    DateTime year = DateTime.Now;
                    var y = year.ToString("yy");
                    var sector = "1";
                    var autoInc = GetLastRecord(LessorCode, "18");

                    Ad.CR_Cas_Administrative_Procedures_No = y + "-" + sector + "-" + "18" + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No;
                    Ad.CR_Cas_Administrative_Procedures_Date = DateTime.Now;
                    string currentTime = DateTime.Now.ToString("HH:mm:ss");
                    Ad.CR_Cas_Administrative_Procedures_Time = TimeSpan.Parse(currentTime);
                    Ad.CR_Cas_Administrative_Procedures_Year = y;
                    Ad.CR_Cas_Administrative_Procedures_Sector = sector;
                    Ad.CR_Cas_Administrative_Procedures_Code = "18";
                    Ad.CR_Cas_Administrative_Procedures_Lessor = LessorCode;
                    Ad.CR_Cas_Administrative_Procedures_Targeted_Action = cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Lessor;
                    Ad.CR_Cas_Administrative_Procedures_User_Insert = Session["UserLogin"].ToString();
                    Ad.CR_Cas_Administrative_Procedures_Type = "I";
                    Ad.CR_Cas_Administrative_Procedures_Action = true;
                    Ad.CR_Cas_Administrative_Procedures_Doc_Date = cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Date;
                    Ad.CR_Cas_Administrative_Procedures_Doc_Start_Date = cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Start_Date;
                    Ad.CR_Cas_Administrative_Procedures_Doc_End_Date = cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_End_Date;
                    Ad.CR_Cas_Administrative_Procedures_Doc_No = "";
                    db.CR_Cas_Administrative_Procedures.Add(Ad);

                    /////////////////////////////////////////////Save contract////////////////////////////////
                    //cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Date = cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Date;
                    //cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Start_Date = cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Start_Date;
                    //cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_End_Date = cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_End_Date;
                    //cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_No = y + "-" + sector + "-" + "18" + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No;
                    //cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Year = y;
                    //cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Sector = "1";//////////////////////////check/////////////////////
                    //cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Code = "18";
                    //cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Lessor = LessorCode;
                    //cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Com_Code = cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Com_Code;//////////////////////////Check////////////////////////
                    //cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Status = "A";
                    var x = cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Annual_Fees * (1 - cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Discount_Rate / 100);
                    cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Annual_Fees = x * (1 + cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Tax_Rate / 100);
                    db.CR_Mas_Basic_Contract.Add(cR_Mas_Basic_Contract);
                    /////////////////////////////////////////////////////////////////////////////////////////////
                    ///////////////////////////////////////////Update branch documentation//////////////////
                    var doc = db.CR_Cas_Sup_Branch_Documentation.FirstOrDefault(d => d.CR_Cas_Sup_Branch_Documentation_Lessor_Code == cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Lessor
                    && d.CR_Cas_Sup_Branch_Documentation_Code == "18");
                    if (doc != null)
                    {
                        doc.CR_Cas_Sup_Branch_Documentation_No = Ad.CR_Cas_Administrative_Procedures_No;
                        doc.CR_Cas_Sup_Branch_Documentation_Date = cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Date;
                        doc.CR_Cas_Sup_Branch_Documentation_Start_Date = cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Start_Date;
                        doc.CR_Cas_Sup_Branch_Documentation_End_Date = cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_End_Date;


                        var mech = db.CR_Cas_Sup_Follow_Up_Mechanism.FirstOrDefault(m => m.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code == cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Lessor &&
                        m.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code == "18");
                        DateTime currentDate = (DateTime)doc.CR_Cas_Sup_Branch_Documentation_End_Date;
                        var nbr = Convert.ToDouble(mech.CR_Cas_Sup_Follow_Up_Mechanism_Befor_Expire);
                        var d = currentDate.AddDays(-nbr);
                        doc.CR_Cas_Sup_Branch_Documentation_About_To_Expire = d;


                        doc.CR_Cas_Sup_Branch_Documentation_Status = "A";
                    }

                    ////////////////////////////////////////////////////////////////////////////////////////
                    db.SaveChanges();
                    TempData["TempModel"] = "تم الحفظ بنجاح";
                    return RedirectToAction("Create", "MasBasicContract");
                }
                else
                {
                    if (cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_End_Date < DateTime.Now)
                    {
                        ViewBag.DocEndDateError = "صلاحية الوثيقة منتهية";
                    }

                    if (cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Date > DateTime.Now)
                    {
                        ViewBag.DocDateError = "الرجاء التأكد من تاريخ المستند";
                    }
                    if (cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Start_Date < cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Date)
                    {
                        ViewBag.DocStartDateError = "تأكد من التاريخ";
                    }

                    if (cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Date == null)
                    {
                        ViewBag.DocDateError = "الرجاء إختيار التاريخ";
                    }

                    if (cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Start_Date == null)
                    {
                        ViewBag.DocStartDateError = "الرجاء إختيار التاريخ";
                    }

                    if (cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_End_Date == null)
                    {
                        ViewBag.DocEndDateError = "الرجاء إختيار التاريخ";
                    }
                    if (CheckExistContract != null)
                    {
                        TempData["TempModel"] = "هنالك عقد ساري المفعول لهذه الشركة";
                    }

                    ViewBag.EndDate = string.Format("{0:yyyy-MM-dd}", cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_End_Date);
                    ViewBag.StartDate = string.Format("{0:yyyy-MM-dd}", cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Start_Date);
                    ViewBag.DocDate = string.Format("{0:yyyy-MM-dd}", cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Date);
                }


            }


            ViewBag.CR_Mas_Basic_Contract_Lessor = new SelectList(db.CR_Mas_Com_Lessor.Where(x => x.CR_Mas_Com_Lessor_Sector != "9"), "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Ar_Short_Name", cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Lessor);
            ViewBag.CR_Mas_Basic_Contract_Sector = new SelectList(db.CR_Mas_Sup_Sector, "CR_Mas_Sup_Sector_Code", "CR_Mas_Sup_Sector_Ar_Name", cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Sector);
            return View(cR_Mas_Basic_Contract);
        }

        // GET: MasBasicContract/Edit/5
        public ActionResult Edit(string id1)
        {
            if (id1 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Mas_Basic_Contract cR_Mas_Basic_Contract = db.CR_Mas_Basic_Contract.Find(id1);
            if (cR_Mas_Basic_Contract == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Date == null)
                {
                    ViewBag.DocDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                }
                else
                {
                    ViewBag.DocDate = string.Format("{0:yyyy-MM-dd}", cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Date);
                }

                if (cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Start_Date == null)
                {
                    ViewBag.StartDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                }
                else
                {
                    ViewBag.StartDate = string.Format("{0:yyyy-MM-dd}", cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Start_Date);
                }

                if (cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_End_Date == null)
                {
                    ViewBag.EndDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                }
                else
                {
                    ViewBag.EndDate = string.Format("{0:yyyy-MM-dd}", cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_End_Date);
                }
                ViewBag.Status = cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Status;
                ViewBag.ContractCode = cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_No.Trim();

                if (cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Status == "A" || cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Status == "1")
                {
                    ViewBag.stat = "حذف";
                    ViewBag.h = "إيقاف";
                }

                if ((cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Status == "D" || cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Status == "0"))
                {
                    ViewBag.stat = "إسترجاع";
                    ViewBag.h = "إيقاف";
                    ViewData["ReadOnly"] = "true";
                }

                if (cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Status == "H" || cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Status == "2")
                {
                    ViewBag.h = "تنشيط";
                    ViewBag.stat = "حذف";
                    ViewData["ReadOnly"] = "true";
                }

                if (cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Status == null)
                {
                    ViewBag.h = "إيقاف";
                    ViewBag.stat = "حذف";
                }
                ViewBag.delete = cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Status;
            }
            ViewBag.CR_Mas_Basic_Contract_Lessor = new SelectList(db.CR_Mas_Com_Lessor.Where(x => x.CR_Mas_Com_Lessor_Sector != "9"), "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Ar_Short_Name", cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Lessor);
            ViewBag.CR_Mas_Basic_Contract_Sector = new SelectList(db.CR_Mas_Sup_Sector, "CR_Mas_Sup_Sector_Code", "CR_Mas_Sup_Sector_Ar_Name", cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Sector);
            return View(cR_Mas_Basic_Contract);
        }

        // POST: MasBasicContract/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Mas_Basic_Contract_No,CR_Mas_Basic_Contract_Year," +
            "CR_Mas_Basic_Contract_Sector,CR_Mas_Basic_Contract_Code,CR_Mas_Basic_Contract_Lessor,CR_Mas_Basic_Contract_Com_Code," +
            "CR_Mas_Basic_Contract_Date,CR_Mas_Basic_Contract_Start_Date,CR_Mas_Basic_Contract_End_Date,CR_Mas_Basic_Contract_Annual_Fees," +
            "CR_Mas_Basic_Contract_Service_Fees,CR_Mas_Basic_Contract_Discount_Rate,CR_Mas_Basic_Contract_Tax_Rate,CR_Mas_Basic_Contract_Tamm_User_Id," +
            "CR_Mas_Basic_Contract_Tamm_User_PassWord,CR_Mas_Basic_Contract_Status,CR_Mas_Basic_Contract_Reasons")]
            CR_Mas_Basic_Contract cR_Mas_Basic_Contract, string save, string delete, string hold)
        {
            if (!string.IsNullOrEmpty(save))
            {
                if (ModelState.IsValid)
                {
                    db.Entry(cR_Mas_Basic_Contract).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            if (delete == "Delete" || delete == "حذف")
            {
                cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Status = "D";
                db.Entry(cR_Mas_Basic_Contract).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            if (delete == "Activate" || delete == "إسترجاع")
            {
                cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Status = "A";
                db.Entry(cR_Mas_Basic_Contract).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (hold == "إيقاف" || hold == "hold")
            {
                cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Status = "H";
                db.Entry(cR_Mas_Basic_Contract).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (hold == "تنشيط" || hold == "Activate")
            {
                cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Status = "A";
                db.Entry(cR_Mas_Basic_Contract).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Status == "A" ||
            cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Status == "Activated" ||
            cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Status == "1" ||
            cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Status == "Undeleted")
            {
                ViewBag.stat = "حذف";
                ViewBag.h = "إيقاف";
            }
            if ((cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Status == "D" ||
                 cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Status == "Deleted" ||
                 cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Status == "0"))
            {
                ViewBag.stat = "إسترجاع";
                ViewBag.h = "إيقاف";
            }
            if (cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Status == "H" ||
                cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Status == "Hold" ||
                cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Status == "2")
            {
                ViewBag.h = "تنشيط";
                ViewBag.stat = "حذف";
            }
            if (cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Status == null)
            {
                ViewBag.h = "إيقاف";
                ViewBag.stat = "حذف";
            }
            ViewBag.CR_Mas_Basic_Contract_Lessor = new SelectList(db.CR_Mas_Com_Lessor.Where(x => x.CR_Mas_Com_Lessor_Sector != "9"), "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Ar_Short_Name", cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Lessor);
            ViewBag.CR_Mas_Basic_Contract_Sector = new SelectList(db.CR_Mas_Sup_Sector, "CR_Mas_Sup_Sector_Code", "CR_Mas_Sup_Sector_Ar_Name", cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Sector);

            return View(cR_Mas_Basic_Contract);
        }

        // GET: MasBasicContract/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Mas_Basic_Contract cR_Mas_Basic_Contract = db.CR_Mas_Basic_Contract.Find(id);
            if (cR_Mas_Basic_Contract == null)
            {
                return HttpNotFound();
            }
            return View(cR_Mas_Basic_Contract);
        }

        // POST: MasBasicContract/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CR_Mas_Basic_Contract cR_Mas_Basic_Contract = db.CR_Mas_Basic_Contract.Find(id);
            db.CR_Mas_Basic_Contract.Remove(cR_Mas_Basic_Contract);
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
