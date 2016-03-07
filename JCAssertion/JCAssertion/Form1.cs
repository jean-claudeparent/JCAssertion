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


namespace JCAssertion
{
    public partial class JCAssertion : Form
    {
        Boolean Annuler = false;
        JCAVariable mesArguments = new JCAssertionCore.JCAVariable();
        JCAConsole maConsole = new JCAssertionCore.JCAConsole();
        String Usage = "usage :" + Environment.NewLine + Environment.NewLine + "JCAssertion /FA:fichierassertion /fv:fichierdevariables";
        public string[] args = new string[0];
        public Boolean Popup = true;
        public String  Message = "";


        // Methode utilisé  par le load et qui peutêtreunittestée
        public int Execute()
        {
            Message = "Démarrage";
            mesArguments = maConsole.Arguments(args);
            if ((mesArguments.GetValeurVariable("FA") == null) || (mesArguments.GetValeurVariable("FA") == ""))
                    {
                        Message = "Ce programme doit recevoir des arguments enligne de commande." + Usage;
                        if(Popup ) System.Windows.Forms.MessageBox.Show(Message);
                        return 99;
                  
                    }
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
