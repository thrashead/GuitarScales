using System.Collections.Generic;
using System.Web.Mvc;
using TDFramework.Common.TDModel;
using TDFramework.Common.Attributes;

namespace Models.GuitarPlanAralikModel
{
	public class GuitarPlanAralik : ITDModel
	{
		public GuitarPlanAralik()
		{
			this.GuitarNotaList = new List<SelectListItem>();
			this.GuitarAralikList = new List<SelectListItem>();
			this.GuitarPlanAralikList = new List<SelectListItem>();
		}

		[PKey]
		[IDColumn]
		public int ID { get; set; }
		public int PlanID { get; set; }
		public int NotaID { get; set; }
		public int AralikID { get; set; }

		[AggregateColumn]
		public dynamic AggColumn { get; set; }

		[NotTableColumn]
		public List<SelectListItem> GuitarNotaList { get; set; }
		[NotTableColumn]
		public List<SelectListItem> GuitarAralikList { get; set; }
		[NotTableColumn]
		public List<SelectListItem> GuitarPlanAralikList { get; set; }
        [NotTableColumn]
        public string PlanlarAdi { get; set; }
        [NotTableColumn]
		public string NotaAdi { get; set; }
		[NotTableColumn]
		public string AralikAdi { get; set; }
    }

	public enum GuitarPlanAralikColumns
	{
		ID,
		PlanID,
		NotaID,
		AralikID
	}
}
