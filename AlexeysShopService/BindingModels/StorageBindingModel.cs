using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace AlexeysShopService.BindingModels
{
    [DataContract]
    public class StorageBindingModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string StorageName { get; set; }
    }
}
