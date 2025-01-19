using PersonalCoachingApp.Business.Operations.User.Dtos;
using PersonalCoachingApp.Business.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalCoachingApp.Business.Operations.User
{
    public interface IUserService
    {
        Task <ServiceMessage> AddUser(AddUserDto user); //It async because of using unit of work.

         ServiceMessage<UserInfoDto> LoginUser(LoginUserDto user);
    }
}
