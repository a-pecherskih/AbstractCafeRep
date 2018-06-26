using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AbstractCafeService.ViewModels
{
    [DataContract]
    public class KitchensLoadViewModel
    {
        [DataMember]
        public string KitchenName { get; set; }
        [DataMember]
        public int TotalCount { get; set; }
        [DataMember]
        public List<KitchensDishLoadViewModel> Dishs { get; set; }
    }

    [DataContract]
    public class KitchensDishLoadViewModel
    {
        [DataMember]
        public string DishName { get; set; }

        [DataMember]
        public int Count { get; set; }
    }
}
