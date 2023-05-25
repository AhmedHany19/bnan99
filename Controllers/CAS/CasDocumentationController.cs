using RentCar.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.UI;

namespace RentCar.Controllers.CAS
{
    public class CasDocumentationController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: CasDocumentation

        [HttpGet]
        [ActionName("Index")]
        public ActionResult Index_Get()
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
            

            Models.CAS.LoadAlerts lAlerts = new Models.CAS.LoadAlerts();
            lAlerts.GetExpiredDocs(LessorCode);

            using (var ctx = new RentCarDBEntities())
            {
                var list = ctx.CR_Cas_Sup_Branch_Documentation
                                    .SqlQuery("select * from CR_Cas_Sup_Branch_Documentation as d,CR_Cas_Sup_Branch as b where d.CR_Cas_Sup_Branch_Documentation_Branch_Code =" +
                                    " b.CR_Cas_Sup_Branch_Code and d.CR_Cas_Sup_Procedures_Type = '1' " +
                                    "and d.CR_Cas_Sup_Branch_Documentation_Lessor_Code = '" + LessorCode + "' and b.CR_Cas_Sup_Lessor_Code = '" + LessorCode + "' " +
                                    "and b.CR_Cas_Sup_Branch_Status != 'D' and b.CR_Cas_Sup_Branch_Status != 'H' ")
                                    .ToList();
                return View(list);
            }

