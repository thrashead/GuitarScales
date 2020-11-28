using Lib;
using Models.GuitarAkorModel;
using Models.GuitarAralikModel;
using Models.GuitarModModel;
using Models.GuitarNotaModel;
using Models.GuitarPlanAkorModel;
using Models.GuitarPlanAralikModel;
using Models.GuitarPlanlarModel;
using Models.GuitarPlanModModel;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TDFramework;
using TDFramework.Common;
using TDLibrary;

namespace GuitarScales.Controllers
{
    public class AjaxController : Controller
    {
        public JsonResult Giris(string sifre)
        {
            bool result = false;

            if (sifre == "yonet")
            {
                Session["giris"] = "yonet";
                result = true;
            }
            else
            {
                Session["giris"] = null;
            }

            return Json(result);
        }

        public JsonResult SessionKontrol()
        {
            return Json(Session["giris"] == null ? false : true);
        }

        public JsonResult NotaCombo()
        {
            Table<GuitarNota> tableNota = new Table<GuitarNota>();

            tableNota.Select();

            return Json(tableNota.Data);
        }

        public JsonResult ModCombo()
        {
            Table<GuitarMod> tableMod = new Table<GuitarMod>();

            tableMod.Select();

            return Json(tableMod.Data);
        }

        public JsonResult AkorCombo()
        {
            Table<GuitarAkor> tableAkor = new Table<GuitarAkor>();

            tableAkor.Select();

            return Json(tableAkor.Data);
        }

        public JsonResult AralikCombo()
        {
            Table<GuitarAralik> tableAralik = new Table<GuitarAralik>();

            tableAralik.Select();

            return Json(tableAralik.Data);
        }

        public JsonResult PlanCombo()
        {
            Table<GuitarPlanlar> tablePlanlar = new Table<GuitarPlanlar>();

            tablePlanlar.Select();

            return Json(tablePlanlar.Data);
        }

        public JsonResult TempNota()
        {
            Table<GuitarNota> tableNota = new Table<GuitarNota>();

            tableNota.Select();

            List<GuitarNota> notalar = new List<GuitarNota>();

            if (tableNota.HasData)
            {
                notalar.AddRange(tableNota.Data as List<GuitarNota>);
                notalar.AddRange(tableNota.Data as List<GuitarNota>);
            }

            return Json(notalar);
        }

        public JsonResult ModDon(int nota, int mod)
        {
            string notaIsim = "";
            string modIsim = "";

            Table<GuitarNota> tableNota = new Table<GuitarNota>();
            tableNota.Columns = new List<GuitarNotaColumns>() {
                GuitarNotaColumns.Diyezli,
                GuitarNotaColumns.Sayisal
            };
            tableNota.WhereList.Add(new Where(GuitarNotaColumns.ID, nota));
            tableNota.Select();

            Table<GuitarMod> tableMod = new Table<GuitarMod>();
            tableMod.Columns = GuitarModColumns.Isim;
            tableMod.WhereList.Add(new Where(GuitarModColumns.ID, mod));
            tableMod.Select();

            if (tableNota.HasData)
            {
                notaIsim = (tableNota.Data as List<GuitarNota>).FirstOrDefault().Diyezli;
            }

            if (tableMod.HasData)
            {
                modIsim = (tableMod.Data as List<GuitarMod>).FirstOrDefault().Isim;
            }

            Modlar modlar = new Modlar();
            return Json(modlar.ModDondur(notaIsim, modIsim));
        }

