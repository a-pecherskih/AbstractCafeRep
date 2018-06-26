using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AbstractCafeService.BindingModel
{
    [DataContract]
    public class MenuBindingModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string MenuName { get; set; }
        [DataMember]
        public decimal Price { get; set; }
        [DataMember]
        public List<MenuDishBindingModel> MenuDishs { get; set; }
    }
}
