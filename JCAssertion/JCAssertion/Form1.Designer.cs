namespace JCAssertion
{
    partial class JCAssertion
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        
        public string gettxbActivite()
        {
            return tbxActivite.Text  ;
        }


        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.tbxFAssertion = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbxFVariables = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbxActivite = new System.Windows.Forms.TextBox();
            this.btnAnnuler = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AccessibleName = "Fichier des assertions";
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Fichier d\'assertion";
            // 
            // tbxFAssertion
            // 
            this.tbxFAssertion.Location = new System.Drawing.Point(128, 4);
            this.tbxFAssertion.Name = "tbxFAssertion";
            this.tbxFAssertion.ReadOnly = true;
            this.tbxFAssertion.Size = new System.Drawing.Size(417, 20);
            this.tbxFAssertion.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AccessibleName = "Fichier de variables";
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Fichier de variables";
            // 
            // label3
            // 
            this.label3.AccessibleName = "Fichier de variables";
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Fichier de variables";
            // 
            // label4
            // 
            this.label4.AccessibleName = "Fichier de variables";
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Fichier de variables";
            // 
            // tbxFVariables
            // 
            this.tbxFVariables.Location = new System.Drawing.Point(128, 31);
            this.tbxFVariables.Name = "tbxFVariables";
            this.tbxFVariables.ReadOnly = true;
            this.tbxFVariables.Size = new System.Drawing.Size(423, 20);
            this.tbxFVariables.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Activité";
            // 
            // tbxActivite
            // 
            this.tbxActivite.Location = new System.Drawing.Point(11, 84);
            this.tbxActivite.Multiline = true;
            this.tbxActivite.Name = "tbxActivite";
            this.tbxActivite.Size = new System.Drawing.Size(534, 123);
            this.tbxActivite.TabIndex = 6;
            this.tbxActivite.Text = "Démarrage";
            this.tbxActivite.TextChanged += new System.EventHandler(this.tbxActivite_TextChanged);
            // 
            // btnAnnuler
            // 
            this.btnAnnuler.Location = new System.Drawing.Point(17, 230);
            this.btnAnnuler.Name = "btnAnnuler";
            this.btnAnnuler.Size = new System.Drawing.Size(75, 27);
            this.btnAnnuler.TabIndex = 7;
            this.btnAnnuler.Text = "Annuler";
            this.btnAnnuler.UseVisualStyleBackColor = true;
            this.btnAnnuler.Click += new System.EventHandler(this.btnAnnuler_Click);
            // 
            // JCAssertion
            // 
            this.AccessibleName = "JC Assertion";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 398);
            this.Controls.Add(this.btnAnnuler);
            this.Controls.Add(this.tbxActivite);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbxFVariables);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbxFAssertion);
            this.Controls.Add(this.label1);
            this.Name = "JCAssertion";
            this.Text = "JC Assertion";
            this.Load += new System.EventHandler(this.JCAssertion_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbxFAssertion;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbxFVariables;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbxActivite;
        private System.Windows.Forms.Button btnAnnuler;
    }
}

