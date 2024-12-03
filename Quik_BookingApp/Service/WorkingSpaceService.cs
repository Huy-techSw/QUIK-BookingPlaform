using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using Quik_BookingApp.BOs.Request;
using Quik_BookingApp.BOs.Response;
using Quik_BookingApp.DAO;
using Quik_BookingApp.DAO.Models;
using Quik_BookingApp.Helper;
using Quik_BookingApp.Repos.Interface;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;


namespace Quik_BookingApp.Service
{
    public class WorkingSpaceService : IWorkingSpaceService
    {
        public readonly QuikDbContext context;
        public readonly IMapper mapper;
        public readonly ILogger<WorkingSpaceService> _logger;
        public readonly IFirebaseService _firebase;
        public readonly IConfiguration _configuration;

        public WorkingSpaceService(QuikDbContext context, IMapper mapper, ILogger<WorkingSpaceService> logger,IFirebaseService firebaseService, IConfiguration configuration)
        {
            this.context = context;
            this.mapper = mapper;
            this._logger = logger;
            this._firebase = firebaseService ?? throw new ArgumentNullException(nameof(firebaseService));
            this._configuration = configuration;
        }

        public async Task<APIResponseData> CreateWS(WorkingSpaceRequestModel ws)
        {
            try
            {
                if (ws == null)
                {
                    return new APIResponseData
                    {
                        ResponseCode = 400,
                        Result = "Failure",
                        Message = "Working space cannot be null",
                        Data = ws
                    };
                }

                if (ws.PricePerHour <= 1000)
                {
                    return new APIResponseData
                    {
                        ResponseCode = 400,
                        Result = "Failure",
                        Message = "Price per hour must be greater than 1000.",
                        Data = ws
                    };
                }

                if (ws.Capacity <= 0)
                {
                    return new APIResponseData
                    {
                        ResponseCode = 400,
                        Result = "Failure",
                        Message = "Capacity must be greater than 0.",
                        Data = ws
                    };
                }

                _logger.LogInformation("Attempting to retrieve business with ID: {BusinessId}", ws.BusinessId);

                var business = await context.Businesses.FindAsync(ws.BusinessId);
                if (business == null)
                {
                    return new APIResponseData
                    {
                        ResponseCode = 400,
                        Result = "Failure",
                        Message = "Business not found.",
                        Data = ws
                    };
                }

                var workingSpace = new WorkingSpace
                {
                    SpaceId = Guid.NewGuid().ToString(),
                    BusinessId = business.BusinessId,
                    ImageId = Guid.NewGuid().ToString(),
                    Title = ws.Title,
                    Description = ws.Description,
                    PricePerHour = ws.PricePerHour,
                    Capacity = ws.Capacity,
                    Location = ws.Location,
                    RoomType = ws.RoomType,
                    Images = new List<ImageWS>()
                };

                if (ws.Image != null && ws.Image.Length > 0)
                {
                    var uploadResult = await _firebase.UploadFileToFirebase(ws.Image, $"workingspaces/{ws.Title}_{DateTime.Now.Ticks}");

                    if (uploadResult.Status == 200)
                    {
                        var newImageWS = new ImageWS
                        {
                            ImageUrl = uploadResult.Data.ToString(),
                            ImageId = Guid.NewGuid().ToString(),
                            SpaceId = ws.SpaceId,
                            WorkingSpaceName = workingSpace.Title,
                            WSCode = "Updated",
                        };
                        workingSpace.Images.Add(newImageWS);

                        _logger.LogInformation("Image uploaded successfully to Firebase for working space: {Title}", ws.Title);
                    }
                    else
                    {
                        return new APIResponseData
                        {
                            ResponseCode = 500,
                            Result = "Failure",
                            Message = "Failed to upload image to Firebase.",
                            Data = ws
                        };
                    }
                }
                else
                {
                    var defaultImageWS = new ImageWS
                    {
                        ImageUrl = _configuration["DefaultImageUrl"]
                    };
                    workingSpace.Images.Add(defaultImageWS);

                    _logger.LogInformation("No image uploaded, using default image for working space: {Title}", ws.Title);
                }

                _logger.LogInformation("Creating working space: {Title} for Business ID: {BusinessId}", workingSpace.Title, workingSpace.BusinessId);

                await context.WorkingSpaces.AddAsync(workingSpace);
                await context.SaveChangesAsync();

                var workingSpaceModel = mapper.Map<WorkingSpace>(workingSpace);

                return new APIResponseData
                {
                    ResponseCode = 201,
                    Result = "Success",
                    Message = "Working space created successfully.",
                    Data = workingSpaceModel
                };
            }
            catch (DbUpdateException dbEx)
            {
                var innerExceptionMessage = dbEx.InnerException != null ? dbEx.InnerException.Message : dbEx.Message;
                _logger.LogError(dbEx, "Database error during working space creation: {Message}", innerExceptionMessage);

                return new APIResponseData
                {
                    ResponseCode = 500,
                    Result = "Failure",
                    Message = "Database error: " + innerExceptionMessage,
                    Data = ws
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating working space: {Message}", ex.ToString());
                return new APIResponseData
                {
                    ResponseCode = 500,
                    Result = "Failure",
                    Message = "Error creating working space.",
                    Data = ws
                };
            }
        }





        public async Task<List<WorkingSpaceRequestRatingMode>> GetAll(string? location = null, string? type =null)
        {
            List<WorkingSpaceRequestRatingMode> _response = new List<WorkingSpaceRequestRatingMode>();

            var query = context.WorkingSpaces
                .Select(ws => new
                {
                    WorkingSpace = ws,
                    AverageRating = ws.Reviews.Any() ? ws.Reviews.Average(r => r.Rating) : 0,
                    BusinessName = ws.Business.BusinessName,
                    ImageUrls = ws.Images.Select(img => img.ImageUrl).ToList()
                });

            // Apply location filter if location parameter is provided
            if (!string.IsNullOrEmpty(location))
            {
                query = query.Where(ws => EF.Functions.Like(ws.WorkingSpace.Location, $"%{location}%"));
            }
            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(ws => EF.Functions.Like(ws.WorkingSpace.RoomType, $"%{type}%"));
            }

            var _data = await query.ToListAsync();

            if (_data != null)
            {
                _response = _data.Select(item =>
                {
                    var workingSpaceModel = mapper.Map<WorkingSpaceRequestRatingMode>(item.WorkingSpace);
                    workingSpaceModel.Rating = item.AverageRating;
                    workingSpaceModel.BusinessName = item.BusinessName;
                    workingSpaceModel.ImageUrls = item.ImageUrls;
                    return workingSpaceModel;
                }).ToList();
            }

            return _response;
        }

