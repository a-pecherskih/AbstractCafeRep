using System.Runtime.Serialization;

namespace AbstractCafeService.BindingModel
{
    [DataContract]
    public class ChefBindingModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string ChefFIO { get; set; }
    }
}
