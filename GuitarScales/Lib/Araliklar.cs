using Models.GuitarAralikModel;
using System;
using System.Collections.Generic;
using System.Linq;
using TDFramework;
using TDFramework.Common;

namespace Lib
{
    public class Araliklar : Notalar
    {
        public string[] AralikDondur(string Key, string Akor)
        {
            string[] donen;

            int indis = NotaList.IndexOf(Key);
            string notalar = NotaList[indis] + ",";
            int[] SeciliAkor = AralikAl(Akor);

            foreach (int i in SeciliAkor)
            {
                notalar += NotaList[(indis + i) % 12] + ",";
                indis += i;
            }

            donen = notalar.TrimEnd(',').Split(',');

            return donen;
        }

        public int[] AralikAl(string AralikAdi)
        {
            Table<GuitarAralik> tableAralik = new Table<GuitarAralik>();

            tableAralik.Columns = GuitarAralikColumns.Formul;
            tableAralik.SelectSettings.Top = 1;
            tableAralik.WhereList.Add(new Where(GuitarAralikColumns.Isim, AralikAdi));
            tableAralik.Select();

            if (tableAralik.HasData)
            {
                GuitarAralik aralik = (tableAralik.Data as List<GuitarAralik>).FirstOrDefault();

                Aralik = new int[aralik.Formul.Split(',').Length];

                for (int i = 0; i < Aralik.Length; i++)
                {
                    Aralik[i] = Convert.ToInt32(aralik.Formul.Split(',')[i]);
                }
            }

            return Aralik;
        }

        public static List<string> ComboDoldur()
        {
            List<string> cmb = new List<string>();

            Table<GuitarAralik> tableAralik = new Table<GuitarAralik>();
            tableAralik.Select();

            if (tableAralik.HasData)
            {
                List<GuitarAralik> araliklar = tableAralik.Data as List<GuitarAralik>;

                foreach (GuitarAralik item in araliklar)
                {
                    cmb.Add(item.Isim);
                }
            }

            return cmb;
        }
    }
}
