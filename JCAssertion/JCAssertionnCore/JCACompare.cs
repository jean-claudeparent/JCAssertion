using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JCAssertionCore;

namespace JCAssertionCore
{
    public  class JCACompare
    {
        /// <summary>
        /// Compare deux entier avec un operateur
        /// </summary>
        /// <param name="ResultatReel">Résultat réel d'un calcul</param>
        /// <param name="Operateur">Operateur de comparaison</param>
        /// <param name="ResultatAttendu">Résultat ¸a comparer avec le résultat réel</param>
        /// <returns>Résultat de comparaison</returns>
        public bool Compare(
            Int64 ResultatReel,
            String Operateur,
            Int64 ResultatAttendu)
            {
                Operateur = Operateur.ToUpper();  
                    
                switch  (Operateur) 
                { 
                    case "=":
                        return (ResultatReel == ResultatAttendu);
                    case "PG":
                        return (ResultatReel > ResultatAttendu);
                    case "PP":
                        return (ResultatReel < ResultatAttendu);
                    case ">":
                        return (ResultatReel > ResultatAttendu);
                    case "<":
                        return (ResultatReel < ResultatAttendu);
                    case ">=":
                        return (ResultatReel >= ResultatAttendu);
                    case "<=":
                        return (ResultatReel <= ResultatAttendu);
                    case "PG=":
                        return (ResultatReel >= ResultatAttendu);
                    case "PP=":
                        return (ResultatReel <= ResultatAttendu);
                    case "!=":
                        return (ResultatAttendu != ResultatReel);
                    case "<>":
                        return (ResultatAttendu != ResultatReel);
                    default :
                        throw new JCAssertionException(
                            "Pour cette comparaison l'opérateur '" +
                    Operateur + "' n'est pas un opérateur valide.");


                    }
            }
    }
}
