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
using System.IO;
using System.Data;


namespace JCASQLODPCore
{
    public class JCASQLODPHelper
    {
        public Int32 MAJLOB(ref System.Data.DataSet BD,
            String Fichier,
            String TypeOracle = "BLOB")
            {
                Byte[] Contenu = new Byte[] { 0x20 };

                Int32 NBRangees = BD.Tables[0].Rows.Count;
                for (Int32 i = (NBRangees - 1); (i < NBRangees) && (NBRangees > 0); i++)
                {
            switch (TypeOracle)
                {
                    case "BLOB":
                        BD.Tables[0].Rows[i][0] = Contenu;
                        break;
                    default:
                        throw new JCASQLODPException(
                            "Le type de données n'est pas suporté pour cette fonction : " +
                            BD.Tables[0].Rows[i][0].GetType().ToString());
                        break;
                } // switch
            } // for
                return NBRangees;

            }
    }  // class
} // namespace
