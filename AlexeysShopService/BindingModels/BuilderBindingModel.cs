using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace AlexeysShopService.BindingModels
{
    [DataContract]
    public class BuilderBindingModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string BuilderFIO { get; set; }
    }
}
