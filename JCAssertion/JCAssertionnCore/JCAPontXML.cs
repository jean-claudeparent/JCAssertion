// License
//
//
//  JCAssertion : Un ensemble d’outils
//              pour configurer et vérifier les environnements 
//              de tests sous windows.
//
//  Copyright 2016 Jean-Claude Parent 
// 
//  Informations : www.noursicain.net
//
// Ce fichier fait partie de JCAssertion.
//
// JCAssertion  est un logiciel libre ; vous pouvez le redistribuer ou le 
// modifier suivant les termes de la GNU General Public License telle que 
// publiée par la Free Software Foundation ; soit la version 2 de la 
// licence, soit (à votre gré) toute version ultérieure.
// 
// JCAssertion est distribué dans l'espoir qu'il sera utile, 
// mais SANS AUCUNE GARANTIE ; sans même la garantie tacite 
// de QUALITÉ MARCHANDE ou d'ADÉQUATION à UN BUT PARTICULIER. 
// Consultez la GNU General Public License pour plus de détails.
// 
// Vous devez avoir reçu une copie de la GNU General Public License 
// en même temps que JCAssertion ; si ce n'est pas le cas, consultez
// <http://www.gnu.org/licenses>. Selon la recommandation  de la fondation 
// le seul texte officiel est celui en anglais car la fondation ne peut garantir
// que la traduction dans une langue  assurera les mêmes protections.
// 
// License

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


        public static   bool  JCAFichierExiste(XmlNode monXMLNode,
            ref string   Message,ref  Dictionary<String, String > Variables,
            ref String MessageEchec)
            {
                Message = Message + Environment.NewLine  + "Assertion FichierExiste\n";
                if (monXMLNode == null) throw new JCAssertionException("Le XML est vide.");
                ValideBalise(monXMLNode, "Fichier");
                string NomFichier = monXMLNode["Fichier"].InnerText;
                NomFichier = JCAVariable.SubstituerVariables(NomFichier, Variables);
                Message = Message + Environment.NewLine  +  "Fichier:" + NomFichier  + "\n";
                Boolean Resultat = File.Exists (NomFichier );
                if (Resultat) 
                    {
                    Message = Message + Environment.NewLine  +
                        "Le fichier existe.";
                    MessageEchec = "";
                    }
                else
                    {
                    Message = Message + Environment.NewLine  +
                        "Le fichier n'existe pas.";
                    if (ValeurBalise (monXMLNode, "MessageEchec") != "")
                        MessageEchec = ValeurBalise (monXMLNode, "MessageEchec")
                            + " (Fichier : " + NomFichier + ")";
                    else
                        MessageEchec = "Le fichier " + NomFichier + " n'existe pas et il devrait exister.";
                }
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

        public static bool JCAContenuFichier(XmlNode monXMLNode,
            ref string Message, ref  Dictionary<String, String> Variables,
            ref String  MessageEchec)
        {
            MessageEchec = "";
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

            // traiter les valeurs multiples
            
            Boolean Resultat = true;
            String Contient = "";
            Boolean ResultatPartiel = true;
             
            foreach (XmlElement monFragmentXML in monXMLNode.SelectNodes("Contient"))
                {
                    
                    Contient = monFragmentXML.InnerText;
                    Contient = JCAVariable.SubstituerVariables(Contient, Variables);
                    Message= Message + Environment.NewLine + 
                        "Vérifier si le fichier contient:" +
                        Contient;
                    ResultatPartiel = Contenu.Contains(Contient);
                    if(!ResultatPartiel) 
                        {
                            Resultat = false;
                            MessageEchec = MessageEchec +
                                Environment.NewLine +
                                "Le texte '" + Contient + 
                                "' n'a pas été trouvée et elle devrait y être";
                        } //if(!ResultatPartiel) 
                } // foreach
                

                    String NeContientPas = "";


                foreach (XmlElement  monFragmentXMLCP in monXMLNode.SelectNodes(
                    "NeContientPas"))
                {
                    
                    NeContientPas = monFragmentXMLCP.InnerText;
                    NeContientPas = JCAVariable.SubstituerVariables(
                        NeContientPas, Variables);
                    Message= Message + Environment.NewLine + 
                        "Vérifier si le fichier ne contient pas:" +
                        NeContientPas;
                    ResultatPartiel = ! Contenu.Contains(NeContientPas);
                    if(!ResultatPartiel) 
                        {
                            Resultat = false;
                            MessageEchec = MessageEchec +
                                Environment.NewLine +
                                "Le texte '" + NeContientPas + 
                                "' a été trouvée et elle me devrait pas y être";
                        } // if(!ResultatPartiel)
                } // foreach 
                


            if (Resultat)
                Message = Message + Environment.NewLine +
                    "L'assertion est vraie";
            else
                {
                Message = Message + Environment.NewLine +
                    "L'assertion est fausse";
                // insérer le message d'échec
                if (ValeurBalise (monXMLNode,"MessageEchec") != "")
                    {
                        MessageEchec = ValeurBalise(monXMLNode, "MessageEchec") +
                            Environment.NewLine + MessageEchec;
                        MessageEchec = JCAVariable.SubstituerVariables(
                            MessageEchec , Variables);
 
                    }
                else
                {
                    MessageEchec = "Assertion fauuse du dichier " + NomFichier  +
                        Environment.NewLine + MessageEchec;
                    MessageEchec = JCAVariable.SubstituerVariables(
                        MessageEchec, Variables);

                }

                }
            return Resultat ;
        }

        public static bool JCAExecuteProgramme(XmlNode monXMLNode, 
            ref string Message, ref  Dictionary<String, String> Variables,
            ref string MessageEchec)
            
        {
            JCAConsole maConsole = new JCAConsole();
            String Sortie = "";
            MessageEchec = "";
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
            else
                {
                // echec
                Message = Message +
                Environment.NewLine +
                "L'assertion est fausse";
                // messageechec
                if (ValeurBalise (monXMLNode, "MessageEchec") != "")
                        MessageEchec = ValeurBalise (monXMLNode, "MessageEchec")
                            + " (Code de retour : " + 
                            maConsole.DernierCodeDeRetour.ToString() +
                             " )";
                    else
                        MessageEchec = 
                            "Le programme " + Programme  + 
                            " a terminé avec le code de retour " +
                            maConsole.DernierCodeDeRetour.ToString();
                }
                

            return Resultat;
        }



    }
}