            //var cR_Cas_Sup_Branch_Documentation = db.CR_Cas_Sup_Branch_Documentation.Where(d => d.CR_Cas_Sup_Procedures_Type == "1" &&
            //d.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode && d.CR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Status != "D")
            //    .Include(c => c.CR_Mas_Sup_Procedures);
            //return View(cR_Cas_Sup_Branch_Documentation);
        }



        [HttpPost]
        [ActionName("Index")]
        public ActionResult Index_Post(String lang, String excelCall)
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
            //var LessorCode = Session["LessorCode"].ToString();
            if (!string.IsNullOrEmpty(excelCall))
            {
                var Doc = new System.Data.DataTable("teste");


                Doc.Columns.Add("نهايته", typeof(string));
                Doc.Columns.Add("بدايته", typeof(string));
                Doc.Columns.Add("التاريخ", typeof(string));
                Doc.Columns.Add("الرقم", typeof(string));
                Doc.Columns.Add("المستند", typeof(string));
                Doc.Columns.Add("الفرع", typeof(string));
                var cR_Cas_Sup_Branch_Documentation = db.CR_Cas_Sup_Branch_Documentation.Where(d => d.CR_Cas_Sup_Procedures_Type == "1" &&
                d.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode)
                .Include(c => c.CR_Mas_Sup_Procedures);


                if (cR_Cas_Sup_Branch_Documentation != null)
                {
                    foreach (var i in cR_Cas_Sup_Branch_Documentation)
                    {
                        Doc.Rows.Add(i.CR_Cas_Sup_Branch_Documentation_End_Date, i.CR_Cas_Sup_Branch_Documentation_Start_Date, i.CR_Cas_Sup_Branch_Documentation_Date,
                                            i.CR_Cas_Sup_Branch_Documentation_No, i.CR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Ar_Name,
                                            i.CR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Ar_Name);
                    }
                }
                var grid = new System.Web.UI.WebControls.GridView();
                grid.DataSource = Doc;
                grid.DataBind();

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=CarDocumentation.xls");
                Response.ContentType = "application/ms-excel";

                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                grid.RenderControl(htw);

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            //var LessorCode = Session["LessorCode"].ToString();
            var ListStart = new SelectList(new[] {
                                              new {ID="1",Name="السبت"},
                                              new{ID="2",Name="الأحد"},
                                              new{ID="3",Name="الإثنين"},
                                              new{ID="4",Name="الثلاثاء"},
                                              new{ID="5",Name="الإربعاء"},
                                              new{ID="6",Name="الخميس"},
                                              new{ID="7",Name="الجمعة"},
                                              }, "ID", "Name", 1);
            ViewData["listStart"] = ListStart;
            var ListEnd = new SelectList(new[] {
                                              new {ID="1",Name="السبت"},
                                              new{ID="2",Name="الأحد"},
                                              new{ID="3",Name="الإثنين"},
                                              new{ID="4",Name="الثلاثاء"},
                                              new{ID="5",Name="الإربعاء"},
                                              new{ID="6",Name="الخميس"},
                                              new{ID="7",Name="الجمعة"},
                                              }, "ID", "Name", 1);
            ViewData["listEnd"] = ListEnd;
            var cR_Cas_Sup_Branch = db.CR_Cas_Sup_Branch.Where(x => x.CR_Cas_Sup_Lessor_Code == LessorCode).Include(c => c.CR_Mas_Com_Lessor);
            return View(cR_Cas_Sup_Branch.ToList());
        }


        public PartialViewResult PartialIndex(string type)
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

            IQueryable<CR_Cas_Sup_Branch_Documentation> cR_Cas_Sup_Branch_Documentation = null;
            if(type=="A")
            {
                cR_Cas_Sup_Branch_Documentation = db.CR_Cas_Sup_Branch_Documentation.Where(d => d.CR_Cas_Sup_Procedures_Type == "1" &&
                d.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode && d.CR_Cas_Sup_Branch_Documentation_Status == "A")
               .Include(c => c.CR_Mas_Sup_Procedures);
            }  
            else if(type=="D")
            {
                cR_Cas_Sup_Branch_Documentation = db.CR_Cas_Sup_Branch_Documentation.Where(d => d.CR_Cas_Sup_Procedures_Type == "1" &&
                d.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode && d.CR_Cas_Sup_Branch_Documentation_Status == "D" )
               .Include(c => c.CR_Mas_Sup_Procedures);
            }
            else if (type == "U")
            {
                cR_Cas_Sup_Branch_Documentation = db.CR_Cas_Sup_Branch_Documentation.Where(d => d.CR_Cas_Sup_Procedures_Type == "1" &&
                d.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode && d.CR_Cas_Sup_Branch_Documentation_Status == "N" )
               .Include(c => c.CR_Mas_Sup_Procedures);
            }
            else if (type == "E")
            {
                cR_Cas_Sup_Branch_Documentation = db.CR_Cas_Sup_Branch_Documentation.Where(d => d.CR_Cas_Sup_Procedures_Type == "1" &&
                d.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode && d.CR_Cas_Sup_Branch_Documentation_Status == "E" )
               .Include(c => c.CR_Mas_Sup_Procedures);
            }
            else if (type == "X")
            {
                cR_Cas_Sup_Branch_Documentation = db.CR_Cas_Sup_Branch_Documentation.Where(d => d.CR_Cas_Sup_Procedures_Type == "1" &&
                d.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode && d.CR_Cas_Sup_Branch_Documentation_Status == "X")
               .Include(c => c.CR_Mas_Sup_Procedures);
            }
            else if (type == "All")
            {
                cR_Cas_Sup_Branch_Documentation = db.CR_Cas_Sup_Branch_Documentation.Where(d => d.CR_Cas_Sup_Procedures_Type == "1" &&
                d.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode )
               .Include(c => c.CR_Mas_Sup_Procedures);
            }
            else
            {
                cR_Cas_Sup_Branch_Documentation = db.CR_Cas_Sup_Branch_Documentation.Where(d => d.CR_Cas_Sup_Procedures_Type == "1" &&
                d.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode && (d.CR_Cas_Sup_Branch_Documentation_Status=="N" || d.CR_Cas_Sup_Branch_Documentation_Status=="E") )
               .Include(c => c.CR_Mas_Sup_Procedures);
            }
            
            
            return PartialView(cR_Cas_Sup_Branch_Documentation.ToList());
        }






            // GET: CasDocumentation/Create
            public ActionResult Create()
        {
            ViewBag.CR_Cas_Sup_Branch_Documentation_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo");
            return View();
        }

        // POST: CasDocumentation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Cas_Sup_Branch_Documentation_Lessor_Code,CR_Cas_Sup_Branch_Documentation_Branch_Code,CR_Cas_Sup_Branch_Documentation_Code,CR_Cas_Sup_Branch_Documentation_No,CR_Cas_Sup_Branch_Documentation_Date,CR_Cas_Sup_Branch_Documentation_Start_Date,CR_Cas_Sup_Branch_Documentation_End_Date,CR_Cas_Sup_Branch_Documentation_Activation,CR_Cas_Sup_Branch_Documentation_Credit_Limit,CR_Cas_Sup_Branch_Documentation_About_To_Expire,CR_Cas_Sup_Branch_Documentation_Status,CR_Cas_Sup_Procedures_Type")] CR_Cas_Sup_Branch_Documentation cR_Cas_Sup_Branch_Documentation)
        {
            if (ModelState.IsValid)
            {
                db.CR_Cas_Sup_Branch_Documentation.Add(cR_Cas_Sup_Branch_Documentation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CR_Cas_Sup_Branch_Documentation_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code",
                "CR_Mas_Com_Lessor_Logo", cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Lessor_Code);
            return View(cR_Cas_Sup_Branch_Documentation);
        }



        //public CR_Cas_Administrative_Procedures GetLastRecord()
        //{
        //    var Lrecord = db.CR_Cas_Administrative_Procedures.Max(Lr => Lr.CR_Cas_Administrative_Procedures_No.Substring(Lr.CR_Cas_Administrative_Procedures_No.Length-7,7));
        //    CR_Cas_Administrative_Procedures T = new CR_Cas_Administrative_Procedures();
        //    if (Lrecord != null)
        //    {
        //        Int64 val = Int64.Parse(Lrecord) + 1;
        //        T.CR_Cas_Administrative_Procedures_No = val.ToString("0000000");
        //    }
        //    else
        //    {
        //        T.CR_Cas_Administrative_Procedures_No = "0000001";
        //    }
        //    return T;
        //}



        public CR_Cas_Administrative_Procedures GetLastRecord(string ProcedureCode, string sector)
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





        // GET: CasDocumentation/Edit/5
        public ActionResult Edit(string id1, string id2, string id3)
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

            if (id1 == null || id2 == null || id3 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CR_Cas_Sup_Branch_Documentation cR_Cas_Sup_Branch_Documentation = db.CR_Cas_Sup_Branch_Documentation.FirstOrDefault(z => z.CR_Cas_Sup_Branch_Documentation_Lessor_Code == id1
            && z.CR_Cas_Sup_Branch_Documentation_Branch_Code == id2 && z.CR_Cas_Sup_Branch_Documentation_Code == id3);
            if (cR_Cas_Sup_Branch_Documentation == null)
            {
                return HttpNotFound();
            }
            else
            {

                ViewBag.stat = "حذف";

                if (cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_No != null)
                {
                    cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_No = cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_No.Trim();

                }


                if (cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Date == null)
                {
                    ViewBag.DocDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                }
                else
                {
                    ViewBag.DocDate = string.Format("{0:yyyy-MM-dd}", cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Date);
                }

                if (cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Start_Date == null)
                {
                    ViewBag.StartDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                }
                else
                {
                    ViewBag.StartDate = string.Format("{0:yyyy-MM-dd}", cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Start_Date);
                }

                if (cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_End_Date == null)
                {
                    ViewBag.EndDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                }
                else
                {
                    ViewBag.EndDate = string.Format("{0:yyyy-MM-dd}", cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_End_Date);
                }

                ViewBag.CR_Mas_Sup_Procedures_Ar_Name = cR_Cas_Sup_Branch_Documentation.CR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Ar_Name;

                if (cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Status == "N")
                {
                    ViewBag.Status = "N";
                    ViewBag.State = "غير مسجل";
                    cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Status = "N";
                }
                else if (cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Status == "A")
                {
                    ViewBag.State = "نشط";
                    ViewBag.Status = "A";
                    cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Status = "A";
                }
                else if (cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Status == "D")
                {
                    ViewBag.State = "محذوف";
                    ViewBag.Status = "D";
                    cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Status = "D";
                }
                else if (cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Status == "E")
                {
                    ViewBag.State = "منتهي";
                    ViewBag.Status = "E";
                    cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Status = "E";
                }
                else if (cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Status == "X")
                {
                    ViewBag.State = "على وشك الإنتهاء";
                    ViewBag.Status = "X";
                    cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Status = "X";
                }
            }


            return View(cR_Cas_Sup_Branch_Documentation);
        }

        public void SaveTracing(string ProcCode, string Lessor, string branchCode, DateTime docDate, DateTime StartDate, DateTime EndDate, string procType, string reason)
        {
            CR_Cas_Administrative_Procedures Ad = new CR_Cas_Administrative_Procedures();
            DateTime year = DateTime.Now;
            var y = year.ToString("yy");
            var sector = "1";
            var ProcedureCode = ProcCode;
            var autoInc = GetLastRecord(ProcedureCode, sector);
            var LessorCode = Lessor;
            Ad.CR_Cas_Administrative_Procedures_No = y + "-" + sector + "-" + ProcedureCode + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No;
            Ad.CR_Cas_Administrative_Procedures_Date = DateTime.Now;
            string currentTime = DateTime.Now.ToString("HH:mm:ss");
            Ad.CR_Cas_Administrative_Procedures_Time = TimeSpan.Parse(currentTime);
            Ad.CR_Cas_Administrative_Procedures_Year = y;
            Ad.CR_Cas_Administrative_Procedures_Sector = sector;
            Ad.CR_Cas_Administrative_Procedures_Code = ProcedureCode;
            Ad.CR_Cas_Administrative_Int_Procedures_Code = int.Parse(ProcedureCode);
            Ad.CR_Cas_Administrative_Procedures_Lessor = LessorCode;
            Ad.CR_Cas_Administrative_Procedures_Targeted_Action = branchCode;
            Ad.CR_Cas_Administrative_Procedures_User_Insert = Session["UserLogin"].ToString();
            Ad.CR_Cas_Administrative_Procedures_Type = procType;
            Ad.CR_Cas_Administrative_Procedures_Action = true;
            Ad.CR_Cas_Administrative_Procedures_Doc_Date = docDate;
            Ad.CR_Cas_Administrative_Procedures_Doc_Start_Date = StartDate;
            Ad.CR_Cas_Administrative_Procedures_Doc_End_Date = EndDate;
            Ad.CR_Cas_Administrative_Procedures_Reasons = reason;
            db.CR_Cas_Administrative_Procedures.Add(Ad);
        }

        // POST: CasDocumentation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Cas_Sup_Branch_Documentation_Lessor_Code," +
            "CR_Cas_Sup_Branch_Documentation_Branch_Code,CR_Cas_Sup_Branch_Documentation_Code," +
            "CR_Cas_Sup_Branch_Documentation_No,CR_Cas_Sup_Branch_Documentation_Date," +
            "CR_Cas_Sup_Branch_Documentation_Start_Date,CR_Cas_Sup_Branch_Documentation_End_Date," +
            "CR_Cas_Sup_Branch_Documentation_Activation,CR_Cas_Sup_Branch_Documentation_Credit_Limit," +
            "CR_Cas_Sup_Branch_Documentation_About_To_Expire,CR_Cas_Sup_Branch_Documentation_Status," +
            "CR_Cas_Sup_Procedures_Type")] CR_Cas_Sup_Branch_Documentation cR_Cas_Sup_Branch_Documentation
            , DateTime CR_Cas_Sup_Branch_Documentation_End_Date, DateTime CR_Cas_Sup_Branch_Documentation_Start_Date, DateTime CR_Cas_Sup_Branch_Documentation_Date,
            string save, string delete, string hold, string Reasons,string CR_Mas_Sup_Procedures_Ar_Name)
        {
            if (!string.IsNullOrEmpty(save))
            {

                if (ModelState.IsValid)

                {
                    using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var mech = db.CR_Cas_Sup_Follow_Up_Mechanism.FirstOrDefault(x => x.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code == cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Lessor_Code &&
                            x.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code == cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Code);


                            if (cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Date <= DateTime.Now &&
                                cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Start_Date >= cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Date &&
                                cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_End_Date > cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Start_Date &&
                                CR_Cas_Sup_Branch_Documentation_End_Date > DateTime.Now && cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Date != null &&
                                cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Start_Date != null && cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_End_Date != null)
                            {

                                if (cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Status == "X")
                                {

                                    /////////////////////////////////////Add Tracing//////////////////////////////////////

                                    SaveTracing(cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Code, cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Lessor_Code,
                                        cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Branch_Code, (DateTime)cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Date,
                                        (DateTime)cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Start_Date,
                                        (DateTime)cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_End_Date, "W", Reasons);
                                    db.SaveChanges();
                                    TempData["TempModel"] = "Saved";
                                    dbTran.Commit();
                                    return RedirectToAction("Index");
                                    //////////////////////////////////////////////////////////////////////////////////////

                                }
                                else
                                {
                                    cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Date = CR_Cas_Sup_Branch_Documentation_Date;
                                    cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_End_Date = CR_Cas_Sup_Branch_Documentation_End_Date;
                                    cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Start_Date = CR_Cas_Sup_Branch_Documentation_Start_Date;
                                    cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Procedures_Type = "1";


                                    DateTime currentDate = (DateTime)cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_End_Date;
                                    var nbr = Convert.ToDouble(mech.CR_Cas_Sup_Follow_Up_Mechanism_Befor_Expire);
                                    var d = currentDate.AddDays(-nbr);
                                    cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_About_To_Expire = d;


                                    if (cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_End_Date > DateTime.Now.AddDays(nbr))
                                    {
                                        cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Status = "A";

                                    }
                                    else if (cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_End_Date > DateTime.Now && cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_End_Date <= DateTime.Now.AddDays(nbr))
                                    {
                                        cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Status = "X";
                                    }
                                    else
                                    {
                                        cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Status = "E";
                                    }


                                    /////////////////////////////////////Add Tracing//////////////////////////////////////

                                    SaveTracing(cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Code, cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Lessor_Code,
                                        cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Branch_Code, (DateTime)cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Date,
                                        (DateTime)cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Start_Date,
                                        (DateTime)cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_End_Date, "I", Reasons);
                                    //db.SaveChanges();
                                    //////////////////////////////////////////////////////////////////////////////////////


                                    db.Entry(cR_Cas_Sup_Branch_Documentation).State = EntityState.Modified;
                                    db.SaveChanges();
                                    TempData["TempModel"] = "Saved";
                                    dbTran.Commit();
                                    
                                    return RedirectToAction("Index");

                                }
                            }
                            else
                            {
                                if (CR_Cas_Sup_Branch_Documentation_End_Date < DateTime.Now)
                                {
                                    ViewBag.DocEndDateError = "صلاحية الوثيقة منتهية";
                                }

                                if (cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Date > DateTime.Now)
                                {
                                    ViewBag.DocDateError = "الرجاء التأكد من تاريخ المستند";
                                }
                                if (cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Start_Date < CR_Cas_Sup_Branch_Documentation_Date)
                                {
                                    ViewBag.DocStartDateError = "تأكد من التاريخ";
                                }

                                if (cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Date == null)
                                {
                                    ViewBag.DocDateError = "الرجاء إختيار التاريخ";
                                }

                                if (cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Start_Date == null)
                                {
                                    ViewBag.DocStartDateError = "الرجاء إختيار التاريخ";
                                }

                                if (cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_End_Date == null)
                                {
                                    ViewBag.DocEndDateError = "الرجاء إختيار التاريخ";
                                }

                                ViewBag.EndDate = string.Format("{0:yyyy-MM-dd}", cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_End_Date);
                                ViewBag.StartDate = string.Format("{0:yyyy-MM-dd}", cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Start_Date);
                                ViewBag.DocDate = string.Format("{0:yyyy-MM-dd}", cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Date);
                            }


                        }
                        catch (DbEntityValidationException ex)
                        {
                            dbTran.Rollback();
                            throw ex;
                        }
                    }
                }



            }
            if (delete == "Delete" || delete == "حذف")
            {
                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {

                        /////////////////////////////////////Tracing//////////////////////////////////////

                        SaveTracing(cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Code, cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Lessor_Code,
                                        cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Branch_Code, (DateTime)cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Date,
                                        (DateTime)cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Start_Date,
                                        (DateTime)cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_End_Date, "D", Reasons);
                        //////////////////////////////////////////////////////////////////////////////////////
                        ///

                        cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Status = "N";
                        cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_No = null;
                        cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Date = null;
                        cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_End_Date = null;
                        cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Start_Date = null;
                        cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_About_To_Expire = null;
                        cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Procedures_Type = "1";




                        db.Entry(cR_Cas_Sup_Branch_Documentation).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["TempModel"] = "Deleted";
                        dbTran.Commit();
                        return RedirectToAction("Index");

                    }
                    catch (DbEntityValidationException ex)
                    {
                        dbTran.Rollback();
                        throw ex;
                    }
                }

            }

            ViewBag.CR_Mas_Sup_Procedures_Ar_Name = CR_Mas_Sup_Procedures_Ar_Name;
            ViewBag.stat = "حذف";
            ViewBag.Reason = Reasons;



            ////ViewBag.CR_Cas_Sup_Branch_Documentation_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo", cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Lessor_Code);
            ////ViewBag.CR_Cas_Sup_Procedures_Type = new SelectList(db.CR_Mas_Sup_Procedures.Where(x => x.CR_Mas_Sup_Procedures_Type == "1"), "CR_Mas_Sup_Procedures_Code", "CR_Mas_Sup_Procedures_Ar_Name", cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Procedures_Type);
            return View(cR_Cas_Sup_Branch_Documentation);
        }

        // GET: CasDocumentation/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Sup_Branch_Documentation cR_Cas_Sup_Branch_Documentation = db.CR_Cas_Sup_Branch_Documentation.Find(id);
            if (cR_Cas_Sup_Branch_Documentation == null)
            {
                return HttpNotFound();
            }
            return View(cR_Cas_Sup_Branch_Documentation);
        }

        // POST: CasDocumentation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CR_Cas_Sup_Branch_Documentation cR_Cas_Sup_Branch_Documentation = db.CR_Cas_Sup_Branch_Documentation.Find(id);
            db.CR_Cas_Sup_Branch_Documentation.Remove(cR_Cas_Sup_Branch_Documentation);
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
