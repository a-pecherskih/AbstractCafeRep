using System.Runtime.Serialization;

namespace AbstractCafeService.ViewModels
{
    [DataContract]
    public class ChoiceViewModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int CustomerId { get; set; }
        [DataMember]
        public string CustomerFIO { get; set; }
        [DataMember]
        public int MenuId { get; set; }
        [DataMember]
        public string MenuName { get; set; }
        [DataMember]
        public int? ChefId { get; set; }
        [DataMember]
        public string ChefName { get; set; }
        [DataMember]
        public int Count { get; set; }
        [DataMember]
        public decimal Sum { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string DateCreate { get; set; }
        [DataMember]
        public string DateImplement { get; set; }
    }
}
