using System;
using System.Collections.Generic;
using System.Text;

namespace AttaccoAlReee
{
    internal class CRe: CPersona
    {
        public event EventHandler attaccato;
        public CRe(string nome) : base(nome)
        {
        }
        public void SubisciAttacco()
        {
            attaccato?.Invoke(this, EventArgs.Empty);
        }
    }
}
