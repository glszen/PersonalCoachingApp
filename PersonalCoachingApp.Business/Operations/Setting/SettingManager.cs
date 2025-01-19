using PersonalCoachingApp.Data.Entities;
using PersonalCoachingApp.Data.Repositories;
using PersonalCoachingApp.Data.UnitOfWork;
using System;
using System.Linq;

namespace PersonalCoachingApp.Business.Operations.Setting
{
    public class SettingManager : ISettingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<SettingEntity> _settingRepository;

        public SettingManager(IUnitOfWork unitOfWork, IRepository<SettingEntity> settingRepository)
        {
            _unitOfWork = unitOfWork;
            _settingRepository = settingRepository;
        }

        public bool GetMaintencenceState()
        {
            var maintenanceState = _settingRepository.GetById(1).MaintencenceMode;

            return maintenanceState;
        }

        public async Task ToggleMaintenence()
        {
            var setting = _settingRepository.GetById(1);

            setting.MaintencenceMode = !setting.MaintencenceMode;

            _settingRepository.Update(setting);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("An error was encountered while updating the maintenance status.");
            }
        }
    }
}
