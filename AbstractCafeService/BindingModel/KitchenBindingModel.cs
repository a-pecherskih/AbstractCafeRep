using System.Runtime.Serialization;

namespace AbstractCafeService.BindingModel
{
    [DataContract]
    public class KitchenBindingModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string KitchenName { get; set; }
    }
}
