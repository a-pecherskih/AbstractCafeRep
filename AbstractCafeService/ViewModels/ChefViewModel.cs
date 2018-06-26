using System.Runtime.Serialization;

namespace AbstractCafeService.ViewModels
{
    [DataContract]
    public class ChefViewModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string ChefFIO { get; set; }
    }
}
