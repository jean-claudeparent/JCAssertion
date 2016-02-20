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


        private XmlDocument ListeDeCasXML;
        private XmlNode CasCourant;
        private XmlNodeList ListeDeCas;
        private Dictionary<String, String > Variables = new Dictionary<String, String >();


        

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
            ListeDeCasXML = new XmlDocument();
            ListeDeCasXML.Load(FichierDeCas);
            if (ListeDeCasXML == null) throw new Exception("Le document CML est vide oumal structuré");
            XmlElement monRoot = ListeDeCasXML.DocumentElement;
            ListeDeCas = monRoot.GetElementsByTagName("Assertion");
            if (ListeDeCas == null) throw new Exception("Le document CML est vide oumal structuré");
            
            NombreCas = ListeDeCas.Count;
            if (NombreCas > 0)
                {
                  Message = "Chargement de la liste des cas réussie. Nombre de cas " + NombreCas.ToString()  ;
                  NoCasCourant = 1;
                } else
                {
                  Message = "Chargement de la liste des cas échouée. pas de cas ";
                  NoCasCourant = 0;
                }

          }

        public static string RepertoireAssembly()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\";
        }

        public void MAJVariable(String Cle, String Valeur)
        {
            // ajoute la paire au dictionnaire si absent,modifierla valeursiprésent
            if (Variables.ContainsKey(Cle)) Variables.Remove(Cle);
            Variables.Add(Cle,Valeur);
        }

        public int NombreVariables()
        {
            return Variables.Count;
        }

        public String  GetValeurVariable(String Cle)
        {
            
            String valeur;
            if (Variables.TryGetValue(Cle,   out valeur )) return valeur;
            else return null;
        }

        public Boolean ExecuteCas(int NoCas)
        {
            if ((NoCas < 1) || (NoCas > NombreCas)) return false;

            Message = "";
            return ExecuteXMLNode(ListeDeCas.Item(NoCas));
            
        }

        public bool ExecuteXMLNode(XmlNode XMLCas)
        {
            if ((XMLCas["Type"] == null) || (XMLCas["Type"].InnerText == null))
                {
                    Message = "La balise type est introuvable ou n'a pas de valeur.";
                    return false;
                } else
                {
                    String monOperateur = XMLCas["Type"].InnerText  ;
                    Message = "Type : " + monOperateur;
                    switch (monOperateur)
                        {
                            case "FichierExiste":
                                return JCAPontXML.JCAFichierExiste(XMLCas, ref Message );
                        default:
                            Message = Message + "Type inconnu";
                            return false;
                        }
                }
        }


    }
}
