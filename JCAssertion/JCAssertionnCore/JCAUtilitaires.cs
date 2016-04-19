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
