using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Reflection;
using System.IO;



namespace JCAssertionCore
{
    public class Core
    {
        public String FichierDeCas;
        public string FichierJournal;
        public String FichierValeur;
        public Boolean Journaliser = true;
        public int NombreCas = 0;
        public int NoCasCourant = 0;
        public XmlDocument ListeDeCas;
        public XmlNode CasCourant;

        

        public void Load(String NomFichierLoad)
            {
                FichierDeCas = NomFichierLoad;
                FichierValeur = null;
                Load();
            }

        public void Load(String NomFichierLoad, String NomFichierValLoad)
        {
            FichierDeCas = NomFichierLoad;
            FichierValeur = NomFichierValLoad;
            Load();
        }

        public void Load()
            // Charger le fichier de cas et optionnellement
            // le fihier de valeur
            // initialise aussi lenom dujournalet lenombre de cas et le cas courant

        {
            FichierJournal = FichierDeCas + ".log.txt";
        }

        public static string RepertoireAssembly()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\";
        }

    }
}
