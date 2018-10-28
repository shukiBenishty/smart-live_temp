using System;

namespace DAL.API.Nutritionix
{
    class NutritionixApiKeys
    {
        public NutritionixApiKeys()
        {
            this.ApplicationId = "c6419031";
            this.ApplicationSecret = "e093ec458167f97d2794dc3a43414a51";
        }
        public String ApplicationSecret { get; set; }
        public String ApplicationId { get; set; }
    }


}
