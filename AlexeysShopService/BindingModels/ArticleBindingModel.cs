using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexeysShopService.BindingModels
{
	[DataContract]
	public class ArticleBindingModel
    {
		[DataMember]
		public int Id { get; set; }
		[DataMember]
		public string ArticleName { get; set; }
		[DataMember]
		public decimal Cost { get; set; }
		[DataMember]
		public List<ArticlePartBindingModel> ArticleParts { get; set; }

    }
}
