using System.Collections.Generic;
using System.Web.Mvc;
using TDFramework.Common.TDModel;
using TDFramework.Common.Attributes;

namespace Models.GuitarPlanAkorModel
{
	public class GuitarPlanAkor : ITDModel
	{
		public GuitarPlanAkor()
		{
			this.PlanlarList = new List<SelectListItem>();
			this.AkorList = new List<SelectListItem>();
			this.NotaList = new List<SelectListItem>();
		}

		[PKey]
		[IDColumn]
		public int ID { get; set; }
		public int PlanID { get; set; }
		public int NotaID { get; set; }
		public int AkorID { get; set; }

		[AggregateColumn]
		public dynamic AggColumn { get; set; }

		[NotTableColumn]
		public List<SelectListItem> PlanlarList { get; set; }
		[NotTableColumn]
		public List<SelectListItem> AkorList { get; set; }
		[NotTableColumn]
		public List<SelectListItem> NotaList { get; set; }
		[NotTableColumn]
		public string PlanlarAdi { get; set; }
		[NotTableColumn]
		public string AkorAdi { get; set; }
		[NotTableColumn]
		public string NotaAdi { get; set; }
	}

	public enum GuitarPlanAkorColumns
	{
		ID,
		PlanID,
		NotaID,
		AkorID
	}
}
