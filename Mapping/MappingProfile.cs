using AutoMapper;
using DoctorPatient.DTO;
using DoctorPatient.model;

namespace DoctorPatient.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Doctor,DoctorUpdateDto>().ReverseMap();
            CreateMap<Patients,PatientsCreateDto>().ReverseMap();
        }
    }
}
