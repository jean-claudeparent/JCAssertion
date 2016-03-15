using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace JCAssertionCore
{
    public class JCAVariable
    {
        public Dictionary<String, String> Variables = new Dictionary<String, String>();
        

        public static   String ExtraireVariable(String  Argument)
        {
            // retournele nom delaprochainevariableou "" si aucune
            int ouverture = Argument.IndexOf("{{");
            int Fermeture = Argument.IndexOf("}}");
            if ((Fermeture < 1) || (Fermeture < ouverture) || (ouverture < 0)) return "";
            if ((Fermeture < 1) || (Fermeture < ouverture) || (ouverture < 0)) return "";
            return Argument.Substring(ouverture  +2, (Fermeture - 2) - ouverture  );
        }       


        public static String SubstituerVariables(String Argument, Dictionary<String, String> Variables)
            {
                String ProchaineVariable = ExtraireVariable(Argument );
                while (ProchaineVariable != "")
                    {
                        if (!(Variables.ContainsKey(ProchaineVariable))  ) 
                            throw new JCAssertionException("La variable " + ProchaineVariable  + " n'a pas eu de valeur fournie.")  ;
                        Argument = Argument.Replace("{{" + ProchaineVariable + "}}", Variables[ProchaineVariable]);
                        ProchaineVariable = ExtraireVariable(Argument);
                }
                return Argument;
            }

        public void MAJVariable(String Cle, String Valeur)
        {
            // ajoute la paire au dictionnaire si absent,modifierla valeursiprésent
            if (Variables.ContainsKey(Cle)) Variables.Remove(Cle);
            Variables.Add(Cle,Valeur);
            Trier();
            
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

        public Dictionary<String, String> GetDictionnaireVariable()
        {
            return Variables;
        }

        public void  EcrireFichier(String NomFichier)
        {
            // Créer le document xml desvariables
            Trier();
            XmlDocument monXMLDeVariables = new XmlDocument();
            XmlDeclaration maDeclaration = monXMLDeVariables.CreateXmlDeclaration("1.0","UTF-8", null);
            XmlElement root = monXMLDeVariables.DocumentElement;
            monXMLDeVariables.InsertBefore(maDeclaration ,root);
            
            XmlElement monElementListe = monXMLDeVariables.CreateElement (String.Empty ,"ListeDeVariables" , String.Empty );
            monXMLDeVariables.AppendChild(monElementListe);
            foreach (KeyValuePair<String, String>  maPaire in Variables  )
            {
                XmlElement maVariable = 
                    monXMLDeVariables.CreateElement (String.Empty,"Variable",String.Empty);
                maVariable.SetAttribute("Cle", maPaire.Key);
                maVariable.SetAttribute("Valeur", maPaire.Value );

                monElementListe.AppendChild(maVariable);
                
            }
            
            // sauver le document
            monXMLDeVariables.Save(NomFichier );

        }

        public void LireFichier(String NomFichier, Boolean Ajouter = false )
        {
            if(! Ajouter) Variables = new Dictionary<String, String>();
            XmlDocument DocLiteVariable = new XmlDocument();
            DocLiteVariable.Load(NomFichier);
            XmlNodeList Liste = DocLiteVariable.SelectNodes("/ListeDeVariables/Variable");
            if (Liste != null)
                {
                    foreach (XmlNode maVariable in Liste)
                    {
                        
                        if (maVariable.Attributes["Cle"] == null) throw new JCAssertionException("L'attribut cle est null" + maVariable.InnerXml.ToString() );
                        if (maVariable.Attributes["Valeur"] == null) throw new JCAssertionException("L'attribut Valeur est null" + maVariable.InnerXml.ToString());

                        if ((maVariable.Attributes["Cle"].Value != null) && (maVariable.Attributes["Valeur"].Value != null))
                            MAJVariable(maVariable.Attributes["Cle"].Value    , maVariable.Attributes["Valeur"].Value );
                 }
                    Trier();
                }

        }
        

         public void Trier()
        {
            var maListe = Variables.Keys.ToList();
            maListe.Sort();
            Dictionary<String, String> VariablesTriees = new Dictionary<String, String>();
            foreach (var maCle in maListe )
                {
                    VariablesTriees.Add(maCle, GetValeurVariable(maCle ));
                }
            Variables = VariablesTriees;

        }

        public Boolean EstEgal(Dictionary<String, String> VarAcomparer, out String Detail )
        {
            Detail = "Égal";
            if (Variables.Count() != VarAcomparer.Count() )
                {
                    Detail = "Le nombre de variables est différent, " + Variables.Count().ToString() + " et " + VarAcomparer.Count().ToString();
                    return false;
                }
            foreach (KeyValuePair<String, String> maPaire in Variables  )
                {
                  if(! VarAcomparer.ContainsKey(maPaire.Key))
                      {
                          Detail = "Une clé de variable manque soit : " + maPaire.Key;
                          return false;
                      }
                  String maValeur;
                  VarAcomparer.TryGetValue(maPaire.Key  ,out maValeur );

                  if (maPaire.Value != maValeur)
                  {
                      Detail = "La valeur d'une clé de vraiable est différent  soit clé : " + maPaire.Key + " valeur : " + maPaire.Value + " autre valeur : " + maValeur  ;
                      return false;
                  }
                }
            return true ;
        }

        public   String ExtrairePaire(String  Argument, out String Valeur)
        {
            Valeur = "";
            if ((Argument == null ) || (Argument == "")) return "";
            if(Argument.IndexOf("=") < 0 ) Argument = Argument + "=" + Argument;
            //todo enlever la prochaine ligne pour permettre lesstring vides dan lesarhuments
            if(Argument.IndexOf("=") == (Argument.Length - 1) ) Argument = Argument + "=" + Argument.Substring (0, Argument.Length - 1);
            String Cle = Argument.Substring (0,Argument.IndexOf ("="));
            Valeur = Argument.Substring (Argument.IndexOf("=") + 1 , Argument.Length - 1 - Cle.Length );
            return Cle;
        }
        


    }
}
