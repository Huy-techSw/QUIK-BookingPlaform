using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quik_BookingApp.BOs.Request;
using Quik_BookingApp.BOs.Response;
using Quik_BookingApp.DAO;
using Quik_BookingApp.Repos.Interface;
using Swashbuckle.AspNetCore.Annotations;

namespace Quik_BookingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _service;
        private readonly QuikDbContext _dbContext;

        public BookingController(IBookingService service, QuikDbContext _dbContext)
        {
            this._service = service;
            this._dbContext = _dbContext;
        }

        [SwaggerOperation(
            Summary = "Retrieve all bookings",
            Description = "Gets a list of all bookings. If no bookings are found, a 404 Not Found response is returned."
        )]
        [HttpGet("GetAllBookings")]
        public async Task<IActionResult> GetAllBookings()
        {
            var data = await _service.GetAllBookings();
            if (data == null || data.Count == 0)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [SwaggerOperation(
            Summary = "Retrieve all bookings by username",
            Description = "Gets a list of all bookings for the specified username. If no bookings are found, a 404 Not Found response is returned."
        )]
        [HttpGet("GetAllBookingsByUsername/{username}")]
        public async Task<IActionResult> GetAllBookingsByUsername(string username)
        {
            try
            {
                var data = await _service.GetAllBookingsByUsername(username);

                if (data == null || !data.Any())
                {
                    return NotFound($"No bookings found for user {username}.");
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error occurred while retrieving bookings: {ex.Message}");
            }
        }


        [SwaggerOperation(
            Summary = "Create a new booking",
            Description = "Tạo mới booking không cần truyền bookingId"
        )]
        [HttpPost("CreateBooking")]
        public async Task<IActionResult> CreateBooking([FromBody] BookingRequestModel bookingDto)
        {
            if (bookingDto == null)
            {
                return BadRequest("Booking data is required.");
            }

            try
            {
                var createdBooking = await _service.BookSpace(bookingDto);
                return CreatedAtAction(nameof(GetBookingById), new { id = bookingDto.BookingId }, createdBooking);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error occurred while creating the booking: {ex.Message}");
            }
        }

        [SwaggerOperation(
            Summary = "Retrieve a booking by ID",
            Description = "Gets a booking's details by providing the booking ID. If the booking is not found, a 404 Not Found response is returned."
        )]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(string id)
        {
            var booking = await _service.GetBookingById(id);
            if (booking == null)
            {
                return NotFound("Booking not found.");
            }

            return Ok(booking);
        }

        [SwaggerOperation(
            Summary = "Update booking status to 'Đã thanh toán'",
            Description = "Updates the status of a specific booking to 'Đã thanh toán' given the booking ID."
        )]
        [HttpPut("update-status-to-paid/{bookingId}")]
        public async Task<IActionResult> UpdateStatusToPaid(string bookingId)
        {
            try
            {
                await _service.UpdateBookingStatusToPaid(bookingId);
                return Ok(new { Message = "Updated successfully" }); 
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [SwaggerOperation(
           Summary = "Update booking status to 'Chưa thanh toán'",
           Description = "Updates the status of a specific booking to 'Chưa thanh toán' given the booking ID."
       )]
        [HttpPut("update-status-to-unpaid/{bookingId}")]
        public async Task<IActionResult> UpdateStatusToUnPaid(string bookingId)
        {
            try
            {
                await _service.UpdateBookingStatusToUnPaid(bookingId);
                return Ok(new { Message = "Updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [SwaggerOperation(
            Summary = "Update a booking",
            Description = "Updates a booking by ID. The booking ID must match the provided booking data."
        )]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(string id, [FromBody] BookingResponseModel bookingDto)
        {
            if (id != bookingDto.BookingId)
            {
                return BadRequest("Booking ID mismatch.");
            }

            try
            {
                var updatedBooking = await _service.UpdateBooking(id, bookingDto);
                if (updatedBooking == null)
                {
                    return NotFound("Booking not found.");
                }

                return Ok(updatedBooking);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error occurred while updating the booking: {ex.Message}");
            }
        }

        [SwaggerOperation(
            Summary = "Delete a booking",
            Description = "Deletes a booking by its ID. Returns 204 No Content on success, or appropriate error messages if something goes wrong."
        )]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(string id)
        {
            try
            {
                var result = await _service.DeleteBooking(id);

                if (result.ResponseCode == 404) 
                {
                    return NotFound(result.Message); 
                }
                else if (result.ResponseCode == 500) 
                {
                    return StatusCode(500, result.Message); 
                }

                return NoContent(); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error occurred while deleting the booking: {ex.Message}");
            }
        }

        [SwaggerOperation(
                    Summary = "All booking Da thanh toan",
                    Description = "Gets a list of all working spaces. If no working spaces are found, a 404 Not Found response is returned."
               )]
        [HttpGet("GetBookingOfDaThanhToan")]
        public async Task<IActionResult> GetBookingOfDaThanhToan()
        {
            try
            {
                var data = await _service.GetBookingOfDaThanhToan();

                // If no data is found, return a NotFound response
                if (data == null || !data.Any())
                {
                    return NotFound("No bookings found for this username.");
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(500, "An error occurred while processing your request: " + ex.Message);
            }
        }

        [SwaggerOperation(
                    Summary = "All booking Chua thanh toan",
                    Description = "Gets a list of all working spaces. If no working spaces are found, a 404 Not Found response is returned."
               )]
        [HttpGet("GetBookingOfChuaThanhToan")]
        public async Task<IActionResult> GetBookingOfChuaThanhToan()
        {
            try
            {
                var data = await _service.GetBookingOfChuaThanhToan();

                // If no data is found, return a NotFound response
                if (data == null || !data.Any())
                {
                    return NotFound("No bookings found for this username.");
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(500, "An error occurred while processing your request: " + ex.Message);
            }
        }

    }
}

