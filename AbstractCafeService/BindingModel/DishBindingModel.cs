using System.Runtime.Serialization;

namespace AbstractCafeService.BindingModel
{
    [DataContract]
    public class DishBindingModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string DishName { get; set; }
    }
}
