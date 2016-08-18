using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JCASQLODPCore;

namespace JCAssertionCore
{
    public class JCAODPSQLClient
    {
        private JCASQLODPClient monSQLClient = new JCASQLODPClient();
        public enum Action {Aucune, Ouvrir, Fermer };

        public void InitConnection(String User,
            String Password,
            String Serveur = null,
            Action monAction = Action.Aucune  )
        {
        }


        public Boolean SQLAssert(String SQL,
            String ResultatAttendu)
        {
            Boolean Resultat = false;
            Boolean Fermer = (!monSQLClient.SiConnectionOuverte());
            if (!monSQLClient.SiConnectionOuverte())
                monSQLClient.OuvrirConnection();
            Resultat = monSQLClient.AssertSQL(SQL, ResultatAttendu); 
            if (Fermer)
                monSQLClient.FermerConnection();

            return Resultat; 
        }

        public Boolean SQLAssert(String SQL,
            Double  ResultatAttendu)
        {
            return false;
        }



    }
}
