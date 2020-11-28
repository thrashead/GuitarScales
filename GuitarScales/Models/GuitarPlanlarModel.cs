using System.Collections.Generic;
using TDFramework.Common.TDModel;
using TDFramework.Common.Attributes;
using Models.GuitarPlanModModel;
using Models.GuitarPlanAkorModel;

namespace Models.GuitarPlanlarModel
{
	public class GuitarPlanlar : ITDModel
	{
		public GuitarPlanlar()
		{
			this.PlanModList = new List<GuitarPlanMod>();
			this.PlanAkorList = new List<GuitarPlanAkor>();
		}

		[PKey]
		[IDColumn]
		public int ID { get; set; }
		public string Isim { get; set; }
		public bool Aktif { get; set; }

		[AggregateColumn]
		public dynamic AggColumn { get; set; }

		[RTable]
		public List<GuitarPlanMod> PlanModList { get; set; }
		[RTable]
		public List<GuitarPlanAkor> PlanAkorList { get; set; }
	}

	public enum GuitarPlanlarColumns
	{
		ID,
		Isim,
		Aktif
	}
}
