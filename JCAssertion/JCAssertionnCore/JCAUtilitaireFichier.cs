using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using JCAssertionCore;

namespace JCAssertionCore
{
    public class JCAUtilitaireFichier
        
    {
        public void SubstituerVariableFichier(String FichierModele, String FichierResultat, String FichierDeVariables)
        {
            JCAVariable mesVariables = new JCAVariable();
            String monContenu;


            mesVariables.LireFichier(FichierDeVariables);
            monContenu = System.IO.File.ReadAllText(FichierModele );
            monContenu = JCAVariable.SubstituerVariables(monContenu, mesVariables.Variables );
            System.IO.File.WriteAllText(FichierResultat, monContenu);
        }

        public  void EffaceSiExiste(String Fichier)
        {
            if (System.IO.File.Exists(Fichier)) System.IO.File.Delete(Fichier );
        }
    }
}
