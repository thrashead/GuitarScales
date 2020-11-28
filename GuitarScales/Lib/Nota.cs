using System.Collections.Generic;

namespace Lib
{
    public class Notalar
    {
        public List<string> NotaList
        {
            get
            {
                return new List<string> { "A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#" }; ;
            }
        }

        public List<int> NumerikNotaList
        {
            get
            {
                return new List<int> { 10, 11, 12, 1, 2, 3, 4, 5, 6, 7, 8, 9 }; ;
            }
        }

        public int[] Mod { get; set; }
        public int[] Akor { get; set; }
        public int[] Aralik { get; set; }

        public static List<string> NotalarDon(int[] notalar)
        {
            Notalar nota = new Notalar();
            List<string> numerikNotalar = new List<string>();

            foreach (int item in notalar)
            {
                int indexNota = nota.NumerikNotaList.IndexOf(item);

                numerikNotalar.Add(nota.NotaList[indexNota]);
            }

            return numerikNotalar;
        }

        public static List<int> NumerikNotaListDon(List<string> notalar)
        {
            Notalar nota = new Notalar();
            List<int> numerikNotalar = new List<int>();

            foreach (string item in notalar)
            {
                int indexNota = nota.NotaList.IndexOf(item);

                numerikNotalar.Add(nota.NumerikNotaList[indexNota]);
            }

            return numerikNotalar;
        }

        public static string NotaDon(int numNota)
        {
            Notalar nota = new Notalar();
            string numerikNota;

            numerikNota = nota.NotaList[nota.NumerikNotaList.IndexOf(numNota)];

            return numerikNota;
        }

        public static int NumerikNotaDon(string textNota)
        {
            Notalar nota = new Notalar();
            int numerikNota;

            numerikNota = nota.NumerikNotaList[nota.NotaList.IndexOf(textNota)];

            return numerikNota;
        }
    }
}
