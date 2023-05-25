using RentCar.Models;
using System.Linq;
using System.Web.Mvc;


namespace RentCar.Controllers
{
    public class SharedController : Controller
    {
        // GET: SharedQuestion
        private RentCarDBEntities db = new RentCarDBEntities();
        public static string codeTask;

        [ActionName("Question")]
        public ActionResult Question_GET()
        {
            if (codeTask == "1402")
            {
                codeTask = "";
                return RedirectToAction("Index", "Supporting");
            }
            else if (codeTask == "1501")
            {
                BrandController.TaskCode = null;
                codeTask = "";
                return RedirectToAction("Index", "Brand");
            }
            else if (codeTask == "1502")
            {
                ModelController.TaskCode = null;
                codeTask = "";
                return RedirectToAction("Index", "Model");
            }
            else if (codeTask == "1503")
            {
                codeTask = "";
                return RedirectToAction("Index", "Category");
            }
            else if (codeTask == "1504")
            {
                codeTask = "";
                return RedirectToAction("Index", "Color");
            }
            else if (codeTask == "1505")
            {
                codeTask = "";
                return RedirectToAction("Index", "Features");
            }
            else if (codeTask == "1506")
            {
                codeTask = "";
                return RedirectToAction("Index", "Specifications");
            }
            else if (codeTask == "1507")
            {
                codeTask = "";
                return RedirectToAction("Index", "Additional");
            }
            else if (codeTask == "1508")
            {
                codeTask = "";
                return RedirectToAction("Index", "CategoryCar");
            }
            else if (codeTask == "1510")
            {
                codeTask = "";
                return RedirectToAction("Index", "Choices");
            }
            else if (codeTask == "1601")
            {
                codeTask = "";
                return RedirectToAction("Index", "Nationalities");
            }
            else if (codeTask == "1603")
            {
                codeTask = "";
                return RedirectToAction("Index", "Jobs");
            }
            else if (codeTask == "1604")
            {
                codeTask = "";
                return RedirectToAction("Index", "Membership");
            }
            else if (codeTask == "1605")
            {
                codeTask = "";
                return RedirectToAction("Index", "City");
            }
            else if (codeTask == "1701")
            {
                codeTask = "";
                return RedirectToAction("Index", "User");
            }
            else if (codeTask == "1902")
            {
                codeTask = "";
                return RedirectToAction("Index", "Tasks");
            }
            else
            {
                codeTask = "";
                return View();
            }
        }

