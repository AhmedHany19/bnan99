using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using Image = System.Drawing.Image;

namespace BnanFollowUpContracts
{
    public static class SendMailForPatch
    {
        public static void SendMailBeforeOneDay(CR_Cas_Contract_Basic contract)
        {

            string imagePath = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName, "images\\1day.jpeg");
            Image image = Image.FromFile(imagePath);
            string logoPath = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName, "images\\3.png");
            Image logo = Image.FromFile(logoPath);

            // Create a graphics object from the image
            Graphics graphics = Graphics.FromImage(image);

            // Set the position where the logo should be placed
            Point logoPosition = new Point(50, 101);
            Size logosize = new Size(110, 60);
            // Define the font and brush for the text\
            Font companyfont = new Font("Segoe UI", 23, FontStyle.Bold);
            Brush companybrush = new SolidBrush(Color.White);

            // Draw the logo on the image
            graphics.DrawImage(logo, new Rectangle(logoPosition, logosize), 0, 0, logo.Width, logo.Height, GraphicsUnit.Pixel);

            // Draw the text on the image
            string Companyname = contract.CR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Ar_Long_Name;
            if (Companyname.Length <= 13)
            {
                graphics.DrawString(Companyname, companyfont, companybrush, new PointF(750, 102));
            }
            else if (Companyname.Length <= 20 && Companyname.Length > 13)
            {
                graphics.DrawString(Companyname, companyfont, companybrush, new PointF(650, 102));

            }
            else if (Companyname.Length <= 25 && Companyname.Length > 20)
            {
                graphics.DrawString(Companyname, companyfont, companybrush, new PointF(550, 102));
            }
            else if (Companyname.Length <= 29 && Companyname.Length > 25)
            {
                graphics.DrawString(Companyname, companyfont, companybrush, new PointF(450, 102));
            }
            else
            {
                graphics.DrawString(Companyname, companyfont, companybrush, new PointF(350, 102));
            }

