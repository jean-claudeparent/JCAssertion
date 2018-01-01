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

namespace JCAssertionCore
{
    public class JCAVariable
    {
        
        // ^propriétés et attributs
        public Dictionary<String, String> Variables = new Dictionary<String, String>();
        

    
        public class Constantes
            {
            public const String JCA_FichierDeVariables = "JCA.FichierDeVariables";
        
            }
        /// <summary>
        /// Retourne le nom de  la prochaine variable
        /// trouvée dans le texte.
        /// Les variables sont délimitées par
        /// "{{" et "}}"
        /// </summary>
        /// <param name="Argument">Texte dans lequel rechercher la variable</param>
        /// <returns>Retourne le nom de la première variable trouvée dans le texte. Retourne une cha¸ine vide si aucune n'est trouvée.</returns>
        public static   String ExtraireVariable(String  Argument)
        {
            int ouverture = Argument.IndexOf(
                "{{");
            int Fermeture = Argument.IndexOf(
                "}}");
            if ((Fermeture < 1) || 
                (Fermeture < ouverture) || 
                (ouverture < 0))
                return "";
            return Argument.Substring(ouverture  +2, (Fermeture - 2) - ouverture  );
        }


        /// <summary>
        /// Remplace toutes
        /// les balises de variable
        /// par le contenu du dictionnaire
        /// des valeurs de variables.
        /// Si la variable n'est pas
        /// dans le dictionnaire cela lance une exception.
        /// </summary>
        /// <param name="Argument">Texte dans lequel on doit remplacer les balises de variables</param>
        /// <param name="Variables">Le dictionnaire des clés versus valeurs des variables.</param>
        /// <returns>Le texte avec les balises remplacées par le contenu ou une exception</returns>
        public static String SubstituerVariables(String Argument, Dictionary<String, String> Variables)
        {
            String ProchaineVariable = ExtraireVariable(Argument );
            while (ProchaineVariable != "")
                {
                   if (!(Variables.ContainsKey(ProchaineVariable))  ) 
                            throw new JCAssertionException("La variable " + ProchaineVariable  + " n'a pas eu de valeur fournie.")  ;
                        Argument = Argument.Replace(
                            "{{" + ProchaineVariable + "}}", 
                            Variables[ProchaineVariable]);
                        ProchaineVariable = ExtraireVariable(Argument);
                }
                return Argument;
            }

        /// <summary>
        /// Si la clé existe dans le dictionnaire
        /// de contenu de variables, la
        /// valeur est mise à jour.
        /// Sinon la clé et la veleur sont crées 
        /// dans le dictionnaire.
        /// Le dictionnaire est ensuite trié.
        /// </summary>
        /// <param name="Cle">Clé de la variables</param>
        /// <param name="Valeur">Valeur de la variable</param>
        public void MAJVariable(
            String Cle, 
            String Valeur)
        {
            if (Variables.ContainsKey(Cle))
                Variables.Remove(Cle);
            Variables.Add(Cle,Valeur);
            Trier();
            
        }

       /// <summary>
       /// Retourne le nombre de variables
       /// dans le dictionnaire.
       /// </summary>
       /// <returns>Nombre de variables dans le dictionnaire</returns>
        public int NombreVariables()
        {
            return Variables.Count;
        }


        /// <summary>
        /// GetValeurVariable : retourne la valeur d'une clé identifiant une variable 
        /// </summary>
        /// <returns>Retourne la valeur asociée à ka cké dans le dictionnaire des variables. Retourne null si la lcé n'existe pas.</returns>
        public String  GetValeurVariable(String Cle)
        {
            
            String valeur;
            if (Variables.TryGetValue(Cle,
                out valeur )) return valeur;
            else return null;
        }

        /// <summary>
        /// retourne le dictionnaire des variables
        /// </summary>
        /// <returns>Dictionnaire des variables</returns>
        public Dictionary<String, String> GetDictionnaireVariable()
        {
            return Variables;
        }

        /// <summary>
        /// Écrit le dictionnaire
        /// des variables comme un
        /// document xml dans un fichier xml. 
        /// Le dictionnaire est trié.
        /// </summary>
        /// <param name="NomFichier">Nom du fichier avec le chemin</param>
        public void  EcrireFichier(String NomFichier)
        {
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
        
        /// <summary>
        /// Lit un fichier xml
        /// définissant le dictionnaire de variable
        /// et le met dans le dictionnaire.
        /// </summary>
        /// <param name="NomFichier">
        /// Nom du fichier xml contenant le dictionnaire en format xml
        /// </param>
        /// <param name="Ajouter">
        /// Si true le contenu du fichier
        /// sera ajouté au dictionnaire existant.
        /// Si une clé existe dans le dictionnaire
        /// actuel la valeur sera remplacée par celle du fichier.
        /// Si fakse seul le contenu du fichier sera conservé
        /// dans le dictionnaire.</param>
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
                    // maj de JCA.FichierDeVariables
                    MAJVariable(Constantes.JCA_FichierDeVariables, NomFichier);
                    Trier();
                }

        }
        

        /// <summary>
        /// trie le dictionnaire des variables
        /// par ordre croissant  alphabétique de clé.
        /// </summary>
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

        /// <summary>
        /// Compare deux dictionnaires
        /// de variables.  
        /// </summary>
        /// <param name="VarAcomparer">Dictionnaire à comparer</param>
        /// <param name="Detail">
        /// Si les deux 
        /// dictionnaires sont différents.
        /// retourne Une string comportant
        /// l'explication des différences en sortie</param>
        /// <returns>true si les deux dictionnaires sont pareils,
        /// false si non</returns>
        public Boolean EstEgal(
            Dictionary<String,String> VarAcomparer, 
            out String Detail)
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

        /// <summary>
        /// retourne la clé 
        /// et dans une variable de sortie
        /// la valeur d'une paire sous la forme
        /// cle=valeur
        /// si jamais la clause = valeur n'existe pas
        /// la valeur retournée sera la clé
        /// </summary>
        /// <param name="Argument">Texte à analyser
        /// sous la forma cle=valeur ou cle</param>
        /// <param name="Valeur"></param>
        /// <returns></returns>
        public   String ExtrairePaire(
            String  Argument, 
            out String Valeur)
        {
            Valeur = "";
            if ((Argument == null ) || 
                (Argument == ""))
                  return "";
            if(Argument.IndexOf("=") < 0 )
                Argument = Argument + "=" + Argument;
            String Cle = Argument.Substring (0,Argument.IndexOf ("="));
            Valeur = Argument.Substring (Argument.IndexOf("=") + 1 , Argument.Length - 1 - Cle.Length );
            return Cle;
        }
        
        /// <summary>
        /// retourne si une clé
        /// existe dans le dictionnaire
        /// de variables
        /// </summary>
        /// <param name="Cle">Clé de l'entrée au dictionnaire</param>
        /// <returns>True si la clé existe dans le dicyionnaire, false sinon</returns>
        public bool CleExiste(
            string Cle)
        {
            return Variables.ContainsKey(Cle);

        }
        /// <summary>
        /// Enlève une clé et
        /// sa valeur du dictionnaire de variables.
        /// Si la clé n'existe pas il n'y a pas d'exception de lancée.
        /// </summary>
        /// <param name="Cle">Clé à emlever</param>
        public void EnleverCle (
            string Cle)
        {
            if (Variables.ContainsKey(Cle))
                Variables.Remove(Cle);
            
        }


    }
}
