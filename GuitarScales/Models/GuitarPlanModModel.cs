using System.Collections.Generic;
using System.Web.Mvc;
using TDFramework.Common.TDModel;
using TDFramework.Common.Attributes;

namespace Models.GuitarPlanModModel
{
	public class GuitarPlanMod : ITDModel
	{
		public GuitarPlanMod()
		{
			this.PlanlarList = new List<SelectListItem>();
			this.ModList = new List<SelectListItem>();
			this.NotaList = new List<SelectListItem>();
		}

		[PKey]
		[IDColumn]
		public int ID { get; set; }
		public int PlanID { get; set; }
		public int NotaID { get; set; }
		public int ModID { get; set; }

		[AggregateColumn]
		public dynamic AggColumn { get; set; }

		[NotTableColumn]
		public List<SelectListItem> PlanlarList { get; set; }
		[NotTableColumn]
		public List<SelectListItem> ModList { get; set; }
		[NotTableColumn]
		public List<SelectListItem> NotaList { get; set; }
		[NotTableColumn]
		public string PlanlarAdi { get; set; }
		[NotTableColumn]
		public string ModAdi { get; set; }
		[NotTableColumn]
		public string NotaAdi { get; set; }
	}

	public enum GuitarPlanModColumns
	{
		ID,
		PlanID,
		NotaID,
		ModID
	}
}
