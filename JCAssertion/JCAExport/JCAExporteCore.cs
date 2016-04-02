// License
//
//
//  JCAssertion : Un ensemble d’outils
//              pour configurer et vérifier les environnements 
//              de tests sous windows.
//
//  Copyright 2016 Jean-Claude Parent 
// 
//  Informations : www.noursicain.net
//
// Ce fichier fait partie de JCAssertion.
//
// JCAssertion  est un logiciel libre ; vous pouvez le redistribuer ou le 
// modifier suivant les termes de la GNU General Public License telle que 
// publiée par la Free Software Foundation ; soit la version 2 de la 
// licence, soit (à votre gré) toute version ultérieure.
// 
// JCAssertion est distribué dans l'espoir qu'il sera utile, 
// mais SANS AUCUNE GARANTIE ; sans même la garantie tacite 
// de QUALITÉ MARCHANDE ou d'ADÉQUATION à UN BUT PARTICULIER. 
// Consultez la GNU General Public License pour plus de détails.
// 
// Vous devez avoir reçu une copie de la GNU General Public License 
// en même temps que JCAssertion ; si ce n'est pas le cas, consultez
// <http://www.gnu.org/licenses>. Selon la recommandation  de la fondation 
// le seul texte officiel est celui en anglais car la fondation ne peut garantir
// que la traduction dans une langue  assurera les mêmes protections.
// 
// License

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
            String Cle;
            String Valeur;


            Message = "";

            if (args.Count() < 2)
            {
                Message = "Pas assez d'arguments. " + Environment.NewLine + Usage ;
                return 99;
            }

            mesArgs = maConsole.Arguments(args);

            if ((mesArgs.GetValeurVariable("F") == null) || (mesArgs.GetValeurVariable("F") == ""))
                {
                    Message = "L'argument /F (Chemin du fichier de sortie n'a pas été spécfié.)" + Environment.NewLine + Usage;
                    return 99;
                }

            // Traitemen OK
            String NomFichier = mesArgs.GetValeurVariable("F");
            if (System.IO.File.Exists (NomFichier)) mesVariables.LireFichier(NomFichier );
            // Ajouter ou maj les variables
            
            foreach (var monParem in mesArgs.Variables )
                {
                    
                    if ((monParem.Key.Length > 0 ) && (monParem.Key.Substring(0,1).ToUpper()   == "V"))
                    {
                        Cle = mesArgs.ExtrairePaire(monParem.Value, out  Valeur);
                        mesVariables.MAJVariable (Cle ,Valeur );

                    }
                }
                      
            //Sauvegarder
            mesVariables.EcrireFichier(NomFichier);
            mesArgs.EcrireFichier(NomFichier + ".debug.xml");
            Message = Environment.NewLine +  "Fichier : " + NomFichier + " mis à jour.";
            
            return 0;
        }

        
        
    } // class
} // namespace
