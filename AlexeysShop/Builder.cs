using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlexeysShopModel
{
    public class Builder
    {
        public int Id { get; set; }

		[Required]
		public string BuilderFIO { get; set; }

		[ForeignKey("BuilderId")]
		public virtual List<Contract> Contracts { get; set; }
	}
}
