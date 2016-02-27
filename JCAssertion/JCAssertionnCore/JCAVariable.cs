using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace JCAssertionCore
{
    public class JCAVariable
    {
        public Dictionary<String, String> Variables = new Dictionary<String, String>();
        

        public static   String ExtraireVariable(String  Argument)
        {
            // retournele nom delaprochainevariableou "" si aucune
            int ouverture = Argument.IndexOf("{{");
            int Fermeture = Argument.IndexOf("}}");
            if ((Fermeture < 1) || (Fermeture < ouverture) || (ouverture < 0)) return "";
            if ((Fermeture < 1) || (Fermeture < ouverture) || (ouverture < 0)) return "";
            return Argument.Substring(ouverture  +2, (Fermeture - 2) - ouverture  );
        }       


        public static String SubstituerVariables(String Argument, Dictionary<String, String> Variables)
            {
                String ProchaineVariable = ExtraireVariable(Argument );
                while (ProchaineVariable != "")
                    {
                        if (!(Variables.ContainsKey(ProchaineVariable))  ) throw new JCAssertionException("La variable " + ProchaineVariable  + "n'a pas eu de valeur fournie.")  ;
                        Argument = Argument.Replace("{{" + ProchaineVariable + "}}", Variables[ProchaineVariable]);
                        ProchaineVariable = ExtraireVariable(Argument);
                }
                return Argument;
            }

        public void MAJVariable(String Cle, String Valeur)
        {
            // ajoute la paire au dictionnaire si absent,modifierla valeursiprésent
            if (Variables.ContainsKey(Cle)) Variables.Remove(Cle);
            Variables.Add(Cle,Valeur);
        }

       
        public int NombreVariables()
        {
            return Variables.Count;
        }

        public String  GetValeurVariable(String Cle)
        {
            
            String valeur;
            if (Variables.TryGetValue(Cle,   out valeur )) return valeur;
            else return null;
        }

        public Dictionary<String, String> GetDictionnaireVariable()
        {
            return Variables;
        }

        public void  EcrireFichier(String NomFichier)
        {
            
        }

        public void LireFichier(String NomFichier)
        {

        }
        
    }
}
