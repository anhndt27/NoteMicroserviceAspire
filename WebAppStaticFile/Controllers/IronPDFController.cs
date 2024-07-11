using Microsoft.AspNetCore.Mvc;
using WebAppStaticFile.Models;
using WebAppStaticFile.Security;


namespace WebAppStaticFile.Controllers
{
    [ApiController]
    public class IronPDFController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;
        private readonly ILogger<IronPDFController> _logger;


        public IronPDFController(IWebHostEnvironment env, IConfiguration configuration, ILogger<IronPDFController> logger)
        {
            _env = env;
            _configuration = configuration;
            _logger = logger;
        }

        [Route("")]
        [HttpGet]
        public ActionResult Get()
        {
            return Ok("Web api is running - 1/31/20223");
        }

        [Route("applywatermark")]
        [HttpPost]
        public async Task<ActionResult> ApplyWatermark(string studentInfo, IFormFile file, bool setPass = false)
        {
            string filePath = String.Empty;
            string webRootPath = _env.WebRootPath;
            var unixTimestamp = (double)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;

            string uploads = Path.Combine(webRootPath, @"pdf\material_pdf");
            if (file.Length > 0)
            {
                filePath = Path.Combine(uploads, unixTimestamp.ToString() + "." + "pdf");
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }

            var pdf = new PdfDocument(filePath);

            string useInfoString = string.Empty;

            if (studentInfo.Length >= 20)
            {
                for (int i = 1; i < 65; i++)
                {
                    useInfoString += $"{studentInfo}&nbsp;&nbsp;&nbsp;&nbsp;";
                }
            }
            if (15 <= studentInfo.Length && studentInfo.Length < 20)
            {
                for (int i = 1; i < 80; i++)
                {
                    useInfoString += $"{studentInfo}&nbsp;&nbsp;&nbsp;&nbsp;";
                }
            }
            if (studentInfo.Length < 15)
            {
                for (int i = 1; i < 100; i++)
                {
                    useInfoString += $"{studentInfo}&nbsp;&nbsp;&nbsp;&nbsp;";
                }
            }

            string watermark = $"<h3 style='width:200vw;height:200vh;color:red;line-height: 5em;letter-spacing: 5px;transform: rotate(-45deg);color: #7c93ab;font-size: 26px;'>{useInfoString}</h3>";
            int watermarkOpacity = 25;
            var newPdf = pdf.ApplyWatermark(watermark, watermarkOpacity, IronPdf.Editing.VerticalAlignment.Middle, IronPdf.Editing.HorizontalAlignment.Center);
            if (setPass)
            {
                if (!string.IsNullOrEmpty(studentInfo))
                {
                    string[] arrSpit = studentInfo.Split(' ');
                    if (arrSpit.Length >= 2)
                    {
                        var randomPass = new Random().Next();
                        pdf.SecuritySettings.OwnerPassword = $"{randomPass}"; //pass for edit 
                        pdf.SecuritySettings.UserPassword = $"{arrSpit.Last()}";  //pass for open
                    }
                }
            }
            System.IO.DirectoryInfo di = new DirectoryInfo(webRootPath + @"\pdf\material_pdf");

            foreach (FileInfo fileInFolder in di.GetFiles())
            {
                fileInFolder.Delete();
            }

            string physicalPath = Path.Combine(webRootPath + @"\pdf\material_pdf", unixTimestamp.ToString()) + "." + "pdf";
            newPdf.SaveAs(physicalPath);
            string fileName = unixTimestamp.ToString() + "." + "pdf";
            return Ok(fileName);
        }