        //public async Task<List<WorkingSpaceRequestRatingMode>> GetAllType(string? type = null)
        //{
        //    List<WorkingSpaceRequestRatingMode> _response = new List<WorkingSpaceRequestRatingMode>();

        //    var query = context.WorkingSpaces
        //        .Select(ws => new
        //        {
        //            WorkingSpace = ws,
        //            AverageRating = ws.Reviews.Any() ? ws.Reviews.Average(r => r.Rating) : 0,
        //            BusinessName = ws.Business.BusinessName,
        //            ImageUrls = ws.Images.Select(img => img.ImageUrl).ToList()
        //        });

        //    // Apply location filter if location parameter is provided
        //    if (!string.IsNullOrEmpty(type))
        //    {
        //        query = query.Where(ws => EF.Functions.Like(ws.WorkingSpace.RoomType, $"%{type}%"));
        //    }

        //    var _data = await query.ToListAsync();

        //    if (_data != null)
        //    {
        //        _response = _data.Select(item =>
        //        {
        //            var workingSpaceModel = mapper.Map<WorkingSpaceRequestRatingMode>(item.WorkingSpace);
        //            workingSpaceModel.Rating = item.AverageRating;
        //            workingSpaceModel.BusinessName = item.BusinessName;
        //            workingSpaceModel.ImageUrls = item.ImageUrls;
        //            return workingSpaceModel;
        //        }).ToList();
        //    }

        //    return _response;
        //}

        //public async Task<List<WorkingSpaceRequestRatingMode>> GetAll()
        //{
        //    List<WorkingSpaceRequestRatingMode> _response = new List<WorkingSpaceRequestRatingMode>();

