using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Quik_BookingApp.DAO.Models;
using Quik_BookingApp.BOs.Request;
using Quik_BookingApp.Repos.Interface;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Quik_BookingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IEmailService emailService;

        /// <summary>
        ///  <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userService">An instance of the user service for accessing user data.</param>
        /// <param name="emailService">An instance of the email service for sending emails.</param>
        
        public UserController(IUserService userService, IEmailService emailService)
        {
            this.userService = userService;
            this.emailService = emailService;
        }

        [SwaggerOperation(
             Summary = "Retrieve all users",
             Description = "Returns a list of all users. If no users are found, it returns a 404 Not Found response."
         )]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var data = await userService.GetAll();
            if (data == null || data.Count == 0)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [HttpGet("GetBookingsOfUser")]
        public async Task<IActionResult> GetBookingsOfUser(string username)
        {
            try
            {
                var data = await userService.GetBookingsOfUser(username);

                if(data == null || data.Any())
                {
                    throw new Exception("No booking of this user");
                }

                return Ok(data);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            
        }

        [SwaggerOperation(
             Summary = "Retrieve user by username",
             Description = "This API allows you to get user details by providing a username. If the user is not found, a 404 Not Found response will be returned."
        )]
        [HttpGet("GetById/{username}")]
        public async Task<IActionResult> GetById(string username)
        {
            var data = await userService.GetByUserId(username);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [SwaggerOperation(
           Summary = "Create a new user",
           Description = "Creates a new user using the details provided in the request body. If successful, returns a 201 Created response."
       )]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] User userAccount)
        {
            var response = await userService.CreateUser(userAccount);
            if (response.ResponseCode == 201)
            {
                return CreatedAtAction(nameof(GetById), new { username = userAccount.Username }, response);
            }
            return StatusCode(response.ResponseCode, response);
        }

        [SwaggerOperation(
            Summary = "Register a new user",
            Description = "Registers a new user by processing the user registration request. Returns the result of the registration process."
        )]
        [HttpPost("UserRegisteration")]
        public async Task<IActionResult> UserRegisteration(UserRegister userRegister)
        {
            var data = await this.userService.UserRegisteration(userRegister);
            return Ok(data);
        }

        [SwaggerOperation(
            Summary = "Confirm user registration",
            Description = "Confirms a user’s registration by verifying the provided OTP."
        )]
        [HttpPost("ConfirmRegisteration")]
        public async Task<IActionResult> ConfirmRegisteration(string userid, string username, int otptext)
        {
            var data = await this.userService.ConfirmRegister(userid, username, otptext);
            return Ok(data);
        }

        private string GenerateOtp(int length = 6)
        {
            var random = new Random();
            var otp = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                otp.Append(random.Next(0, 10)); 
            }

            return otp.ToString();
        }

        [SwaggerOperation(
            Summary = "Reset user password",
            Description = "Resets a user’s password by verifying the old password and updating it to a new password."
        )]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string username, string oldpassword, string newpassword)
        {
            var data = await this.userService.ResetPassword(username, oldpassword, newpassword);
            return Ok(data);
        }

        [SwaggerOperation(
            Summary = "Forgot password",
            Description = "Handles forgotten password cases. Initiates the process to reset the user's password."
        )]
        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string username)
        {
            var data = await this.userService.ForgetPassword(username);
            return Ok(data);
        }

        [SwaggerOperation(
            Summary = "Update user password",
            Description = "Updates the user’s password by verifying an OTP and setting a new password."
        )]
        [HttpPost("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword(string username, string password, string otptext)
        {
            var data = await this.userService.UpdatePassword(username, password, otptext);
            return Ok(data);
        }

        [SwaggerOperation(
            Summary = "Update user status",
            Description = "Updates the status of a user by providing the username and the new status value."
        )]
        [HttpPost("UpdateStatus")]
        public async Task<IActionResult> UpdateStatus(string username, string status)
        {
            var data = await this.userService.UpdateStatus(username, status);
            return Ok(data);
        }

        [SwaggerOperation(
            Summary = "Update user role",
            Description = "Updates the role of a user by providing the username and the new role."
        )]
        [HttpPost("UpdateRole")]
        public async Task<IActionResult> UpdateRole(string username, string role)
        {
            var data = await this.userService.UpdateRole(username, role);
            return Ok(data);
        }

        [SwaggerOperation(
           Summary = "Send email",
           Description = "Sends an email using the pre-configured mail request data."
        )]
        [HttpPost("SendMail/{username}")]
        public async Task<IActionResult> SendMail(string username)
        {
            try
            {
                MailRequest mailrequest = new MailRequest();
                mailrequest.ToEmail = "anchorle3543@gmail.com";
                mailrequest.Subject = "Welcome to Huy world";
                mailrequest.Body = GetHtmlcontent();
                await emailService.SendEmailAsync(mailrequest);
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private string GetHtmlcontent()
        {
            string Response = "<div style=\"width:100%;background-color:lightblue;text-align:center;margin:10px\">";
            Response += "<h1>Welcome to QuanG HuY</h1>";
            Response += "<img src=\"https://drive.google.com/file/d/1OAcGjE-e5_4BnxdqMuM4Ezw8pEIqP0VZ/view?usp=sharing\" />";
            Response += "<h2>Thanks for see me <3</h2>";
            Response += "<a href=\"https://drive.google.com/file/d/1OAcGjE-e5_4BnxdqMuM4Ezw8pEIqP0VZ/view?usp=sharing\">Please join membership by click the link</a>";
            Response += "<div><h1> Contact us : anchorle3543@gmail.com</h1></div>";
            Response += "</div>";
            return Response;
        }
    }
}
