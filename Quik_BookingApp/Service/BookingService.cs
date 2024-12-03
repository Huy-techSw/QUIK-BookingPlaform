using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Quik_BookingApp.BOs.Request;
using Quik_BookingApp.BOs.Response;
using Quik_BookingApp.DAO;
using Quik_BookingApp.DAO.Models;
using Quik_BookingApp.Helper;
using Quik_BookingApp.Modal;
using Quik_BookingApp.Repos.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quik_BookingApp.Service
{
    public class BookingService : IBookingService
    {
        private readonly QuikDbContext _context;
        public readonly IMapper _mapper;
        private readonly EmailService _emailService;
        public readonly ILogger<BookingService> _logger;
        public const decimal CommissionPerPerson = 4000;

        public BookingService(QuikDbContext context, IMapper _mapper, ILogger<BookingService> _logger, EmailService emailService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            this._mapper = _mapper ?? throw new ArgumentNullException(nameof(_mapper));
            this._logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
            _emailService = emailService;
        }

        public async Task<List<BookingResponseModel>> GetAllBookings()
        {
            List<BookingResponseModel> _response = new List<BookingResponseModel>();

            // Fetch bookings with related WorkingSpace, Business, and Images data
            var _data = await _context.Bookings
                .Include(b => b.WorkingSpace)
                    .ThenInclude(ws => ws.Business)
                .Include(b => b.WorkingSpace.Images)
                .ToListAsync();

            if (_data != null)
            {
                _response = _data.Select(booking => new BookingResponseModel
                {
                    BookingId = booking.BookingId,
                    Username = booking.Username,
                    SpaceId = booking.SpaceId,
                    PaymentId = booking.PaymentId,
                    BookingDate = booking.BookingDate,
                    StartTime = booking.StartTime,
                    EndTime = booking.EndTime,
                    NumberOfPeople = booking.NumberOfPeople,
                    TotalAmount = booking.TotalAmount,
                    DepositAmount = booking.DepositAmount,
                    RemainingAmount = booking.RemainingAmount,

                    // Map Location from WorkingSpace
                    Location = booking.WorkingSpace?.Location,

                    Status = booking.Status,

                    // Map additional data
                    BusinessName = booking.WorkingSpace?.Business?.BusinessName,
                    Title = booking.WorkingSpace?.Title,
                    Description = booking.WorkingSpace?.Description,
                    PricePerHour = booking.WorkingSpace?.PricePerHour ?? 0,
                    RoomType = booking.WorkingSpace?.RoomType,
                    Capacity = booking.WorkingSpace?.Capacity ?? 0,
                    ImageUrl = booking.WorkingSpace?.Images.FirstOrDefault()?.ImageUrl
                }).ToList();
            }

            return _response;
        }



        public async Task<List<BookingResponseModel>> GetBookingOfDaThanhToan()
        {
            try
            {
                // Retrieve the list of bookings for the specified user with status 'Đã thanh toán'
                var bookings = await _context.Bookings
                    .Where(b => b.Status.Equals("Đã thanh toán"))
                    .ToListAsync();

                if (bookings == null || !bookings.Any())
                {
                    throw new Exception("No paid bookings found for this user.");
                }

                var bookingResponseList = new List<BookingResponseModel>();

                foreach (var booking in bookings)
                {
                    // Fetch the WorkingSpace associated with each booking
                    var workingSpace = await _context.WorkingSpaces
                        .Include(ws => ws.Business)
                        .Include(ws => ws.Images)
                        .FirstOrDefaultAsync(ws => ws.SpaceId == booking.SpaceId);

                    if (workingSpace == null)
                    {
                        throw new Exception($"Working space with ID {booking.SpaceId} not found.");
                    }

                    // Map the booking and working space data into BookingResponseModel
                    var bookingResponse = new BookingResponseModel
                    {
                        BookingId = booking.BookingId,
                        Username = booking.Username,
                        SpaceId = booking.SpaceId,
                        PaymentId = booking.PaymentId,
                        BookingDate = booking.BookingDate,
                        StartTime = booking.StartTime,
                        EndTime = booking.EndTime,
                        NumberOfPeople = booking.NumberOfPeople,
                        TotalAmount = booking.TotalAmount,
                        DepositAmount = booking.DepositAmount,
                        RemainingAmount = booking.RemainingAmount,
                        Status = booking.Status,
                        Location = workingSpace.Location,
                        BusinessName = workingSpace.Business?.BusinessName,
                        Title = workingSpace.Title,
                        Description = workingSpace.Description,
                        PricePerHour = booking.WorkingSpace?.PricePerHour ?? 0,
                        RoomType = workingSpace.RoomType,
                        Capacity = booking.WorkingSpace?.Capacity ?? 0,
                        ImageUrl = workingSpace.Images.FirstOrDefault()?.ImageUrl
                    };

                    bookingResponseList.Add(bookingResponse);
                }

                return bookingResponseList;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving paid bookings: " + ex.Message);
            }
        }

        public async Task<List<BookingResponseModel>> GetBookingOfChuaThanhToan()
        {
            try
            {
                // Retrieve the list of bookings for the specified user with status 'Đã thanh toán'
                var bookings = await _context.Bookings
                    .Where(b => b.Status.Equals("Chưa thanh toán"))
                    .ToListAsync();

                if (bookings == null || !bookings.Any())
                {
                    throw new Exception("No paid bookings found for this user.");
                }

                var bookingResponseList = new List<BookingResponseModel>();

                foreach (var booking in bookings)
                {
                    // Fetch the WorkingSpace associated with each booking
                    var workingSpace = await _context.WorkingSpaces
                        .Include(ws => ws.Business)
                        .Include(ws => ws.Images)
                        .FirstOrDefaultAsync(ws => ws.SpaceId == booking.SpaceId);

                    if (workingSpace == null)
                    {
                        throw new Exception($"Working space with ID {booking.SpaceId} not found.");
                    }

                    // Map the booking and working space data into BookingResponseModel
                    var bookingResponse = new BookingResponseModel
                    {
                        BookingId = booking.BookingId,
                        Username = booking.Username,
                        SpaceId = booking.SpaceId,
                        PaymentId = booking.PaymentId,
                        BookingDate = booking.BookingDate,
                        StartTime = booking.StartTime,
                        EndTime = booking.EndTime,
                        NumberOfPeople = booking.NumberOfPeople,
                        TotalAmount = booking.TotalAmount,
                        DepositAmount = booking.DepositAmount,
                        RemainingAmount = booking.RemainingAmount,
                        Status = booking.Status,
                        Location = workingSpace.Location,
                        BusinessName = workingSpace.Business?.BusinessName,
                        Title = workingSpace.Title,
                        Description = workingSpace.Description,
                        PricePerHour = booking.WorkingSpace?.PricePerHour ?? 0,
                        RoomType = workingSpace.RoomType,
                        Capacity = booking.WorkingSpace?.Capacity ?? 0,
                        ImageUrl = workingSpace.Images.FirstOrDefault()?.ImageUrl
                    };

                    bookingResponseList.Add(bookingResponse);
                }

                return bookingResponseList;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving paid bookings: " + ex.Message);
            }
        }





        public async Task<APIResponseData> BookSpace(BookingRequestModel bookingRequest)
        {
            try
            {
                // Validate NumberOfPeople
                if (bookingRequest.NumberOfPeople <= 0)
                {
                    return new APIResponseData
                    {
                        ResponseCode = 400,
                        Result = "Failed",
                        Message = "Number of people must be greater than zero.",
                    };
                }

                // Validate StartTime and EndTime
                if (bookingRequest.StartTime >= bookingRequest.EndTime)
                {
                    return new APIResponseData
                    {
                        ResponseCode = 400,
                        Result = "Failed",
                        Message = "Start time must be before end time.",
                    };
                }

                // Validate that EndTime is at least 1 hour after StartTime
                if ((bookingRequest.EndTime - bookingRequest.StartTime).TotalHours < 1)
                {
                    return new APIResponseData
                    {
                        ResponseCode = 400,
                        Result = "Failed",
                        Message = "End time must be at least 1 hour after start time.",
                    };
                }

                var space = await _context.WorkingSpaces.FindAsync(bookingRequest.SpaceId);
                if (space == null)
                {
                    _logger.LogWarning("Working space with ID {SpaceId} not found.", bookingRequest.SpaceId);
                    return new APIResponseData
                    {
                        ResponseCode = 404,
                        Result = "Failed",
                        Message = "Working space not found.",
                    };
                }

                // Check for existing bookings
                var existingBooking = await _context.Bookings
                    .AnyAsync(b => b.SpaceId == bookingRequest.SpaceId &&
                                   b.StartTime < bookingRequest.EndTime &&
                                   b.EndTime > bookingRequest.StartTime);

                if (existingBooking)
                {
                    return new APIResponseData
                    {
                        ResponseCode = 409,
                        Result = "Failed",
                        Message = "The workspace is already booked for the selected time.",
                    };
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == bookingRequest.Username);
                if (user == null)
                {
                    _logger.LogWarning("User with username {Username} not found.", bookingRequest.Username);
                    return new APIResponseData
                    {
                        ResponseCode = 404,
                        Result = "Failed",
                        Message = "User not found.",
                    };
                }

                var durationInHours = (bookingRequest.EndTime - bookingRequest.StartTime).TotalHours;
                var totalAmount = space.PricePerHour * (decimal)durationInHours;

                // Additional checks for non-negative amounts
                if (totalAmount < 0)
                {
                    return new APIResponseData
                    {
                        ResponseCode = 400,
                        Result = "Failed",
                        Message = "Total amount cannot be negative.",
                    };
                }

                var booking = new Booking
                {
                    BookingId = Guid.NewGuid().ToString(),
                    Username = bookingRequest.Username,
                    SpaceId = bookingRequest.SpaceId,
                    StartTime = bookingRequest.StartTime,
                    EndTime = bookingRequest.EndTime,
                    NumberOfPeople = bookingRequest.NumberOfPeople,
                    BookingDate = DateTime.Now,
                    TotalAmount = totalAmount,
                    DepositAmount = CommissionPerPerson * bookingRequest.NumberOfPeople,
                    RemainingAmount = totalAmount - CommissionPerPerson * bookingRequest.NumberOfPeople,
                    Status = "Chưa thanh toán",
                };

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                string subject = "Xác nhận đặt phòng thành công";
                string body = $@"
            <h2>Cảm ơn bạn đã đặt phòng với Quik Booking!</h2>
            <p>Chi tiết đặt phòng của bạn:</p>
            <ul>
                <li>Mã đặt phòng: {booking.BookingId}</li>
                <li>Không gian: {space.Description}</li>
                <li>Thời gian: {booking.StartTime} đến {booking.EndTime}</li>
                <li>Tiền cần thanh toán: {booking.RemainingAmount} VND</li>
                <li>------------------------------------------------------</li>
                <li>Cảm ơn bạn đã tin dùng QUIK !</li>
            </ul>";

                await _emailService.SendEmailAsync(user.Email, subject, body);
                _logger.LogInformation("Booking created successfully for user {UserId} and space {SpaceId}.", user.Email, bookingRequest.SpaceId);

                var bookingResponse = new BookingResponseModel
                {
                    BookingId = booking.BookingId,
                    Username = booking.Username,
                    SpaceId = booking.SpaceId,
                    StartTime = booking.StartTime,
                    EndTime = booking.EndTime,
                    NumberOfPeople = booking.NumberOfPeople,
                    BookingDate = booking.BookingDate,
                    TotalAmount = booking.TotalAmount,
                    DepositAmount = booking.DepositAmount,
                    RemainingAmount = booking.RemainingAmount,
                    Status = booking.Status,
                    Location = space.Location,
                    PaymentId = Guid.NewGuid(),
                };

                return new APIResponseData
                {
                    ResponseCode = 201,
                    Result = "Success",
                    Message = "Booking created successfully with PaymentId.",
                    Data = bookingResponse
                };
            }
            catch (DbUpdateException dbEx)
            {
                var innerExceptionMessage = dbEx.InnerException != null ? dbEx.InnerException.Message : dbEx.Message;
                _logger.LogError(dbEx, "Database error while creating booking: {Message}", innerExceptionMessage);

                return new APIResponseData
                {
                    ResponseCode = 500,
                    Result = "Failed",
                    Message = "Database error: " + innerExceptionMessage,
                    Data = dbEx
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the booking.");

                return new APIResponseData
                {
                    ResponseCode = 500,
                    Result = "Failed",
                    Message = "An unexpected error occurred: " + ex.Message,
                    Data = ex
                };
            }
        }



        public async Task<BusinessResponseModel> GetBookingById(string bookingId)
        {
            try
            {
                var booking = await _context.Bookings.FindAsync(bookingId);
                if (booking == null)
                {
                    return null;
                }
                var bookingModal = _mapper.Map<BusinessResponseModel>(booking);
                return bookingModal;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving booking by ID.");
                return null;
            }
        }

        public async Task<List<BookingResponseModel>> GetAllBookingsByUsername(string username)
        {
            try
            {
                // Retrieve all bookings for the specified user
                var bookings = await _context.Bookings
                    .Where(b => b.Username == username)
                    .ToListAsync();

                if (bookings == null || !bookings.Any())
                {
                    throw new Exception($"No bookings found for user {username}.");
                }

                var bookingResponseList = new List<BookingResponseModel>();

                foreach (var booking in bookings)
                {
                    // Fetch the WorkingSpace associated with each booking
                    var workingSpace = await _context.WorkingSpaces
                        .Include(ws => ws.Business)
                        .Include(ws => ws.Images)
                        .FirstOrDefaultAsync(ws => ws.SpaceId == booking.SpaceId);

                    if (workingSpace == null)
                    {
                        throw new Exception($"Working space with ID {booking.SpaceId} not found.");
                    }

                    // Map the booking and working space data into BookingResponseModel
                    var bookingResponse = new BookingResponseModel
                    {
                        BookingId = booking.BookingId,
                        Username = booking.Username,
                        SpaceId = booking.SpaceId,
                        PaymentId = booking.PaymentId,
                        BookingDate = booking.BookingDate,
                        StartTime = booking.StartTime,
                        EndTime = booking.EndTime,
                        NumberOfPeople = booking.NumberOfPeople,
                        TotalAmount = booking.TotalAmount,
                        DepositAmount = booking.DepositAmount,
                        RemainingAmount = booking.RemainingAmount,

                        // Map Location from WorkingSpace
                        Location = booking.WorkingSpace?.Location,

                        Status = booking.Status,

                        // Map additional data
                        BusinessName = booking.WorkingSpace?.Business?.BusinessName,
                        Title = booking.WorkingSpace?.Title,
                        Description = booking.WorkingSpace?.Description,
                        PricePerHour = booking.WorkingSpace?.PricePerHour ?? 0,
                        RoomType = booking.WorkingSpace?.RoomType,
                        Capacity = booking.WorkingSpace?.Capacity ?? 0,
                        ImageUrl = booking.WorkingSpace?.Images.FirstOrDefault()?.ImageUrl
                    };

                    bookingResponseList.Add(bookingResponse);
                }

                return bookingResponseList;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving bookings: " + ex.Message);
            }
        }


        public async Task<BusinessResponseModel> UpdateBookingStatusToPaid(string bookingId)
        {
            try
            {
                // Find the booking by ID
                var booking = await _context.Bookings.FindAsync(bookingId);
                if (booking == null)
                {
                    throw new Exception("Booking not found.");
                }

                // Update the booking status to "Đã thanh toán"
                booking.Status = "Đã thanh toán";

                // Save the changes to the database
                _context.Bookings.Update(booking);
                await _context.SaveChangesAsync();

                // Retrieve the business information related to the working space
                var workingSpace = await _context.WorkingSpaces.FindAsync(booking.SpaceId);
                if (workingSpace == null)
                {
                    throw new Exception("Working space not found.");
                }

                var business = await _context.Businesses.FindAsync(workingSpace.BusinessId);
                if (business == null)
                {
                    throw new Exception("Business not found.");
                }

                // Map business to response model
                var businessResponse = _mapper.Map<BusinessResponseModel>(business);

                return businessResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating the booking status.");
                throw;
            }
        }

        public async Task<BusinessResponseModel> UpdateBookingStatusToUnPaid(string bookingId)
        {
            try
            {
                // Find the booking by ID
                var booking = await _context.Bookings.FindAsync(bookingId);
                if (booking == null)
                {
                    throw new Exception("Booking not found.");
                }

                // Update the booking status to "Chưa thanh toán"
                booking.Status = "Chưa thanh toán";

                // Save the changes to the database
                _context.Bookings.Update(booking);
                await _context.SaveChangesAsync();

                // Retrieve the business information related to the working space
                var workingSpace = await _context.WorkingSpaces.FindAsync(booking.SpaceId);
                if (workingSpace == null)
                {
                    throw new Exception("Working space not found.");
                }

                var business = await _context.Businesses.FindAsync(workingSpace.BusinessId);
                if (business == null)
                {
                    throw new Exception("Business not found.");
                }

                // Map business to response model
                var businessResponse = _mapper.Map<BusinessResponseModel>(business);

                return businessResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating the booking status.");
                throw;
            }
        }


        public async Task<BusinessResponseModel> UpdateBooking(string bookingId, BookingResponseModel bookingRequest)
        {
            try
            {
                var booking = await _context.Bookings.FindAsync(bookingId);
                if (booking == null)
                {
                    throw new Exception("Booking not found.");
                }

                var workingSpace = await _context.WorkingSpaces.FindAsync(bookingRequest.SpaceId);
                if (workingSpace == null)
                {
                    throw new Exception("Working space not found.");
                }

                booking.SpaceId = bookingRequest.SpaceId;
                booking.StartTime = bookingRequest.StartTime;
                booking.EndTime = bookingRequest.EndTime;
                booking.TotalAmount = (booking.EndTime - booking.StartTime).Hours * workingSpace.PricePerHour;


                _context.Bookings.Update(booking);
                await _context.SaveChangesAsync();

                var business = await _context.Businesses.FindAsync(workingSpace.BusinessId);
                if (business == null)
                {
                    throw new Exception("Business not found.");
                }

                var businessResponse = _mapper.Map<BusinessResponseModel>(business);

                return businessResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating the booking.");
                throw;
            }
        }


        public async Task<APIResponse> DeleteBooking(string bookingId)
        {
            try
            {
                var booking = await _context.Bookings.FindAsync(bookingId);
                if (booking == null)
                {
                    return new APIResponse
                    {
                        ResponseCode = 404,
                        Result = "Failed",
                        Message = "Booking not found."
                    };
                }

                _context.Bookings.Remove(booking); // Xóa đặt chỗ khỏi cơ sở dữ liệu
                await _context.SaveChangesAsync();

                return new APIResponse
                {
                    ResponseCode = 204,
                    Result = "Success",
                    Message = "Booking deleted successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting the booking.");
                return new APIResponse
                {
                    ResponseCode = 500,
                    Result = "Failed",
                    Message = "Error occurred while deleting the booking.",
                };
            }
        }

    }
}