        //    // Get all working spaces with their average rating, business name, and associated images
        //    var _data = await context.WorkingSpaces
        //        .Select(ws => new
        //        {
        //            WorkingSpace = ws,
        //            AverageRating = ws.Reviews.Any() ? ws.Reviews.Average(r => r.Rating) : 0,
        //            BusinessName = ws.Business.BusinessName,
        //            ImageUrls = ws.Images.Select(img => img.ImageUrl).ToList() 
        //        })
        //        .ToListAsync();

        //    if (_data != null)
        //    {
        //        _response = _data.Select(item =>
        //        {
        //            var workingSpaceModel = mapper.Map<WorkingSpaceRequestRatingMode>(item.WorkingSpace);
        //            workingSpaceModel.Rating = item.AverageRating;
        //            workingSpaceModel.BusinessName = item.BusinessName;
        //            workingSpaceModel.ImageUrls = item.ImageUrls; // Gán danh sách URL ảnh vào model
        //            return workingSpaceModel;
        //        }).ToList();
        //    }

        //    return _response;
        //}



        public async Task<WorkingSpaceResponseAmenities> GetBySpaceId(string spaceId)
        {
            try
            {
                var workingSpace = await context.WorkingSpaces
                                                 .Include(ws => ws.Amenities)
                                                 .Include(ws => ws.Business)
                                                 .Include(ws => ws.Reviews)
                                                 .Include(ws => ws.Images)
                                                 .FirstOrDefaultAsync(ws => ws.SpaceId == spaceId);

                if (workingSpace == null)
                {
                    return null;
                }

                var averageRating = workingSpace.Reviews != null && workingSpace.Reviews.Any()
                                     ? workingSpace.Reviews.Average(r => r.Rating)
                                     : 0;

                var workingSpaceModel = mapper.Map<WorkingSpaceResponseAmenities>(workingSpace);
                workingSpaceModel.Rating = averageRating; 
                workingSpaceModel.BusinessName = workingSpace.Business?.BusinessName;
                workingSpaceModel.ImageUrls = workingSpace.Images.Select(img => img.ImageUrl).ToList();

                return workingSpaceModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving working space by ID.");
                return null;
            }
        }

        public async Task<List<WorkingSpaceRequestModel>> GetWorkingSpacesForWorkingAreaAsync()
        {
            return await GetWorkingSpacesByRoomTypeAsync("Không gian làm việc chung");
        }

        public async Task<List<WorkingSpaceRequestModel>> GetWorkingSpacesForCommonSpaceAsync()
        {
            return await GetWorkingSpacesByRoomTypeAsync("Study hub");
        }

        public async Task<List<WorkingSpaceRequestModel>> GetWorkingSpacesForWorkingCafeAsync()
        {
            return await GetWorkingSpacesByRoomTypeAsync("Cafe làm việc");
        }

        public async Task<List<WorkingSpaceRequestModel>> GetWorkingSpacesForEventSpaceAsync()
        {
            return await GetWorkingSpacesByRoomTypeAsync("Không gian sự kiện");
        }

        private async Task<List<WorkingSpaceRequestModel>> GetWorkingSpacesByRoomTypeAsync(string roomType)
        {
            try
            {
                var workingSpaces = await context.WorkingSpaces
                    .Where(ws => ws.RoomType.ToLower() == roomType.ToLower()) // Use ToLower() for case-insensitive comparison
                    .ToListAsync();

                return mapper.Map<List<WorkingSpaceRequestModel>>(workingSpaces);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving working spaces of room type: {RoomType}", roomType);
                return null;
            }
        }


        public async Task<List<WorkingSpaceRequestRatingMode>> SearchWorkingSpacesByLocationAsync(string location)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(location))
                {
                    _logger.LogInformation("Location search term is empty.");
                    return new List<WorkingSpaceRequestRatingMode>();
                }

                var workingSpaces = await context.WorkingSpaces
                    .Where(ws => ws.Location.ToLower().Contains(location.ToLower())) // Case-insensitive search
                    .ToListAsync();

                var result = mapper.Map<List<WorkingSpaceRequestRatingMode>>(workingSpaces);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching for working spaces by location: {Location}", location);
                return null; 
            }
        }

    }
}
