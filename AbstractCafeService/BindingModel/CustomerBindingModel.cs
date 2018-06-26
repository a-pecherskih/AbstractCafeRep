using System.Runtime.Serialization;

namespace AbstractCafeService.BindingModel
{
    [DataContract]
    public class CustomerBindingModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Mail { get; set; }
        [DataMember]
        public string CustomerFIO { get; set; }
    }
}
