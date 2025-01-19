using Microsoft.EntityFrameworkCore;
using PersonalCoachingApp.Business.Operations.Package.Dtos;
using PersonalCoachingApp.Business.Types;
using PersonalCoachingApp.Data.Entities;
using PersonalCoachingApp.Data.Repositories;
using PersonalCoachingApp.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonalCoachingApp.Business.Operations.Package
{
    public class PackageManager : IPackageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<PackageEntity> _packageRepository;
        private readonly IRepository<PackageFeatureEntity> _packageFeatureRepository;

        public PackageManager(IUnitOfWork unitOfWork, IRepository<PackageEntity> packageRepository, IRepository<PackageFeatureEntity> packageFeatureRepository)
        {
            _unitOfWork = unitOfWork;
            _packageRepository = packageRepository;
            _packageFeatureRepository = packageFeatureRepository;
        }

        public async Task <ServiceMessage> AddPackage(AddPackageDto package)
        {
            var hasPackage =  _packageRepository.GetAll(x => x.PackageName.ToLower() == package.Name.ToLower());

            if (hasPackage.Any())
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Package already exist."
                };
            }

            await _unitOfWork.BeginTransaction();

            var PackageEntity = new PackageEntity
            {
                PackageName = package.Name,
                PackageType = package.PackageType,
                Price = package.PackagePrice,
                Description = package.Description,
                StockQuantity = package.StockQuantity,
                TrainingDuration = package.TrainingDuration,
            };

            _packageRepository.Add(PackageEntity);

            try
            {
               await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                string logFilePath = "error_log.txt";
                string errorMessage = $"[{DateTime.Now}] An error occurred while registering the package: {ex.Message}";
                System.IO.File.AppendAllText(logFilePath, errorMessage + Environment.NewLine);

                throw new Exception("An error occurred while registering the package.", ex);
            }

            foreach (var featureId in package.FeatureIds)
            {
                var packageFeature = new PackageFeatureEntity
                {
                    FeatureId = featureId,
                    PackageId = PackageEntity.Id,
                };

                _packageFeatureRepository.Add(packageFeature); //Sending database.
            }

            try
            {
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                await _unitOfWork.RollBackTransaction();
                throw new Exception("An error occurred while adding package feature, process rollback.");
            }

            return new ServiceMessage { IsSucceed = true };
        }

        public async Task <ServiceMessage> AdjustPackagePrice(int id, int changeBy)
        {
            var package = _packageRepository.GetById(id);

            if (package == null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "No package found for this id."
                };
            }

            package.Price = changeBy;

            _packageRepository.Update(package);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("An error occurred while updating the price");
            }

            return new ServiceMessage() { IsSucceed = true };
        }

        public async Task <ServiceMessage> DeletePackage(int id)
        {
            var package = _packageRepository.GetById(id);

            if (package == null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "No package found to delete."
                };
            }

            _packageRepository.Delete(id);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }

            catch (Exception)
            {
                throw new Exception("Failed to delete package.");

            }
            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "An error occurred during deletion."
            };
        }

       public async Task <List<PackageDto>> GetAllPackages()
        {
            var packages = await _packageRepository.GetAll()
                .Select(x => new PackageDto
                {
                    Id = x.Id,
                    Name = x.PackageName,
                    PackagePrice = x.Price,
                    PackageType = x.PackageType,
                    Description = x.Description,
                    TrainingDuration = x.TrainingDuration,
                    StockQuantity = x.StockQuantity,
                    Features = x.PackageFeatures.Select(f => new PackageFeatureDto
                    {
                        Id = f.Id,
                        Title = f.Feature.Title,
                    }).ToList()
                }).ToListAsync();

            return packages;
        }

        public async Task <PackageDto> GetPackage(int id)
        {
            var package = await _packageRepository.GetAll(x => x.Id == id) 
                .Select(x => new PackageDto
                {
                    Id = x.Id,
                    Name = x.PackageName,
                    PackagePrice = x.Price,
                    PackageType = x.PackageType,
                    Description = x.Description,
                    TrainingDuration = x.TrainingDuration,
                    StockQuantity = x.StockQuantity,
                    Features = x.PackageFeatures.Select(f => new PackageFeatureDto
                    {
                        Id = f.Id,
                        Title = f.Feature.Title,
                    }).ToList()
                }).FirstOrDefaultAsync();

            if (package == null)
            {
                throw new Exception("Package not found.");
            }

            return package;
        }

        public async Task <ServiceMessage> UpdatePackage(UpdatePackageDto package)
        {
            var packageEntity = _packageRepository.GetById(package.Id);

            if (packageEntity == null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Package not found."
                };
            }

            await _unitOfWork.BeginTransaction();

            packageEntity.PackageName = package.Name;
            packageEntity.Price = package.PackagePrice;
            packageEntity.TrainingDuration = package.TrainingDuration;
            packageEntity.StockQuantity = package.StockQuantity;
            packageEntity.Description = package.Description;
            packageEntity.PackageType = package.PackageType;

            _packageRepository.Update(packageEntity);

            try
            {
               await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollBackTransaction();
                throw new Exception("An error occurred during updation.");
            }

            var packageFeatures = _packageFeatureRepository.GetAll(x => x.PackageId == x.PackageId).ToList();
            foreach (var packageFeature in packageFeatures)
            {
                _packageFeatureRepository.Delete(packageFeature, false); //Hard Delete
            }

            foreach (var featureId in package.FeatureIds)
            {
                var packageFeature = new PackageFeatureEntity
                {
                    PackageId = packageEntity.Id,
                    FeatureId = featureId,
                };

                _packageFeatureRepository.Add(packageFeature);
            }

            try
            {
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                await _unitOfWork.RollBackTransaction();
                throw new Exception("An error occurred while updating package information. Changes rollback.");
            }

            return new ServiceMessage { IsSucceed = true };
        }
    }
}
