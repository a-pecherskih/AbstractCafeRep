using System.Runtime.Serialization;

namespace AbstractCafeService.BindingModel
{
    [DataContract]
    public class MenuDishBindingModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int MenuId { get; set; }
        [DataMember]
        public int DishId { get; set; }
        [DataMember]
        public int Count { get; set; }
    }
}
