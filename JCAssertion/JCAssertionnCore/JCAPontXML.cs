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
        public static   bool  JCAFichierExiste(XmlNode monXMLNode, ref string   Message, Dictionary<String, String > Variables)
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



    }
}
