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
using JCAssertionCore;
using System.Data;



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
        
        private Oracle.ManagedDataAccess.Client.OracleConnection maConnection;
        private Oracle.ManagedDataAccess.Client.OracleDataReader monReader;
        private Oracle.ManagedDataAccess.Client.OracleCommand maCommandeSQL =
            new Oracle.ManagedDataAccess.Client.OracleCommand();



        /// <summary>
        /// CreerConnectionString : retourne la chaîne de conntextion
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
                    throw new JCAssertionException("Pour une connection à la base de données le user est obligatoire");
                else
                    monResultat = monResultat + ";User Id=" + User;

                // traiter le passwird
                if ((Password  == null) || (Password  == ""))
                    throw new JCAssertionException(
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

        }

        /// <summary>
        /// FermerConnection: Ferme la connection à ka vase de dibb.es
        /// en utilisant ODP. Peut retourner des exceptions.
        /// </summary>
        public void FermerConnection()
        {
            if (maConnection != null)
                maConnection.Close()  ;

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
                    throw new JCAssertionException(
                        "Aucune commande SQL n'a été fournie.");
                }
            maCommandeSQL.Connection = maConnection;
            maCommandeSQL.CommandText = maCommandeSQLString;
            maCommandeSQL.CommandType = CommandType.Text;
            monReader = maCommandeSQL.ExecuteReader();
             

           
        }

        /// <summary>
        /// AssertSQLVrai: Fait un select retournant un booleen  
        /// sur la connection courante et retourne la valeur
        /// </summary>
        public Boolean AssertSQLVrai(String CommandeSQL)
        {
          SQLSelect(CommandeSQL);
          if (monReader.GetType().Equals("null"))
              throw new JCAssertionException("La connabde SQL :" +
            CommandeSQL + ": ne retourne pas un réultat du type " +
            monReader.GetType().ToString() );

          return false ;
        }
        
    }
}
