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
                Message = Message + "\nAssertion FichierExiste";
                if (monXMLNode == null) throw new JCAssertionException("Le XML est vide.");
                if (monXMLNode["Fichier"] == null) throw new JCAssertionException("Le XML ne contient pas la balise Fichier." + monXMLNode .InnerXml);
                if (monXMLNode["Fichier"].InnerText == null) throw new JCAssertionException("La balise Fichier est vide." + monXMLNode.InnerXml);
                if (monXMLNode["Fichier"].InnerText == "") throw new JCAssertionException("La balise Fichier est vide." + monXMLNode.InnerXml);
                string NomFichier = monXMLNode["Fichier"].InnerText;
                Message = Message + "\nFichier:" + NomFichier  + "\n";
                return File.Exists (NomFichier );
            }


    }
}
