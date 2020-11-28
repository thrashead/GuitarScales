using Models.GuitarModModel;
using System;
using System.Collections.Generic;
using System.Linq;
using TDFramework;
using TDFramework.Common;

namespace Lib
{
    public class Modlar : Notalar
    {
        public string[] ModDondur(string Key, string Mod)
        {
            string[] donen;

            int indis = NotaList.IndexOf(Key);
            string notalar = NotaList[indis] + ",";
            int[] SeciliMod = ModAl(Mod);

            foreach (int i in SeciliMod)
	        {
                notalar += NotaList[(indis + i)%12] + ",";
                indis += i;
	        }

            donen = notalar.TrimEnd(',').Split(',');

            return donen;
        }

        public int[] ModAl(string ModAdi)
        {
            Table<GuitarMod> tableMod = new Table<GuitarMod>();

            tableMod.Columns = GuitarModColumns.Formul;
            tableMod.SelectSettings.Top = 1;
            tableMod.WhereList.Add(new Where(GuitarModColumns.Isim, ModAdi));
            tableMod.Select();

            if (tableMod.HasData)
            {
                GuitarMod mod = (tableMod.Data as List<GuitarMod>).FirstOrDefault();

                Mod = new int[mod.Formul.Split(',').Length];

                for (int i = 0; i < Mod.Length; i++)
                {
                    Mod[i] = Convert.ToInt32(mod.Formul.Split(',')[i]);
                }
            }

            return Mod;
        }

        public static List<string> ComboDoldur()
        {
            List<string> cmb = new List<string>();

            Table<GuitarMod> tableMod = new Table<GuitarMod>();
            tableMod.Select();

            if (tableMod.HasData)
            {
                List<GuitarMod> modlar = tableMod.Data as List<GuitarMod>;

                foreach (GuitarMod item in modlar)
                {
                    cmb.Add(item.Isim);
                }
            }

            return cmb;
        }
    }
}
