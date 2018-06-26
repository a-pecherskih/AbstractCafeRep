using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AbstractCafeService.ViewModels
{
    [DataContract]
    public class MenuViewModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string MenuName { get; set; }
        [DataMember]
        public decimal Price { get; set; }
        [DataMember]
        public List<MenuDishViewModel> MenuDishs { get; set; }
    }
}
