using System.Collections.Generic;
using TDFramework.Common.TDModel;
using TDFramework.Common.Attributes;
using Models.GuitarPlanAralikModel;

namespace Models.GuitarAralikModel
{
	public class GuitarAralik : ITDModel
	{
		public GuitarAralik()
		{
			this.GuitarPlanAralikList = new List<GuitarPlanAralik>();
		}

		[PKey]
		[IDColumn]
		public int ID { get; set; }
		public string Isim { get; set; }
		public string Formul { get; set; }

		[AggregateColumn]
		public dynamic AggColumn { get; set; }

		[RTable]
		public List<GuitarPlanAralik> GuitarPlanAralikList { get; set; }
	}

	public enum GuitarAralikColumns
	{
		ID,
		Isim,
		Formul
	}
}
