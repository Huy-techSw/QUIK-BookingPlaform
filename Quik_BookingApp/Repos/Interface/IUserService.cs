using Quik_BookingApp.BOs.Request;
using Quik_BookingApp.BOs.Response;
using Quik_BookingApp.DAO.Models;
using Quik_BookingApp.Helper;
using Quik_BookingApp.Modal;


namespace Quik_BookingApp.Repos.Interface
{
    public interface IUserService
    {
        Task<List<UserModal>> GetAll();
        Task<List<BookingRequestModel>> GetBookingsOfUser(string username);
        Task<UserModal> GetByUserId(string username);
        Task<APIResponse> CreateUser(User user);
        Task<APIResponse> ConfirmRegister(string userid, string username, int otptext);
        Task<APIResponseData> UserRegisteration(UserRegister userRegister);
        Task<APIResponse> ResetPassword(string username, string oldpassword, string newpassword);
        Task<APIResponse> ForgetPassword(string username);
        Task<APIResponse> UpdatePassword(string username, string Password, string Otptext);
        //Task<APIResponse> UpdateOtp(string username, string otpText, string otpType);
        //Task<bool> ValidateOTP(string username, string OTPText);
        //Task UpdatePWDManager(string username, string password);
        //Task<bool> Validatepwdhistory(string Username, string password);
        Task<APIResponse> UpdateStatus(string name, string userstatus);
        Task<APIResponse> UpdateRole(string name, string userrole);


        //Task Add(User user);

        //Task Update(User user);

        //Task<User?> Login(string email, string password);

        //Task<User?> GetByVerificationToken(string token);
    }
}
