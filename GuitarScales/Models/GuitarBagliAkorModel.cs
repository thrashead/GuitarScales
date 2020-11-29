using System;
using TDFramework.Common.TDModel;
using TDFramework.Common.Attributes;

namespace Models.GuitarBagliAkorModel
{
	public class GuitarBagliAkor : ITDModel
	{
		[PKey]
		[IDColumn]
		public int ID { get; set; }
		public int ModID { get; set; }
		public int AkorID { get; set; }
		public int Derece { get; set; }
		public int NotaAdet { get; set; }

		[AggregateColumn]
		public dynamic AggColumn { get; set; }
	}

	public enum GuitarBagliAkorColumns
	{
		ID,
		ModID,
		AkorID,
		Derece,
		NotaAdet
	}
}
