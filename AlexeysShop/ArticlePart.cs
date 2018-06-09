using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexeysShopModel
{
    public class ArticlePart
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public int PartId { get; set; }
        public int Count { get; set; }
		public virtual Article Article { get; set; }
		public virtual Part Part { get; set; }
	}
}
