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
using Oracle.DataAccess;
using Oracle.ManagedDataAccess;
using System.Data;
using System.Xml;
using JCASQLODPCore;
using Oracle.ManagedDataAccess.Client;

 


namespace JCASQLODPCore
{
    /// <summary>
    /// JCASQLODPCore : Classe définissant les propriétés et méthodes
    /// pour accéder une base de données oracle avec le oracle data provider.
    /// </summary>
    public class JCASQLODPClient
    {
        public String Serveur;
        public String User;
        public String Password;
        private Boolean ConnectionOuverte = false ; // Indique si la connection est instanciée et ouverte
        public String Resume = "";
        public Boolean ActiverResume = false;
        public Boolean ExceptionDetaillee = true;
        public String  DernierResultat = ""; //  dernier resultat de assert en texte


        private Oracle.ManagedDataAccess.Client.OracleConnection maConnection;
        private Oracle.ManagedDataAccess.Client.OracleDataReader monReader;
        private Oracle.ManagedDataAccess.Client.OracleCommand maCommandeSQL =
           new Oracle.ManagedDataAccess.Client.OracleCommand();
        private JCASQLODPHelper Helper = new JCASQLODPHelper() ;
        

        
        /// <summary>
        /// CreerConnectionString : retourne la chaîne de connection
        /// crée à partir des propriétés de la classe. Peut
        /// lever une exception.
        /// </summary>
        public String CreerConnectionString()
            {
                String  monResultat = "Data Source=";
                if ((Serveur == null) || (Serveur == ""))
                    monResultat = monResultat + "localhost";
                else
                    monResultat = monResultat + Serveur;

                // traiter le user
                if ((User == null) || (User == ""))
                    throw new JCASQLODPException("Pour une connection à la base de données le user est obligatoire");
                else
                    monResultat = monResultat + ";User Id=" + User;

                // traiter le passwird
                if ((Password  == null) || (Password  == ""))
                    throw new JCASQLODPException(
                        "Pour une connection à la base de données le mot de passe est obligatoire");
                else
                    monResultat = monResultat + ";Password=" + Password ;

                
                return monResultat ;
            }

        /// <summary>
        /// OuvrirConnection: Ouvre la connection à ka vase de dibb.es
        /// en utilisant ODP. Peut retourner des exceptions.
        /// </summary>
        public void OuvrirConnection()
        {
            if (maConnection == null )
                maConnection = new Oracle.ManagedDataAccess.Client.OracleConnection(CreerConnectionString());
            maConnection.Open();
            ConnectionOuverte = true;

        }

        /// <summary>
        /// FermerConnection: Ferme la connection à ka vase de dibb.es
        /// en utilisant ODP. Peut retourner des exceptions.
        /// </summary>
        public void FermerConnection()
        {
            if ((maConnection != null) &&
                (ConnectionOuverte))
                    maConnection.Close();
            ConnectionOuverte = false;

        }

        /// <summary>
        /// SQLSelect: Fait un select sur la connection courante
        /// et met le resultat dans monReader. La commande
        /// est passé en paramètre.
        /// </summary>
        public void SQLSelect (String maCommandeSQLString)
        {
            
            if ((maCommandeSQLString == null) ||
                (maCommandeSQLString == ""))
                {
                    throw new JCASQLODPException(
                        "Aucune commande SQL n'a été fournie.");
                }
            maCommandeSQL.Connection = maConnection;
            maCommandeSQL.CommandText = maCommandeSQLString;
            maCommandeSQL.CommandType = CommandType.Text;
            try {
              monReader = maCommandeSQL.ExecuteReader();
              monReader.Read();
            }
            catch (OracleException excep)
            {
                if (ExceptionDetaillee)
                    throw new JCASQLODPException("Commande SQL:" + Environment.NewLine +
                    maCommandeSQLString + Environment.NewLine +
                    excep.Message, excep);
                else throw excep;
            }
            if (ActiverResume) Resumer();
             

           
        }

