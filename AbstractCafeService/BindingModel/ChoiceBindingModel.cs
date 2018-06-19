using System.Runtime.Serialization;

namespace AbstractCafeService.BindingModel
{
    [DataContract]
    public class ChoiceBindingModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int CustomerId { get; set; }
        [DataMember]
        public int MenuId { get; set; }
        [DataMember]
        public int? ChefId { get; set; }
        [DataMember]
        public int Count { get; set; }
        [DataMember]
        public decimal Sum { get; set; }
    }
}
