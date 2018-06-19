using System.Runtime.Serialization;

namespace AbstractCafeService.BindingModel
{
    [DataContract]
    public class KitchenDishBindingModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int KitchenId { get; set; }
        [DataMember]
        public int DishId { get; set; }
        [DataMember]
        public int Count { get; set; }
    }
}
