using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace AlexeysShopService.BindingModels
{
    [DataContract]
    public class StoragePartBindingModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int StorageId { get; set; }
        [DataMember]
        public int PartId { get; set; }
        [DataMember]
        public int Count { get; set; }
    }
}
