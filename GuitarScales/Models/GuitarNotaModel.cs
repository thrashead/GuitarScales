using System.Collections.Generic;
using TDFramework.Common.TDModel;
using TDFramework.Common.Attributes;
using Models.GuitarPlanModModel;
using Models.GuitarPlanAkorModel;

namespace Models.GuitarNotaModel
{
	public class GuitarNota : ITDModel
	{
		public GuitarNota()
		{
			this.PlanModList = new List<GuitarPlanMod>();
			this.PlanAkorList = new List<GuitarPlanAkor>();
		}

		[PKey]
		[IDColumn]
		public int ID { get; set; }
		public string Diyezli { get; set; }
        public string GelenekselDiyezli { get; set; }
        public int Sayisal { get; set; }
		public string Bemollu { get; set; }
		public string GelenekselBemollu { get; set; }

		[AggregateColumn]
		public dynamic AggColumn { get; set; }

		[RTable]
		public List<GuitarPlanMod> PlanModList { get; set; }
		[RTable]
		public List<GuitarPlanAkor> PlanAkorList { get; set; }
	}

	public enum GuitarNotaColumns
	{
		ID,
		Diyezli,
		GelenekselDiyezli,
		Sayisal,
		Bemollu,
		GelenekselBemollu
	}
}
