using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlexeysShopModel
{
    public class Part
    {
        public int Id { get; set; }

		[Required]
		public string PartName { get; set; }

		[ForeignKey("PartId")]
		public virtual List<ArticlePart> ArticleParts { get; set; }

		[ForeignKey("PartId")]
		public virtual List<StoragePart> StorageParts { get; set; }
	}
}
