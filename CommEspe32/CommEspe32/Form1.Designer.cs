namespace CommEspe32
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbIP = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbPorta = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbComando = new System.Windows.Forms.TextBox();
            this.btnEnvia = new System.Windows.Forms.Button();
            this.rtbResposta = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnListen = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbIP
            // 
            this.tbIP.Location = new System.Drawing.Point(52, 6);
            this.tbIP.Name = "tbIP";
            this.tbIP.Size = new System.Drawing.Size(183, 20);
            this.tbIP.TabIndex = 0;
            this.tbIP.TextChanged += new System.EventHandler(this.tbIP_TextChanged);
            this.tbIP.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbIP_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "IP:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Porta: ";
            // 
            // tbPorta
            // 
            this.tbPorta.Location = new System.Drawing.Point(52, 35);
            this.tbPorta.Name = "tbPorta";
            this.tbPorta.Size = new System.Drawing.Size(64, 20);
            this.tbPorta.TabIndex = 3;
            this.tbPorta.TextChanged += new System.EventHandler(this.tbPorta_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(264, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Comando:";
            // 
            // tbComando
            // 
            this.tbComando.Enabled = false;
            this.tbComando.Location = new System.Drawing.Point(267, 29);
            this.tbComando.Name = "tbComando";
            this.tbComando.Size = new System.Drawing.Size(392, 20);
            this.tbComando.TabIndex = 6;
            // 
            // btnEnvia
            // 
            this.btnEnvia.Enabled = false;
            this.btnEnvia.Location = new System.Drawing.Point(665, 28);
            this.btnEnvia.Name = "btnEnvia";
            this.btnEnvia.Size = new System.Drawing.Size(75, 23);
            this.btnEnvia.TabIndex = 7;
            this.btnEnvia.Text = "Envia";
            this.btnEnvia.UseVisualStyleBackColor = true;
            this.btnEnvia.Click += new System.EventHandler(this.btnEnvia_Click);
            // 
            // rtbResposta
            // 
            this.rtbResposta.Enabled = false;
            this.rtbResposta.Location = new System.Drawing.Point(12, 114);
            this.rtbResposta.Name = "rtbResposta";
            this.rtbResposta.ReadOnly = true;
            this.rtbResposta.Size = new System.Drawing.Size(728, 177);
            this.rtbResposta.TabIndex = 8;
            this.rtbResposta.Text = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Resposta:";
            // 
            // btnListen
            // 
            this.btnListen.Image = global::CommEspe32.Properties.Resources.Check;
            this.btnListen.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnListen.Location = new System.Drawing.Point(122, 33);
            this.btnListen.Name = "btnListen";
            this.btnListen.Size = new System.Drawing.Size(75, 23);
            this.btnListen.TabIndex = 4;
            this.btnListen.Text = "Listen";
            this.btnListen.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnListen.UseVisualStyleBackColor = true;
            this.btnListen.Click += new System.EventHandler(this.btnListen_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 303);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.rtbResposta);
            this.Controls.Add(this.btnEnvia);
            this.Controls.Add(this.tbComando);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnListen);
            this.Controls.Add(this.tbPorta);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbIP);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "Esp32 Comm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbIP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbPorta;
        private System.Windows.Forms.Button btnListen;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbComando;
        private System.Windows.Forms.Button btnEnvia;
        private System.Windows.Forms.RichTextBox rtbResposta;
        private System.Windows.Forms.Label label4;
    }
}

