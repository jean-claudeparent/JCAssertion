using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;



namespace JCAssertionCore
{
    public   class JCAPontXML
    {
        public static   bool  JCAFichierExiste(XmlNode monXMLNode, ref string   Message,ref  Dictionary<String, String > Variables)
            {
                Message = Message + Environment.NewLine  + "Assertion FichierExiste\n";
                if (monXMLNode == null) throw new JCAssertionException("Le XML est vide.");
                if (monXMLNode["Fichier"] == null) throw new JCAssertionException("Le XML ne contient pas la balise Fichier." + monXMLNode .InnerXml);
                if (monXMLNode["Fichier"].InnerText == null) throw new JCAssertionException("La balise Fichier est vide." + monXMLNode.InnerXml);
                if (monXMLNode["Fichier"].InnerText == "") throw new JCAssertionException("La balise Fichier est vide." + monXMLNode.InnerXml);
                string NomFichier = monXMLNode["Fichier"].InnerText;
                NomFichier = JCAVariable.SubstituerVariables(NomFichier, Variables);
                Message = Message + Environment.NewLine  +  "Fichier:" + NomFichier  + "\n";
                Boolean Resultat = File.Exists (NomFichier );
                if (Resultat) Message = Message + Environment.NewLine  + "Le fichier existe.";
                else Message = Message + Environment.NewLine  + "Le fichier n'existe pas.";
                return Resultat;
            }

        public static bool JCASubstituerVariablesFichier(XmlNode monXMLNode, ref string Message, ref  Dictionary<String, String> Variables)
        {
            Message = Message + Environment.NewLine + "Assertion SubstituerVariablesFichier";
            if (monXMLNode == null) throw new JCAssertionException("Le XML est vide.");
            
            // Valider le fichoer de modèle
            if (monXMLNode["FichierModele"] == null) throw new JCAssertionException("Le XML ne contient pas la balise FichierModele." + monXMLNode.InnerXml);
            if (monXMLNode["FichierModele"].InnerText == null) throw new JCAssertionException("La balise FichierModele est vide." + monXMLNode.InnerXml);
            if (monXMLNode["FichieModeler"].InnerText == "")
                throw new JCAssertionException("La balise FichierModele est vide." + monXMLNode.InnerXml);
            string NomFichierModele = monXMLNode["FichierModele"].InnerText;
            NomFichierModele = JCAVariable.SubstituerVariables(NomFichierModele, Variables);
            Message = Message + Environment.NewLine + "Fichier de modèle:" + NomFichierModele + Environment.NewLine  ;
            if(File.Exists(NomFichierModele))
                throw new JCAssertionException("La fichier modèle n'existe pas." + monXMLNode.InnerXml);

            // Valider le fichoer de sortie
            if (monXMLNode["FichierSortie"] == null)
                throw new JCAssertionException("Le XML ne contient pas la balise FichierSortie." + monXMLNode.InnerXml);
            if (monXMLNode["FichieSortie"].InnerText == null)
                throw new JCAssertionException("La balise FichierSortie est vide." + monXMLNode.InnerXml);
            if (monXMLNode["FichieSortier"].InnerText == "")
                throw new JCAssertionException("La balise FichierSortie est vide." + monXMLNode.InnerXml);
            string NomFichierSortie = monXMLNode["FichierSortie"].InnerText;
            NomFichierSortie = JCAVariable.SubstituerVariables(NomFichierSortie, Variables);
            Message = Message + Environment.NewLine + "Fichier de sortie:" + NomFichierSortie + Environment.NewLine;
            

            Boolean Resultat = false ;
            if (Resultat) Message = Message + Environment.NewLine
                + "La substitution des variables dans le fichier a réussie.";
            else Message = Message + Environment.NewLine +
                "La substitution des variables dans le fichier a échouée.";
            return Resultat;
        }



    }
}
