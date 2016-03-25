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
using System.Diagnostics;


namespace JCAssertionCore
{
    public class JCAConsole
        // Fonctions pourles appplications consoles

    {
        public int ExecuteProgramme(String Programme, String mesArguments, ref String Sortie )
        {
            Process monProcessus = new Process();
            String InfoErreur = "";
            int CR = 99;

            InfoErreur = "Exception lors de la préparation des paramètres de  " +
                        "l'exécution de " + Programme;    
            try {
                monProcessus.StartInfo.UseShellExecute = false  ;
                monProcessus.StartInfo.RedirectStandardOutput = true;
                monProcessus.StartInfo.RedirectStandardError = true;
                monProcessus.StartInfo.FileName = Programme ;
                monProcessus.StartInfo.Arguments = mesArguments;
                // lancer
                InfoErreur = "Exception lors du lancement de " +
                        "l'exécution de " + Programme;
                monProcessus.Start();
                // attendre la fin de l'exécution
                InfoErreur = "Exception pendant " +
                        "l'exécution de " + Programme;
                monProcessus.WaitForExit();
            // recuperer les résultats
                InfoErreur = "Exception lors de la récupération des informations après la fin de " +
                        "l'exécution de " + Programme;
                Sortie = monProcessus.StandardError.ReadToEnd();
                Sortie = Sortie + monProcessus.StandardOutput.ReadToEnd();
                monProcessus.Refresh();


               CR = monProcessus.ExitCode;


                
                
           
            
            } catch (Exception excep)
                {
                    throw new JCAssertionException(InfoErreur +
                        Environment.NewLine + excep.GetType() +
                        Environment.NewLine + excep.Message   ,
                        excep);
                }

            
            return CR;
        }

        

        public JCAVariable  Arguments(string[] args)
        {
            String Cle;
            String Valeur;
            JCAVariable Resultat = new JCAVariable();

            foreach  (String  Param in args  )
                {
                    if ((Param != null ) && (Param != "")  )
                        {
                            if(ExtraireParam(Param ,out Cle, out Valeur ))
                                Resultat.MAJVariable (Cle,Valeur );
                        }
                }
            return Resultat;
        }

        public bool   ExtraireParam(string monParam, out String Cle, out String Valeur)
        {
            Cle = "";
            Valeur = "";
            if(monParam == null) return false ;
            if (monParam == "") return false;

            if ((monParam.Substring(0,1) == "/" ) && (! monParam.Contains(":") ))
                // Param de forme /quelquechose mais sans le séparateur de valeur (:),alors normaliser
                {
                    monParam = monParam + ":" + monParam.Substring(1,monParam.Length - 1).ToUpper() ;
                }
            
            // le / n'est plus utilel'enlever
            if (monParam.Substring(0,1) == "/" )
                {
                    monParam = monParam.Substring (1, monParam.Length -1);
                }
            // extraire vraiment
            if(monParam.Contains (":"))
                {
                    Cle = monParam.Substring(0, monParam.IndexOf (":")).ToUpper ();
                    Valeur = monParam.Substring(monParam.IndexOf(":") + 1, monParam.Length - Cle.Length  - 1); 
                } else
                {
                    Cle = monParam;
                    Valeur = monParam;
                };


            if (Cle == "")
                return false;
            else
                return true;
        }
        


    }
}
