using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model.SaveModel
{
    public class MaterialInfo
    {
        public long id { get; set; }
        public string testingStandard { get; set; }
        public string manufacturerForShort { get; set; }
        public string manufacturerForEnglish { get; set; }
        public string manufacturerFullName { get; set; }
        public string tradeName { get; set; }
        public string kind { get; set; }
        public string mark { get; set; }
        public string materialIdentity { get; set; }
        public string color { get; set; }
        public string fn { get; set; }
        public string otherCertificate { get; set; }
        public string purpose { get; set; }
        public string characteristic { get; set; }
        public string shape { get; set; }
        public string processingMethod { get; set; }
        public List<physicsPropertie> physicsProperties { get; set; }
        public List<machinePropertie>  machineProperties { get; set; }
        public List<temperaturePropertie>  temperatureProperties { get; set; }
        public List<electricPropertie>  electricProperties { get; set; }
        public List<flamePropertie>  flameProperties { get; set; }
        public List<hardnessPropertie> hardnessProperties { get; set; }
        public List<injectioncondition>  injectionconditions { get; set; }


    }
}
