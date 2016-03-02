using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JCAssertionCore;


namespace JCAExporte
{
    public class JCAExporteCore
    {
        public int ExecuteExporte(string[] args, out String Message)
        {
            JCAVariable mesArgs = new JCAssertionCore.JCAVariable();
            JCAConsole maConsole = new JCAssertionCore.JCAConsole ();

            Message = "";

            if (args.Count() < 2)
            {
                Message = "Pas assez d'argument, usage :" + Environment.NewLine + "JCAExporte /F:fichier /V:cle=valeur";
                return 99;
            }

            mesArgs = maConsole.Arguments(args);


            return 0;
        }

        
        
    } // class
} // namespace
