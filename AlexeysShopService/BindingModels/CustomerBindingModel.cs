using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace AlexeysShopService.BindingModels
{
    [DataContract]
    public class CustomerBindingModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Mail { get; set; }
        [DataMember]
        public string CustomerFIO { get; set; }

    }
}
