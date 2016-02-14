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
        public String Message = "";


        private XmlDocument ListeDeCasXML;
        private XmlNode CasCourant;
        private XmlNodeList ListeDeCas;

        

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
            ListeDeCasXML = new XmlDocument();
            ListeDeCasXML.Load(FichierDeCas);
            if (ListeDeCasXML == null) throw new Exception("Le document CML est vide oumal structuré");
            XmlElement monRoot = ListeDeCasXML.DocumentElement;
            ListeDeCas = monRoot.GetElementsByTagName("Assertion");
            if (ListeDeCas == null) throw new Exception("Le document CML est vide oumal structuré");
            
            NombreCas = ListeDeCas.Count;
            if (NombreCas > 0)
                {
                  Message = "Chargement de la liste des cas réussie. Nombre de cas " + NombreCas.ToString()  ;
                  NoCasCourant = 1;
                } else
                {
                  Message = "Chargement de la liste des cas échouée. pas de cas ";
                  NoCasCourant = 0;
                }

          }

        public static string RepertoireAssembly()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\";
        }

    }
}
