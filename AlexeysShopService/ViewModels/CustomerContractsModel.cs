using System.Runtime.Serialization;

namespace AlexeysShopService.ViewModels
{
    [DataContract]
    public class CustomerContractsModel
    {
        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]
        public string DateCreate { get; set; }
        [DataMember]
        public string ArticleName { get; set; }
        [DataMember]
        public int Count { get; set; }
        [DataMember]
        public decimal Cost { get; set; }
        [DataMember]
        public string Status { get; set; }
    }
}
