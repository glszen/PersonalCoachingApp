using PersonalCoachingApp.Business.Operations.Feature.Dtos;
using PersonalCoachingApp.Business.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalCoachingApp.Business.Operations.Feature
{
    public interface IFeatureService
    {
        Task <ServiceMessage> AddFeature(AddFeatureDto feature); 
        Task <List<FeatureDto>> GetAllFeatures(); 
        Task <ServiceMessage> DeleteFeature(int id); 
    }
}
