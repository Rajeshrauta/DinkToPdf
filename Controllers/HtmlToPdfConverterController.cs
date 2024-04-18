using DinkToPdf.Contracts;
using DinkToPdf;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.RegularExpressions;
using System.Drawing.Printing;
using PaperKind = DinkToPdf.PaperKind;

namespace HtmlToPdfConverter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HtmlToPdfConverterController : ControllerBase
    {
        private readonly IConverter _pdfConverter;
        private readonly IConfiguration _configuration;

        public HtmlToPdfConverterController(IConverter pdfConverter, IConfiguration configuration)
        {
            _pdfConverter = pdfConverter;
            _configuration = configuration;
        }

        [HttpPost("ConvertHtmlToPdf")]
        public IActionResult ConvertHtmlToPdf([FromBody] string htmlContent)
        {
            htmlContent = htmlContent.Replace("\"", "'");

            var gbl = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins =
                {
                    Bottom = 10,
                    Top = 10,
                    Left = 10,
                    Right = 10,
                },
                DocumentTitle = "DinkToPdf"
            };

            var objsetting = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlContent,
                WebSettings =
                        {
                            DefaultEncoding = "utf-8",
                            UserStyleSheet = null,
                            LoadImages = true,
                            Background = true,
                        }
            };

            var pdfBytes = _pdfConverter.Convert(new HtmlToPdfDocument
            {
                GlobalSettings = gbl,
                Objects = {objsetting}

            });

            return File(pdfBytes, "application/pdf", "Converted.pdf");
        }

        //[HttpGet("ConvertWebsiteToPdf")]
        //public IActionResult ConvertWebsiteToPdf(string url)
        //{
        //    // Download HTML content from the URL
        //    string htmlContent;
        //    using (var client = new WebClient())
        //    {
        //        htmlContent = client.DownloadString(url);
        //    }

        //    // Convert HTML content to PDF
        //    var pdfBytes = _pdfConverter.Convert(new HtmlToPdfDocument
        //    {
        //        Objects = {
        //            new ObjectSettings
        //            {
        //                HtmlContent = htmlContent,
        //                WebSettings =
        //                {
        //                    LoadImages = true, 
        //                    Background = true,
        //                }
        //            }
        //        }
        //    });

        //    return File(pdfBytes, "application/pdf", "website.pdf");
        //}



        [HttpPost("ConvertWebsiteToPdf")]
        public IActionResult ConvertWebsiteToPdf(string url)
        {
            // Download HTML content from the URL
            var gbl = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins =
                {
                    Bottom = 10,
                    Top = 10,
                    Left = 10,
                    Right = 10,
                },
                DocumentTitle = "DinkToPdf"
            };

            var objsetting = new ObjectSettings
            {
                PagesCount = true,
                Page = url,
                WebSettings =
                {
                    DefaultEncoding = "utf-8",
                    UserStyleSheet = null,
                    LoadImages = true,
                    Background = true,
                }
            };

            var pdfBytes = _pdfConverter.Convert(new HtmlToPdfDocument
            {
                GlobalSettings = gbl,
                Objects = { objsetting }

            });

            return File(pdfBytes, "application/pdf", "Converted.pdf");
        }



        //[HttpGet("ConvertWebsiteToPdf1")]
        //public IActionResult ConvertWebsiteToPdf1(string url)
        //{
        //    // Download HTML content from the URL
        //    string htmlContent;
        //    using (var client = new WebClient())
        //    {
        //        htmlContent = client.DownloadString(url);
        //    }

        //    // Fetch images and replace URLs in the HTML content
        //    htmlContent = FetchAndReplaceImageUrls(htmlContent, url);

        //    // Convert modified HTML content to PDF
        //    var pdfBytes = _pdfConverter.Convert(new HtmlToPdfDocument
        //    {
        //        Objects = {
        //    new ObjectSettings
        //    {
        //        HtmlContent = htmlContent
        //    }
        //}
        //    });

        //    return File(pdfBytes, "application/pdf", "website.pdf");
        //}

        //private string FetchAndReplaceImageUrls(string htmlContent, string baseUrl)
        //{
        //    // Regex pattern to match <img> tags
        //    var imgPattern = "<img.*?src=\"(.*?)\".*?>";

        //    // Match all <img> tags
        //    var matches = Regex.Matches(htmlContent, imgPattern);

        //    // Iterate through matched <img> tags
        //    foreach (Match match in matches)
        //    {
        //        // Get the URL of the image
        //        var imageUrl = match.Groups[1].Value;

        //        // Check if the URL is relative or absolute
        //        if (!Uri.TryCreate(imageUrl, UriKind.Absolute, out _))
        //        {
        //            // Combine relative URL with base URL to get absolute URL
        //            imageUrl = new Uri(new Uri(baseUrl), imageUrl).AbsoluteUri;
        //        }

        //        // Download the image and save it locally
        //        var imageName = Path.GetFileName(imageUrl);
        //        var localImagePath = Path.Combine("Files", "Images", imageName); 
        //        using (var client = new WebClient())
        //        {
        //            client.DownloadFile(imageUrl, localImagePath);
        //        }

        //        // Replace image URL in the HTML content with local path
        //        htmlContent = htmlContent.Replace(match.Groups[1].Value, localImagePath);
        //    }

        //    return htmlContent;
        //}

        //[HttpPost("convert")]
        //public async Task<IActionResult> ConvertToPdf([FromBody] IHtmlContent content)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var converter = new WkHtmlToPdfConverter(_configuration["WKHTMLTOPDF_PATH"]); // Assuming environment variable is set

        //    // Configure conversion options as needed
        //    converter.SetPaperSize(PaperSize.A4);

        //    var pdf = await converter.ConvertAsync(content.Html);

        //    if (pdf == null)
        //    {
        //        return StatusCode(500, "PDF conversion failed");
        //    }

        //    return File(pdf, "application/pdf", "converted.pdf");
        //}
    }
}
