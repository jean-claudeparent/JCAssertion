using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JCAssertionCore;


namespace JCAExporte
{
    class JCAExporteCore
    {

        private JCAVariable Arguments = new JCAVariable();

        public int Execute(string[] args, out String Message)
        {   
            if (args.Count() < 2)
                {
                    Message = "Pas assez d'argument, usage :" + Environment.NewLine + "JCAExporte /F:fichier /V:cle=valeur";
                    return 99;
                }

            Message = "Pas encore implémenté";
            return 99;
        }

    }
}