        [Route("printpage")]
        [HttpPost]
        public ActionResult PrintPage([FromBody] string htmlString)
        {

            var renderer = new ChromePdfRenderer
            {
                RenderingOptions =
                {
                    HtmlFooter = new HtmlHeaderFooter()
                    {
                        MaxHeight = 15, // millimeters
			            HtmlFragment = "<div style='text-align: right; padding: 0 1rem 1rem 0'><i>Page {page}<i/></div>",
                        DrawDividerLine = true,
                    },
                    HtmlHeader = new HtmlHeaderFooter()
                    {
                        MaxHeight = 30,
                        HtmlFragment = "<div style='display: flex; flex-direction: row; padding: 0.5rem'> <img style='width: 6rem; height: 6rem' src='./images/logo.jpg'/> <div style='width: 100%; padding: 0.5rem'> <div style='display: flex; justify-content: space-between; padding-right: 0.5rem;font-family: sans-serif'> <div> <span style='font-weight: 700; font-size: 1.25rem; '> Academic Succes with </span> <h2 style='margin: 0.25rem 0 0.25rem 0; color: #3175BB; font-size: 1.75rem'>PRE-UNI NEW COLLEGE</h2> <span style='font-weight: 600;font-size: 0.75rem; '> OC, Selective School, and HSC Coaching Speciallists </span> </div> <div style='display: flex; flex-direction: column; justify-content: center;align-items: start; font-size: 0.7rem;font-weight: 600;'> <span>5 The Screscent, Strathfield NSW 2135</span> <span>Tel. 9746 7000</span> <span>Fax.9746 6999</span> <span>Website: www.newcollenge.com.au</span> <span>E-mail : info@newcollege.com.au</span> </div> </div> <hr style='height: 2px; border: none; background-color: black'/> </div> </div>",
                        DrawDividerLine = true,
                    },
                    MarginLeft = 5,
                    MarginRight = 5,
                    MarginTop = 35,
                }
            };
            var htmlToPdf = new HtmlToPdf();
            string webRootPath = _env.WebRootPath;
            string physicalPath = string.Empty;
            PdfDocument PDF;
            string pdfName = "pdf";
            var unixTimestamp = (double)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            if (htmlString != null)
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(webRootPath + @"\pdf\cbt_pdf");

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                PDF = renderer.RenderHtmlAsPdf(htmlString);

                string waterMarkString = String.Empty;
                for (int i = 1; i < 90; i++)
                {
                    waterMarkString += "Pre-Uni New College" + "&nbsp;&nbsp;&nbsp;&nbsp;";
                }

                string watermark = $"<h3 style='position: absolute;top: -40em; left: -40em;transform: rotate(-45deg);width: 100em; height: 125em;overflow: hidden;line-height: 5em;letter-spacing: 5px;color: #7c93ab;font-size: 32px;'>{waterMarkString}</h3>";
                int watermarkOpacity = 25;
                var newPdf = PDF.ApplyWatermark(watermark, watermarkOpacity, IronPdf.Editing.VerticalAlignment.Middle, IronPdf.Editing.HorizontalAlignment.Center);

                physicalPath = Path.Combine(webRootPath + @"\pdf\cbt_pdf", unixTimestamp + "." + pdfName);
                newPdf.SaveAs(physicalPath);
            }
            //string domainName = HttpContext.Request.Host.Value;
            string pdfPath = unixTimestamp + "." + pdfName;
            return Ok(pdfPath);
        }

        [Route("printpage-with-watermark")]
        [HttpPost]
        public ActionResult PrintPageWithWaterMark([FromBody] ParamsPrint paramsPrint, bool setPass = false)
        {

            var renderer = new ChromePdfRenderer
            {
                RenderingOptions =
                {
                    HtmlFooter = new HtmlHeaderFooter()
                    {
                        MaxHeight = 15, // millimeters
			            HtmlFragment = "<div style='text-align: right; padding: 0 1rem 1rem 0'><i>Page {page}<i/></div>",
                        DrawDividerLine = true,
                    },
                    HtmlHeader = new HtmlHeaderFooter()
                    {
                        MaxHeight = 30,
                        HtmlFragment = "<div style='display: flex; flex-direction: row; padding: 0.5rem'> <img style='width: 6rem; height: 6rem' src='./images/logo.jpg'/> <div style='width: 100%; padding: 0.5rem'> <div style='display: flex; justify-content: space-between; padding-right: 0.5rem;font-family: sans-serif'> <div> <span style='font-weight: 700; font-size: 1.25rem; '> Academic Succes with </span> <h2 style='margin: 0.25rem 0 0.25rem 0; color: #3175BB; font-size: 1.75rem'>PRE-UNI NEW COLLEGE</h2> <span style='font-weight: 600;font-size: 0.75rem; '> OC, Selective School, and HSC Coaching Speciallists </span> </div> <div style='display: flex; flex-direction: column; justify-content: center;align-items: start; font-size: 0.7rem;font-weight: 600;'> <span>5 The Screscent, Strathfield NSW 2135</span> <span>Tel. 9746 7000</span> <span>Fax.9746 6999</span> <span>Website: www.newcollenge.com.au</span> <span>E-mail : info@newcollege.com.au</span> </div> </div> <hr style='height: 2px; border: none; background-color: black'/> </div> </div>",
                        DrawDividerLine = true,
                    },
                    MarginLeft = 5,
                    MarginRight = 5,
                    MarginTop = 35,
                }
            };
            var htmlToPdf = new HtmlToPdf();
            string webRootPath = _env.WebRootPath;
            string physicalPath = string.Empty;
            PdfDocument PDF;
            string pdfName = "pdf";
            var unixTimestamp = (double)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            if (paramsPrint.HtmlString != null)
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(webRootPath + @"\pdf\s-math");

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                PDF = renderer.RenderHtmlAsPdf(paramsPrint.HtmlString);


                string useInfoString = string.Empty;

                if (paramsPrint.StudentInfo.Length >= 20)
                {
                    for (int i = 1; i < 65; i++)
                    {
                        useInfoString += paramsPrint.StudentInfo + "&nbsp;&nbsp;&nbsp;&nbsp;";
                    }
                }
                if (15 <= paramsPrint.StudentInfo.Length && paramsPrint.StudentInfo.Length < 20)
                {
                    for (int i = 1; i < 80; i++)
                    {
                        useInfoString += paramsPrint.StudentInfo + "&nbsp;&nbsp;&nbsp;&nbsp;";
                    }
                }
                if (paramsPrint.StudentInfo.Length < 15)
                {
                    for (int i = 1; i < 100; i++)
                    {
                        useInfoString += paramsPrint.StudentInfo + "&nbsp;&nbsp;&nbsp;&nbsp;";
                    }
                }

                string watermark = $"<h3 style='position: absolute;top: -40em; left: -40em;transform: rotate(-45deg);width: 100em; height: 125em;overflow: hidden;line-height: 5em;letter-spacing: 5px;color: #7c93ab;font-size: 32px;'>{useInfoString}</h3>";
                int watermarkOpacity = 25;
                var newPdf = PDF.ApplyWatermark(watermark, watermarkOpacity, IronPdf.Editing.VerticalAlignment.Middle, IronPdf.Editing.HorizontalAlignment.Center);

                if (setPass)
                {
                    if (!string.IsNullOrEmpty(paramsPrint.StudentInfo))
                    {
                        string[] arrSpit = paramsPrint.StudentInfo.Split(' ');
                        if (arrSpit.Length >= 2)
                        {
                            var randomPass = new Random().Next();
                            PDF.SecuritySettings.OwnerPassword = $"{randomPass}"; //pass for edit 
                            PDF.SecuritySettings.UserPassword = $"{arrSpit.Last()}";  //pass for open
                        }
                    }
                }
                physicalPath = Path.Combine(webRootPath + @"\pdf\s-math", unixTimestamp + "." + pdfName);
                newPdf.SaveAs(physicalPath);
            }
            string pdfPath = unixTimestamp + "." + pdfName;
            return Ok(pdfPath);
        }

        [Route("deleteFile")]
        [HttpGet]
        public ActionResult DeleteFile(string fileName)
        {
            string webRootPath = _env.WebRootPath;
            System.IO.DirectoryInfo di = new DirectoryInfo(webRootPath + @"\pdf\material_pdf");

            foreach (FileInfo fileInFolder in di.GetFiles())
            {
                if (fileInFolder.Name == fileName)
                {
                    fileInFolder.Delete();
                }
            }
            return Ok();
        }

        [Route("Download")]
        [HttpGet]
        public ActionResult URL2PDF(string enCode)
        {
            bool checkDate = true;
            try
            {
                string deCode = enCode.DecryptValue();
                string[] arrDeCode = deCode.Split('|');
                if (arrDeCode.Length == 4)
                {
                    string linkPdf = arrDeCode[0];
                    string studentId = arrDeCode[1];
                    long timeValidDuration = long.Parse(arrDeCode[2]);
                    string fileName = arrDeCode[3];
                    if (timeValidDuration > 0)
                    {
                        checkDate = (DateTime.UtcNow.Ticks <= timeValidDuration) ? true : false;
                    }
                    if (checkDate)
                    {
                        Stream stream = null;
                        var renderer = new ChromePdfRenderer();
                        var pdf = renderer.RenderUrlAsPdf(linkPdf);
                        //Get the Stream returned from the response
                        if (!string.IsNullOrEmpty(studentId))
                        {
                            var randomPass = new Random().Next();
                            pdf.SecuritySettings.OwnerPassword = $"{randomPass}";  //pass for edit
                            pdf.SecuritySettings.UserPassword = $"{studentId}";  //pass for open (student Id)
                        }
                        stream = pdf.Stream;
                        return File(stream, "application/octet-stream", fileName);
                    }
                    return NotFound("File not found");
                }
            }
            catch
            {
                return NotFound("File not found");
            }
            return NotFound("Not found");
        }


        [Route("URLPDF")]
        [HttpGet]
        public ActionResult URLPDF([FromQuery] string enCode = "none")
        {
            string deCode = enCode.DecryptValue();
            string[] arrDeCode = deCode.Split('|');
            if (arrDeCode.Length == 10)
            {
                string cyber = _configuration.GetValue<string>("Cyber");
                bool check = Security.Security.CheckEncryptValueDate(arrDeCode[7], 48);
                if (check)
                {
                    string url = string.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, Request.Path, Request.QueryString);
                    string renderUrl = string.Empty;
                    string webRootPath = _env.WebRootPath;
                    var unixTimestamp = (double)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
                    string fileName = Path.Combine(unixTimestamp + "ttc_report" + ".pdf");
                    Stream stream = null;
                    try
                    {
                        var renderer = new ChromePdfRenderer();
                        if (url.Contains("localhost"))
                        {
                            renderUrl = $"http://localhost:30549/PrintTestReport/TTC/{arrDeCode[0]}/{arrDeCode[1]}/{arrDeCode[2]}/{arrDeCode[3]}/{arrDeCode[4]}/{arrDeCode[5]}/{arrDeCode[6]}/{arrDeCode[8]}";
                        }
                        else
                        {
                            renderUrl = $"{cyber}/PrintTestReport/TTC/{arrDeCode[0]}/{arrDeCode[1]}/{arrDeCode[2]}/{arrDeCode[3]}/{arrDeCode[4]}/{arrDeCode[5]}/{arrDeCode[6]}/{arrDeCode[8]}";
                        }
                        var pdf = renderer.RenderUrlAsPdf(renderUrl);
                        //Get the Stream returned from the response
                        if (arrDeCode[9] == "setPass")
                        {
                            var randomPass = new Random().Next();
                            pdf.SecuritySettings.OwnerPassword = $"{randomPass}";  //pass for edit
                            pdf.SecuritySettings.UserPassword = $"{arrDeCode[5]}";  //pass for open (student Id)
                        }
                        stream = pdf.Stream;
                        return File(stream, "application/octet-stream", $"{fileName}");
                    }
                    catch
                    {
                        return NotFound("Not found");
                    }
                }
            }
            return NotFound("Not found");
        }





        
        [Route("convert-html-to-pdf")]
        [HttpPost]
        public ActionResult ConverHtmlToPDF(PDFModel model)
        {
            try
            {
                var renderer = new ChromePdfRenderer
                {
                    RenderingOptions =
                    {
                        MarginLeft = 5,
                        MarginRight = 5,
                        MarginTop = 5,
                    }
                };

                string webRootPath = _env.WebRootPath;
                string physicalPath = string.Empty;
                PdfDocument PDF;
                string pdfName = "pdf";
                var unixTimestamp = (double)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
                if (model.HtmlText != null)
                {
                    PDF = renderer.RenderHtmlAsPdf(ReplateImage(model));
                    physicalPath = Path.Combine(webRootPath + @"\pdf\pdfReceipt", unixTimestamp + "." + pdfName);
                    PDF.SaveAs(physicalPath);
                }
                MemoryStream ms = new MemoryStream();
                FileStream file = new FileStream(physicalPath, FileMode.Open, FileAccess.Read);
                byte[] bytes = new byte[file.Length];
                file.Read(bytes, 0, (int)file.Length);
                ms.Write(bytes, 0, (int)file.Length);
                file.Close();
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/octet-stream", $"{unixTimestamp}.{pdfName}");
            }
            catch(Exception e)
            {
                return NotFound();
            }
        }

        
        [Route("ncmg-convert-html-to-pdf")]
        [HttpPost]
        public ActionResult NcmgConvertHtmlToPdf(PDFModel model)
        {
            try
            {
                var renderer = new ChromePdfRenderer
                {
                    RenderingOptions =
                    {
                        MarginLeft = 5,
                        MarginRight = 5,
                        MarginTop = 5,
                    }
                };

                string webRootPath = _env.WebRootPath;
                string physicalPath = string.Empty;
                PdfDocument PDF;
                string pdfName = "pdf";
                var unixTimestamp = (double)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
                if (model.HtmlText != null)
                {
                    PDF = renderer.RenderHtmlAsPdf(ReplateImage(model));
                    string folderPath = Path.Combine(webRootPath, "pdf", "pdfReceiptData");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    physicalPath = Path.Combine(folderPath, $"{unixTimestamp}.{pdfName}");
                    PDF.SaveAs(physicalPath);
                }
                MemoryStream ms = new MemoryStream();
                FileStream file = new FileStream(physicalPath, FileMode.Open, FileAccess.Read);
                byte[] bytes = new byte[file.Length];
                file.Read(bytes, 0, (int)file.Length);
                ms.Write(bytes, 0, (int)file.Length);
                file.Close();
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/octet-stream", $"{unixTimestamp}.{pdfName}");
            }
            catch(Exception e)
            {
                return NotFound();
            }
        }




        [Route("pdf-merge")]
        [HttpPost]
        public ActionResult PDFMerge([FromBody] MergePDFModel model)
        {
            if (model.IsLog)
            {
                _logger.LogWarning($"Urls: {string.Join(", ", model.Urls)}  \nFileName: {model.FileName} \nisLog: {model.IsLog}");
            }
            var unixTimestamp = (double)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            string fileName = string.IsNullOrEmpty(model.FileName) ? Path.Combine(unixTimestamp + "report" + ".pdf"): checkFileName(model.FileName);
            Stream stream = null;
            List<PdfDocument> listPdf = new List<PdfDocument>();
            try
            {
                foreach(var url in model.Urls)
                {
                    var renderer = new ChromePdfRenderer();
                    var pdf = renderer.RenderUrlAsPdf(url);
                    listPdf.Add(pdf);
                }
                var merged = PdfDocument.Merge(listPdf);
                stream = merged.Stream;
                return File(stream, "application/octet-stream", $"{fileName}");
            }
            catch(Exception e )
            {
                _logger.LogWarning($"PDF Merge Error : {e.Message}");
                return NotFound("Not found");
            }
        }

        private string checkFileName(string fileName)
        {
            if(fileName.Contains(".pdf"))
                return fileName;
            return fileName + ".pdf";
        }
        

        private string ReplateImage(PDFModel model)
        {
            foreach (var text in model.ImageReplate)
            {
                foreach(var id in model.StudentId)
                {
                    string filePaths = Path.Combine(_env.ContentRootPath, "studentphoto", $"{id}.jpg");
                    if (text.Contains(id))
                    {
                        try
                        {
                            byte[] bytes = System.IO.File.ReadAllBytes(filePaths);
                            string file = Convert.ToBase64String(bytes);
                            string imageNew = $"<img class=\"border rounded\" style=\"width: 100px\" src=\"data:image/png;base64,{file}\" alt=\"\" onerror=\"showErrorImg(this)\">";
                            model.HtmlText = model.HtmlText.Replace(text, imageNew);
                        }
                        catch(Exception e)
                        {

                        }
                    }
                }
            }
            return model.HtmlText;  
        }
    }
}

