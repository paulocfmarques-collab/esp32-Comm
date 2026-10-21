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
using System.Collections.Concurrent;

namespace CommEspe32
{
    public partial class Form1 : Form
    {
        IPAddress Servidor;
        int Porta;
        bool bContinue = false;
        Thread t;
        UdpClient udp;
        private ConcurrentQueue<string> filaRespostas = new ConcurrentQueue<string>();
        private AutoResetEvent respostaRecebida = new AutoResetEvent(false);

        private bool alterado;
        
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
                    udp.Client.ReceiveTimeout = 5000;


                    bContinue = true;
                    btnListen.Image = Properties.Resources.X;
                    tbIP.Enabled = false;
                    tbPorta.Enabled = false;
                    btnEnvia.Enabled = true;
                    tbComando.Enabled = true;
                    rtbResposta.Enabled = true;
                    lbComandos.Enabled = true;
                    t = new Thread(ReceiveThread);
                    t.Start();

                    FillLbComandos();
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
                lbComandos.Enabled = false;
                btnListen.Image = Properties.Resources.Check;
                StopListen();
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

            while (bContinue)
            {
                try
                {
                    byte[] message = udp.Receive(ref remoto);

                    string texto = Encoding.UTF8.GetString(message);

                    filaRespostas.Enqueue(texto);

                    respostaRecebida.Set();

                    rtbResposta.Invoke((MethodInvoker)(() =>
                    {
                        rtbResposta.AppendText(texto);

                        if (!texto.EndsWith("\n"))
                            rtbResposta.AppendText(Environment.NewLine);

                        rtbResposta.SelectionStart = rtbResposta.Text.Length;
                        rtbResposta.ScrollToCaret();
                    }));
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode == SocketError.TimedOut)
                        continue;

                    break;
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
            }
        }
        
        private void btnEnvia_Click(object sender, EventArgs e)
        {
            if(lbComandos.SelectedIndex != -1)
            {
                tbComando.Text = lbComandos.SelectedItem.ToString();
            }

            sendCommand(tbComando.Text);
        
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopListen();
        }

        private void tbComando_TextChanged(object sender, EventArgs e)
        {
            alterado = true;
        }

        private void tbComando_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter && alterado)
            {
                alterado = false;
                btnEnvia.PerformClick();
            }
        }

        private void tbComando_Enter(object sender, EventArgs e)
        {
            lbComandos.ClearSelected();
        }

        private void sendCommand(string sCommand)
        {
            IPEndPoint destino = new IPEndPoint(Servidor, Porta);
            byte[] buffer = Encoding.ASCII.GetBytes(sCommand);
            udp.Send(buffer, buffer.Length, destino);
        }

        private bool ReceiveAnswer(out string resposta)
        {
            resposta = string.Empty;

            if (!respostaRecebida.WaitOne(5000))
            {
                return false;
            }

            return filaRespostas.TryDequeue(out resposta);
        }

        private void FillLbComandos()
        {
            ClearQueue();

            sendCommand("help");

            string resposta;

            if (ReceiveAnswer(out resposta))
            {
                lbComandos.Items.Clear();

                string[] linhas = resposta.Split(
                    new string[] { "\r\n", "\n" },
                    StringSplitOptions.None);

                for (int i = 1; i < linhas.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(linhas[i]))
                    {
                        lbComandos.Items.Add(linhas[i]);
                    }
                }

                rtbResposta.Clear();
            }
            else
            {
                MessageBox.Show(
                    "O equipamento não respondeu.",
                    "Timeout",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void ClearQueue()
        {
            string lixo;

            while (filaRespostas.TryDequeue(out lixo))
            {
            }
        }

        private void StopListen()
        {
            bContinue = false;

            if (udp != null)
            {
                udp.Close();
                udp = null;
            }

            if (t != null && t.IsAlive)
            {
                t.Join(1000);
            }
        }

    }
}
