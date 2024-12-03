using AutoMapper;
using Quik_BookingApp.BOs.Request;
using Quik_BookingApp.BOs.Response;
using Quik_BookingApp.DAO.Models;
using Quik_BookingApp.Modal;

namespace Quik_BookingApp.Repos.AutoMapper
{
    //public class MappingProfileExtension : Profile
    //{
    //    public MappingProfileExtension()
    //    {
    //        CreateMap<User, UserModal>().ForMember(item => item.Status, opt => opt.MapFrom(
    //item => (item.IsActive != null && item.IsActive) ? "Active" : "Inactive")).ReverseMap();
    //        CreateMap<WorkingSpace, WorkingSpaceRequestModel>();
    //        CreateMap<Booking, BookingResponseModel>();
    //        CreateMap<Business, BusinessResponseModel>();
    //        CreateMap<UserRegister, User>()
    //            .ForMember(dest => dest.Password, opt => opt.Ignore()) // Password will be hashed manually
    //            .ForMember(dest => dest.Role, opt => opt.Ignore());    // Default role will be set in the service
    //        CreateMap<Payment, VNPayPaymentRequestModel>();
    //        CreateMap<WorkingSpace, WorkingSpaceResponse>();
    //        CreateMap<Amenity, AmenityRequestModel>();
    //        CreateMap<WorkingSpace, WorkingSpaceResponseAmenities>();
    //        CreateMap<User, UserResponseModel>();
    //        CreateMap<SignupModel, User>()
    //            // Default value: False
    //            .ForMember(dest => dest.EmailVerified, opt => opt.MapFrom(src => false))
    //            .ForMember(dest => dest.EmailVerificationToken, opt => opt.MapFrom(src => src.EmailVerifiedToken));
    //    }
    //}
}
