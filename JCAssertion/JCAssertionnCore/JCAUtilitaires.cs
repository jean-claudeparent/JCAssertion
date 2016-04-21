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
    public  class JCAUtilitaires
    {

        public Int64 JournalEveDelai = 600; // delai de recherche dans le journal des événements en secondes
        public String JournalEveSource = ""; // source recherchée
        public Boolean JournalApplication = true;
        public Boolean JournalEveSysteme = false;
        public Boolean JournalEveSecurite = false;
        public int JournalEveNombreMax = 1000;
        private Int64 JournalEVeMombreTrouve = 0;
        private Boolean JournalEveMaxAtteint = false;

        public Int64 getJournalEVeMombreTrouve()
        {
          return JournalEVeMombreTrouve;
         }

        public bool getJournalEveMaxAtteint()
        {
            return JournalEveMaxAtteint;
        }

        public String  EntreeeFormattee(EventLogEntry EJ)
        {
            String nl = Environment.NewLine;
 
            String Resultat = "";
            Resultat = nl + "Type d'emtrée:" + EJ.EntryType.ToString() + nl;
            Resultat = Resultat +  "Source:" + EJ.Source + nl ;
            Resultat = Resultat + "Message:" + nl + "===" + nl ;
            Resultat = Resultat + EJ.Message + nl;
            Resultat = Resultat + "===" + nl;

            return Resultat;
        }


        

           


        // Journal personnalisé pas encore implémenté



        public Boolean  EntreeRetenue(EventLogEntry monEE,
            String TexteRecherche)
            {
                return true;
            }

        public String RechercheJournalEve(String TexteRecherche)

            // retourne les entrées du event log
            // pour la source, si spécifiée, pour le texte
            // recherché si spécifié et pour les entrées
            // créer dans les x dernières secondes
            {
                String Resultat = "";
                int i = 0;

                EventLog monEV = new EventLog();
                monEV.Log = "Application"; 
                foreach (EventLogEntry monLE in monEV.Entries  )
                    {
                        if (!(i < 20)) break;
                            Resultat = Resultat +
                            "Entrée:" +
                            i.ToString() +
                            EntreeeFormattee(monLE);
                            i = i + 1;
                    }

                return Resultat ;
            }


        public static Boolean EVSourceExiste(String Source = "JCAssertion")
            {
            try
                {
                    if (! System.Diagnostics.EventLog.SourceExists (Source ))
                        return false ;
                    else 
                        return true ;
                     
                } catch (Exception excep) 
                    {
                        return false ;
                    }
            }


        public static void EventLogErreur(String Texte, 
            String Source = "JCAssertion")
            {
                EventLog(Texte, Source, EventLogEntryType.Error);
            }

        public static void EventLog(String Source, 
            String Texte, EventLogEntryType monType)
        {
            if (EVSourceExiste(Source))
                {
                    EventLog monLog = new EventLog();
                    monLog.Source = Source;
                    monLog.WriteEntry(Texte, monType);
                } 
        } 
            


    }
}