            string guid = Guid.NewGuid().ToString() + ".png";
            string savedModified = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, guid);
            image.Save(savedModified);


            string htmlBody = string.Format("<html><body><h1>Contract Number {0}</h1><br><img src=cid:Contract style='width:100%;height:100%'></body></html>", contract.CR_Cas_Contract_Basic_No);

            AlternateView avHtml = AlternateView.CreateAlternateViewFromString
               (htmlBody, null, MediaTypeNames.Text.Html);

            LinkedResource inline = new LinkedResource(savedModified, MediaTypeNames.Image.Jpeg);
            inline.ContentId = string.Format("Contract Number {0}", contract.CR_Cas_Contract_Basic_No);
            avHtml.LinkedResources.Add(inline);

            MailMessage mail = new MailMessage();
            mail.AlternateViews.Add(avHtml);


            /*if (contract.CR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Email != null)
            {
                mail.From = new MailAddress(contract.CR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Email);
            }*/
            mail.From = new MailAddress("Bnanrent@outlook.com");


            /*if (contract.CR_Mas_Renter_Information.CR_Mas_Renter_Information_Email == null)
            {
                *//*         mail.To.Add(contract.CR_Mas_Renter_Information.CR_Mas_Renter_Information_Email);*//*
            }*/
            mail.To.Add("bnanbnanmail@gmail.com");
            mail.Subject = string.Format("Contract Number {0} will end after day", contract.CR_Cas_Contract_Basic_No);

            mail.Body = inline.ContentId;

            mail.IsBodyHtml = true;

            SmtpClient smtpClient = new SmtpClient("smtp.office365.com");
            smtpClient.Port = 587;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential("Bnanrent@outlook.com", "bnan123123");

            // Send the message
            smtpClient.Send(mail);
            mail.Dispose();
            File.Delete(savedModified);

        }

        public static void SendMailBeforeFourHours(CR_Cas_Contract_Basic contract)
        {
            string imagePath = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName, "images\\4hours.jpeg");
            Image image = Image.FromFile(imagePath);
            string logoPath = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName, "images\\3.png");
            Image logo = Image.FromFile(logoPath);

            // Create a graphics object from the image
            Graphics graphics = Graphics.FromImage(image);

            // Set the position where the logo should be placed
            Point logoPosition = new Point(50, 101);
            Size logosize = new Size(110, 60);
            // Define the font and brush for the text\
            Font companyfont = new Font("Segoe UI", 23, FontStyle.Bold);
            Brush companybrush = new SolidBrush(Color.White);

            // Draw the logo on the image
            graphics.DrawImage(logo, new Rectangle(logoPosition, logosize), 0, 0, logo.Width, logo.Height, GraphicsUnit.Pixel);

            // Draw the text on the image
            string Companyname = contract.CR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Ar_Long_Name;
            if (Companyname.Length <= 13)
            {
                graphics.DrawString(Companyname, companyfont, companybrush, new PointF(750, 102));
            }
            else if (Companyname.Length <= 20 && Companyname.Length > 13)
            {
                graphics.DrawString(Companyname, companyfont, companybrush, new PointF(650, 102));

            }
            else if (Companyname.Length <= 25 && Companyname.Length > 20)
            {
                graphics.DrawString(Companyname, companyfont, companybrush, new PointF(550, 102));
            }
            else if (Companyname.Length <= 29 && Companyname.Length > 25)
            {
                graphics.DrawString(Companyname, companyfont, companybrush, new PointF(450, 102));
            }
            else
            {
                graphics.DrawString(Companyname, companyfont, companybrush, new PointF(350, 102));
            }

            string guid = Guid.NewGuid().ToString() + ".png";
            string savedModified = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, guid);
            image.Save(savedModified);



            string htmlBody = string.Format("<html><body><h1>Contract Number {0}</h1><br><img src=cid:Contract style='width:100%;height:100%'></body></html>", contract.CR_Cas_Contract_Basic_No);

            AlternateView avHtml = AlternateView.CreateAlternateViewFromString
               (htmlBody, null, MediaTypeNames.Text.Html);

            LinkedResource inline = new LinkedResource(savedModified, MediaTypeNames.Image.Jpeg);
            inline.ContentId = string.Format("Contract Number {0}", contract.CR_Cas_Contract_Basic_No);
            avHtml.LinkedResources.Add(inline);

            MailMessage mail = new MailMessage();
            mail.AlternateViews.Add(avHtml);



            /*if (contract.CR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Email != null)
            {
                mail.From = new MailAddress(contract.CR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Email);
            }*/
            mail.From = new MailAddress("Bnanrent@outlook.com");


            /* if (contract.CR_Mas_Renter_Information.CR_Mas_Renter_Information_Email == null)
             {
                 *//*         mail.To.Add(contract.CR_Mas_Renter_Information.CR_Mas_Renter_Information_Email);*//*
             }*/
            mail.To.Add("bnanbnanmail@gmail.com");
            mail.Subject = string.Format("Contract Number {0} will end after 4 hours", contract.CR_Cas_Contract_Basic_No);


            mail.Body = inline.ContentId;

            mail.IsBodyHtml = true;

            SmtpClient smtpClient = new SmtpClient("smtp.office365.com");
            smtpClient.Port = 587;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential("Bnanrent@outlook.com", "bnan123123");

            // Send the message
            smtpClient.Send(mail);
            mail.Dispose();
            File.Delete(savedModified);
        }

        public static void SendMailWhenEnd(CR_Cas_Contract_Basic contract)
        {

            string imagePath = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName, "images\\End.jpeg");
            Image image = Image.FromFile(imagePath);
            string logoPath = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName, "images\\3.png");
            Image logo = Image.FromFile(logoPath);

            // Create a graphics object from the image
            Graphics graphics = Graphics.FromImage(image);

            // Set the position where the logo should be placed
            Point logoPosition = new Point(50, 101);
            Size logosize = new Size(110, 60);
            // Define the font and brush for the text\
            Font companyfont = new Font("Segoe UI", 23, FontStyle.Bold);
            Brush companybrush = new SolidBrush(Color.White);

            // Draw the logo on the image
            graphics.DrawImage(logo, new Rectangle(logoPosition, logosize), 0, 0, logo.Width, logo.Height, GraphicsUnit.Pixel);

            // Draw the text on the image
            string Companyname = contract.CR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Ar_Long_Name;
            if (Companyname.Length <= 13)
            {
                graphics.DrawString(Companyname, companyfont, companybrush, new PointF(750, 102));
            }
            else if (Companyname.Length <= 20 && Companyname.Length > 13)
            {
                graphics.DrawString(Companyname, companyfont, companybrush, new PointF(650, 102));

            }
            else if (Companyname.Length <= 25 && Companyname.Length > 20)
            {
                graphics.DrawString(Companyname, companyfont, companybrush, new PointF(550, 102));
            }
            else if (Companyname.Length <= 29 && Companyname.Length > 25)
            {
                graphics.DrawString(Companyname, companyfont, companybrush, new PointF(450, 102));
            }
            else
            {
                graphics.DrawString(Companyname, companyfont, companybrush, new PointF(350, 102));
            }

            string guid = Guid.NewGuid().ToString() + ".png";
            string savedModified = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, guid);
            image.Save(savedModified);



            string htmlBody = string.Format("<html><body><h1>Contract Number {0}</h1><br><img src=cid:Contract style='width:100%;height:100%'></body></html>", contract.CR_Cas_Contract_Basic_No);

            AlternateView avHtml = AlternateView.CreateAlternateViewFromString
               (htmlBody, null, MediaTypeNames.Text.Html);

            LinkedResource inline = new LinkedResource(savedModified, MediaTypeNames.Image.Jpeg);
            inline.ContentId = string.Format("Contract Number {0}", contract.CR_Cas_Contract_Basic_No);
            avHtml.LinkedResources.Add(inline);

            MailMessage mail = new MailMessage();
            mail.AlternateViews.Add(avHtml);



            /*if (contract.CR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Email != null)
            {
                mail.From = new MailAddress(contract.CR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Email);
            }*/
            mail.From = new MailAddress("Bnanrent@outlook.com");


            /*   if (contract.CR_Mas_Renter_Information.CR_Mas_Renter_Information_Email == null)
               {
                   *//*         mail.To.Add(contract.CR_Mas_Renter_Information.CR_Mas_Renter_Information_Email);*//*
               }*/
            mail.To.Add("bnanbnanmail@gmail.com");
            mail.Subject = string.Format("Contract Number {0} Ended", contract.CR_Cas_Contract_Basic_No);

            mail.Body = inline.ContentId;

            mail.IsBodyHtml = true;

            SmtpClient smtpClient = new SmtpClient("smtp.office365.com");
            smtpClient.Port = 587;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential("Bnanrent@outlook.com", "bnan123123");

            // Send the message
            smtpClient.Send(mail);
            mail.Dispose();
            File.Delete(savedModified);
        }
    }
}