        [HttpPost]
        [ActionName("Question")]
        public ActionResult Question_POST()
        {
            //Supporting
            if (SupportingController.TaskCode == "1402")
            {
                codeTask = "1402";
                ViewBag.titre = "الشركات المساندة / الأسئلة الأكثر شيوعا";
                var supporting = db.CR_Mas_Msg_Questions_Answer.Where(x => x.CR_Mas_Msg_Tasks_Code == "1402");
                return View(supporting.ToList());
            }
            //Brand
            else if (BrandController.TaskCode == "1501")
            {
                BrandController.TaskCode = null;
                codeTask = "1501";
                ViewBag.titre = "الماركات / الأسئلة الأكثر شيوعا";
                var brand = db.CR_Mas_Msg_Questions_Answer.Where(x => x.CR_Mas_Msg_Tasks_Code == "1501");
                return View(brand.ToList());
            }
            //Model
            else if (ModelController.TaskCode == "1502")
            {
                ModelController.TaskCode = null;
                codeTask = "1502";
                ViewBag.titre = "الطرازات / الأسئلة الأكثر شيوعا";
                var model = db.CR_Mas_Msg_Questions_Answer.Where(x => x.CR_Mas_Msg_Tasks_Code == "1502");
                return View(model.ToList());
            }
            //Category
            else if (CategoryController.TaskCode == "1503")
            {
                codeTask = "1503";
                ViewBag.titre = "الفئات / الأسئلة الأكثر شيوعا";
                var category = db.CR_Mas_Msg_Questions_Answer.Where(x => x.CR_Mas_Msg_Tasks_Code == "1503");
                return View(category.ToList());
            }
            //Color
            else if (ColorController.TaskCode == "1504")
            {
                codeTask = "1504";
                ViewBag.titre = "الألوان / الأسئلة الأكثر شيوعا";
                var color = db.CR_Mas_Msg_Questions_Answer.Where(x => x.CR_Mas_Msg_Tasks_Code == "1504");
                return View(color.ToList());
            }
            //Features
            else if (FeaturesController.TaskCode == "1505")
            {
                codeTask = "1505";
                ViewBag.titre = "المميزات / الأسئلة الأكثر شيوعا";
                var features = db.CR_Mas_Msg_Questions_Answer.Where(x => x.CR_Mas_Msg_Tasks_Code == "1505");
                return View(features.ToList());
            }
            //Specifications
            else if (SpecificationsController.TaskCode == "1506")
            {
                codeTask = "1506";
                ViewBag.titre = "المواصفات / الأسئلة الأكثر شيوعا";
                var specifications = db.CR_Mas_Msg_Questions_Answer.Where(x => x.CR_Mas_Msg_Tasks_Code == "1506");
                return View(specifications.ToList());
            }
            //Additional
            else if (AdditionalController.TaskCode == "1507")
            {
                codeTask = "1507";
                ViewBag.titre = "الإضافات / الأسئلة الأكثر شيوعا";
                var additional = db.CR_Mas_Msg_Questions_Answer.Where(x => x.CR_Mas_Msg_Tasks_Code == "1507");
                return View(additional.ToList());
            }
            //CategoryCar
            else if (CategoryCarController.TaskCode == "1508")
            {
                codeTask = "1508";
                ViewBag.titre = "تحديد الفئة / الأسئلة الأكثر شيوعا";
                var categoryCar = db.CR_Mas_Msg_Questions_Answer.Where(x => x.CR_Mas_Msg_Tasks_Code == "1508");
                return View(categoryCar.ToList());
            }
            //Choices
            else if (ChoicesController.TaskCode == "1510")
            {
                codeTask = "1508";
                ViewBag.titre = "الخيارات / الأسئلة الأكثر شيوعا";
                var choice = db.CR_Mas_Msg_Questions_Answer.Where(x => x.CR_Mas_Msg_Tasks_Code == "1510");
                return View(choice.ToList());
            }
            //Nationalities
            else if (NationalitiesController.TaskCode == "1601")
            {
                codeTask = "1601";
                ViewBag.titre = "الجنسيات / الأسئلة الأكثر شيوعا";
                var nationalities = db.CR_Mas_Msg_Questions_Answer.Where(x => x.CR_Mas_Msg_Tasks_Code == "1601");
                return View(nationalities.ToList());
            }
            //Jobs
            else if (JobsController.TaskCode == "1603")
            {
                codeTask = "1603";
                ViewBag.titre = "المهن / الأسئلة الأكثر شيوعا";
                var jobs = db.CR_Mas_Msg_Questions_Answer.Where(x => x.CR_Mas_Msg_Tasks_Code == "1603");
                return View(jobs.ToList());
            }
            //Membership
            else if (MembershipController.TaskCode == "1604")
            {
                codeTask = "1604";
                ViewBag.titre = "العضويات / الأسئلة الأكثر شيوعا";
                var membership = db.CR_Mas_Msg_Questions_Answer.Where(x => x.CR_Mas_Msg_Tasks_Code == "1604");
                return View(membership.ToList());
            }
            //City
            else if (CityController.TaskCode == "1605")
            {
                codeTask = "1605";
                ViewBag.titre = "المدن / الأسئلة الأكثر شيوعا";
                var city = db.CR_Mas_Msg_Questions_Answer.Where(x => x.CR_Mas_Msg_Tasks_Code == "1605");
                return View(city.ToList());
            }
            //User
            else if (UserController.TaskCode == "1701")
            {
                codeTask = "1701";
                ViewBag.titre = "المستخدمين / الأسئلة الأكثر شيوعا";
                var user = db.CR_Mas_Msg_Questions_Answer.Where(x => x.CR_Mas_Msg_Tasks_Code == "1701");
                return View(user.ToList());
            }
            //Tasks
            else if (TasksController.TaskCode == "1902")
            {
                codeTask = "1902";
                ViewBag.titre = "الشاشات و المهام / الأسئلة الأكثر شيوعا";
                var tasks = db.CR_Mas_Msg_Questions_Answer.Where(x => x.CR_Mas_Msg_Tasks_Code == "1902");
                return View(tasks.ToList());
            }
            else
                return RedirectToAction("Index", "Home");
        }
    }
}