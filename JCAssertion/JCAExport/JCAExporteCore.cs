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
            JCAVariable mesVariables = new JCAssertionCore.JCAVariable();
            JCAConsole maConsole = new JCAssertionCore.JCAConsole ();
            String Usage = "usage :" + Environment.NewLine + Environment.NewLine + "JCAExporte /F:fichier /V1:cle=valeur /v2=cle2=valeur2 ...";

            Message = "";

            if (args.Count() < 2)
            {
                Message = "Pas assez d'arguments. " + Environment.NewLine + Usage ;
                return 99;
            }

            mesArgs = maConsole.Arguments(args);

            if ((mesArgs.GetValeurVariable("F") == null) || (mesArgs.GetValeurVariable("F") == ""))
                {
                    Message = "L'argument /F (Chemin du fichier de sortie nMa oas été spécfié.)" + Environment.NewLine + Usage;
                    return 99;
                }

            // Traitemen OK
            String NomFichier = mesArgs.GetValeurVariable("F");
            if (System.IO.File.Exists (NomFichier)) mesVariables.LireFichier(NomFichier );
            // Ajouter ou maj les variables

            //Sauvegarder
            mesVariables.EcrireFichier(NomFichier);

            Message = "Fichier : " + NomFichier + " mis à jour.";
            
            return 0;
        }

        
        
    } // class
} // namespace