        public JsonResult AkorDon(int nota, int akor)
        {
            string notaIsim = "";
            string akorIsim = "";

            Table<GuitarNota> tableNota = new Table<GuitarNota>();
            tableNota.Columns = new List<GuitarNotaColumns>() {
                GuitarNotaColumns.Diyezli,
                GuitarNotaColumns.Sayisal
            };
            tableNota.WhereList.Add(new Where(GuitarNotaColumns.ID, nota));
            tableNota.Select();

            Table<GuitarAkor> tableAkor = new Table<GuitarAkor>();
            tableAkor.Columns = GuitarAkorColumns.Isim;
            tableAkor.WhereList.Add(new Where(GuitarAkorColumns.ID, akor));
            tableAkor.Select();

            if (tableNota.HasData)
            {
                notaIsim = (tableNota.Data as List<GuitarNota>).FirstOrDefault().Diyezli;
            }

            if (tableAkor.HasData)
            {
                akorIsim = (tableAkor.Data as List<GuitarAkor>).FirstOrDefault().Isim;
            }

            Akorlar akorlar = new Akorlar();
            return Json(akorlar.AkorDondur(notaIsim, akorIsim));
        }

        public JsonResult AralikDon(int nota, int aralık)
        {
            string notaIsim = "";
            string aralıkIsim = "";

            Table<GuitarNota> tableNota = new Table<GuitarNota>();
            tableNota.Columns = new List<GuitarNotaColumns>() {
                GuitarNotaColumns.Diyezli,
                GuitarNotaColumns.Sayisal
            };
            tableNota.WhereList.Add(new Where(GuitarNotaColumns.ID, nota));
            tableNota.Select();

            Table<GuitarAralik> tableAralik = new Table<GuitarAralik>();
            tableAralik.Columns = GuitarAralikColumns.Isim;
            tableAralik.WhereList.Add(new Where(GuitarAralikColumns.ID, aralık));
            tableAralik.Select();

            if (tableNota.HasData)
            {
                notaIsim = (tableNota.Data as List<GuitarNota>).FirstOrDefault().Diyezli;
            }

            if (tableAralik.HasData)
            {
                aralıkIsim = (tableAralik.Data as List<GuitarAralik>).FirstOrDefault().Isim;
            }

            Araliklar araliklar = new Araliklar();
            return Json(araliklar.AralikDondur(notaIsim, aralıkIsim));
        }

        public JsonResult PlanKaydet(string isim, string plan)
        {
            List<Plan> _planlar = JsonConvert.DeserializeObject<List<Plan>>(plan);
            int planID = 0;

            Table<GuitarPlanlar> tablePlanlar = new Table<GuitarPlanlar>();
            GuitarPlanlar planlar = new GuitarPlanlar()
            {
                Aktif = true,
                Isim = isim
            };

            tablePlanlar.Values = planlar;
            tablePlanlar.Insert(true);

            if (tablePlanlar.HasData)
            {
                planID = (int)tablePlanlar.Data;

                foreach (Plan item in _planlar)
                {
                    if (item.Tip == "mod")
                    {
                        Table<GuitarPlanMod> tablePlanMod = new Table<GuitarPlanMod>();
                        GuitarPlanMod planMod = new GuitarPlanMod()
                        {
                            ModID = item.TipID,
                            NotaID = item.NotaID,
                            PlanID = planID
                        };
                        tablePlanMod.Values = planMod;
                        tablePlanMod.Insert();

                        if (tablePlanMod.Error != null)
                        {
                            return Json(null);
                        }
                    }
                    else if (item.Tip == "akor")
                    {
                        Table<GuitarPlanAkor> tablePlanAkor = new Table<GuitarPlanAkor>();
                        GuitarPlanAkor planAkor = new GuitarPlanAkor()
                        {
                            AkorID = item.TipID,
                            NotaID = item.NotaID,
                            PlanID = planID
                        };
                        tablePlanAkor.Values = planAkor;
                        tablePlanAkor.Insert();

                        if (tablePlanAkor.Error != null)
                        {
                            return Json(null);
                        }
                    }
                    else if (item.Tip == "aralık")
                    {
                        Table<GuitarPlanAralik> tablePlanAralik = new Table<GuitarPlanAralik>();
                        GuitarPlanAralik planAkor = new GuitarPlanAralik()
                        {
                            AralikID = item.TipID,
                            NotaID = item.NotaID,
                            PlanID = planID
                        };
                        tablePlanAralik.Values = planAkor;
                        tablePlanAralik.Insert();

                        if (tablePlanAralik.Error != null)
                        {
                            return Json(null);
                        }
                    }
                }
            }
            else if (tablePlanlar.Error != null)
            {
                return Json(null);
            }

            return Json(planID);
        }

