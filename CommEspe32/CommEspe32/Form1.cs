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
        private volatile bool bContinue = false;
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

        private async void btnListen_Click(object sender, EventArgs e)
        {
            if (!bContinue)
            {
                if (string.IsNullOrWhiteSpace(tbIP.Text) ||
                    string.IsNullOrWhiteSpace(tbPorta.Text))
                {
                    MessageBox.Show(
                        "IP / Porta devem estar preenchidos.",
                        "Erro",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }

                string ip = tbIP.Text.Trim();

                if (!IPAddress.TryParse(ip, out Servidor))
                {
                    MessageBox.Show(
                        "Digite um endereço IP válido.",
                        "Erro",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    tbIP.Focus();
                    return;
                }

                if (!Int32.TryParse(tbPorta.Text, out Porta) ||
                    Porta < 1 ||
                    Porta > 65535)
                {
                    MessageBox.Show(
                        "Digite uma porta válida entre 1 e 65535.",
                        "Erro",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    tbPorta.Focus();
                    return;
                }

                try
                {
                    ClearQueue();

                    udp = new UdpClient(Porta);
                    udp.Client.ReceiveBufferSize = 19000;
                    udp.Client.ReceiveTimeout = 1000;

                    bContinue = true;

                    btnListen.Image = Properties.Resources.X;
                    tbIP.Enabled = false;
                    tbPorta.Enabled = false;
                    btnEnvia.Enabled = true;
                    tbComando.Enabled = true;
                    rtbResposta.Enabled = true;
                    lbComandos.Enabled = true;

                    t = new Thread(ReceiveThread);
                    t.IsBackground = true;
                    t.Start();

                    await FillLbComandosAsync();
                }
                catch (SocketException ex)
                {
                    StopListen();

                    MessageBox.Show(
                        "Não foi possível abrir a porta UDP.\r\n\r\n" +
                        ex.Message,
                        "Erro UDP",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    AtualizarInterfaceParada();
                }
            }
            else
            {
                StopListen();
                AtualizarInterfaceParada();
            }
        }

        private void btnEnvia_Click(object sender, EventArgs e)
        {
            if (lbComandos.SelectedIndex != -1)
            {
                tbComando.Text = lbComandos.SelectedItem.ToString();
            }

            if (string.IsNullOrWhiteSpace(tbComando.Text))
            {
                MessageBox.Show(
                    "Digite ou selecione um comando.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                tbComando.Focus();
                return;
            }

            sendCommand(tbComando.Text);
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

        private void ReceiveThread()
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

                    if (!rtbResposta.IsDisposed && rtbResposta.IsHandleCreated)
                    {
                        rtbResposta.BeginInvoke((MethodInvoker)delegate
                        {
                            rtbResposta.AppendText(texto);

                            if (!texto.EndsWith("\n"))
                            {
                                rtbResposta.AppendText(Environment.NewLine);
                            }

                            rtbResposta.SelectionStart = rtbResposta.Text.Length;
                            rtbResposta.ScrollToCaret();
                        });
                    }
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode == SocketError.TimedOut)
                    {
                        continue;
                    }

                    break;
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (InvalidOperationException)
                {
                    break;
                }
            }
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
            if (udp == null)
            {
                MessageBox.Show(
                    "A comunicação UDP não está iniciada.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (string.IsNullOrWhiteSpace(sCommand))
            {
                return;
            }

            IPEndPoint destino = new IPEndPoint(Servidor, Porta);
            byte[] buffer = Encoding.ASCII.GetBytes(sCommand.Trim());

            udp.Send(buffer, buffer.Length, destino);
        }

        private bool ReceiveAnswer(out string resposta)
        {
            resposta = string.Empty;

            DateTime limite = DateTime.Now.AddSeconds(5);

            while (DateTime.Now < limite)
            {
                if (filaRespostas.TryDequeue(out resposta))
                {
                    return true;
                }

                int tempoRestante = (int)(limite - DateTime.Now).TotalMilliseconds;

                if (tempoRestante <= 0)
                {
                    break;
                }

                respostaRecebida.WaitOne(tempoRestante);
            }

            return filaRespostas.TryDequeue(out resposta);
        }

        private async Task FillLbComandosAsync()
        {
            ClearQueue();

            rtbResposta.Clear();
            lbComandos.Items.Clear();

            sendCommand("help");

            string resposta = string.Empty;

            bool recebeu = await Task.Run(delegate
            {
                return ReceiveAnswer(out resposta);
            });

            if (!recebeu)
            {
                MessageBox.Show(
                    "O equipamento não respondeu em 5 segundos.",
                    "Timeout",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            string[] linhas = resposta.Split(
                new string[] { "\r\n", "\n" },
                StringSplitOptions.None);

            for (int i = 1; i < linhas.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(linhas[i]))
                {
                    lbComandos.Items.Add(linhas[i].Trim());
                }
            }

            rtbResposta.Clear();
        }

        private void ClearQueue()
        {
            string lixo;

            while (filaRespostas.TryDequeue(out lixo))
            {
            }

            respostaRecebida.Reset();
        }

        private void StopListen()
        {
            bContinue = false;

            UdpClient udpAtual = udp;
            udp = null;

            if (udpAtual != null)
            {
                try
                {
                    udpAtual.Close();
                }
                catch
                {
                    // O socket já pode ter sido fechado.
                }
            }

            if (t != null && t.IsAlive)
            {
                t.Join(1500);
            }

            t = null;

            ClearQueue();
        }

        private void AtualizarInterfaceParada()
        {
            tbIP.Enabled = true;
            tbPorta.Enabled = true;

            btnEnvia.Enabled = false;
            tbComando.Enabled = false;
            rtbResposta.Enabled = false;
            lbComandos.Enabled = false;

            btnListen.Image = Properties.Resources.Check;
        }
    }
}
