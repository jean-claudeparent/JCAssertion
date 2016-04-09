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
using System.Reflection;
using System.IO;



namespace JCAssertionCore
{
    public class JCACore
    {
        public String FichierDeCas;
        public string FichierJournal;
        public String FichierValeur;
        public Boolean Journaliser = true;
        public int NombreCas = 0;
        public int NoCasCourant = 0;
        public String Message = "";
        public String MessageEchec = "";
        
        public JCAssertionCore.JCAVariable Variables = new JCAssertionCore.JCAVariable() ;
        

        private XmlDocument ListeDeCasXML;
        private XmlNodeList ListeDeCas;
        private Boolean JournalInitialise = false;

        public void MessageAjoutter(String Texte)
            {
                Message = Message + Environment.NewLine  + Texte;
            }
        
        

        public void Load(String NomFichierLoad)
            {
                FichierDeCas = NomFichierLoad;
                
                Load();
            }

        

        public void Load()
            // Charger le fichier de cas et optionnellement
            // le fihier de valeur
            // initialise aussi lenom dujournalet lenombre de cas et le cas courant

        {
            if (FichierJournal == null )
                FichierJournal = FichierDeCas + ".log.txt";
            JournalInitialise = false;
            ListeDeCasXML = new XmlDocument();
            ListeDeCasXML.Load(FichierDeCas);
            if (ListeDeCasXML == null) throw new Exception("Le document XML est vide oumal structuré");
            XmlElement monRoot = ListeDeCasXML.DocumentElement;
            ListeDeCas = monRoot.GetElementsByTagName("Assertion");
            if (ListeDeCas == null) throw new Exception("Le document CML est vide oumal structuré");
            
            NombreCas = ListeDeCas.Count;
            if (NombreCas > 0)
                {
                  Journalise("Chargement de la liste des cas réussie. Nombre de cas " + NombreCas.ToString())  ;
                  NoCasCourant = 0;
                } else
                {
                  Journalise("Chargement de la liste des cas échouée. pas de cas ");
                  NoCasCourant = 0;
                }

          }

        public static string RepertoireAssembly()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\";
        }
        public XmlNodeList getListeDeCas()
        {
            return ListeDeCas;
        }



            
        

        

        public bool ExecuteCas(XmlNode XMLCas)
        {
            XmlDocument monDoc = new XmlDocument ();
            MessageEchec = "";
            NoCasCourant = NoCasCourant + 1;
            Message = "Cas numéro : " + NoCasCourant ;
            // vérifier et corriger si On est dans un node xml trop haut
            if (XMLCas["Assertion"] != null )
               {
                   monDoc.InnerXml  = XMLCas.InnerXml  ;
                   XMLCas = monDoc.GetElementsByTagName("Assertion").Item (0);
                                  
               }
            if ((XMLCas["Type"] == null) || (XMLCas["Type"].InnerText == null))
                {
                    MessageAjoutter("La balise type est introuvable ou n'a pas de valeur." + XMLCas.InnerXml  );
                    MessageEchec = 
                        "La balise type est introuvable ou n'a pas de valeur." + XMLCas.InnerXml;
                    return false;
                } else
                {
                    Boolean Resultat = false;
                    String monOperateur = XMLCas["Type"].InnerText  ;
                    MessageAjoutter( "Type : " + monOperateur);
                    try {
                        monOperateur = XMLCas["Type"].InnerText ;
                    switch (monOperateur)
                        {
                            case "FichierExiste":
                                Resultat =  JCAPontXML.JCAFichierExiste(XMLCas,
                                    ref  Message, ref  Variables.Variables, 
                                    ref MessageEchec  );
                                Journalise(Message);
                                return Resultat;

                            case "SubstituerVariablesFichier":
                                Resultat = JCAPontXML.JCASubstituerVariablesFichier(XMLCas, ref  Message, ref  Variables.Variables);
                                Journalise(Message);
                                return Resultat;

                            case "ContenuFichier":
                                Resultat = JCAPontXML.JCAContenuFichier (XMLCas, ref  Message, ref  Variables.Variables);
                                Journalise(Message);
                                return Resultat;
                            case "ExecuteProgramme":
                                Resultat = JCAPontXML.JCAExecuteProgramme(XMLCas, 
                                    ref  Message, ref  Variables.Variables , ref  MessageEchec );
                                Journalise(Message);
                                return Resultat;
                        
                        default:
                            MessageAjoutter("Type inconnu");
                            MessageEchec =
                            "Type inconnu : " + monOperateur ;
                            
                            Journalise(Message);
                            return false;
                        }
                        } catch (JCAssertionException excep)
                        {
                            MessageAjoutter( excep.Message);
                            MessageEchec = 
                                "Erreur technique : " + excep.Message;
                            
                            Journalise(Message);
                            return false;

                        }
                }
            }


            public void Journalise(String monMessage)
            {
                if (FichierJournal == null) Journaliser = false;
                if (Journaliser)
                    { 
                        if (! JournalInitialise )
                        {
                            if (File.Exists(FichierJournal)) File.Delete(FichierJournal);
                            JournalInitialise = true;
                        }
                        StreamWriter fileJournal =  File.AppendText(FichierJournal);
                        fileJournal.WriteLine(monMessage );
                        fileJournal.Close();
                    }
            }            
        

        


    }
}
