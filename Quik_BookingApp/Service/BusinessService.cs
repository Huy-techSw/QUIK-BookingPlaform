using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quik_BookingApp.BOs.Request;
using Quik_BookingApp.BOs.Response;
using Quik_BookingApp.DAO;
using Quik_BookingApp.DAO.Models;
using Quik_BookingApp.Helper;
using Quik_BookingApp.Repos.Interface;

namespace Quik_BookingApp.Service
{
    public class BusinessService : IBusinessService
    {
        private readonly QuikDbContext _context;
        public readonly IMapper _mapper;
        public readonly ILogger<BusinessService> _logger;

        public BusinessService(QuikDbContext context, IMapper _mapper, ILogger<BusinessService> _logger)
        {
            _context = context;
            this._mapper = _mapper;
            this._logger = _logger;
        }

        //public async Task<List<BusinessResponseModel>> GetAllBusiness()
        //{
        //    List<BusinessResponseModel> _response = new List<BusinessResponseModel>();
        //    var _data = await _context.Businesses.ToListAsync();
        //    if (_data != null)
        //    {
        //        _response = _mapper.Map<List<Business>, List<BusinessResponseModel>>(_data);
        //    }
        //    return _response;
        //}

        public async Task<List<BusinessResponseRatingMode>> GetAllBusiness()
        {
            List<BusinessResponseRatingMode> _response = new List<BusinessResponseRatingMode>();

            // Get all businesses with their average rating from working spaces
            var _data = await _context.Businesses
                .Select(b => new
                {
                    Business = b,
                    AverageRating = b.WorkingSpaces.Any() ? b.WorkingSpaces.Average(ws => ws.Rating) : 0 // Calculate average rating
                })
                .ToListAsync();

            if (_data != null)
            {
                _response = _data.Select(item =>
                {
                    var businessResponseModel = _mapper.Map<BusinessResponseRatingMode>(item.Business);
                    businessResponseModel.Rating = item.AverageRating; // Add average rating to the model
                    return businessResponseModel;
                }).ToList();
            }

            return _response;
        }


        public async Task<List<WSWBNameResponse>> GetListWSOfBusiness(string businessId)
        {
            try
            {
                // Retrieve the list of working spaces for the specified businessId
                var workingSpaces = await _context.WorkingSpaces
                    .Where(ws => ws.BusinessId == businessId)
                    .Include(ws => ws.Business)
                    .ToListAsync();

                if (workingSpaces == null || !workingSpaces.Any())
                {
                    throw new Exception("No working spaces found for this business.");
                }

                // Map to response model (assuming you have AutoMapper configured)
                var _response = _mapper.Map<List<WSWBNameResponse>>(workingSpaces);

                return _response;
            }
            catch (Exception ex)
            {
                // It's a good practice to log the exception here
                throw new Exception("An error occurred while retrieving working spaces: " + ex.Message);
            }
        }


        public async Task<BusinessResponseModel> GetBusinessById(string bid)
        {
            try
            {
                // Find the business by its ID
                var business = await _context.Businesses.FindAsync(bid);
                if (business == null)
                {
                    throw new Exception("Business not found.");
                }

                // Map the business entity to BusinessResponseModel
                var businessResponse = _mapper.Map<BusinessResponseModel>(business);

                return businessResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving the business.");
                throw; // Rethrow the exception so it can be handled by the calling method
            }
        }

        public async Task<APIResponseData> RegisterBusiness(BusinessRequestModel businessRequest)
        {
            try
            {
                // Check if the business with the same email already exists
                var existingBusiness = await _context.Businesses
                    .FirstOrDefaultAsync(b => b.Email == businessRequest.Email);

                if (existingBusiness != null)
                {
                    return new APIResponseData
                    {
                        ResponseCode = 400,
                        Result = "Error",
                        Message = "A business with this email already exists.",
                        Data = existingBusiness
                    };
                }

                // Map the request model to the Business entity
                var newBusiness = new Business
                {
                    BusinessId = Guid.NewGuid().ToString(),
                    BusinessName = businessRequest.BusinessName,
                    PhoneNumber = businessRequest.PhoneNumber,
                    Email = businessRequest.Email,
                    Password = businessRequest.Password, // Make sure to hash this in a real implementation
                    Location = businessRequest.Location,
                    Description = businessRequest.Description
                };

                // Add the new business to the database
                _context.Businesses.Add(newBusiness);
                await _context.SaveChangesAsync();

                // Map the created business to the response model
                var businessResponse = _mapper.Map<BusinessResponseModel>(newBusiness);

                return new APIResponseData
                {
                    ResponseCode = 200,
                    Result = "Success",
                    Message = "Business created successfully.",
                    Data = businessResponse
                };
            }
            catch (Exception ex)
            {
                return new APIResponseData
                {
                    ResponseCode = 500,
                    Result = "Error",
                    Message = $"An error occurred: {ex.Message}",
                    Data = businessRequest.BusinessName
                };
            }
        }


        public async Task<APIResponseData> UpdateBusiness(string businessId, UpdateBusinessModel businessRequest)
        {
            try
            {
                var existingBusiness = await _context.Businesses.FindAsync(businessId);

                if (existingBusiness == null)
                {
                    return new APIResponseData
                    {
                        ResponseCode = 404,
                        Message = "Business not found",
                        Result = "Failed",
                        Data = existingBusiness
                    };
                }

                // Update the business entity with the new data from businessRequest
                existingBusiness.BusinessName = businessRequest.BusinessName;
                existingBusiness.PhoneNumber = businessRequest.PhoneNumber;
                existingBusiness.Email = businessRequest.Email;
                existingBusiness.Password = businessRequest.Password;
                existingBusiness.Location = businessRequest.Location;
                existingBusiness.Description = businessRequest.Description;

                _context.Businesses.Update(existingBusiness);
                await _context.SaveChangesAsync();

                var updatedBusiness = _mapper.Map<BusinessResponseModel>(existingBusiness);

                return new APIResponseData
                {
                    ResponseCode = 200,
                    Message = "Business updated successfully",
                    Result = "Success",
                    Data = updatedBusiness
                };
            }
            catch (Exception ex)
            {
                return new APIResponseData
                {
                    ResponseCode = 500,
                    Message = "An error occurred while updating the business: " + ex.Message,
                    Result = "Failed",
                    Data = null
                };
            }
        }
    }
}
