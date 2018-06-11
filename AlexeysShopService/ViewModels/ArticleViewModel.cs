using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace AlexeysShopService.ViewModels
{
    [DataContract]
    public class ArticleViewModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string ArticleName { get; set; }
        [DataMember]
        public decimal Cost { get; set; }
        [DataMember]
        public List<ArticlePartViewModel> ArticleParts { get; set; }
    }
}
