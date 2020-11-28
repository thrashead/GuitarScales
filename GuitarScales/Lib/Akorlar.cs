using Models.GuitarAkorModel;
using System;
using System.Collections.Generic;
using System.Linq;
using TDFramework;
using TDFramework.Common;

namespace Lib
{
    public class Akorlar : Notalar
    {
        public string[] AkorDondur(string Key, string Akor)
        {
            string[] donen;

            int indis = NotaList.IndexOf(Key);
            string notalar = NotaList[indis] + ",";
            int[] SeciliAkor = AkorAl(Akor);

            foreach (int i in SeciliAkor)
            {
                notalar += NotaList[(indis + i) % 12] + ",";
                indis += i;
            }

            donen = notalar.TrimEnd(',').Split(',');

            return donen;
        }

        public int[] AkorAl(string AkorAdi)
        {
            Table<GuitarAkor> tableAkor = new Table<GuitarAkor>();

            tableAkor.Columns = GuitarAkorColumns.Formul;
            tableAkor.SelectSettings.Top = 1;
            tableAkor.WhereList.Add(new Where(GuitarAkorColumns.Isim, AkorAdi));
            tableAkor.Select();

            if (tableAkor.HasData)
            {
                GuitarAkor akor = (tableAkor.Data as List<GuitarAkor>).FirstOrDefault();

                Akor = new int[akor.Formul.Split(',').Length];

                for (int i = 0; i < Akor.Length; i++)
                {
                    Akor[i] = Convert.ToInt32(akor.Formul.Split(',')[i]);
                }
            }

            return Akor;
        }

        public static List<string> ComboDoldur()
        {
            List<string> cmb = new List<string>();

            Table<GuitarAkor> tableAkor = new Table<GuitarAkor>();
            tableAkor.Select();

            if (tableAkor.HasData)
            {
                List<GuitarAkor> akorlar = tableAkor.Data as List<GuitarAkor>;

                foreach (GuitarAkor item in akorlar)
                {
                    cmb.Add(item.Isim);
                }
            }

            return cmb;
        }

        ////////////

        public static GuitarAkor AkorBul(string Formul)
        {
            GuitarAkor don = null;

            Table<GuitarAkor> tableAkor = new Table<GuitarAkor>();
            tableAkor.SelectSettings.Top = 1;
            tableAkor.WhereList.Add(new Where(GuitarAkorColumns.Formul, Formul));
            tableAkor.Select();

            if (tableAkor.HasData)
            {
                don = ((List<GuitarAkor>)tableAkor.Data).FirstOrDefault();
            }

            return don;
        }

        public static List<GuitarAkor> TriadOlustur(List<int> notalar)
        {
            List<int> modNotalar = modCokla(notalar);
            List<GuitarAkor> triadDon = new List<GuitarAkor>();

            for (int i = 0; i < modNotalar.Count; i++)
            {
                for (int j = i + 1; j < modNotalar.Count; j++)
                {
                    for (int k = j + 1; k < modNotalar.Count; k++)
                    {
                        int fark1 = modNotalar[j] - modNotalar[i];
                        int fark2 = modNotalar[k] - modNotalar[j];

                        if (fark1 < 0)
                            fark1 = (modNotalar[j] + 12) - modNotalar[i];
                        else if (fark1 == 0)
                            break;

                        if (fark2 < 0)
                            fark2 = (modNotalar[k] + 12) - modNotalar[j];
                        else if (fark2 == 0)
                            break;

                        string formul = fark1.ToString() + "," + fark2.ToString();

                        GuitarAkor akorbul = AkorBul(formul);

                        if (akorbul != null)
                        {
                            akorbul.Nota = modNotalar[i];
                            akorbul.NotaIsim = NotaDon(modNotalar[i]);

                            if (triadDon.Where(a => a.Nota == akorbul.Nota && a.ID == akorbul.ID).Count() <= 0)
                                triadDon.Add(akorbul);
                        }
                    }
                }
            }

            return triadDon;
        }

        public static List<GuitarAkor> TetrachordOlustur(List<int> notalar)
        {
            List<int> modNotalar = modCokla(notalar);
            List<GuitarAkor> tetraDon = new List<GuitarAkor>();

            for (int i = 0; i < modNotalar.Count; i++)
            {
                for (int j = i + 1; j < modNotalar.Count; j++)
                {
                    for (int k = j + 1; k < modNotalar.Count; k++)
                    {
                        for (int l = k + 1; l < modNotalar.Count; l++)
                        {
                            int fark1 = modNotalar[j] - modNotalar[i];
                            int fark2 = modNotalar[k] - modNotalar[j];
                            int fark3 = modNotalar[l] - modNotalar[k];

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

                            string formul = fark1.ToString() + "," + fark2.ToString() + "," + fark3.ToString();

                            GuitarAkor akorbul = AkorBul(formul);

                            if (akorbul != null)
                            {
                                akorbul.Nota = modNotalar[i];
                                akorbul.NotaIsim = NotaDon(modNotalar[i]);

                                if (tetraDon.Where(a => a.Nota == akorbul.Nota && a.ID == akorbul.ID).Count() <= 0)
                                    tetraDon.Add(akorbul);
                            }
                        }
                    }
                }
            }

            return tetraDon;
        }

        public static List<GuitarAkor> PentachordOlustur(List<int> notalar)
        {
            List<int> modNotalar = modCokla(notalar);
            List<GuitarAkor> pentaDon = new List<GuitarAkor>();

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

                                GuitarAkor akorbul = AkorBul(formul);

                                if (akorbul != null)
                                {
                                    akorbul.Nota = modNotalar[i];
                                    akorbul.NotaIsim = NotaDon(modNotalar[i]);

                                    if (pentaDon.Where(a => a.Nota == akorbul.Nota && a.ID == akorbul.ID).Count() <= 0)
                                        pentaDon.Add(akorbul);
                                }
                            }
                        }
                    }
                }
            }

            return pentaDon;
        }

        public static List<GuitarAkor> HektachordOlustur(List<int> notalar)
        {
            List<int> modNotalar = modCokla(notalar);
            List<GuitarAkor> hektaDon = new List<GuitarAkor>();

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

                                    GuitarAkor akorbul = AkorBul(formul);

                                    if (akorbul != null)
                                    {
                                        akorbul.Nota = modNotalar[i];
                                        akorbul.NotaIsim = NotaDon(modNotalar[i]);

                                        if (hektaDon.Where(a => a.Nota == akorbul.Nota && a.ID == akorbul.ID).Count() <= 0)
                                            hektaDon.Add(akorbul);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return hektaDon;
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
