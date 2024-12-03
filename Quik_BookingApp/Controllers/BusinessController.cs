using Microsoft.AspNetCore.Mvc;
using Quik_BookingApp.DAO;
using Quik_BookingApp.Repos.Interface;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using Quik_BookingApp.BOs.Response;
using Quik_BookingApp.DAO.Models;
using Quik_BookingApp.Helper;
using Quik_BookingApp.BOs.Request;

namespace Quik_BookingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessController : ControllerBase
    {
        private readonly IBusinessService _service;
        private readonly QuikDbContext _dbContext;

        public BusinessController(IBusinessService service, QuikDbContext _dbContext)
        {
            this._service = service;
            this._dbContext = _dbContext;
        }

        [SwaggerOperation(
            Summary = "Retrieve all businesses",
            Description = "Gets a list of all registered businesses. If no businesses are found, a 404 Not Found response is returned."
        )]
        [HttpGet("GetAllBusiness")]
        public async Task<IActionResult> GetAllBusiness()
        {
            var data = await _service.GetAllBusiness();
            if (data == null || data.Count == 0)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [SwaggerOperation(
             Summary = "Retrieve all working spaces of a business",
             Description = "Gets a list of all working spaces for a business using the provided business ID. If no working spaces are found, a 404 Not Found response is returned."
        )]
        [HttpGet("GetWSsOfBusiness")]
        public async Task<IActionResult> GetListWSOfBusiness(string businessId)
        {
            try
            {
                var data = await _service.GetListWSOfBusiness(businessId);

                if (data == null || !data.Any())
                {
                    return NotFound("No working spaces found for this business.");
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request: " + ex.Message);
            }
        }

        [SwaggerOperation(
            Summary = "Retrieve a business by ID",
            Description = "Gets a business's details by providing the business ID. If the business is not found, a 404 Not Found response is returned."
        )]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBusinessById(string id)
        {
            var business = await _service.GetBusinessById(id);
            if (business == null)
            {
                return NotFound("Business not found.");
            }

            return Ok(business);
        }

        [SwaggerOperation(
             Summary = "Create a new business",
             Description = "Creates a new business and returns the created business information. If the email is already registered, an error message is returned."
         )]
        [HttpPost("CreateBusiness")]
        public async Task<IActionResult> RegisterBusiness([FromBody] BusinessRequestModel businessRequest)
        {
            if (businessRequest == null)
            {
                return BadRequest("Business request cannot be null.");
            }

            var result = await _service.RegisterBusiness(businessRequest);

            if (result.ResponseCode == 200)
            {
                return Ok(result);
            }
            else if (result.ResponseCode == 400)
            {
                return BadRequest(result.Message);
            }
            else
            {
                return StatusCode(500, result.Message);
            }
        }

        [HttpPut("UpdateBusiness/{businessId}")]
        [SwaggerOperation(
    Summary = "Update a business",
    Description = "Updates the details of a business based on the provided business ID and request model. If the business is not found, a 404 Not Found response is returned."
)]
        public async Task<IActionResult> UpdateBusiness(string businessId, [FromBody] UpdateBusinessModel businessRequest)
        {
            if (businessRequest == null)
            {
                return BadRequest("Business request cannot be null.");
            }

            var result = await _service.UpdateBusiness(businessId, businessRequest);

            if (result.ResponseCode == 200)
            {
                return Ok(result);
            }
            else if (result.ResponseCode == 404)
            {
                return NotFound(result.Message);
            }
            else
            {
                return StatusCode(500, result.Message);
            }
        }

    }
}
