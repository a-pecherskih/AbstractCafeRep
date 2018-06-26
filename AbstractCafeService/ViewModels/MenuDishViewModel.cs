using System.Runtime.Serialization;

namespace AbstractCafeService.ViewModels
{
    [DataContract]
    public class MenuDishViewModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int MenuId { get; set; }
        [DataMember]
        public int DishId { get; set; }
        [DataMember]
        public string DishName { get; set; }
        [DataMember]
        public int Count { get; set; }
    }
}
