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


namespace JCAssertion
{
    public partial class JCAssertion : Form
    {
        Boolean Annuler = false;
        JCAVariable mesArguments = new JCAssertionCore.JCAVariable();
        JCAConsole maConsole = new JCAssertionCore.JCAConsole();
        String Usage = "usage :" + Environment.NewLine + Environment.NewLine + "JCAssertion /FA:fichierassertion /fv:fichierdevariables";
        public string[] args = new string[0];
        public Boolean Interactif = true;
        public String  Message = "";

        // Methpde pour ajouter du texte dans l'activté
        private void AjouteActivite(String Texte )
        {
            tbxActivite.Text = tbxActivite.Text + Environment.NewLine + Texte;
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
               
            return 0;
        }


        public JCAssertion()
        {
            InitializeComponent();
        }

        private void JCAssertion_Load(object sender, EventArgs e)
        {
            String Message = "";

            // c.est ici que ca se âsse
            try {
                
                
                mesArguments.EcrireFichier("d:\\Devcenter\\debug.txt");

                //System.Windows.Forms.MessageBox.Show("Ce programme doit recevoir des arugments enligne de commande." + Usage);
                   


                    
             } catch (Exception excep)
                 {
                     throw excep;

                }

            // end run
            


        }
    }
}
