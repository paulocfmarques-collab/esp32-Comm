using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommEspe32
{
    public partial class Form1 : Form
    {
        IPAddress Servidor;
        int Porta;
        bool bContinue = false;
        Thread t;
        UdpClient udp;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            if (!bContinue)
            {
                if (tbIP.Text == "" || tbPorta.Text == "")
                {
                    MessageBox.Show("IP / Porta devem estar preenchidos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    string ip = tbIP.Text.Trim();

                    if (!IPAddress.TryParse(ip, out Servidor))
                    {
                        MessageBox.Show("Digite um endeço IP válido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        tbIP.Focus();
                        return;
                    }

                    Porta = Int32.Parse(tbPorta.Text);

                    udp = new UdpClient(Porta);
                    udp.Client.ReceiveBufferSize = 19000;

                    bContinue = true;
                    btnListen.Image = Properties.Resources.X;
                    tbIP.Enabled = false;
                    tbPorta.Enabled = false;
                    btnEnvia.Enabled = true;
                    tbComando.Enabled = true;
                    rtbResposta.Enabled = true;
                    t = new Thread(ReceiveThread);
                    t.Start();
                }
            }
            else
            {
                bContinue = false;
                tbIP.Enabled = true;
                tbPorta.Enabled = true;
                btnEnvia.Enabled = false;
                tbComando.Enabled = false;
                rtbResposta.Enabled = false;
                btnListen.Image = Properties.Resources.Check;
                if(t.IsAlive)
                {
                    t.Abort();
                }
            }
        }

        private void tbPorta_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            txt.Text = System.Text.RegularExpressions.Regex.Replace(txt.Text, "[^0-9]", "");
            txt.SelectionStart = txt.Text.Length;
        }

        private void tbIP_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbIP_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) &&
               !char.IsDigit(e.KeyChar) &&
                e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        void ReceiveThread()
        {
            IPEndPoint remoto = new IPEndPoint(IPAddress.Any, 0);

            rtbResposta.Invoke((MethodInvoker)delegate
            {
                rtbResposta.Clear();
            });

            while (bContinue)
            {
                try
                {
                    byte[] message = udp.Receive(ref remoto);

                    rtbResposta.Invoke((MethodInvoker)delegate
                    {
                        rtbResposta.AppendText(Encoding.UTF8.GetString(message) + Environment.NewLine);
                        rtbResposta.SelectionStart = rtbResposta.Text.Length;
                        rtbResposta.ScrollToCaret();
                    });
                }
                catch
                {
                    break;
                }
            }
        }

        private void btnEnvia_Click(object sender, EventArgs e)
        {
            IPEndPoint destino = new IPEndPoint(Servidor, Porta);
            byte[] buffer = Encoding.ASCII.GetBytes(tbComando.Text);
            udp.Send(buffer, buffer.Length, destino);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (udp != null)
            {
                udp.Close();
            }
        }
    }
}
