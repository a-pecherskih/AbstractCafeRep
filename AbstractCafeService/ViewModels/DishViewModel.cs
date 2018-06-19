using System.Runtime.Serialization;

namespace AbstractCafeService.ViewModels
{
    [DataContract]
    public class DishViewModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string DishName { get; set; }
    }
}
