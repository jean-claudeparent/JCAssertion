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
            if (monXMLNode == null) 
                throw new JCAssertionException("Le XML est vide.");
            
            // Valider le fichoer de modèle
            if (monXMLNode["FichierModele"] == null) 
                throw new JCAssertionException("Le XML ne contient pas la balise FichierModele." + monXMLNode.InnerXml);
            if (monXMLNode["FichierModele"].InnerText == null) 
                throw new JCAssertionException("La balise FichierModele est vide." + monXMLNode.InnerXml);
            if (monXMLNode["FichierModele"].InnerText == "")
                throw new JCAssertionException("La balise FichierModele est vide." + monXMLNode.InnerXml);
            
            
            string NomFichierModele = monXMLNode["FichierModele"].InnerText;
            NomFichierModele = JCAVariable.SubstituerVariables(NomFichierModele, Variables);
            Message = Message + Environment.NewLine + "Fichier de modèle:" + NomFichierModele + Environment.NewLine  ;
            if(!File.Exists(NomFichierModele))
                throw new JCAssertionException("Le fichier modèle n'existe pas." + monXMLNode.InnerXml);
            
            // Valider le fichier de sortie
            if (monXMLNode["FichierSortie"] == null)
                throw new JCAssertionException("Le XML ne contient pas la balise FichierSortie." + monXMLNode.InnerXml);
            
            if (monXMLNode["FichierSortie"].InnerText == null)
                throw new JCAssertionException("La balise FichierSortie est vide." + monXMLNode.InnerXml);
            
            if (monXMLNode["FichierSortie"].InnerText == "")
                throw new JCAssertionException("La balise FichierSortie est vide." + monXMLNode.InnerXml);
            
            string NomFichierSortie = monXMLNode["FichierSortie"].InnerText;
            NomFichierSortie = JCAVariable.SubstituerVariables(NomFichierSortie, Variables);
            Message = Message + Environment.NewLine + "Fichier de sortie:" + NomFichierSortie + Environment.NewLine;
            
            //Valider le fichier de variable
            // FichierVariables

            if (monXMLNode["FichierVariables"] == null)
                throw new JCAssertionException("Le XML ne contient pas la balise FichierVariables." + monXMLNode.InnerXml);
            if (monXMLNode["FichierVariables"].InnerText == null)
                throw new JCAssertionException("La balise FichierVariables est vide." + monXMLNode.InnerXml);
            if (monXMLNode["FichierVariables"].InnerText == "")
                throw new JCAssertionException("La balise FichierVariables est vide." + monXMLNode.InnerXml);
            string NomFichierVariables = monXMLNode["FichierVariables"].InnerText;
            NomFichierVariables = JCAVariable.SubstituerVariables(NomFichierVariables, Variables);
            Message = Message + Environment.NewLine + "Fichier de variables :" + NomFichierVariables + Environment.NewLine;
            if (!File.Exists(NomFichierVariables))
                throw new JCAssertionException("Le fichier de variables n'existe pas." + monXMLNode.InnerXml);
            
            
            JCAUtilitaireFichier UF = new JCAUtilitaireFichier ();
            
            UF.SubstituerVariableFichier(NomFichierModele , NomFichierSortie , NomFichierVariables)  ;

            Message = Message + Environment.NewLine
                + "La substitution des variables dans le fichier a réussie.";
            return true ;
        }



    }
}
