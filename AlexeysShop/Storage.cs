using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlexeysShopModel
{
    public class Storage
    {
        public int Id { get; set; }

		[Required]
		public string StorageName { get; set; }

		[ForeignKey("StorageId")]
		public virtual List<StoragePart> StorageParts { get; set; }

	}
}
