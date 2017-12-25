using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace JCAssertionCore
{
    public partial class JCAPontXML
    {
        /// <summary>
        /// Évalue une assertion qui ouvre
        /// une connection oracle
        /// avec ODP.MET.
        /// </summary>
        /// <param name="monXMLNode">Document xml contenant l'assertion à évaluer</param>
        /// <param name="Message">Cette chaîne sera modifié pour donner de l'information sur l'évaluation de l'assertion</param>
        /// <param name="Variables">Dictionnaire des variables pouvant être substituée dans l'assertion</param>
        /// <param name="MessageEchec">Contiendra de l'information supplémentaire si l'assertion est fausse</param>
        /// <param name="monODPSQLClient">Object servant `travailler avec la bd</param>
        /// <returns>retourne si l'assertion est vraie ou fausse. S</returns>
        public bool JCAConnectionOracle(XmlNode monXMLNode,
            ref string Message, ref Dictionary<String, String> Variables,
            ref string MessageEchec,
            ref JCASQLClient monODPSQLClient)
        {
            Message = Message + Environment.NewLine +
            "Assertion ConnectionOracle" + Environment.NewLine;
            MessageEchec = "";
            if (monXMLNode == null)
                throw new JCAssertionException("Le XML est vide.");
            ValideBalise(monXMLNode, "User");
            ValideBalise(monXMLNode, "Password");
            string MonUser = ValeurBalise(monXMLNode, "User");
            string MonPassword = ValeurBalise(monXMLNode, "Password");
            string MonServeur = ValeurBalise(monXMLNode, "Serveur");
            string MonActionTexte = ValeurBalise(monXMLNode, "Action");
            string monModeCache = ValeurBalise(
                monXMLNode, "Cache");

            // remplacer les variables
            MonUser = JCAVariable.SubstituerVariables(
                MonUser, Variables);
            MonPassword = JCAVariable.SubstituerVariables(
                MonPassword, Variables);
            MonServeur = JCAVariable.SubstituerVariables(
                MonServeur, Variables);
            monModeCache = JCAVariable.SubstituerVariables(
                monModeCache , Variables);
            MonActionTexte = JCAVariable.SubstituerVariables(
                MonActionTexte, Variables).ToUpper();


            // Donner du feedback eb modifiant Message
            if (monModeCache == "")
                Message = Message + Environment.NewLine +
                  "User : " + MonUser + Environment.NewLine +
                  "Password : " + MonPassword 
                  + Environment.NewLine;
            else
                Message = Message + Environment.NewLine +
                "User : (Caché)" + Environment.NewLine +
                "Password : (Caché)"
                + Environment.NewLine;



            Message = Message + 
                "Serveur/instance : " + MonServeur + Environment.NewLine;

            // Traiter l'action
            JCASQLClient.Action monAction =
                JCASQLClient.Action.Aucune;
            if (MonActionTexte.Contains("OUVRIR"))
            {
                monAction =
                    JCASQLClient.Action.Ouvrir;
                Message = Message +
                    "Ouvrir la connection à la base de données" +
                    Environment.NewLine;
            }
            if (MonActionTexte.Contains("FERMER"))
            {
                monAction =
                    JCASQLClient.Action.Fermer;
                Message = Message +
                        "Fermer la connection à la base de données" +
                        Environment.NewLine;
            }
            try
            {
                monODPSQLClient.InitConnection(MonUser,
                 MonPassword,
                 MonServeur,
                 monAction);
            }
            catch (Exception excep)
            {
                throw new JCAssertionException(
                    "Erreur technique lors de la connection au serveur Oracle " +
                excep.Message, excep);
            }
            // La seule facon que cette assertion retourne false
            // c'est qu'une exception se produise
            return true;
        }

    }
}
