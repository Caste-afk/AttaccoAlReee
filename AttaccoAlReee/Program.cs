using System;
using System.Collections.Generic;
using System.IO;

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

            string benvenuto = "===BENVENUTO IN CATTURA IL REEE===";
            Console.WriteLine(benvenuto);
            ScriviSuFile(benvenuto);

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
            string[] righe = text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);//elimina dall'array possibili spazi vuoti, senza non va!!!

            re = new CRe(righe[0].Trim());

            string[] stringaGuardie = righe[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (string guardia in stringaGuardie)
            {
                guardie.Add(new CGuardia(guardia.Trim().ToLower()));
            }

            string[] stringaPedoni = righe[2].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (string pedone in stringaPedoni)
            {
                pedoni.Add(new CPedone(pedone.Trim().ToLower()));
            }

            string[] resto = new string[righe.Length - 3];
            Array.Copy(righe, 3, resto, 0, resto.Length);
            return resto;
        }

        private static void LeggiCombattimento(string[] azioni)
        {
            if (azioni.Length > 100)
            {
                Console.WriteLine("Numero comandi >100!!\nUscita in corso!");
                return;
            }

            foreach (var a in azioni)
            {
                if (string.IsNullOrWhiteSpace(a))
                    continue; // ignora righe vuote

                string[] dati = a.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                string azione = dati[0].Trim().ToLower();
                string nome = dati.Length > 1 ? dati[1].Trim().ToLower() : "";

                if (azione == "cattura")
                {
                    // controlla sempre prima le guardie
                    int idxGuardia = TrovaPersone(nome, false);
                    if (idxGuardia != -1)
                    {
                        CGuardia g = guardie[idxGuardia];
                        string msg = g.Cattura();
                        Console.WriteLine(msg);
                        ScriviSuFile(msg);
                        guardie.RemoveAt(idxGuardia);

                        if (idxGuardia <= guardiaAttiva && guardiaAttiva > 0)
                            guardiaAttiva--;
                    }
                    else
                    {
                        int idxPedone = TrovaPersone(nome, true);
                        if (idxPedone != -1)
                        {
                            CPedone p = pedoni[idxPedone];
                            string msg = p.Cattura();
                            Console.WriteLine(msg);
                            ScriviSuFile(msg);
                            pedoni.RemoveAt(idxPedone);

                            if (idxPedone <= pedoneAttivo && pedoneAttivo > 0)
                                pedoneAttivo--;
                        }
                        else
                        {
                            string msg = $"{nome} è già stato catturato!";
                            Console.WriteLine(msg);
                            ScriviSuFile(msg);
                        }
                    }
                }
                else if (azione == "attacca")
                {
                    if (guardiaAttiva < guardie.Count)
                    {
                        string msg = $"{re.GetNome()} è sotto attacco!    {guardie[guardiaAttiva].DifendiRe(null, null)}";
                        Console.WriteLine(msg);
                        ScriviSuFile(msg);
                        guardiaAttiva++;
                    }
                    else if (pedoneAttivo < pedoni.Count)
                    {
                        string msg = $"{re.GetNome} è sotto attacco!{pedoni[pedoneAttivo].DifendiRe(null, null)}";
                        Console.WriteLine(msg);
                        ScriviSuFile(msg);
                        pedoneAttivo++;
                    }
                    else
                    {
                        string msg = "Nessuno può più difendere il re. Hai vinto!";
                        Console.WriteLine(msg);
                        ScriviSuFile(msg);
                        return;
                    }

                    re.SubisciAttacco();
                }
                else if (azione == "end")
                {
                    string msg = "Re non catturato, hai perso!\n\n";
                    Console.WriteLine(msg);
                    ScriviSuFile(msg);
                    return;
                }
            }
        }

        private static int TrovaPersone(string nome, bool lista)// true pedone, false guardia
        {
            if (lista)
            {
                for (int i = 0; i < pedoni.Count; i++)
                {
                    if (pedoni[i].GetNome().Trim().ToLower() == nome)
                        return i;
                }
            }
            else
            {
                for (int i = 0; i < guardie.Count; i++)
                {
                    if (guardie[i].GetNome().Trim().ToLower() == nome)
                        return i;
                }
            }
            return -1;
        }

        private static void ScriviSuFile(string text)
        {
            string path = @"data\registro_partite.txt";
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                sw.WriteLine(text);
            }
        }
    }
}
