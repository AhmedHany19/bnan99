using RentCar.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RentCar
{
    public static class SendMailForPatch
    {
        public static void SendMailBeforeOneDay(CR_Cas_Contract_Basic contract)
        {

      /*      string projectFolder = Server.MapPath(string.Format("~/{0}/", "images"));*/
 /*           string image1 = Path.Combine(projectFolder, "4.png");
             string image2 = Path.Combine(projectFolder, "3.png");*/

            Image image = Image.FromFile("C:\\Users\\hp\\Desktop\\4.png");
            Image logo = Image.FromFile("C:\\Users\\hp\\Desktop\\3.png");

            // Create a graphics object from the image
            Graphics graphics = Graphics.FromImage(image);

            // Set the position where the logo should be placed
            Point logoPosition = new Point(50, 70);
            Size logosize = new Size(110, 60);
            // Define the font and brush for the text\
            Font companyfont = new Font("Segoe UI", 23, FontStyle.Bold);
            Font renterfont = new Font("Segoe UI", 18, FontStyle.Bold);
            Brush companybrush = new SolidBrush(Color.White);
            Font font = new Font("Arial", 16);
            Font carfont = new Font("Arial", 14);
            Brush brush = new SolidBrush(Color.Black);

            // Draw the logo on the image
            graphics.DrawImage(logo, new Rectangle(logoPosition, logosize), 0, 0, logo.Width, logo.Height, GraphicsUnit.Pixel);

            // Draw the text on the image
            string Companyname = contract.CR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Ar_Long_Name;
            if (Companyname.Length <= 13)
            {
                graphics.DrawString(Companyname, companyfont, companybrush, new PointF(1650, 70));
            }
            else if (Companyname.Length <= 20 && Companyname.Length > 13)
            {
                graphics.DrawString(Companyname, companyfont, companybrush, new PointF(1550, 70));

            }
            else if (Companyname.Length <= 25 && Companyname.Length > 20)
            {
                graphics.DrawString(Companyname, companyfont, companybrush, new PointF(1450, 70));
            }
            else if (Companyname.Length <= 29 && Companyname.Length > 25)
            {
                graphics.DrawString(Companyname, companyfont, companybrush, new PointF(1320, 70));
            }
            else
            {
                graphics.DrawString(Companyname, companyfont, companybrush, new PointF(1200, 70));
            }

            string Rentername = contract.CR_Mas_Renter_Information.CR_Mas_Renter_Information_Ar_Name.Trim();
            if (Rentername.Length <= 20)
            {
                graphics.DrawString(Rentername, renterfont, companybrush, new PointF(1300, 138));
            }
            else if (Rentername.Length <= 32 && Rentername.Length > 20)
            {
                graphics.DrawString(Rentername, renterfont, companybrush, new PointF(1200, 138));

            }
            else if (Rentername.Length <= 41 && Rentername.Length > 32)
            {
                graphics.DrawString(Rentername, renterfont, companybrush, new PointF(1020, 138));

            }
            else if (Rentername.Length > 41)
            {
                graphics.DrawString(Rentername, renterfont, companybrush, new PointF(800, 138));

            }
            graphics.DrawString(contract.CR_Mas_Renter_Information.CR_Mas_Renter_Information_En_Name, renterfont, companybrush, new PointF(210, 138));
            graphics.DrawString(contract.CR_Cas_Contract_Basic_No, font, brush, new PointF(800, 335));
            graphics.DrawString(contract.CR_Cas_Contract_Basic_Start_Date.ToString(), font, brush, new PointF(800, 400));
            graphics.DrawString(contract.CR_Cas_Contract_Basic_Expected_End_Date.ToString(), font, brush, new PointF(800, 470));
            graphics.DrawString(contract.CR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Collect_Ar_Name.Trim(), carfont, brush, new PointF(730, 535));
            graphics.DrawString(contract.CR_Cas_Contract_Basic_Net_Value.ToString(), font, brush, new PointF(880, 595));



            string guid = Guid.NewGuid().ToString() + ".png";
            string savedModified = Path.Combine("C:\\Users\\hp\\Desktop", guid);
            image.Save(savedModified);
            


            string htmlBody = "<html><body><h1>Contract Summary </h1><br><img src=cid:Contract style='width:100%;height:100%'></body></html>";

            AlternateView avHtml = AlternateView.CreateAlternateViewFromString
               (htmlBody, null, MediaTypeNames.Text.Html);

            LinkedResource inline = new LinkedResource(savedModified, MediaTypeNames.Image.Jpeg);
            inline.ContentId = "Contract";
            avHtml.LinkedResources.Add(inline);

            MailMessage mail = new MailMessage();
            mail.AlternateViews.Add(avHtml);

     

            /*if (contract.CR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Email != null)
            {
                mail.From = new MailAddress(contract.CR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Email);
            }*/
            mail.From = new MailAddress("Bnanrent@outlook.com");


            if (contract.CR_Mas_Renter_Information.CR_Mas_Renter_Information_Email != null)
            {
                /*         mail.To.Add(contract.CR_Mas_Renter_Information.CR_Mas_Renter_Information_Email);*/
                mail.To.Add("bnanbnanmail@gmail.com");
            }
            mail.Subject = " Contract Mail ";
            mail.Body = inline.ContentId;

            mail.IsBodyHtml = true;

            SmtpClient smtpClient = new SmtpClient("smtp.office365.com");
            smtpClient.Port = 587;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential("Bnanrent@outlook.com", "bnan123123");

            // Send the message
            //smtpClient.Send(mail);


        }
    }
}
