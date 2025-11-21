using System;
using System.Collections.Generic;
using System.Text;

namespace AttaccoAlReee
{
    internal class CPedone : CPersona, IProtettori
    {
        public bool catturato { get; private set; }

        public CPedone(string nome) : base(nome)
        {
            catturato = false;
        }
        public string DifendiRe(object sender, EventArgs e)
        {
            return $"{nome} si prepara!";
        }

        public string Cattura()
        {
            catturato = true;
            return $"{nome} è stato catturato!";
        }

    }
}
