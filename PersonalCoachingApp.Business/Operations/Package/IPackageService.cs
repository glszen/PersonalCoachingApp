using PersonalCoachingApp.Business.Operations.Package.Dtos;
using PersonalCoachingApp.Business.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalCoachingApp.Business.Operations.Package
{
    public interface IPackageService
    {
        Task <ServiceMessage> AddPackage(AddPackageDto package); 
        Task <PackageDto> GetPackage(int id); 
        Task <List<PackageDto>> GetAllPackages(); 
        Task <ServiceMessage> AdjustPackagePrice(int id, int changeBy); 
        Task <ServiceMessage> DeletePackage(int id); 
        Task <ServiceMessage> UpdatePackage(UpdatePackageDto package); 
    }
}
