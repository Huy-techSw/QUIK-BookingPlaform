using Quik_BookingApp.BOs.Request;
using Quik_BookingApp.BOs.Response;
using Quik_BookingApp.Helper;


namespace Quik_BookingApp.Repos.Interface
{
    public interface IWorkingSpaceService
    {
        //Task<List<WorkingSpaceRequestRatingMode>> GetAll(string? location = null);
        //Task<List<WorkingSpaceRequestRatingMode>> GetAllType(string? type = null);
        Task<List<WorkingSpaceRequestRatingMode>> GetAll(string? location = null, string? type = null);

        Task<WorkingSpaceResponseAmenities> GetBySpaceId(string spaceId);
        Task<APIResponseData> CreateWS(WorkingSpaceRequestModel ws);

        Task<List<WorkingSpaceRequestModel>> GetWorkingSpacesForWorkingAreaAsync();
        Task<List<WorkingSpaceRequestModel>> GetWorkingSpacesForWorkingCafeAsync();
        Task<List<WorkingSpaceRequestModel>> GetWorkingSpacesForCommonSpaceAsync();
        Task<List<WorkingSpaceRequestModel>> GetWorkingSpacesForEventSpaceAsync();

        Task<List<WorkingSpaceRequestRatingMode>> SearchWorkingSpacesByLocationAsync(string location);
    }
}
