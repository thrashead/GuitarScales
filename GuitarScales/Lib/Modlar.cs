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
                notalar += NotaList[(indis + i) % 12] + ",";
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

        ////////////

        public static GuitarMod ModBul(string Formul)
        {
            GuitarMod don = null;

            Table<GuitarMod> tableMod = new Table<GuitarMod>();
            tableMod.SelectSettings.Top = 1;
            tableMod.WhereList.Add(new Where(GuitarModColumns.Formul, Formul));
            tableMod.Select();

            if (tableMod.HasData)
            {
                don = ((List<GuitarMod>)tableMod.Data).FirstOrDefault();
            }

            return don;
        }

        public static List<GuitarMod> PentaModOlustur(List<int> notalar)
        {
            List<int> modNotalar = modCokla(notalar);
            List<GuitarMod> pentaDon = new List<GuitarMod>();

            for (int i = 0; i < modNotalar.Count; i++)
            {
                for (int j = i + 1; j < modNotalar.Count; j++)
                {
                    for (int k = j + 1; k < modNotalar.Count; k++)
                    {
                        for (int l = k + 1; l < modNotalar.Count; l++)
                        {
                            for (int m = l + 1; m < modNotalar.Count; m++)
                            {
                                int fark1 = modNotalar[j] - modNotalar[i];
                                int fark2 = modNotalar[k] - modNotalar[j];
                                int fark3 = modNotalar[l] - modNotalar[k];
                                int fark4 = modNotalar[m] - modNotalar[l];

                                if (fark1 < 0)
                                    fark1 = (modNotalar[j] + 12) - modNotalar[i];
                                else if (fark1 == 0)
                                    break;

                                if (fark2 < 0)
                                    fark2 = (modNotalar[k] + 12) - modNotalar[j];
                                else if (fark2 == 0)
                                    break;

                                if (fark3 < 0)
                                    fark3 = (modNotalar[l] + 12) - modNotalar[k];
                                else if (fark3 == 0)
                                    break;

                                if (fark4 < 0)
                                    fark4 = (modNotalar[m] + 12) - modNotalar[l];
                                else if (fark4 == 0)
                                    break;

                                string formul = fark1.ToString() + "," + fark2.ToString() + "," + fark3.ToString() + "," + fark4.ToString();

                                GuitarMod modbul = ModBul(formul);

                                if (modbul != null)
                                {
                                    modbul.Nota = modNotalar[i];
                                    modbul.NotaIsim = NotaDon(modNotalar[i]);

                                    if (pentaDon.Where(a => a.Nota == modbul.Nota && a.ID == modbul.ID).Count() <= 0)
                                        pentaDon.Add(modbul);
                                }
                            }
                        }
                    }
                }
            }

            return pentaDon;
        }

        public static List<GuitarMod> HektaModOlustur(List<int> notalar)
        {
            List<int> modNotalar = modCokla(notalar);
            List<GuitarMod> hektaDon = new List<GuitarMod>();

            for (int i = 0; i < modNotalar.Count; i++)
            {
                for (int j = i + 1; j < modNotalar.Count; j++)
                {
                    for (int k = j + 1; k < modNotalar.Count; k++)
                    {
                        for (int l = k + 1; l < modNotalar.Count; l++)
                        {
                            for (int m = l + 1; m < modNotalar.Count; m++)
                            {
                                for (int n = m + 1; n < modNotalar.Count; n++)
                                {
                                    int fark1 = modNotalar[j] - modNotalar[i];
                                    int fark2 = modNotalar[k] - modNotalar[j];
                                    int fark3 = modNotalar[l] - modNotalar[k];
                                    int fark4 = modNotalar[m] - modNotalar[l];
                                    int fark5 = modNotalar[n] - modNotalar[m];

                                    if (fark1 < 0)
                                        fark1 = (modNotalar[j] + 12) - modNotalar[i];
                                    else if (fark1 == 0)
                                        break;

                                    if (fark2 < 0)
                                        fark2 = (modNotalar[k] + 12) - modNotalar[j];
                                    else if (fark2 == 0)
                                        break;

                                    if (fark3 < 0)
                                        fark3 = (modNotalar[l] + 12) - modNotalar[k];
                                    else if (fark3 == 0)
                                        break;

                                    if (fark4 < 0)
                                        fark4 = (modNotalar[m] + 12) - modNotalar[l];
                                    else if (fark4 == 0)
                                        break;

                                    if (fark5 < 0)
                                        fark5 = (modNotalar[n] + 12) - modNotalar[m];
                                    else if (fark5 == 0)
                                        break;

                                    string formul = fark1.ToString() + "," + fark2.ToString() + "," + fark3.ToString() + "," + fark4.ToString() + "," + fark5.ToString();

                                    GuitarMod modbul = ModBul(formul);

                                    if (modbul != null)
                                    {
                                        modbul.Nota = modNotalar[i];
                                        modbul.NotaIsim = NotaDon(modNotalar[i]);

                                        if (hektaDon.Where(a => a.Nota == modbul.Nota && a.ID == modbul.ID).Count() <= 0)
                                            hektaDon.Add(modbul);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return hektaDon;
        }

        public static List<GuitarMod> HeptaModOlustur(List<int> notalar)
        {
            List<int> modNotalar = modCokla(notalar);
            List<GuitarMod> heptaDon = new List<GuitarMod>();

            for (int i = 0; i < modNotalar.Count; i++)
            {
                for (int j = i + 1; j < modNotalar.Count; j++)
                {
                    for (int k = j + 1; k < modNotalar.Count; k++)
                    {
                        for (int l = k + 1; l < modNotalar.Count; l++)
                        {
                            for (int m = l + 1; m < modNotalar.Count; m++)
                            {
                                for (int n = m + 1; n < modNotalar.Count; n++)
                                {
                                    for (int o = n + 1; o < modNotalar.Count; o++)
                                    {
                                        int fark1 = modNotalar[j] - modNotalar[i];
                                        int fark2 = modNotalar[k] - modNotalar[j];
                                        int fark3 = modNotalar[l] - modNotalar[k];
                                        int fark4 = modNotalar[m] - modNotalar[l];
                                        int fark5 = modNotalar[n] - modNotalar[m];
                                        int fark6 = modNotalar[o] - modNotalar[n];

                                        if (fark1 < 0)
                                            fark1 = (modNotalar[j] + 12) - modNotalar[i];
                                        else if (fark1 == 0)
                                            break;

                                        if (fark2 < 0)
                                            fark2 = (modNotalar[k] + 12) - modNotalar[j];
                                        else if (fark2 == 0)
                                            break;

                                        if (fark3 < 0)
                                            fark3 = (modNotalar[l] + 12) - modNotalar[k];
                                        else if (fark3 == 0)
                                            break;

                                        if (fark4 < 0)
                                            fark4 = (modNotalar[m] + 12) - modNotalar[l];
                                        else if (fark4 == 0)
                                            break;

                                        if (fark5 < 0)
                                            fark5 = (modNotalar[n] + 12) - modNotalar[m];
                                        else if (fark5 == 0)
                                            break;

                                        if (fark6 < 0)
                                            fark6 = (modNotalar[o] + 12) - modNotalar[n];
                                        else if (fark6 == 0)
                                            break;

                                        string formul = fark1.ToString() + "," + fark2.ToString() + "," + fark3.ToString() + "," + fark4.ToString() + "," + fark5.ToString() + "," + fark6.ToString();

                                        GuitarMod modbul = ModBul(formul);

                                        if (modbul != null)
                                        {
                                            modbul.Nota = modNotalar[i];
                                            modbul.NotaIsim = NotaDon(modNotalar[i]);

                                            if (heptaDon.Where(a => a.Nota == modbul.Nota && a.ID == modbul.ID).Count() <= 0)
                                                heptaDon.Add(modbul);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return heptaDon;
        }

        public static List<GuitarMod> OktaModOlustur(List<int> notalar)
        {
            List<int> modNotalar = modCokla(notalar);
            List<GuitarMod> oktaDon = new List<GuitarMod>();

            for (int i = 0; i < modNotalar.Count; i++)
            {
                for (int j = i + 1; j < modNotalar.Count; j++)
                {
                    for (int k = j + 1; k < modNotalar.Count; k++)
                    {
                        for (int l = k + 1; l < modNotalar.Count; l++)
                        {
                            for (int m = l + 1; m < modNotalar.Count; m++)
                            {
                                for (int n = m + 1; n < modNotalar.Count; n++)
                                {
                                    for (int o = n + 1; o < modNotalar.Count; o++)
                                    {
                                        for (int p = o + 1; p < modNotalar.Count; p++)
                                        {
                                            int fark1 = modNotalar[j] - modNotalar[i];
                                            int fark2 = modNotalar[k] - modNotalar[j];
                                            int fark3 = modNotalar[l] - modNotalar[k];
                                            int fark4 = modNotalar[m] - modNotalar[l];
                                            int fark5 = modNotalar[n] - modNotalar[m];
                                            int fark6 = modNotalar[o] - modNotalar[n];
                                            int fark7 = modNotalar[p] - modNotalar[o];

                                            if (fark1 < 0)
                                                fark1 = (modNotalar[j] + 12) - modNotalar[i];
                                            else if (fark1 == 0)
                                                break;

                                            if (fark2 < 0)
                                                fark2 = (modNotalar[k] + 12) - modNotalar[j];
                                            else if (fark2 == 0)
                                                break;

                                            if (fark3 < 0)
                                                fark3 = (modNotalar[l] + 12) - modNotalar[k];
                                            else if (fark3 == 0)
                                                break;

                                            if (fark4 < 0)
                                                fark4 = (modNotalar[m] + 12) - modNotalar[l];
                                            else if (fark4 == 0)
                                                break;

                                            if (fark5 < 0)
                                                fark5 = (modNotalar[n] + 12) - modNotalar[m];
                                            else if (fark5 == 0)
                                                break;

                                            if (fark6 < 0)
                                                fark6 = (modNotalar[o] + 12) - modNotalar[n];
                                            else if (fark6 == 0)
                                                break;

                                            if (fark7 < 0)
                                                fark7 = (modNotalar[p] + 12) - modNotalar[o];
                                            else if (fark7 == 0)
                                                break;

                                            string formul = fark1.ToString() + "," + fark2.ToString() + "," + fark3.ToString() + "," + fark4.ToString() + "," + fark5.ToString() + "," + fark6.ToString() + "," + fark7.ToString();

                                            GuitarMod modbul = ModBul(formul);

                                            if (modbul != null)
                                            {
                                                modbul.Nota = modNotalar[i];
                                                modbul.NotaIsim = NotaDon(modNotalar[i]);

                                                if (oktaDon.Where(a => a.Nota == modbul.Nota && a.ID == modbul.ID).Count() <= 0)
                                                    oktaDon.Add(modbul);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return oktaDon;
        }

        static List<int> modCokla(List<int> notalar, int adet = 3)
        {
            List<int> notalarTemp = new List<int>();

            for (int i = 0; i < adet; i++)
            {
                notalarTemp.AddRange(notalar);
            }

            return notalarTemp;
        }
    }
}
