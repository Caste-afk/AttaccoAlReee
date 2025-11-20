using System;
using System.Collections.Generic;

namespace AttaccoAlReee
{
    internal class Program
    {
        static CRe re;
        static List<CGuardia> guardie;
        static List<CPedone> pedoni;
        static int pedoneAttivo;
        static int guardiaAttiva;

        static void Main(string[] args)
        {
            guardie = new List<CGuardia>();
            pedoni = new List<CPedone>();
            pedoneAttivo = 0;
            guardiaAttiva = 0;

            Console.WriteLine("===BENVENUTO IN CATTURA IL REEE===\n");

            string text = leggiInput(); // legge il file di testo
            string[] azioni = CostruisciPersonaggi(text); // costruisce i personaggi
            LeggiCombattimento(azioni);
        }

        private static string leggiInput()
        {
            string path = @"data\file.txt";
            using (StreamReader r = new StreamReader(path))
            {
                return r.ReadToEnd();
            }
        }

        private static string[] CostruisciPersonaggi(string text)
        {
            string[] righe = text.Split(new[] { '\n', '\r' });

            re = new CRe(righe[0]);

            string[] stringaGuardie = righe[1].Split(' ');
            foreach (string guardia in stringaGuardie)
            {
                guardie.Add(new CGuardia(guardia.ToLower()));
            }

            string[] stringaPedoni = righe[2].Split(' ');
            foreach (string pedone in stringaPedoni)
            {
                pedoni.Add(new CPedone(pedone.ToLower()));
            }

            string[] resto = new string[righe.Length - 3];
            Array.Copy(righe, 3, resto, 0, resto.Length);
            return resto;
        }

        private static void LeggiCombattimento(string[] azioni)
        {
            foreach (var a in azioni)
            {
                if (string.IsNullOrWhiteSpace(a))
                    continue; //ignora righe vuote

                string[] dati = a.Split(' ');
                string azione = dati[0].ToLower();
                string nome = dati.Length > 1 ? dati[1].ToLower() : "";

                if (azione == "cattura")
                {
                    int idxGuardia = TrovaPersone(nome, false);
                    int idxPedone = TrovaPersone(nome, true);

                    if (idxGuardia != -1)
                    {
                        CGuardia g = guardie[idxGuardia];

                        Console.WriteLine(g.Cattura());
                        guardie.RemoveAt(idxGuardia);

                        if (idxGuardia <= guardiaAttiva && guardiaAttiva > 0)
                            guardiaAttiva--;
                    }
                    else if (idxPedone != -1)
                    {
                        CPedone p = pedoni[idxPedone];
                        Console.WriteLine(p.Cattura());
                        pedoni.RemoveAt(idxPedone);

                        if (idxPedone <= pedoneAttivo && pedoneAttivo > 0)
                            pedoneAttivo--;
                    }
                    else
                    {
                        Console.WriteLine($"{nome} è già catturato!");
                    }
                }
                else if (azione == "attacca")
                {
                    if (guardiaAttiva < guardie.Count)
                    {
                        Console.WriteLine(guardie[guardiaAttiva].DifendiRe(null, null));
                        guardiaAttiva++;
                    }
                    else if (pedoneAttivo < pedoni.Count)
                    {
                        Console.WriteLine(pedoni[pedoneAttivo].DifendiRe(null, null));
                        pedoneAttivo++;
                    }
                    else
                    {
                        Console.WriteLine("Nessuno può più difendere il re. Hai vinto!");
                        return;
                    }

                    re.SubisciAttacco();
                }
                else if (azione == "end")
                {
                    Console.WriteLine("Re non catturato, hai perso!");
                    return;
                }
            }
        }

        private static int TrovaPersone(string nome, bool lista)
        {
            if (lista)
            {
                for (int i = 0; i < pedoni.Count; i++)
                {
                    if (pedoni[i].GetNome().ToLower() == nome)
                        return i;
                }
            }
            else
            {
                for (int i = 0; i < guardie.Count; i++)
                {
                    if (guardie[i].GetNome().ToLower() == nome)
                        return i;
                }
            }
            return -1;
        }
    }
}
