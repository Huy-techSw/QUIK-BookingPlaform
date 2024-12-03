using Quik_BookingApp.BOs.Request;
using Quik_BookingApp.BOs.Response;
using Quik_BookingApp.DAO.Models;
using Quik_BookingApp.Helper;

namespace Quik_BookingApp.Repos.Interface
{
    public interface IBusinessService
    {

        Task<List<BusinessResponseRatingMode>> GetAllBusiness();
        Task<List<WSWBNameResponse>> GetListWSOfBusiness(string businessId);
        Task<BusinessResponseModel> GetBusinessById(string bid);

        Task<APIResponseData> RegisterBusiness(BusinessRequestModel business);
        Task<APIResponseData> UpdateBusiness(string businessId, UpdateBusinessModel businessRequest);
    }
}
