using System.Runtime.Serialization;

namespace AbstractCafeService.ViewModels
{
    [DataContract]
    public class KitchenDishViewModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int KitchenId { get; set; }
        [DataMember]
        public int DishId { get; set; }
        [DataMember]
        public string DishName { get; set; }
        [DataMember]
        public int Count { get; set; }
    }
}
