using System;
using System.Collections.Generic;
using System.Text;

namespace AttaccoAlReee
{
    internal interface IProtettori
    {
        public abstract string Cattura();
        public abstract string DifendiRe(object sender, EventArgs e);
    }
}
