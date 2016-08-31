// License
//
//
//  JCAssertion : Un ensemble d’outils
//              pour configurer et vérifier les environnements 
//              de tests sous windows.
//
//  Copyright 2016 Jean-Claude Parent 
// 
//  Informations : www.jcassertion.org
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
using JCAssertionCore;



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
                if (!Resultat) Resultat = System.IO.Directory.Exists(NomFichier);  
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

        public static bool JCASubstituerVariablesFichier(XmlNode monXMLNode, 
            ref string Message, 
            ref  Dictionary<String,String> Variables)
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
                                "' n'a pas été trouvé et il devrait être dans le fichier";
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
                                "' a été trouvé et il me devrait pas être dans le fichier";
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

        public static bool JCAMAJVariables(XmlNode monXMLNode,
            ref string Message, 
            ref  Dictionary<String, String> Variables,
            ref String MessageEchec)
        {
            Message = Message + Environment.NewLine + 
                "Assertion MAJVariables" + Environment.NewLine  ;
            if (monXMLNode == null) throw new JCAssertionException("Le XML est vide.");
            ValideBalise(monXMLNode, "Cle");
            string MaCle = ValeurBalise (monXMLNode,"Cle");
            MaCle = JCAVariable.SubstituerVariables(MaCle, Variables);
            Message = Message + "Clé:" + MaCle + Environment.NewLine  ;
            ValideBalise(monXMLNode, "Valeur");
            
            string MaValeur = ValeurBalise (monXMLNode,"Valeur");
            MaValeur = JCAVariable.SubstituerVariables(MaValeur, Variables);
            Message = Message + "Valeur:" + MaValeur + Environment.NewLine;
            
            JCAVariable VariableTemp = new JCAVariable() ;
            VariableTemp.Variables  = Variables;
            VariableTemp.MAJVariable (MaCle, MaValeur  );
            Variables = VariableTemp.Variables;
            if(VariableTemp.GetValeurVariable(
                JCAVariable.Constantes.JCA_FichierDeVariables) != null )
                {
                    VariableTemp.EcrireFichier (VariableTemp.GetValeurVariable(
                        JCAVariable.Constantes.JCA_FichierDeVariables));
                         
                }



            Message = Message + Environment.NewLine +
                    "Valeur de variable mise à jour.";
            MessageEchec = "";
            
            
            return true ;
        }

        public bool JCAConnectionOracle(XmlNode monXMLNode, 
            ref string Message, ref  Dictionary<String, String> Variables,
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
                
                // remplacer les variables
                MonUser = JCAVariable.SubstituerVariables(
                    MonUser, Variables);
                MonPassword = JCAVariable.SubstituerVariables(
                    MonPassword, Variables);
                MonServeur = JCAVariable.SubstituerVariables(
                    MonServeur, Variables);
                // Donner du feedback
                Message = Message + Environment.NewLine +
                    "User : " + MonUser + Environment.NewLine +
                    "Password : " + MonPassword + Environment.NewLine +
                    "Serveur/instance : " + MonServeur + Environment.NewLine  ;

                // Traiter l'action
                MonActionTexte = JCAVariable.SubstituerVariables(
                    MonActionTexte, Variables).ToUpper() ;
                JCASQLClient.Action monAction =
                    JCASQLClient.Action.Aucune;
                if (MonActionTexte.Contains("OUVRIR"))
                    {
                        monAction =
                            JCASQLClient.Action.Ouvrir ;
                        Message = Message +
                            "Ouvrir la connection à la base de données" +
                            Environment.NewLine; 
                    }
                if (MonActionTexte.Contains("FERMER"))
                    {
                        monAction =
                            JCASQLClient.Action.Fermer ;
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
                } catch (Exception excep) 
                {
                    throw new JCAssertionException(
                        "Erreur technique lors de la connection au serveur Oracle " +
                    excep.Message , excep  );
                }
            // La seule facon que cette assertion retourne false
            // c'est qu'une exception se produise
                return true;
            }

     
        public bool JCAAssertSQL(XmlNode monXMLNode, 
            ref string Message, ref  Dictionary<String, String> Variables,
            ref string MessageEchec,
            ref JCASQLClient monODPSQLClient)
            {
                Message = Message + Environment.NewLine +
                "Assertion AssertSQL" + Environment.NewLine;
                MessageEchec = "";
                if (monXMLNode == null) 
                    throw new JCAssertionException("Le XML est vide.");
                ValideBalise(monXMLNode, "SQL");
            if ((! SiBaliseExiste(monXMLNode ,"AttenduNombre")) &&
                (!SiBaliseExiste(monXMLNode, "AttenduTexte")))
                throw new JCAssertionException(
                    "Une des deux balises suivantes doit exister :AttenduNombre ou AttenduTexte ");
            if ((SiBaliseExiste(monXMLNode, "AttenduNombre")) &&
                (SiBaliseExiste(monXMLNode, "AttenduTexte")))
                throw new JCAssertionException(
                    "Une seule des deux balises suivantes doit exister dans le xml :AttenduNombre ou AttenduTexte ");
            String monSQL = ValeurBalise (monXMLNode,"SQL" );
            monSQL = JCAVariable.SubstituerVariables(
                    monSQL , Variables);
                
            Message = Message + monSQL + Environment .NewLine ;
            Boolean Resultat = false;
            String monOperateur = "";
            if (SiBaliseExiste(monXMLNode, "AttenduNombre"))
                {
                    monOperateur = ValeurBalise(monXMLNode,"Operateur");
                    if (monOperateur == "")
                        monOperateur = "=";
                    monOperateur  = JCAVariable.SubstituerVariables(
                    monOperateur , Variables);
            
                    Message = Message + "Opérateur : " +
                        monOperateur + Environment.NewLine;
                    Double ResultatAttendu = 0;
                    String monANString = "";
                try 
                    {
                        monANString = ValeurBalise(
                            monXMLNode,"AttenduNombre");
                    monANString = JCAVariable.SubstituerVariables(
                    monANString , Variables);

                    ResultatAttendu = Convert.ToDouble(monANString); 
                            
                    } catch (FormatException excep)
                    {
                        throw new JCAssertionException(
                            "La balise AttenduNombre comporte une valeur (" +
                            monANString + ") ne pouvant pas être convertie en nombre" +
                            monXMLNode.InnerXml +
                            Environment.NewLine +
                            excep.Message, excep); 
                    } catch (Exception excep)
                    {
                        throw excep;
                    }
                Message = Message +
                    "Valeur attendue : " +
                    ResultatAttendu.ToString() +
                    Environment.NewLine; 
                Resultat = monODPSQLClient.SQLAssert(monSQL , 
                        ResultatAttendu, monOperateur);
                if (! (Resultat))
                    MessageEchec =  JCAVariable.SubstituerVariables(
                        ValeurBalise(
                        monXMLNode,"MessageEchec"), Variables );
                        
 

                } // if
            else
                {
                    String ResultatnAttenduTexte = ValeurBalise(
                            monXMLNode, "AttenduTexte");
                    ResultatnAttenduTexte = JCAVariable.SubstituerVariables(
                        ResultatnAttenduTexte, Variables);
                    Message = Message +
                    "Valeur attendue : " +
                    ResultatnAttenduTexte +
                    Environment.NewLine;
                    Resultat = monODPSQLClient.SQLAssert(monSQL,
                            ResultatnAttenduTexte);
                    if (!(Resultat))
                        MessageEchec = JCAVariable.SubstituerVariables(
                            ValeurBalise(
                            monXMLNode, "MessageEchec"), Variables);
                       
                
                } // end else

            if (Resultat)
                Message = Message +
                    "L'expression évaluée est vraie" +
                    Environment.NewLine;
            else
                Message = Message +
                    "L'expression évaluée est fausse" +
                    Environment.NewLine;

 
                return Resultat ;
            }


    }
}