        /// <summary>
        /// AssertSQL (CommandeSQL,ResultatAttendu): Fait un select 
        ///
        /// retournant un nombre  
        /// sur la connection courante et retourne si
        /// la valeur est égale (ou est évaluée avec un autre operateur) au Resultat attendu.
        /// Si aucune rangée n'est retournée par le select on 
        /// retourne false. 
        /// Modifie aussi la propriété DernierResultat
        /// </summary>
        public Boolean AssertSQL(String CommandeSQL, 
            Double  ResultatAttendu,
            String Operateur = "=")
        {
            Decimal TypeDecimal = 0;
            Int64 TypeInt64 = 0;
            Int32 TypeInt32 = 0;
            Int16 TypeInt16 = 0;
            int TypeInt = 0;
            
            String TypeString = "";
            Double TypeDouble = 0;
            Boolean TypeTrouve = false;
            
            DernierResultat = "Problème technique";
          Boolean ResultatAssertion = false;
          Operateur = Operateur.ToUpper(); 
          SQLSelect(CommandeSQL);
          if (ActiverResume)
              Resume = Resume +
                  "Valeur attendue : " +
                  ResultatAttendu.ToString() +
                  Environment.NewLine;
          if (!monReader.HasRows)
              {
                DernierResultat = "Le select ne retourne aucune rangées";
                return false;
              }
          
            // Si la colonne est null retourner false
          if (monReader.IsDBNull(0))
              {
                  DernierResultat = "null";
                return false;
              }

          Double monResultat = 0;
          
          // Garder le type en traitement pour l'exception
          String TypeDeColonne = monReader.GetFieldType(0).ToString();
 
          // peu importe le type numérique ramener cela en type double
          try {
            // TypeInt64
          if (monReader.GetFieldType(0) == TypeInt64.GetType())
          {
              monResultat = Convert.ToDouble(monReader.GetInt64(0));
              TypeTrouve = true;
              DernierResultat = monReader.GetInt64(0).ToString(); 
          }

          // TypeInt32
          if (monReader.GetFieldType(0) == TypeInt32.GetType())
          {
              monResultat = Convert.ToDouble(monReader.GetInt32(0));
              TypeTrouve = true;
              DernierResultat = monReader.GetInt32(0).ToString();
          }

          // TypeInt16
          if (monReader.GetFieldType(0) == TypeInt16.GetType())
          {
              monResultat = Convert.ToDouble(monReader.GetInt16(0));
              TypeTrouve = true;
              DernierResultat = monReader.GetInt16(0).ToString();
          }

          // TypeInt
          if (monReader.GetFieldType(0) == TypeInt.GetType())
          {
              monResultat = Convert.ToDouble(monReader.GetInt16(0));
              TypeTrouve = true;
              DernierResultat = monReader.GetInt16(0).ToString();
          }

          // TypeDecimal
            if (monReader.GetFieldType(0) == TypeDecimal.GetType())
                  {
                    monResultat = Convert.ToDouble(monReader.GetDecimal(0));
                    TypeTrouve = true;
                    DernierResultat = monReader.GetDecimal(0).ToString();
                  }
            // TypeDouble
            if (monReader.GetFieldType(0) == TypeDouble.GetType())
            {
                monResultat = monReader.GetDouble(0);
                TypeTrouve = true;
                DernierResultat = Convert.ToString(monReader.GetDouble(0));
            }
              } catch  (Exception excep)
                {
                    throw new JCASQLODPException 
                    ("Erreur de programmationtype " + TypeDeColonne + " " +
                  excep.Message ,
                    excep);
                }

              // TypeString
              if (monReader.GetFieldType(0) == TypeString.GetType())
              {
                  throw new JCASQLODPException("La commande SQL retourne une chaîne de caractère et le résultat attendu est un nombre, commande = " +
                  CommandeSQL);
              }
              if (!TypeTrouve)
              {
                  DernierResultat = "Erreur de programmation : type non supporté " +
                      TypeDeColonne;
                  throw new JCASQLODPException("La commande SQL retourne un type de données non supporté, commande = " +
                  CommandeSQL +
                  "Type non supporté : " +
                  monReader.GetFieldType(0).ToString ());
              }
              

          switch (Operateur)
          {
              case "!=":
                  ResultatAssertion = (!(ResultatAttendu == monResultat));
                  break;
              case "PG":
                  ResultatAssertion = (monResultat > ResultatAttendu);
                  break;
              case "PP":
                  ResultatAssertion = (monResultat < ResultatAttendu);
                  break;
              case "PG=":
                  ResultatAssertion = (monResultat >= ResultatAttendu);
                  break;
              case "PP=":
                  ResultatAssertion = (monResultat <= ResultatAttendu);
                  break; 
              default:
                  ResultatAssertion = (ResultatAttendu == monResultat);
                  break;
          }

          return ResultatAssertion;
        }

        

