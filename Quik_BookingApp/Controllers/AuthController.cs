using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quik_BookingApp.BOs.Request;
using Quik_BookingApp.BOs.Response;
using Quik_BookingApp.DAO.Models;
using Quik_BookingApp.Enums;
using Quik_BookingApp.Helper;
using Quik_BookingApp.Repos.Interface;
using Quik_BookingApp.Service;
using System.Net;
using System.Security.Policy;

namespace Quik_BookingApp.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    //public class AuthController : ControllerBase
    //{
    //    private readonly IUserService _userService;
    //    private readonly IEmailService _emailService;
    //    private readonly TokenService _tokenService;
    //    private readonly IMapper _mapper;

    //    public AuthController(IUserService userService,
    //        IEmailService emailService,
    //        TokenService tokenService,
    //        IMapper mapper)
    //    {
    //        _userService = userService;
    //        _emailService = emailService;
    //        _tokenService = tokenService;
    //        _mapper = mapper;
    //    }

    //    /// <summary>
    //    /// Login to the system
    //    /// </summary>
    //    /// <param name="request"></param>
    //    /// <remarks>
    //    /// 
    //    /// </remarks>
    //    [HttpPost("login")]
    //    [AllowAnonymous]
    //    [ProducesResponseType(StatusCodes.Status200OK)]
    //    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //    public async Task<ActionResult<ApiResponseModel<UserResponseModel>>> Login([FromBody] LoginModel request)
    //    {
    //        if (!ModelState.IsValid) return BadRequest();
    //        var result = await _userService.Login(request.Email, request.Password);
    //        if (result is null) return NotFound(new ApiResponseModel<string>
    //        {
    //            StatusCode = HttpStatusCode.NotFound,
    //            Message = "User not found"
    //        });
    //        var accessToken = _tokenService.GenerateToken(result, new List<string> { result.Role.ToString() });
    //        var response = _mapper.Map<UserResponseModel>(result);
    //        response.AccessToken = accessToken;

    //        return Ok(new ApiResponseModel<UserResponseModel>
    //        {
    //            StatusCode = HttpStatusCode.OK,
    //            Message = "Success",
    //            Data = response
    //        });
    //    }

    //    /// <summary>
    //    /// Verifies the user's email based on the provided token.
    //    /// </summary>
    //    /// <param name="token">The token used to verify the email.</param>
    //    /// <returns>A response message indicating the result of the verification process.</returns>
    //    [HttpGet("verify-email")]
    //    [ProducesResponseType(StatusCodes.Status200OK)]
    //    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    //    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //    public async Task<ActionResult<string>> VerifyEmail(string token)
    //    {
    //        if (string.IsNullOrEmpty(token)) return BadRequest(new ApiResponseModel<string>
    //        {
    //            StatusCode = HttpStatusCode.BadRequest,
    //            Message = "Invalid verified email token!"
    //        });
    //        if (!ModelState.IsValid) return BadRequest();
    //        var user = await _userService.GetByVerificationToken(token);
    //        if (user == null)
    //            return NotFound(new ApiResponseModel<string>
    //            {
    //                StatusCode = HttpStatusCode.NotFound,
    //                Message = "User is not found!"
    //            });

    //        user.EmailVerified = true;
    //        await _userService.Update(user);

    //        return Ok(new ApiResponseModel<string>
    //        {
    //            Message = "Verified successfully!",
    //            StatusCode = HttpStatusCode.OK
    //        });
    //    }

    //    /// <summary>
    //    /// Sign up into Uber System
    //    /// </summary>
    //    /// <param name="request"></param>
    //    /// <returns></returns>
    //    [HttpPost("sign-up")]
    //    [AllowAnonymous]
    //    [ProducesResponseType(StatusCodes.Status200OK)]
    //    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    //    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    //    public async Task<ActionResult<ApiResponseModel<string>>> Signup([FromBody] SignupModel request)
    //    {
    //        if (request.Id <= 0)
    //            return BadRequest(new ApiResponseModel<string>
    //            {
    //                StatusCode = HttpStatusCode.BadRequest,
    //                Message = "Invalid user's id!"
    //            });
    //        if (!ModelState.IsValid) return BadRequest();
    //        // Authenticate for role
    //        if (request.Role != (int)UserRole.User && request.Role != (int)UserRole.Business && request.Role != (int)UserRole.Admin)
    //            return BadRequest(new ApiResponseModel<string>
    //            {
    //                StatusCode = HttpStatusCode.BadRequest,
    //                Message = "Invalid role's value in the system!"
    //            });
    //        var user = _mapper.Map<User>(request);
    //        // Generate verified email token
    //        user.EmailVerificationToken = Guid.NewGuid().ToString();
    //        await _userService.Add(user);
    //        // Send verification email
    //        var verificationLink = Url.Action(nameof(VerifyEmail), "Auth",
    //            new { token = user.EmailVerificationToken }, Request.Scheme);
    //        await _emailService.SendVerificationEmailAsync(request.Email, verificationLink);

    //        return Ok(new ApiResponseModel<string>
    //        {
    //            StatusCode = HttpStatusCode.OK,
    //            Message = "Success! Please verify your email",
    //            Data = user.EmailVerificationToken
    //        });
    //    }

    //}
}
