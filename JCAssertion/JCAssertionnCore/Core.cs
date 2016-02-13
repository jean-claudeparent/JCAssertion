using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace JCAssertionCore
{
    public class Core
    {
        public String FichierDeCas;
        public string FichierJournal;
        public Boolean Journaliser = true;
        public int NombreCas = 0;
        public int NoCasCourant = 0;
        public XmlDocument ListeDeCas;
        public XmlNode CasCourant;

        

        public void Load(String NomFichierLoad)
            {
            }

    }
}
