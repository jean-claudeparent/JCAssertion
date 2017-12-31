﻿// License
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

namespace JCAssertionCore
{
    public partial class JCAPontXML
    {
        /// <summary>
        /// Nodifie le dictionnaire de variable
        /// en utilisant la vraie métode
        /// des objects Variables
        /// </summary>
        /// <param name="Variables"></param>
        /// <param name="Cle"></param>
        /// <param name="Valeur"></param>
        private void MAJVariable(
            ref Dictionary<String, String> Variables,
            string Cle,
            string Valeur)
        {
            JCAVariable VariablesTemp = new JCAVariable();
            VariablesTemp.Variables = Variables;
            VariablesTemp.MAJVariable(Cle,Valeur);
            Variables = VariablesTemp.Variables; 
        }

        /// <summary>
        /// Retourne la valeur d'une variable
        /// en utilisant la méthode de
        /// l'objet variable
        /// </summary>
        /// <param name="Variables"></param>
        /// <param name="Cle"></param>
        /// <returns></returns>
        private string  getValeurVariable(
            Dictionary<String, String> Variables,
            string Cle)
        {
            JCAVariable VariablesTemp = new JCAVariable();
            VariablesTemp.Variables = Variables;
            return VariablesTemp.GetValeurVariable(Cle);  
        }

        /// <summary>
        /// retourne une expression d'assertion
        /// formattée.
        /// </summary>
        /// <param name="ValeurReelle"></param>
        /// <param name="Operateur"></param>
        /// <param name="ValeurAttendue"></param>
        /// <param name="TypeString">Indique si les valeurs sont des strings plutôt que des nombres</param>
        /// <returns>Expression formattée</returns>
        private string FormaterExpression(
            string ValeurReelle,
            string Operateur,
            string ValeurAttendue,
            bool TypeString = false)
        {
            string Resultat =
                "(Valeur Réelle):";
            if (TypeString)
                Resultat = Resultat + "\"";
            Resultat = Resultat +
                ValeurReelle;
            if (TypeString)
                Resultat = Resultat + "\"";
            Resultat = Resultat +
                " " + Operateur + " ";
            if (TypeString)
                Resultat = Resultat + "\"";
            Resultat = Resultat +
                ValeurAttendue;
            if (TypeString)
                Resultat = Resultat + "\"";
            Resultat = Resultat  +
                " :(Valeur attendue)";
            return Resultat;
        }
    

    } // classe
} // namespace
