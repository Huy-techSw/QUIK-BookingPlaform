using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using Quik_BookingApp.BOs.Request;
using Quik_BookingApp.DAO;
using Quik_BookingApp.DAO.Models;
using Quik_BookingApp.Enums;
using Quik_BookingApp.Helper;
using Quik_BookingApp.Modal;
using Quik_BookingApp.Repos.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quik_BookingApp.Service
{
    public class UserService : IUserService
    {
        public readonly QuikDbContext context;
        public readonly IMapper mapper;
        public readonly ILogger<UserService> _logger;
        public readonly IEmailService emailService;
        //private readonly IUnitOfWork _unitOfWork;

        public UserService(QuikDbContext context, IMapper mapper, ILogger<UserService> logger, IEmailService emailService)
        {
            this.context = context;
            this.mapper = mapper;
            this._logger = logger;
            this.emailService = emailService;

        }

        //public async Task<User?> GetByVerificationToken(string token)
        //{
        //    var userRepository = _unitOfWork.Repository<User>();
        //    return await userRepository.GetAll().AsNoTracking()
        //        .FirstOrDefaultAsync(u => u.EmailVerificationToken == token);
        //}

        //public async Task<User?> Login(string email, string password)
        //{
        //    var userRepository = _unitOfWork.Repository<User>();
        //    var users = await userRepository.GetAllAsync();

        //    var user = users.FirstOrDefault(u => u.Email == email && u.Password == password);
        //    return user;
        //}

        //public async Task Add(User user)
        //{
        //    try
        //    {
        //        var userRepository = _unitOfWork.Repository<User>();
        //        var customerRepository = _unitOfWork.Repository<Business>();

        //        if (user is not null)
        //        {
        //            await _unitOfWork.BeginTransaction();
        //            // check duplicate user
        //            var existedUser = await userRepository.GetAsync(u => u.Username == user.Username || u.Email == user.Email);
        //            if (existedUser is not null) throw new Exception("User already exists.");

        //            await userRepository.InsertAsync(user);

        //            // add customer or driver into tables
        //            if (user.Role.Equals((int)UserRole.User))
        //            {
        //                var customer = mapper.Map<User>(user);
        //                await userRepository.InsertAsync(customer);
        //            }
        //            else if (user.Role.Equals((int)UserRole.Business))
        //            {
        //                var driver = mapper.Map<Business>(user);
        //                await customerRepository.InsertAsync(driver);
        //            }
        //            await _unitOfWork.CommitTransaction();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        await _unitOfWork.RollbackTransaction();
        //        throw;
        //    }
        //}


        //public async Task Update(User user)
        //{
        //    try
        //    {
        //        var userRepository = _unitOfWork.Repository<User>();
        //        if (user is not null)
        //        {
        //            await _unitOfWork.BeginTransaction();
        //            await userRepository.UpdateAsync(user);
        //            await _unitOfWork.CommitTransaction();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        await _unitOfWork.RollbackTransaction();
        //        throw;
        //    }
        //}

        public async Task<APIResponse> CreateUser(User user)
        {
            try
            {
                if (user == null)
                {
                    return new APIResponse
                    {
                        ResponseCode = 400,
                        Result = "Failure",
                        Message = "User cannot be null"
                    };
                }

                // Add the user to the database
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                // Create a response with the newly created user
                var userModal = mapper.Map<UserModal>(user);

                return new APIResponse
                {
                    ResponseCode = 201,
                    Result = "Success",
                    Message = "User created successfully.",
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user.");
                return new APIResponse
                {
                    ResponseCode = 500,
                    Result = "Failure",
                    Message = "Error creating user."
                };
            }
        }

        public async Task<List<UserModal>> GetAll()
        {
            List<UserModal> _response = new List<UserModal>();
            var _data = await context.Users.ToListAsync();
            if (_data != null)
            {
                _response = mapper.Map<List<User>, List<UserModal>>(_data);
            }
            return _response;

        }

        public async Task<List<BookingRequestModel>> GetBookingsOfUser(string username)
        {
            try
            {
                var bookings = await context.Users.Where(u => u.Username == username).ToListAsync();

                if (bookings == null || !bookings.Any())
                {
                    throw new Exception("No booking with this user");
                }

                var response = mapper.Map<List<BookingRequestModel>>(bookings);
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while retieving bookings" + ex.Message);
            }
        }

        public async Task<UserModal> GetByUserId(string username)
        {
            try
            {
                UserModal _response = new UserModal();
                var data = await context.Users.FindAsync(username);
                if (data != null)
                {
                    _response = mapper.Map<User, UserModal>(data);
                }
                return _response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user by ID.");
                return null;
            }
        }

        public async Task<APIResponse> ConfirmRegister(string userid, string username, int otptext)
        {
            APIResponse response = new APIResponse();
            bool otpresponse = await ValidateOTP(username, otptext.ToString());
            if (!otpresponse)
            {
                response.Result = "fail";
                response.Message = "Invalid OTP or Expired";
            }
            else
            {
                var _tempdata = await context.Tempusers.FirstOrDefaultAsync(item => item.Id.Equals(userid));
                var _user = new User()
                {
                    Name = _tempdata.Name,
                    Password = _tempdata.Password,
                    Email = _tempdata.Email,
                    PhoneNumber = _tempdata.Phone,
                    Role = "User"
                };
                await context.Users.AddAsync(_user);
                await context.SaveChangesAsync();
                await UpdatePWDManager(username, _tempdata.Password);
                response.Result = "pass";
                response.Message = "Registered successfully.";
            }
            return response;
        }

        public async Task<APIResponseData> UserRegisteration(UserRegister userRegister)
        {
            APIResponseData response = new APIResponseData();
            bool isvalid = true;

            try
            {
                // Check for duplicate username
                var existingUser = await context.Users
                    .Where(item => item.Username == userRegister.Username)
                    .FirstOrDefaultAsync();
                if (existingUser != null)
                {
                    isvalid = false;
                    response.ResponseCode = 409;
                    response.Result = "Failed";
                    response.Message = "Username already exists";
                    response.Data = existingUser;
                }

                // Check for duplicate email
                var existingEmail = await context.Users
                    .Where(item => item.Email == userRegister.Email)
                    .FirstOrDefaultAsync();
                if (existingEmail != null)
                {
                    isvalid = false;
                    response.ResponseCode = 409;
                    response.Result = "Failed";
                    response.Message = "Email already exists";
                    response.Data = existingEmail;
                }

                if (isvalid)
                {
                    // Map UserRegister to User entity
                    var newUser = mapper.Map<User>(userRegister);

                    // Hash the password before saving (ensure you have a method to hash passwords)
                    newUser.Password = HashPassword(userRegister.Password);

                    newUser.OTPVerified = false;
                    newUser.IsActive = true;
                    newUser.IsLocked = false;
                    newUser.Role = "User";  // Default role, can be modified as per your logic
                    newUser.ImageId = "default-image-id";  // Default ImageId if needed
                    newUser.Status = "Active";            // Default Status value

                    _logger.LogInformation("Registering new user: {Username}, {Email}", newUser.Username, newUser.Email);

                    // Add new user to the context
                    await context.Users.AddAsync(newUser);
                    await context.SaveChangesAsync();

                    // Send OTP (your existing logic)
                    string otpText = Generaterandomnumber();
                    await UpdateOtp(newUser.Username, otpText, "Register");
                    await SendOtpMail(newUser.Email, otpText, newUser.Name);

                    response.ResponseCode = 200;
                    response.Result = "Success";
                    response.Message = otpText;
                    response.Data = otpText;
                }
            }
            catch (DbUpdateException dbEx)
            {
                var innerExceptionMessage = dbEx.InnerException != null ? dbEx.InnerException.Message : dbEx.Message;
                _logger.LogError(dbEx, "Database error during user registration: {Message}", innerExceptionMessage);

                response.ResponseCode = 500;
                response.Result = "Failed";
                response.Message = "Database error: " + innerExceptionMessage;
                response.Data = innerExceptionMessage;
                return response;
            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _logger.LogError(ex, "Error during user registration: {Message}", innerExceptionMessage);

                response.ResponseCode = 500;
                response.Result = "Failed";
                response.Message = "Error: " + innerExceptionMessage;
                response.Data = innerExceptionMessage;
                return response;
            }


            return response;
        }

        private string HashPassword(string password)
        {
            // Use a proper hashing algorithm, e.g., BCrypt or SHA256
            return BCrypt.Net.BCrypt.HashPassword(password); // Example using BCrypt
        }


        public async Task<APIResponse> ResetPassword(string name, string oldpassword, string newpassword)
        {
            APIResponse response = new APIResponse();
            var _user = await context.Users.FirstOrDefaultAsync(item => item.Name == name &&
            item.Password == oldpassword && item.Status == "Active");
            if (_user != null)
            {
                var _pwdhistory = await Validatepwdhistory(name, newpassword);
                if (_pwdhistory)
                {
                    response.Result = "fail";
                    response.Message = "Don't use the same password that used in last 3 transaction";
                }
                else
                {
                    _user.Password = newpassword;
                    await context.SaveChangesAsync();
                    await UpdatePWDManager(name, newpassword);
                    response.Result = "pass";
                    response.Message = "Password changed.";
                }
            }
            else
            {
                response.Result = "fail";
                response.Message = "Failed to validate old password.";
            }
            return response;
        }

        public async Task<APIResponse> ForgetPassword(string name)
        {
            APIResponse response = new APIResponse();
            var _user = await context.Users.FirstOrDefaultAsync(item => item.Name == name && item.Status == "Active");
            if (_user != null)
            {
                string otptext = Generaterandomnumber();
                await UpdateOtp(name, otptext, "forgetpassword");
                await SendOtpMail(_user.Email, otptext, _user.Name);
                response.Result = "pass";
                response.Message = "OTP sent";

            }
            else
            {
                response.Result = "fail";
                response.Message = "Invalid User";
            }
            return response;
        }

        public async Task<APIResponse> UpdatePassword(string name, string Password, string Otptext)
        {
            APIResponse response = new APIResponse();

            bool otpvalidation = await ValidateOTP(name, Otptext);
            if (otpvalidation)
            {
                bool pwdhistory = await Validatepwdhistory(name, Password);
                if (pwdhistory)
                {
                    response.Result = "fail";
                    response.Message = "Don't use the same password that used in last 3 transaction";
                }
                else
                {
                    var _user = await context.Users.FirstOrDefaultAsync(item => item.Name == name && item.Status == "Active");
                    if (_user != null)
                    {
                        _user.Password = Password;
                        await context.SaveChangesAsync();
                        await UpdatePWDManager(name, Password);
                        response.Result = "pass";
                        response.Message = "Password changed";
                    }
                }
            }
            else
            {
                response.Result = "fail";
                response.Message = "Invalid OTP";
            }
            return response;
        }

        private async Task UpdateOtp(string username, string otptext, string otptype)
        {
            var _opt = new OtpManager()
            {
                Username = username,
                OtpText = otptext,
                Expiration = DateTime.Now.AddMinutes(30),
                CreatedDate = DateTime.Now,
                OtpType = otptype
            };
            await context.OtpManagers.AddAsync(_opt);
            await context.SaveChangesAsync();
        }

        private async Task<bool> ValidateOTP(string username, string OTPText)
        {
            bool response = false;
            var _data = await context.OtpManagers.FirstOrDefaultAsync(item => item.Username == username
            && item.OtpText == OTPText && item.Expiration > DateTime.Now);
            if (_data != null)
            {
                response = true;
            }
            return response;
        }

        private async Task UpdatePWDManager(string username, string password)
        {
            var _opt = new PwdManager()
            {
                Username = username,
                Password = password,
                ModifyDate = DateTime.Now
            };
            await context.PwdManagers.AddAsync(_opt);
            await context.SaveChangesAsync();
        }

        private string Generaterandomnumber()
        {
            Random random = new Random();
            string randomno = random.Next(0, 1000000).ToString("D6");
            return randomno;
        }

        private async Task SendOtpMail(string useremail, string OtpText, string Name)
        {
            var mailrequest = new MailRequest();
            mailrequest.ToEmail = useremail;
            mailrequest.Subject = "Thank for registering : OTP";
            mailrequest.Body = GenerateEmailBody(Name, OtpText);
        }

        private string GenerateEmailBody(string name, string otpText)
        {
            string emailbody = "<div style='width:100%;background-color:grey'>";
            emailbody += "<hi>Hi " + name + ", Thank for registering</h1>";
            emailbody += "<h2>Please enter OTP text and complete the registration</h2>";
            emailbody += "<h2>OTP Text is :" + otpText + "</h2>";
            emailbody += "</div>";

            return emailbody;
        }

        private async Task<bool> Validatepwdhistory(string Username, string password)
        {
            bool response = false;
            var _pwd = await context.PwdManagers.Where(item => item.Username == Username).
                OrderByDescending(p => p.ModifyDate).Take(3).ToListAsync();
            if (_pwd.Count > 0)
            {
                var validate = _pwd.Where(o => o.Password == password);
                if (validate.Any())
                {
                    response = true;
                }
            }
            return response;
        }

        public async Task<APIResponse> UpdateStatus(string name, string userstatus)
        {
            APIResponse response = new APIResponse();
            var _user = await context.Users.FirstOrDefaultAsync(item => item.Name == name);
            if (_user != null)
            {
                _user.Status = userstatus;
                await context.SaveChangesAsync();
                response.Result = "pass";
                response.Message = "User Status changed";
            }
            else
            {
                response.Result = "fail";
                response.Message = "Invalid User";
            }
            return response;
        }

        public async Task<APIResponse> UpdateRole(string name, string userrole)
        {
            APIResponse response = new APIResponse();
            var _user = await context.Users.FirstOrDefaultAsync(item => item.Name == name && item.Status == "Active");
            if (_user != null)
            {
                _user.Role = userrole;
                await context.SaveChangesAsync();
                response.Result = "pass";
                response.Message = "User Role changed";
            }
            else
            {
                response.Result = "fail";
                response.Message = "Invalid User";
            }
            return response;
        }

    }
}
