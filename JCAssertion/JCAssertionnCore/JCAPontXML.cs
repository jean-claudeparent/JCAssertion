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


        public static String  ValeurBalise(XmlNode monXMLNode, String maBalise)
        {
            if (monXMLNode == null)
                return "";
            if (monXMLNode[maBalise] == null)
                return "";
            if (monXMLNode[maBalise].InnerText == null)
                return "";
            if (monXMLNode[maBalise].InnerText == "")
                return "";

            return monXMLNode[maBalise].InnerText;
        }

        public static Boolean  SiBaliseExiste(XmlNode monXMLNode, String maBalise)
            {
                if (monXMLNode == null)
                    return false;
                if (monXMLNode[maBalise] == null)
                    return false;
                if (monXMLNode[maBalise].InnerText == null)
                    return false;
                if (monXMLNode[maBalise].InnerText == "")
                    return false;
            
                return true;
            }
        

        public static void ValideBalise(XmlNode monXMLNode, String maBalise)
            {
                if (monXMLNode == null)
                    throw new JCAssertionException("Le XML est vide.");
            
                if (monXMLNode[maBalise] == null)
                    throw new JCAssertionException("Le XML ne contient pas la balise " + 
                        maBalise  + "." + monXMLNode.InnerXml);
                if (monXMLNode[maBalise].InnerText == null)
                    throw new JCAssertionException("La balise " +
                        maBalise + " est vide." + monXMLNode.InnerXml);
                if (monXMLNode[maBalise].InnerText == "")
                    throw new JCAssertionException("La balise " +
                        maBalise + " est vide." + monXMLNode.InnerXml);
            }


        public static   bool  JCAFichierExiste(XmlNode monXMLNode, ref string   Message,ref  Dictionary<String, String > Variables)
            {
                Message = Message + Environment.NewLine  + "Assertion FichierExiste\n";
                if (monXMLNode == null) throw new JCAssertionException("Le XML est vide.");
                ValideBalise(monXMLNode, "Fichier");
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
            
            // Valider le fichier de modèle

            ValideBalise(monXMLNode, "FichierModele");
            string NomFichierModele = monXMLNode["FichierModele"].InnerText;
            NomFichierModele = JCAVariable.SubstituerVariables(NomFichierModele, Variables);
            Message = Message + Environment.NewLine + "Fichier de modèle:" + NomFichierModele + Environment.NewLine  ;
            if(!File.Exists(NomFichierModele))
                throw new JCAssertionException("Le fichier modèle n'existe pas." + monXMLNode.InnerXml);
            
            // Valider le fichier de sortie
            ValideBalise(monXMLNode, "FichierSortie");
            
            
            string NomFichierSortie = monXMLNode["FichierSortie"].InnerText;
            NomFichierSortie = JCAVariable.SubstituerVariables(NomFichierSortie, Variables);
            Message = Message + Environment.NewLine + "Fichier de sortie:" + NomFichierSortie + Environment.NewLine;
            
            //Valider le fichier de variable
            
            ValideBalise(monXMLNode, "FichierVariables");
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

        public static bool JCAContenuFichier(XmlNode monXMLNode, ref string Message, ref  Dictionary<String, String> Variables)
        {
            Message = Message + Environment.NewLine + "Assertion ContenuFichier";
            if (monXMLNode == null) 
                throw new JCAssertionException("Le XML est vide.");
            ValideBalise(monXMLNode, "Fichier");
            if((!SiBaliseExiste(monXMLNode,"Contient")) && 
                (!SiBaliseExiste(monXMLNode,"NeContientPas")))
                 throw new JCAssertionException("Le XML doit contenir au moins une balise Contient ouNeContientPas.");
            String NomFichier = ValeurBalise (monXMLNode,"Fichier");
            NomFichier = JCAVariable.SubstituerVariables(NomFichier, Variables);
            Message = Message + Environment.NewLine + "Fichier à tester:" + NomFichier;
            if (!System.IO.File.Exists(NomFichier))
                throw new JCAssertionException("Le fichier " + NomFichier + "n'existe pas" );
            String Contenu = System.IO.File.ReadAllText(NomFichier );

            
            String Contient = ValeurBalise (monXMLNode,"Contient") ;
            Contient = JCAVariable.SubstituerVariables(Contient , Variables);
            
            String NeContientPas = ValeurBalise (monXMLNode,"NeContientPas");
            NeContientPas = JCAVariable.SubstituerVariables(NeContientPas, Variables);
            Boolean Resultat = true;

            if(Contient != "")
                {
                    Message= Message + Environment.NewLine + "Vérifier si le fichier contient:" + Contient;
                    Resultat = Resultat && Contenu.Contains(Contient )  ;
                }
            if(NeContientPas != "")
                {
                    Message= Message + Environment.NewLine + "Vérifier si le fichier ne contient pas:" + NeContientPas;
                    Resultat = Resultat && !Contenu.Contains(NeContientPas);
                
                }
            if (Resultat)
                Message = Message + Environment.NewLine +
                    "L'assertion est vraie";
            else
                Message = Message + Environment.NewLine +
                    "L'assertion est fausse";
            return Resultat ;
        }

        public static bool JCAExecuteProgramme(XmlNode monXMLNode, ref string Message, ref  Dictionary<String, String> Variables)
        {
            JCAConsole maConsole = new JCAConsole();
            String Sortie = "";
            Message = Message + Environment.NewLine + "Assertion ExecuteProgramme";
            if (monXMLNode == null) 
                throw new JCAssertionException("Le XML est vide.");
            
            // Valider le fichier de modèle

            ValideBalise(monXMLNode, "Programme");
            String Programme = ValeurBalise(monXMLNode, "Programme");
            Programme = JCAVariable.SubstituerVariables(Programme , Variables);
            
            String Arguments = ValeurBalise(monXMLNode, "Arguments");
            Arguments = JCAVariable.SubstituerVariables(Arguments , Variables);

            Boolean Resultat = (maConsole.ExecuteProgramme(Programme, 
                Arguments, ref Sortie) == 0);

            Message = Message + Environment.NewLine +
                "Résultat de l'exécution de " + Programme +
                Environment.NewLine + Sortie +
                Environment.NewLine + "Fin des résultats de " + Programme ;
            if (Resultat)
                Message = Message + Environment.NewLine +
                    "L'assertion est vraie";
            else Message = Message +
                Environment.NewLine + "L'assertion est fausse";
            return Resultat;
        }



    }
}
