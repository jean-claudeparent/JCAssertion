// License
//
//
//  JCAssertion : Un ensemble d’outils
//              pour configurer et vérifier les environnements 
//              de tests sous windows.
//
//  Copyright 2016 Jean-Claude Parent 
// 
//  Informations : www.jcassertion.org
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
using System.Diagnostics.Eventing.Reader;
 


namespace JCAssertionCore
{
    public  class JCAUtilitaires
    {

        public int JournalEveNombreMax = 20;
        public Boolean LancerExceptionJE = false;


        

        

        


        

           


        private String FormatterPropriete(IList<EventProperty> Liste )
            // methode de r&d
            {
                String Resultat = "";
                foreach (EventProperty maProp in Liste)
                    {
                        Resultat = Resultat + Environment.NewLine +
                            maProp.Value; 
                    }
                return Resultat ;
            }

        private String FormatterEventRecord(EventRecord monER)
            {
                String Resultat = Environment.NewLine + "===" +
                    Environment.NewLine  ;
                Resultat = Resultat + "Quand:" +
                    monER.TimeCreated.ToString() + Environment.NewLine  ;
                Resultat = Resultat + monER.FormatDescription();
                
                
 
            Resultat = Resultat +
                    FormatterPropriete(monER.Properties);
                return Resultat ;
            }
        
        
        public String LogExplore()
            {
                String Resultat = "";
                EventLogQuery monELQ = new EventLogQuery("Application", PathType.LogName  );
                monELQ.ReverseDirection = true;

                EventLogReader monELReader = new EventLogReader(monELQ);
                monELReader.BatchSize = 1;


                EventRecord monER;
                int i = 0;
                while ((monER = monELReader.ReadEvent()) != null )
                    {
                        Resultat = Resultat + 
                            FormatterEventRecord(monER);
                        
                        i = i + 1;
                        if (i > JournalEveNombreMax) break;
                    }

                return Resultat;
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
                        if (excep != null) return false;
                        return false ;
                    }
            }


        public void EventLogErreur(String Texte, 
            String Source = "JCAssertion")
            {
                EventLog(Texte, Source, EventLogEntryType.Error);
            }

        public void EventLog(String Texte,String Source, 
            EventLogEntryType monType)
        {
            if (EVSourceExiste(Source))
            {
                try
                {
                    EventLog monLog = new EventLog();
                    monLog.Source = Source;
                    monLog.WriteEntry(Texte, monType);
                }
                catch (Exception)
                {
                    if (LancerExceptionJE) throw ;
                    else return;
                }
            } // if
            else if (LancerExceptionJE) throw new Exception("Source non définie : " + Source );
        } 
            


    }
}
