using System;
using System.Collections.Generic;
using System.Text;

namespace AttaccoAlReee
{
    abstract internal class CPersona
    {
        protected string nome;
        
        protected CPersona(string nome)
        {
            this.nome = nome;
        }

        public string GetNome()
        {
            return nome;
        }

    }
}