        public JsonResult PlanGetir(int planID)
        {
            List<Plan> planlar = new List<Plan>();

            Table<GuitarPlanlar> tablePlan = new Table<GuitarPlanlar>();
            tablePlan.WhereList.Add(new Where(GuitarPlanlarColumns.ID, planID));
            tablePlan.SelectSettings.Top = 1;
            tablePlan.Select();

            if (tablePlan.HasData)
            {
                GuitarPlanlar plan = (tablePlan.Data as List<GuitarPlanlar>).FirstOrDefault();

                List<GuitarPlanMod> planModlar = PlanModlar(planID);

                foreach (GuitarPlanMod planMod in planModlar)
                {
                    planlar.Add(new Plan()
                    {
                        Isim = plan.Isim,
                        NotaID = planMod.NotaID,
                        NotaIsim = ModAkorAralikNotaIsim("nota", planMod.NotaID),
                        Tip = "mod",
                        TipID = planMod.ModID,
                        TipIsim = planMod.ModAdi
                    });
                }

                List<GuitarPlanAkor> planAkorlar = PlanAkorlar(planID);

                foreach (GuitarPlanAkor planAkor in planAkorlar)
                {
                    planlar.Add(new Plan()
                    {
                        Isim = plan.Isim,
                        NotaID = planAkor.NotaID,
                        NotaIsim = ModAkorAralikNotaIsim("nota", planAkor.NotaID),
                        Tip = "akor",
                        TipID = planAkor.AkorID,
                        TipIsim = planAkor.AkorAdi
                    });
                }

                List<GuitarPlanAralik> planAraliklar = PlanAraliklar(planID);

                foreach (GuitarPlanAralik planAralik in planAraliklar)
                {
                    planlar.Add(new Plan()
                    {
                        Isim = plan.Isim,
                        NotaID = planAralik.NotaID,
                        NotaIsim = ModAkorAralikNotaIsim("nota", planAralik.NotaID),
                        Tip = "aralık",
                        TipID = planAralik.AralikID,
                        TipIsim = planAralik.AralikAdi
                    });
                }
            }

            return Json(planlar);
        }

        private List<GuitarPlanMod> PlanModlar(int planID)
        {
            List<GuitarPlanMod> planModlar = new List<GuitarPlanMod>();

            Table<GuitarPlanMod> tablePlanMod = new Table<GuitarPlanMod>();
            tablePlanMod.WhereList.Add(new Where(GuitarPlanModColumns.PlanID, planID));
            tablePlanMod.Select();

            if (tablePlanMod.HasData)
            {
                planModlar = tablePlanMod.Data as List<GuitarPlanMod>;

                foreach (GuitarPlanMod item in planModlar)
                {
                    item.ModAdi = ModAkorAralikNotaIsim("mod", item.ModID);
                }
            }

            return planModlar;
        }

        private List<GuitarPlanAkor> PlanAkorlar(int planID)
        {
            List<GuitarPlanAkor> planAkorlar = new List<GuitarPlanAkor>();

            Table<GuitarPlanAkor> tablePlanAkor = new Table<GuitarPlanAkor>();
            tablePlanAkor.WhereList.Add(new Where(GuitarPlanAkorColumns.PlanID, planID));
            tablePlanAkor.Select();

            if (tablePlanAkor.HasData)
            {
                planAkorlar = tablePlanAkor.Data as List<GuitarPlanAkor>;

                foreach (GuitarPlanAkor item in planAkorlar)
                {
                    item.AkorAdi = ModAkorAralikNotaIsim("akor", item.AkorID);
                }
            }

            return planAkorlar;
        }

