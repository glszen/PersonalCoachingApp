using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalCoachingApp.Data.Entities
{
    public class SettingEntity : BaseEntity //Keeps information about whether there is maintenance on the project.
    {
        public bool MaintencenceMode { get; set; }
    }
}
