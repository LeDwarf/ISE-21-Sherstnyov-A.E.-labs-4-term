using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace AlexeysShopService.BindingModels
{
    [DataContract]
    public class ContractBindingModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int CustomerId { get; set; }
        [DataMember]
        public int ArticleId { get; set; }
        [DataMember]
        public int? BuilderId { get; set; }
        [DataMember]
        public int Count { get; set; }
        [DataMember]
        public decimal Cost { get; set; }

    }
}
