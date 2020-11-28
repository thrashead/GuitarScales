using System.Collections.Generic;
using TDFramework.Common.TDModel;
using TDFramework.Common.Attributes;
using Models.GuitarPlanAkorModel;

namespace Models.GuitarAkorModel
{
	public class GuitarAkor : ITDModel
	{
		public GuitarAkor()
		{
			this.PlanAkorList = new List<GuitarPlanAkor>();
		}

		[PKey]
		[IDColumn]
		public int ID { get; set; }
		public string Isim { get; set; }
		public string Formul { get; set; }

		[AggregateColumn]
		public dynamic AggColumn { get; set; }

		[RTable]
		public List<GuitarPlanAkor> PlanAkorList { get; set; }

		[NotTableColumn]
		public int Nota { get; set; }

		[NotTableColumn]
		public string NotaIsim { get; set; }
	}

	public enum GuitarAkorColumns
	{
		ID,
		Isim,
		Formul
	}
}
