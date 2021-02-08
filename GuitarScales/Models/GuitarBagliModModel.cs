using TDFramework.Common.TDModel;
using TDFramework.Common.Attributes;

namespace Models.GuitarBagliModModel
{
	public class GuitarBagliMod : ITDModel
	{
		[PKey]
		[IDColumn]
		public int ID { get; set; }
		public int ModID { get; set; }
		public int BagliModID { get; set; }
		public int Derece { get; set; }

		[AggregateColumn]
		public dynamic AggColumn { get; set; }
	}

	public enum GuitarBagliModColumns
	{
		ID,
		ModID,
		BagliModID,
		Derece
	}
}
