using System.Net;
using System.Net.Mail;
using System.Web;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace FunctionApp1
{
    public class Function1
    {
        private readonly ILogger _logger;

        public Function1(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
        }

        [Function("Function1")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var queryParams = HttpUtility.ParseQueryString(req.Url.Query);
            var gmail = queryParams["gmail"];

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            string fromAddress = "yusif2006ofi@gmail.com";
            string toAddress = $"{gmail}";
            string subject = "Gmail from Azure Function";
            string body = "Hello, this is a Azure Function email";

            MailMessage mail = new MailMessage(fromAddress, toAddress, subject, body);

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587, 
                Credentials = new NetworkCredential("yusif2006ofi@gmail.com", "@Yusif200628711com@"), 
                EnableSsl = true 
            };

            try
            {
                smtpClient.Send(mail);
                response.WriteString("Email sent successfully!");
            }
            catch (Exception ex)
            {
                response.WriteString("Error sending email: " + ex.Message);
            }

            response.WriteString("Welcome to Azure Functions!");
            response.WriteString($"{gmail}");

            return response;
        }
    }
}
