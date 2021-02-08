using System.Collections.Generic;
using TDFramework.Common.TDModel;
using TDFramework.Common.Attributes;
using Models.GuitarPlanModModel;

namespace Models.GuitarModModel
{
	public class GuitarMod : ITDModel
	{
		public GuitarMod()
		{
			this.PlanModList = new List<GuitarPlanMod>();
		}

		[PKey]
		[IDColumn]
		public int ID { get; set; }
		public string Isim { get; set; }
		public string Formul { get; set; }

		[AggregateColumn]
		public dynamic AggColumn { get; set; }

		[NotTableColumn]
		public int Nota { get; set; }

		[NotTableColumn]
		public string NotaIsim { get; set; }

		[RTable]
		public List<GuitarPlanMod> PlanModList { get; set; }
	}

	public enum GuitarModColumns
	{
		ID,
		Isim,
		Formul
	}
}
