using Microsoft.EntityFrameworkCore;
using PersonalCoachingApp.Business.DataProtection;
using PersonalCoachingApp.Business.Operations.User.Dtos;
using PersonalCoachingApp.Business.Types;
using PersonalCoachingApp.Data.Entities;
using PersonalCoachingApp.Data.Enums;
using PersonalCoachingApp.Data.Repositories;
using PersonalCoachingApp.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PersonalCoachingApp.Business.Operations.User
{
    public class UserManager : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<UserEntity> _userRepository;
        private readonly IDataProtection _protector;

        public UserManager(IUnitOfWork unitOfWork, IRepository<UserEntity> userRepository, IDataProtection protector)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _protector = protector;
        }

        public async Task <ServiceMessage> AddUser(AddUserDto user)
        {
            var hasMail = _userRepository.GetAll(x => x.Email.ToLower() == user.Email.ToLower());

            if (hasMail.Any())
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Email address already exists"
                };
            }

            var userEntity = new UserEntity()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = _protector.Protect(user.Password),
                BirthDate = user.BirthDate.ToUniversalTime(),
                UserType = UserType.Customer
            };

            _userRepository.Add(userEntity);

            try
            {
              await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "A database error occurred during registration: " + dbEx.Message + dbEx.InnerException
                };
            }
            catch (Exception ex)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "An unexpected error occurred during registration: " + ex.Message
                };
            }

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "User successfully registered"
            };
        }

        public ServiceMessage<UserInfoDto> LoginUser(LoginUserDto user)
        {
            var userEntity =  _userRepository.Get(x => x.Email.ToLower() == user.Email.ToLower());

            if (userEntity == null)
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSucceed = false,
                    Message = "We could not find the username and password."
                };
            }

            var unprotectedPassword = _protector.UnProtect(userEntity.Password);

            if (unprotectedPassword == user.Password)
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSucceed = true,
                    Data = new UserInfoDto
                    {
                        Email = userEntity.Email,
                        FirstName = userEntity.FirstName,
                        LastName = userEntity.LastName,
                        UserType = userEntity.UserType
                    }
                };
            }
            else
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSucceed = false,
                    Message = "We could not find the username and password."
                };
            }
        }

    }
}