        private List<GuitarPlanAralik> PlanAraliklar(int planID)
        {
            List<GuitarPlanAralik> planAraliklar = new List<GuitarPlanAralik>();

            Table<GuitarPlanAralik> tablePlanAralik = new Table<GuitarPlanAralik>();
            tablePlanAralik.WhereList.Add(new Where(GuitarPlanAralikColumns.PlanID, planID));
            tablePlanAralik.Select();

            if (tablePlanAralik.HasData)
            {
                planAraliklar = tablePlanAralik.Data as List<GuitarPlanAralik>;

                foreach (GuitarPlanAralik item in planAraliklar)
                {
                    item.AralikAdi = ModAkorAralikNotaIsim("aralık", item.AralikID);
                }
            }

            return planAraliklar;
        }

        private string ModAkorAralikNotaIsim(string tip, int tipID)
        {
            string isim = "";

            if (tip == "mod")
            {
                Table<GuitarMod> tableMod = new Table<GuitarMod>();
                tableMod.WhereList.Add(new Where(GuitarModColumns.ID, tipID));
                tableMod.SelectSettings.Top = 1;
                tableMod.Select();

                if (tableMod.HasData)
                {
                    isim = (tableMod.Data as List<GuitarMod>).FirstOrDefault().Isim;
                }
            }
            else if (tip == "akor")
            {
                Table<GuitarAkor> tableAkor = new Table<GuitarAkor>();
                tableAkor.WhereList.Add(new Where(GuitarAkorColumns.ID, tipID));
                tableAkor.SelectSettings.Top = 1;
                tableAkor.Select();

                if (tableAkor.HasData)
                {
                    isim = (tableAkor.Data as List<GuitarAkor>).FirstOrDefault().Isim;
                }
            }
            else if (tip == "aralık")
            {
                Table<GuitarAralik> tableAralik = new Table<GuitarAralik>();
                tableAralik.WhereList.Add(new Where(GuitarAralikColumns.ID, tipID));
                tableAralik.SelectSettings.Top = 1;
                tableAralik.Select();

                if (tableAralik.HasData)
                {
                    isim = (tableAralik.Data as List<GuitarAralik>).FirstOrDefault().Isim;
                }
            }
            else if (tip == "nota")
            {
                Table<GuitarNota> tableNota = new Table<GuitarNota>();
                tableNota.WhereList.Add(new Where(GuitarNotaColumns.ID, tipID));
                tableNota.SelectSettings.Top = 1;
                tableNota.Select();

                if (tableNota.HasData)
                {
                    isim = (tableNota.Data as List<GuitarNota>).FirstOrDefault().Diyezli;
                }
            }

            return isim;
        }

        public JsonResult PlanSil(int planID)
        {
            Table<GuitarPlanMod> tablePlanMod = new Table<GuitarPlanMod>();
            tablePlanMod.WhereList.Add(new Where(GuitarPlanModColumns.PlanID, planID));
            tablePlanMod.Delete();

            if (tablePlanMod.Error == null)
            {
                Table<GuitarPlanAkor> tablePlanAkor = new Table<GuitarPlanAkor>();
                tablePlanAkor.WhereList.Add(new Where(GuitarPlanAkorColumns.PlanID, planID));
                tablePlanAkor.Delete();

                if (tablePlanAkor.Error == null)
                {
                    Table<GuitarPlanlar> tablePlan = new Table<GuitarPlanlar>();
                    tablePlan.WhereList.Add(new Where(GuitarPlanlarColumns.ID, planID));
                    tablePlan.Delete();

                    if (tablePlan.Error == null)
                    {
                        return Json(true);
                    }
                }
            }

            return Json(false);
        }

