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
        public Boolean Interactif = true;
        public String  Message = "";

        // Methpde pour ajouter du texte dans l'activté
        private void AjouteActivite(String Texte )
        {
            tbxActivite.Text = tbxActivite.Text + Environment.NewLine + Texte;
        }

        public int ExecuteAssertion()
        {
            try {
                return Execute();
                } catch (Exception excep)
                {
                    Message = excep.Message ;
                    AjouteActivite(Message);
                    throw excep;
                }
        }

        // Methode utilisé  par le load et qui peutêtreunittestée
        public int Execute()
        {
            Message = "Démarrage";
            mesArguments = maConsole.Arguments(args);
            if ((mesArguments.GetValeurVariable("FA") == null) || (mesArguments.GetValeurVariable("FA") == ""))
                    {
                        Message = "Ce programme doit recevoir des arguments enligne de commande." + Usage;
                        AjouteActivite(Message);        
                         if(Interactif  ) System.Windows.Forms.MessageBox.Show(Message);
                        return 99;
                    }
            // Au moins le nom de fichier d'assertion est fourni
            // Valider un peu les arguments
            String FichierAssertion = mesArguments.GetValeurVariable("FA");
            String FichierVariable = "";
            if ((mesArguments.GetValeurVariable("FV") != null) &&
                (mesArguments.GetValeurVariable("FV") != "")) FichierVariable = mesArguments.GetValeurVariable("FV");
            
            if (!System.IO.File.Exists(FichierAssertion))
            {
                Message = "Le fichier d'assertion . " +
                    FichierAssertion + " n'existe pas.";
                AjouteActivite(Message);        
                        
                if (Interactif) System.Windows.Forms.MessageBox.Show(Message);
                return 99;
            }
           if((FichierVariable != "" ) &&
               (System.IO.File.Exists (FichierVariable)))
           {
                Message = "Le fichier de variables . " +
                    FichierVariable + " n'existe pas.";
                if (Interactif) System.Windows.Forms.MessageBox.Show(Message);
                return 99;
            } 
            //
            // commencer le traitementproprement dit
            tbxFAssertion.Text = FichierAssertion;
            tbxFVariables.Text = FichierVariable ;
            AjouteActivite("Lecture du fichier d'assertion : " 
                + FichierAssertion );
            monJCACore.Load(FichierAssertion ); 
            if(FichierVariable != "")
            {
                AjouteActivite("Lecture du fichier de variables : "
                + FichierVariable);
                monJCACore.Variables.LireFichier(FichierVariable );
            }

            int i = 1;
            foreach (XmlNode monCas in monJCACore.getListeDeCas())
                {
                    AjouteActivite("Exécution di cas " + i.ToString() );
                    if (monJCACore.ExecuteCas(monCas))
                        AjouteActivite("Assertion vraie") ;
                    else AjouteActivite("Assertion fausse");
                    i = i++;
                }



                return 0;
        }


        public JCAssertion()
        {
            InitializeComponent();
        }

        private void JCAssertion_Load(object sender, EventArgs e)
        {
            
            try {
                ExecuteAssertion();
                    
             } catch (Exception excep)
                 {
                     Console.WriteLine(excep.Message );
                    if(! Interactif ) 
                        Environment.Exit(99);
                }

            // end run
            


        }
    }
}
