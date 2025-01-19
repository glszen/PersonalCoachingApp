using Microsoft.EntityFrameworkCore;
using PersonalCoachingApp.Business.Operations.Feature.Dtos;
using PersonalCoachingApp.Business.Types;
using PersonalCoachingApp.Data.Entities;
using PersonalCoachingApp.Data.Repositories;
using PersonalCoachingApp.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PersonalCoachingApp.Business.Operations.Feature
{
    public class FeatureManager : IFeatureService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<FeatureEntity> _repository;

        public FeatureManager(IUnitOfWork unitOfWork, IRepository<FeatureEntity> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task <List<FeatureDto>> GetAllFeatures()
        {

            var features = await _repository.GetAll()
                .Select(x => new FeatureDto
                {
                    Id = x.Id,
                    Title = x.Title
                }).ToListAsync();

            return features;
        }

        public async Task <ServiceMessage> AddFeature(AddFeatureDto feature)
        {
            
            var hasFeature = _repository.GetAll(x => x.Title.ToLower() == feature.Title.ToLower()).Any();

            if (hasFeature)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Property already exists."
                };
            }
            var featureEntity = new FeatureEntity
            {
                Title = feature.Title,
            };

            _repository.Add(featureEntity); 

            try
            {
                
                 await _unitOfWork.SaveChangesAsync(); 
            }
            catch (Exception)
            {
                
                throw new Exception("An error occurred while saving the property.");
            }

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Feature added successfully"
            };
        }

        public async Task<ServiceMessage> DeleteFeature(int id)
        {
            var feature = _repository.GetById(id); 

            if (feature == null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "No feature found to delete."
                };
            }

            _repository.Delete(id);  

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
                Message = "An error occurred during deletion:"
            };
        }

    }
}