        /// <summary>
        /// AssertSQLString (CommandeSQL,ResultatAttendu): Fait un select 
        /// retournant un texte  
        /// sur la connection courante et retourne si
        /// la valeur est égale au Resultat attendu.
        /// Si aucune rangée n'est retournée par le select on retourne false
        /// </summary>
        public Boolean AssertSQL(String CommandeSQL,
            String  ResultatAttendu)
        {

            DernierResultat = "Erreur rencontrée";
            SQLSelect(CommandeSQL);
            if (!monReader.HasRows)
                {
                    DernierResultat = "Le select n'a retourné aucune ligne";
                    return false;
                }
            String  monResultat = "";
            
            try
            {
                monResultat =
                    monReader.GetString(0);
            }
            catch (Exception excep)
            {
                DernierResultat = "Erreur";
                throw new JCASQLODPException("La connande SQL :" +
              CommandeSQL + ": ne retourne pas un résultat de type chaîne de caractère", excep);
            }
            DernierResultat = monResultat;
            return (ResultatAttendu == monResultat);
        }

        

        /// <summary>
        /// SiConnectionOuverte retourne si la connection est ouverte
        /// </summary>
        public Boolean SiConnectionOuverte()
        {
            return ConnectionOuverte;
        }

        /// <summary>
        /// ChargeLOB : Charge un Large object
        /// dans toutes les rangées définies
        /// par un énoncé SQL.
        /// </summary>
        /// <param name="SQL">Énoncé SQL identifiant 1 à n rangées dMune colonne.
        /// C'est dans cette colonne que leLOB sera chargé</param>
        /// <param name="Fichier">Nom du fichier qui contient le
        /// contenu à mettre dans le LOB</param>
        public Int32  ChargeLOB(
            String SQL,
            String Fichier)
        {
            System.Data.DataSet monDS = new DataSet();
            OracleCommandBuilder monCB;
            Int32 NbRandees = 0;

            
            Helper.ValideFichier(Fichier ); 
            
            // Initialiser la commande sql
            maCommandeSQL.Connection = maConnection; 
            maCommandeSQL.CommandText = SQL;
            maCommandeSQL.CommandType = CommandType.Text;

            // Créer le dataadapter
            OracleDataAdapter momDA = new OracleDataAdapter(maCommandeSQL );
            

            // Charger le dataset
            try {
                momDA.MissingSchemaAction = 
                    MissingSchemaAction.AddWithKey;
                momDA.FillSchema(monDS, SchemaType.Source); 
                
               momDA.Fill(monDS);
               
               


               monCB = 
                   new OracleCommandBuilder(momDA);
               
               // Modifier le dataset et reporter les changements
               NbRandees = Helper.MAJLOB(ref monDS, Fichier );
               Helper.ExploreDataset(monDS);
               momDA.Update(monDS.Tables[0]);
               
                 
            
            }
            catch (System.InvalidOperationException excep)
                {
                    throw new JCASQLODPException("Erreur dans la commande SQL de sélection  " +
                        "La commande doit avoir la forme 'select colonnedecle, colonneblob as blob from table' " +
                    Environment.NewLine + "SQL: " +
                    SQL + Environment.NewLine +
                    excep.Message, excep);
                }
            catch (Exception excep)
                {
                    throw new JCASQLODPException("Erreur d'acces à la base de données " +
                Environment.NewLine +  "SQL: " +
                SQL + Environment.NewLine +
                excep.Message  , excep);
                }

            
            

            return NbRandees;

                
            
        }




