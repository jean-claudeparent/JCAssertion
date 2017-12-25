using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using System.Threading.Tasks;

namespace JCAssertionCore
{
    public class JCAXMLHelper
    {
        public string xmlConnectionOracle(
            String User,
            String Password,
            String Serveur,
            String Action,
            String Cache = null)
        {
            String Resultat = "";
            Resultat = "<Assertion>" 
                + Environment.NewLine +
               "<Type>ConnectionOracle</Type>"
               + Environment.NewLine;

            if (User != null)
                Resultat =
                    Resultat
                    + "<User>" + User
                    + "</User>" + Environment.NewLine;

            if (Password != null)
                Resultat =
                    Resultat
                    + "<Password>" +
                    Password + "</Password>" + Environment.NewLine;
            if (Serveur != null)
                Resultat =
                    Resultat
                    + "<Serveur>" + Serveur
                    + "</Serveur>" + Environment.NewLine;

            if (Action != null)
                Resultat =
                    Resultat
                    + "<Action>" +
                    Action + "</Action>" 
                    + Environment.NewLine;

            if (Cache != null)
                Resultat =
                    Resultat
                    + "<Cache>" +
                    Cache + "</Cache>"
                    + Environment.NewLine;

            Resultat = Resultat +
                "</Assertion>";

            return Resultat;

        }

    }
}
