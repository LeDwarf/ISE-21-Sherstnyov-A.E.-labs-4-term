using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlexeysShopModel
{
    public class Article
    {
        public int Id { get; set; }

		[Required]
		public string ArticleName { get; set; }

		[Required]
		public decimal Cost { get; set; }

		[ForeignKey("ArticleId")]
		public virtual List<Contract> Contract { get; set; }

		[ForeignKey("ArticleId")]
		public virtual List<ArticlePart> ArticleParts { get; set; }

	}
}
