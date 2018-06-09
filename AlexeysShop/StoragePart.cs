using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexeysShopModel
{
    public class StoragePart
    {
        public int Id { get; set; }
        public int StorageId { get; set; }
        public int PartId { get; set; }
        public int Count { get; set; }
		public virtual Storage Storage { get; set; }
		public virtual Part Part { get; set; }

	}
}
