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
                                Resultat =  JCAPontXML.JCAFichierExiste(XMLCas,ref  Message, ref  Variables.Variables );
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
                        
                        default:
                            MessageAjoutter("Type inconnu");
                            Journalise(Message);
                            return false;
                        }
                        } catch (JCAssertionException excep)
                        {
                            MessageAjoutter( excep.Message);
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
