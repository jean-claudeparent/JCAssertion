// License
//
//
//  JCAssertion : Un ensemble d’outils
//              pour configurer et vérifier les environnements 
//              de tests sous windows.
//
//  Copyright 2016,2017 Jean-Claude Parent 
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
using JCASQLODPCore;

namespace JCAssertionCore
{
    /// <summary>
    /// JCASQLClient : Classe définissant les propriétés et méthodes
    /// de plus haut niveau pour le pont xml
    /// pour accéder une base de données.
    /// Actuellement implanté oracle avec le oracle data provider.
    /// À venir coder les appels a oledb ici aussi
    /// </summary>
    public class JCASQLClient : IDisposable
    {

        private JCASQLODPClient monSQLClientODP = new JCASQLODPClient();
        public enum Action {Aucune, Ouvrir, Fermer };
        public String DernierResultat = "";

        protected virtual void Dispose(bool Tout)
        {
            if (Tout)
            {
                if (monSQLClientODP != null)
                    monSQLClientODP.Dispose();
            } else
            {
                if (monSQLClientODP != null)
                    monSQLClientODP.Dispose();
            };
        }

        public void Dispose()
        {
            Dispose(true );
        }

        /// <summary>
        /// ConnectionOuverte : Retourne si la connection
        /// courante courante est ouverte.
        /// Pour l'instant une seule connection est
        /// permise par ODP a oracle.
        /// </summary>
        /// <returns></returns>
        public Boolean ConnectionOuverte()
        {
            return monSQLClientODP.SiConnectionOuverte(); 
        }


        /// <summary>
        /// InitConnection : Définit les param
        /// etres de la
        /// connection et permet d'ouvrir
        /// retourne la chaîne de conntextion ou de ferner celle-ci
        /// </summary>
        public void InitConnection(String User,
            String Password,
            String Serveur = null,
            Action monAction = Action.Aucune  )
        {
            monSQLClientODP.User = User;
            monSQLClientODP.Password = Password;
            monSQLClientODP.Serveur = Serveur;
            if (monAction == Action.Ouvrir)
                monSQLClientODP.OuvrirConnection();
            if (monAction == Action.Fermer)
                monSQLClientODP.FermerConnection(); 
        }

        /// <summary>
        /// SQLAssert : Appelle le SQLAssert
        /// de la connection  courante. 
        /// </summary>
        /// <param name="SQL">Commande SQL servant pour l'assertion</param>
        /// <param name="ResultatAttendu">Résultat attendu à comparer pour que l'assertion soit vraie.</param>
        /// <returns>Retourne si l'assertion est vraie
        /// Modifie aussi l propriété DernierResultat</returns>
        public Boolean SQLAssert(String SQL,
            String ResultatAttendu)
        {
            DernierResultat = "";
            Boolean Resultat = false;
            Boolean Fermer = (!monSQLClientODP.SiConnectionOuverte());
            if (!monSQLClientODP.SiConnectionOuverte())
                monSQLClientODP.OuvrirConnection();
            Resultat = monSQLClientODP.AssertSQL(SQL, ResultatAttendu);
            DernierResultat = monSQLClientODP.DernierResultat; 
            if (Fermer)
                monSQLClientODP.FermerConnection();

            return Resultat; 
        }

        /// <summary>
        /// SQLAssert : Appelle le SQLAssert
        /// de la connection  courante. 
        /// </summary>
        /// <param name="SQL">Commande SQL servant pour l'assertion</param>
        /// <param name="ResultatAttendu">Résultat attendu à comparer pour que l'assertion soit vraie.</param>
        /// <param name="Operateur">Opérateur logique pour comparer le résultat SQL et le résultat attendu. Par exemple plus grand que</param>
        /// <returns>Retourne si l'assertion est vraie</returns>
        public Boolean SQLAssert(String SQL,
            Double  ResultatAttendu,
            String Operateur = "=")
        {

            DernierResultat = "Erreur";
            Boolean Resultat = false;
            Boolean Fermer = (!monSQLClientODP.SiConnectionOuverte());
            if (!monSQLClientODP.SiConnectionOuverte())
                monSQLClientODP.OuvrirConnection();
            Resultat = monSQLClientODP.AssertSQL(SQL, ResultatAttendu, Operateur);
            DernierResultat = monSQLClientODP.DernierResultat; 
            if (Fermer)
                monSQLClientODP.FermerConnection();

            return Resultat; 
        }

        /// <summary>
        /// SQLExecute : excute une commande SQL qui modifie la base de données
        /// courante. Par exemoke un insert dans une table.
        /// Peut lancer des exceptions.
        /// </summary>
        /// <param name="SQL">Commande SQL à exécuter.</param>
        /// <returns>Nombre de rangées de table modifiées si la commande SQL nodifie des rangées.</returns>
        public Int64 SQLExecute(String SQL)
        {


            Boolean Fermer = (!monSQLClientODP.SiConnectionOuverte());

            if (!monSQLClientODP.SiConnectionOuverte())
                monSQLClientODP.OuvrirConnection();
            Int64 Resultat = monSQLClientODP.SQLExecute(SQL);
            if (Fermer)
                monSQLClientODP.FermerConnection();
            return Resultat;
        }

        /// <summary>
        /// Charge un fichier dans une colonne
        /// qui contient un objet binaire large
        /// </summary>
        /// <param name="SQL">SQL indiquant la colonne LOB à renplir ainsi que les rangées à traiter</param>
        /// <param name="Fichier">Fichier contenant le contenu à charger</param>
        /// <returns>Nombre de rangées affectées par le chargement</returns>
        public Int64 SQLChargeLOB(String SQL,
            String Fichier)
        {


            Boolean Fermer = (!monSQLClientODP.SiConnectionOuverte());

            if (!monSQLClientODP.SiConnectionOuverte())
                monSQLClientODP.OuvrirConnection();
            Int64 Resultat = monSQLClientODP.ChargeLOB(SQL,Fichier);
            if (Fermer)
                monSQLClientODP.FermerConnection();
            return Resultat;
        }

        public Int64 ExporteLOB(
            String SQL,
            String Chemin,
            Encoding  TypeEncodage = null)
            {
                Boolean Fermer = (!monSQLClientODP.SiConnectionOuverte());

                if (!monSQLClientODP.SiConnectionOuverte())
                    monSQLClientODP.OuvrirConnection();
            Int64 Resultat = monSQLClientODP.ExporteLOB(
                SQL,Chemin, TypeEncodage);
            DernierResultat = monSQLClientODP.DernierResultat; 
            if (Fermer)
                monSQLClientODP.FermerConnection();
            return Resultat;
        
            }
    }
}
