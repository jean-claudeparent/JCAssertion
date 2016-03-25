// License
//
//
//  JCAssertion : Un ensemble d’outils
//              pour configurer et vérifier les environnements 
//              de tests sous windows.
//
//  Copyright 2016 Jean-Claude Parent 
// 
//  Informations : www.noursicain.net
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JCAssertionCore;
using System.IO;
using System.Xml;



namespace JCAssertion
{
    public partial class JCAssertion : Form
    {
        Boolean Annuler = false;
        JCAVariable mesArguments = new JCAssertionCore.JCAVariable();
        JCAConsole maConsole = new JCAssertionCore.JCAConsole();
        JCACore monJCACore = new JCACore();
        String Usage = "usage :" + Environment.NewLine + Environment.NewLine + "JCAssertion /FA:fichierassertion /fv:fichierdevariables";
        public string[] args = new string[0];
        public Boolean Interactif = false ;
        public String  Message = "";
        int NombreReussi = 0;
        int NombreEchec = 0;
        int NombreCas = 0;
        String  JournalActivite = null;




        private void Informer(String Texte, Boolean Severe = false )
        {
            AjouteActivite(Texte);
            if (Severe)
                {
                    Message = Environment.NewLine + Texte;
                    if (Interactif) System.Windows.Forms.MessageBox.Show(Texte);
                      
                }
        }

        // Methpde pour ajouter du texte dans l'activté
        
        
        private void AjouteActivite(String Texte )
        {
            tbxActivite.Text = tbxActivite.Text + Environment.NewLine + Texte;
            if (JournalActivite != null )
            {
               StreamWriter fileJournal =  File.AppendText(JournalActivite  );
               fileJournal.WriteLine(Environment.NewLine + Texte );
               fileJournal.Close();
            }        
        }

        public int ExecuteAssertion()
        {
            try {
                return Execute();
                } catch (Exception excep)
                {
                    Message = "ERREUR==>" + excep.GetType() +
                        Environment.NewLine   + excep.Message;
                    Console.WriteLine("Debug : " + Message ); 
                    AjouteActivite(Message);
                    throw excep;
                }
        }

        // Methode utilisé  par le load et qui peutêtreunittestée
        public int Execute()
        {
            NombreCas = 0;
            NombreEchec = 0;
            NombreReussi = 0;

            Message = "Démarrage";
            mesArguments = maConsole.Arguments(args);
            
            // Vérifier qu'au moins le nom de fichier d'assertion est fourni
            
            if ((mesArguments.GetValeurVariable("FA") == null) || (mesArguments.GetValeurVariable("FA") == ""))
                    {
                        Informer("Ce programme doit recevoir des arguments enligne de commande." + Usage, true) ;
                        return 99;
                   }
            // Valider un peu les arguments
            String FichierAssertion = mesArguments.GetValeurVariable("FA");
            String FichierVariable = "";
            if ((mesArguments.GetValeurVariable("FV") != null) &&
                (mesArguments.GetValeurVariable("FV") != "")) 
                    FichierVariable = mesArguments.GetValeurVariable("FV");
            
            if (!System.IO.File.Exists(FichierAssertion))
            {
                Informer( "Le fichier d'assertion . " +
                    FichierAssertion + " n'existe pas." , true );
                return 99;
            }

           if((FichierVariable != "" ) &&
               (! System.IO.File.Exists (FichierVariable)))
           {
                Informer  ("Le fichier de variables . " +
                    FichierVariable + " n'existe pas.", true );
                return 99;
            }

           if ((mesArguments.GetValeurVariable("J") != null) &&
               (mesArguments.GetValeurVariable("J") != ""))
               // Désactiver la journalisation  du core
               // Le^programme va faire un  journal plus complet
               monJCACore.Journaliser = false ;
               JournalActivite =    mesArguments.GetValeurVariable("J");
               if (System.IO.File.Exists(JournalActivite))
                   System.IO.File.Delete(JournalActivite);   

            //
            // commencer le traitementproprement dit
            tbxFAssertion.Text = FichierAssertion;
            tbxFVariables.Text = FichierVariable ;
            Informer("Lecture du fichier d'assertion : " 
                + FichierAssertion );
            monJCACore.Load(FichierAssertion );
            NombreCas = monJCACore.NombreCas;
            Informer("Nombre de cas à traiter : " + monJCACore.NombreCas.ToString () );
            if(FichierVariable != "")
            {
                Informer ("Lecture du fichier de variables : "
                + FichierVariable);
                monJCACore.Variables.LireFichier(FichierVariable );
            }

            int i = 1;
            foreach (XmlNode monCas in monJCACore.getListeDeCas())
                {
                    Informer ("Exécution du cas " + i.ToString() );
                    if (monJCACore.ExecuteCas(monCas))
                        {
                            NombreReussi = NombreReussi + 1;
                            Informer ("Assertion vraie") ;
                            Informer (monJCACore.Message);
                        }
                    else
                        {
                            Informer ("Assertion fausse");
                            Informer (monJCACore.Message);
                            NombreEchec = NombreEchec + 1;
                        }
                    i = i++;
                }
            Informer("Fin de l'exécution");
            Informer("Cas réussis : " + NombreReussi.ToString() + " sur " + NombreCas.ToString()  );
            Informer("Cas en échec : " + NombreEchec.ToString() + " sur " + NombreCas.ToString());





                return 0;
        }


        public JCAssertion()
        {
            InitializeComponent();
        }

        private void JCAssertion_Load(object sender, EventArgs e)
        {
            
            try {
                int Resultat = ExecuteAssertion();
                if (Resultat != 0)
                    {
                        Console.WriteLine(Message );
                        AjouteActivite(Message );
                    }
                Console.WriteLine("Cas réussis : " + NombreReussi.ToString() +
                    " sur " + NombreCas.ToString() + " et " + NombreEchec.ToString() + " échecs."  );
                if ((NombreEchec > 0) && (!Interactif)) Environment.Exit(1);
                if (!Interactif) Environment.Exit(Resultat );
             } catch (Exception excep)
                 {
                     Console.WriteLine(excep.Message );
                    if(! Interactif ) 
                        Environment.Exit(99);
                }

            // end run
            


        }

        private void tbxActivite_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
