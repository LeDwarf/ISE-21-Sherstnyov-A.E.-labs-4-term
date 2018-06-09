using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace AlexeysShopService.ViewModels
{
    [DataContract]
    public class ArticlePartViewModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int ArticleId { get; set; }
        [DataMember]
        public int PartId { get; set; }
        [DataMember]
        public string PartName { get; set; }
        [DataMember]
        public int Count { get; set; }

    }
}
