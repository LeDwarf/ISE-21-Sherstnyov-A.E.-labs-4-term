using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace AlexeysShopService.ViewModels
{
    [DataContract]
    public class ContractViewModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int CustomerId { get; set; }
        [DataMember]
        public string CustomerFIO { get; set; }
        [DataMember]
        public int ArticleId { get; set; }
        [DataMember]
        public string ArticleName { get; set; }
        [DataMember]
        public int? BuilderId { get; set; }
        [DataMember]
        public string BuilderName { get; set; }
        [DataMember]
        public int Count { get; set; }
        [DataMember]
        public decimal Cost { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string DateBegin { get; set; }
        [DataMember]
        public string DateBuilt { get; set; }

    }
}