        public JsonResult ModKaydet(string isim, string formul)
        {
            if (Session["giris"] != null)
            {
                Table<GuitarMod> tableMod = new Table<GuitarMod>();

                tableMod.WhereList.Add(new Where(GuitarModColumns.Formul, formul));
                tableMod.SelectSettings.Top = 1;

                tableMod.Select();

                if (tableMod.HasData)
                {
                    return Json(-1);
                }
                else
                {
                    tableMod = new Table<GuitarMod>();
                }

                tableMod.Values = new GuitarMod()
                {
                    Isim = isim,
                    Formul = formul
                };

                tableMod.Insert(true);

                if (tableMod.HasData)
                {
                    return Json((int)tableMod.Data);
                }
                else
                {
                    return Json(0);
                }
            }
            else
            {
                return Json(0);
            }
        }

        public JsonResult AkorKaydet(string isim, string formul)
        {
            if (Session["giris"] != null)
            {
                Table<GuitarAkor> tableAkor = new Table<GuitarAkor>();

                tableAkor.WhereList.Add(new Where(GuitarAkorColumns.Formul, formul));
                tableAkor.SelectSettings.Top = 1;

                tableAkor.Select();

                if (tableAkor.HasData)
                {
                    return Json(-1);
                }
                else
                {
                    tableAkor = new Table<GuitarAkor>();
                }

                tableAkor.Values = new GuitarAkor()
                {
                    Isim = isim,
                    Formul = formul
                };

                tableAkor.Insert(true);

                if (tableAkor.HasData)
                {
                    return Json((int)tableAkor.Data);
                }
                else
                {
                    return Json(0);
                }
            }
            else
            {
                return Json(0);
            }
        }

        public JsonResult ModAkorKontrol(string formul)
        {
            Table<GuitarAkor> tableAkor = new Table<GuitarAkor>();

            tableAkor.WhereList.Add(new Where(GuitarAkorColumns.Formul, formul));
            tableAkor.SelectSettings.Top = 1;

            tableAkor.Select();

            if (tableAkor.HasData)
            {
                return Json(true);
            }

            Table<GuitarMod> tableMod = new Table<GuitarMod>();

            tableMod.WhereList.Add(new Where(GuitarModColumns.Formul, formul));
            tableMod.SelectSettings.Top = 1;

            tableMod.Select();

            if (tableMod.HasData)
            {
                return Json(true);
            }

            return Json(false);
        }

        public JsonResult AkorOlustur(int tip, int nota, int modid)
        {
            List<GuitarAkor> akorlar;
            List<int> notalar = new List<int>() { nota };

            Table<GuitarMod> tableMod = new Table<GuitarMod>();
            tableMod.WhereList.Add(new Where(GuitarModColumns.ID, modid));
            tableMod.SelectSettings.Top = 1;

            tableMod.Select();

            if (tableMod.HasData)
            {
                GuitarMod mod = ((List<GuitarMod>)tableMod.Data).FirstOrDefault();

                int i = nota;

                foreach (string item in mod.Formul.Split(','))
                {
                    int j = (i + item.ToInteger()) % 12 == 0 ? 12 : (i + item.ToInteger()) % 12;

                    notalar.Add(j);

                    i = j;
                }
            }

            switch (tip)
            {
                case 3:
                    akorlar = Akorlar.TriadOlustur(notalar);
                    break;
                case 4:
                    akorlar = Akorlar.TetrachordOlustur(notalar);
                    break;
                case 5:
                    akorlar = Akorlar.PentachordOlustur(notalar);
                    break;
                case 6:
                    akorlar = Akorlar.HektachordOlustur(notalar);
                    break;
                default:
                    akorlar = Akorlar.TriadOlustur(notalar);
                    break;
            }

            return Json(akorlar);
        }

        public class Plan
        {
            public string Isim { get; set; }
            public string Tip { get; set; }
            public int TipID { get; set; }
            public string TipIsim { get; set; }
            public int NotaID { get; set; }
            public string NotaIsim { get; set; }
        }
    }
}