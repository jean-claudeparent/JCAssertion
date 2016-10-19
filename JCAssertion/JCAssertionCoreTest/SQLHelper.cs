using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace JCAssertionCoreTest
{
    /// <summary>
    /// Classe réutilisable pour les test unitaires concernant ls bases de données
    /// </summary>
    public  class SQLHelper
    {
        /// <summary>
        /// Crée un document xml conforme pour ouvrir une  connection oracle
        /// </summary>
        /// <param name="User">Compte d'utilisateur Oracle</param>
        /// <param name="Password">Mot de passe Oracle</param>
        /// <param name="Serveur">Serveur Oracle ou instance Oracle</param>
        /// <returns>Le XML pour ouvrir la connection Oracle</returns>
        public static XmlDocument XMLConnection(String User,
            String Password, String Serveur)
        {
            XmlDocument Resultat = new XmlDocument();
            String monXML = "";
            monXML = "<Assertion><Type>ConnectionOracle</Type>" +
                 "<User>" + User + "</User>" +
                 "<Password>" + Password + "</Password>";
            if (Serveur != "")
                monXML = monXML +
                "<Serveur>" + Serveur + "</Serveur>";
            monXML = monXML +
                "</Assertion>";
            Resultat.InnerXml = monXML; 
            return Resultat; 
        }

        /// <summary>
        /// Crée le xml pour SQLExecute
        /// </summary>
        /// <param name="CommandesSQL">Vecteur de commande SQL à exécuter</param>
        /// <param name="MessageEchec">Message d'échec si le SQLExecute fait une exception</param>
        /// <returns>XML de SQLExecute construit</returns>
        public static XmlDocument XMLSQLExecute(String[] CommandesSQL,
            String MessageEchec = "" )
        {
            XmlDocument Resultat = new XmlDocument();
            String monXML = "";
            monXML = "<Assertion><Type>SQLExecute</Type>;"; 
            // Traiter toutes les commandes sql
            foreach  (String monSQL in CommandesSQL  )
                {
                    if((monSQL != null ) && (monSQL != ""))
                      monXML = monXML + 
                        "<SQL>" + monSQL + "</SQL>";
                }
            if (MessageEchec != "" )
                monXML = monXML +
                    "<MessageEchec>S" + MessageEchec + "</MessageEchec";
            monXML = monXML + "</Assertion>";
            Resultat.InnerXml = monXML; 
            return Resultat; 
        }
 


    }
}
