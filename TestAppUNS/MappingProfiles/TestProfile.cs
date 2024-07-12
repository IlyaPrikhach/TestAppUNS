using AutoMapper;
using TestAppUNS.DAL.Entities;
using TestAppUNS.Models;

namespace TestAppUNS.MappingProfiles
{
    public class TestProfile: Profile
    {
        public TestProfile()
        {
            CreateMap<OrderModel, OrderEntity>();
            CreateMap<OrderEntity, OrderModel>();
        }
    }
}
