using System;
using System.Collections.Generic;
using System.Text;

namespace AttaccoAlReee
{
    internal class CGuardia:CPersona, IProtettori
    {
        public bool catturato { get; private set; }

        public CGuardia(string nome) : base(nome)
        {
            catturato = false;
        }

        public string DifendiRe(object sender, EventArgs e)
        {
            return $"{nome} difende il re!";
        }


        public string Cattura()
        {
            catturato = true;
            return $"{nome} è stato catturato!";
        }
    }
}