        /// <summary>
        /// Resumer : Resume le datareader dans la propriété Resume
        /// Sert à débugger
        /// </summary>
        private void  Resumer()
        {
            //TODO Supporter tous les types de données
            String Resultat = "";
            if (monReader == null )
                {
                    Resume = "Le datereader est null";
                    return;
                }
            Resultat = "Nombre de colonnes du résultat : " + 
                monReader.FieldCount.ToString() +
                Environment.NewLine  ; 
            for (int i = 0; i < monReader.FieldCount ; i++)
                {
                    Resultat = Resultat +
                        "Nom : " +
                        monReader.GetName(i) +
                        Environment.NewLine +
                        "Type : " +
                        monReader.GetFieldType(i) +
                        Environment.NewLine +
                        "Valeur : ";
                switch (monReader.GetFieldType(i).ToString() )
                {
                    case "System.Decimal":
                        Resultat = Resultat +
                            monReader.GetDecimal(i).ToString()  +
                            Environment.NewLine; 
                        break;
                    case "System.Int16":
                        Resultat = Resultat +
                            monReader.GetInt16(i).ToString() +
                            Environment.NewLine;
                        break;
                    case "System.Int32":
                        Resultat = Resultat +
                            monReader.GetInt32(i).ToString() +
                            Environment.NewLine;
                        break;
                    case "System.Int64":
                        Resultat = Resultat +
                            monReader.GetInt64(i).ToString() +
                            Environment.NewLine;
                        break;
                    
                    default:
                        Resultat = Resultat +
                            "Type non implémenté DANS LA FONCTION RESUME : " +
                            monReader.GetFieldType(i).ToString() +
                            Environment.NewLine  ;
                        break; 


                } // end sWitch

                        
                } // end for
            Resume = Resultat ;
        }


        /// <summary>
        /// SQLExecute: Execute une command SQL qui modifie la base de données
        /// sur la connection courante
        /// La commande
        /// est passé en paramètre.
        /// Retourne le nombre de rangées modifiées.
        /// </summary>
        public Int64 SQLExecute(String SQL)
            {
            try {
                maCommandeSQL.Connection = maConnection; 
                maCommandeSQL.CommandText = SQL;
                maCommandeSQL.CommandType = CommandType.Text;
                Int64 Resultat = maCommandeSQL.ExecuteNonQuery();
                return Resultat;
            } catch (OracleException excep)
                {
                    if (ExceptionDetaillee)
                        throw new JCASQLODPException("Commande SQL:" + Environment.NewLine +
                        SQL + Environment.NewLine +
                        excep.Message  , excep);
                    else throw excep; 
                }
            }

        /// <summary>
        /// ExporteLOB : Écrit des colonnes BLOB ou CLOB
        /// dans des fichiers.
        /// </summary>
        /// <param name="SQL">Commande SQL spécifiant les noms des fichiers et la colonne LOB à exporter</param>
        /// <param name="Chemin">Chemin où créer les fichiers</param>
        /// <param name="TypeEncodage">Type d'encodage de fichier, null = byte8</param>
        /// <returns>Nombre de LOB exportés en ichier</returns>
        public Int32 ExporteLOB(
            String SQL,
            String Chemin,
            Encoding  TypeEncodage = null)
            {
                System.Data.DataSet monDS = new DataSet();
                Int32 NbRandees = 0;
                                
                // Initialiser la commande sql
                maCommandeSQL.Connection = maConnection;
                maCommandeSQL.CommandText = SQL;
                maCommandeSQL.CommandType = CommandType.Text;

                // Créer le dataadapter
                OracleDataAdapter momDA = new OracleDataAdapter(maCommandeSQL);


                // Charger le dataset
                try
                {
                    //momDA.MissingSchemaAction =
                      //  MissingSchemaAction.AddWithKey;
                    momDA.FillSchema(monDS, SchemaType.Source);

                    momDA.Fill(monDS);




                   

                    // Exporter les lob
                    NbRandees = Helper.ExporteLOB(monDS,Chemin, 
                        TypeEncodage, ref DernierResultat  );
                    


                }
                catch (System.InvalidOperationException excep)
                {
                    throw new JCASQLODPException("Erreur dans la commande SQL de sélection  " +
                        "La commande doit avoir la forme 'select colonnedecle, colonneblob as blob from table' " +
                    Environment.NewLine + "SQL: " +
                    SQL + Environment.NewLine +
                    excep.Message, excep);
                }
                catch (Exception excep)
                {
                    throw new JCASQLODPException("Erreur d'acces à la base de données " +
                Environment.NewLine + "SQL: " +
                SQL + Environment.NewLine +
                excep.Message, excep);
                }




                return NbRandees;

                
                
            }

    
    } // class
} // namespace
