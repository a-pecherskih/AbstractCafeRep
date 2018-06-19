using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AbstractCafeService.ViewModels
{
    [DataContract]
    public class KitchenViewModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string KitchenName { get; set; }
        [DataMember]
        public List<KitchenDishViewModel> KitchenDishs { get; set; }
    }
}
