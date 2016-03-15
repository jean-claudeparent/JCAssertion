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
            
            monProcessus.StartInfo.UseShellExecute = false  ;
            monProcessus.StartInfo.RedirectStandardOutput = true;
            monProcessus.StartInfo.RedirectStandardError = true;
            monProcessus.StartInfo.FileName = Programme ;
            monProcessus.StartInfo.Arguments = mesArguments;
            monProcessus.Start();
            monProcessus.WaitForExit();
            Sortie = monProcessus.StandardError.ReadToEnd() ;
            Sortie = Sortie + monProcessus.StandardOutput.ReadToEnd();
            monProcessus.Refresh();
            

            int CR = monProcessus.ExitCode;

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
