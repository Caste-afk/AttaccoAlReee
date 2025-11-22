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

        static Dictionary<CGuardia, EventHandler> handlerGuardie = new Dictionary<CGuardia, EventHandler>();
        static Dictionary<CPedone, EventHandler> handlerPedoni = new Dictionary<CPedone, EventHandler>();


        static void Main(string[] args)
        {
            guardie = new List<CGuardia>();
            pedoni = new List<CPedone>();


            ScriviLog("=== BENVENUTO IN CATTURA IL REEE ===\n");

            string text = LeggiInput();
            string[] azioni = CostruisciPersonaggi(text);

            LeggiCombattimento(azioni);

            ScriviLog("\n=== FINE PARTITA ===");
        }

        private static string LeggiInput()
        {
            string path = @"data\file.txt";
            using StreamReader r = new StreamReader(path);
            return r.ReadToEnd();
        }

        private static string[] CostruisciPersonaggi(string text)
        {
            string[] righe = text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            re = new CRe(righe[0].Trim());

            string[] nomiGuardie = righe[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (string g in nomiGuardie)
            {
                var guardia = new CGuardia(g.Trim().ToLower());
                EventHandler handler = (sender, e) =>
                {
                    string msg = guardia.DifendiRe(sender, e);
                    if (!string.IsNullOrEmpty(msg))
                        ScriviLog($"    La Guardia {guardia.GetNome()} sta difendendo");
                };
                re.attaccato += handler;
                handlerGuardie[guardia] = handler;
                guardie.Add(guardia);
            }

            string[] nomiPedoni = righe[2].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (string p in nomiPedoni)
            {
                var pedone = new CPedone(p.Trim().ToLower());
                EventHandler handler = (sender, e) =>
                {
                    string msg = pedone.DifendiRe(sender, e);
                    if (!string.IsNullOrEmpty(msg))
                        ScriviLog($"    Il Pedone {pedone.GetNome()} si sta preparando");
                };
                re.attaccato += handler;
                handlerPedoni[pedone] = handler;
                pedoni.Add(pedone);
            }

            string[] resto = new string[righe.Length - 3];
            Array.Copy(righe, 3, resto, 0, resto.Length);
            return resto;
        }

        private static void LeggiCombattimento(string[] azioni)
        {
            foreach (var riga in azioni)
            {
                if (string.IsNullOrWhiteSpace(riga)) continue;

                string[] dati = riga.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                string comando = dati[0].ToLower();
                string nome = dati.Length > 1 ? dati[1].ToLower() : "";

                switch (comando)
                {
                    case "cattura":
                        if (!CatturaPersonaggio(nome))
                            ScriviLog($"    {nome} è già stato catturato!");
                        break;

                    case "attacca":
                        ScriviLog($"\n=== ATTACCO AL RE {re.GetNome().ToUpper()} ===");
                        ScriviLog($"Il Re {re.GetNome()} è sotto attacco!");
                        re.SubisciAttacco();
                        break;

                    case "end":
                        ScriviLog($"\nRe non catturato, hai perso!");
                        return;
                }
            }
        }

        private static bool CatturaPersonaggio(string nome)
        {
            foreach (var g in guardie)
            {
                if (g.GetNome() == nome && !g.catturato)
                {
                    re.attaccato -= handlerGuardie[g];
                    handlerGuardie.Remove(g);
                    ScriviLog($"    Cattura Guardia {g.GetNome()}");
                    g.Cattura();
                    return true;
                }
            }

            foreach (var p in pedoni)
            {
                if (p.GetNome() == nome && !p.catturato)
                {
                    re.attaccato -= handlerPedoni[p];
                    handlerPedoni.Remove(p);
                    ScriviLog($"    Cattura Pedone {p.GetNome()}");
                    p.Cattura();
                    return true;
                }
            }

            return false;
        }

        private static void ScriviLog(string msg)
        {
            string path = @"data\output.txt";
            Console.WriteLine(msg);
            using StreamWriter sw = new StreamWriter(path, true);
            sw.WriteLine(msg);
        }
    }
}
