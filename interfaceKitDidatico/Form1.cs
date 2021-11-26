﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace interfaceKitDidatico
{
    public partial class PaginaInicial : Form
    {
       
        int contador=0;
        int tamanho_palavra = 10;
        bool alerta_aberto = false;
        public static byte[] buffer = new byte[10];
        public PaginaInicial()
        {
            InitializeComponent();
            //serialPort1.Open();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                serialPort1.Write(buffer, 0, tamanho_palavra);
            }
            catch
            {
                if (!alerta_aberto)
                {
                    alerta_aberto = true;
                    DialogResult result = MessageBox.Show("Erro: Porta desconectada. Conecte novamente a placa e clique em 'OK'.");
                   if (result == DialogResult.OK)
                    {
                        try
                        {
                            serialPort1.Open();
                        }
                        catch
                        {
                            MessageBox.Show("Erro: Não foi possível reconectar a placa. Tente novamente.");
                        }
                        alerta_aberto = false;
                    }
                }
            }
        }

        private void serialPort1_ErrorReceived(object sender, System.IO.Ports.SerialErrorReceivedEventArgs e)
        {
            //MessageBox.Show("Error");
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen){
                serialPort1.Close();
            }
            
            serialPort1.PortName = comboBox1.Text;
            try 
            {
                serialPort1.Open();
            }
            catch (IOException error)
            {
                MessageBox.Show("Erro: Seleção de uma porta serial que não está sendo utilizada. Confira se a placa está conectada ao computador, entre no 'Gerenciado de Dispositivos' e veja em 'Portas (COM e LPT)' qual porta está sendo utilizada na comunicação com a placa.");
            } 
        }

        private void iniciarVerificacao_Click(object sender, EventArgs e)
        {
            textBox9.Text = "Verificando";
            int i;
            for (i=0; i <= 10; i++)
            {
                try
                {
                    comunicacao();
                }
                catch (TimeoutException error)
                {
                    MessageBox.Show("Erro: Seleção da porta serial errada. Confira se a placa está conectada ao computador, entre no 'Gerenciado de Dispositivos' e veja em 'Portas (COM e LPT)' qual porta está sendo utilizada na comunicação com a placa.");
                    break;
                }
            }
           
        }
        
        public void comunicacao()
        {
            //Teste de Comunicacao
            int i;
            for (i = 0; i < tamanho_palavra; i++)
            {
                buffer[i] = 0x41;
            }

            serialPort1.Write(buffer, 0, tamanho_palavra);

            string[] texto = new string[tamanho_palavra];
            for (i = 0; i < tamanho_palavra; i++)
            {
                texto[i] = buffer[i].ToString();
            }

            textBox4.Text = texto[0] + texto[1] + texto[2] + texto[3] + texto[4] + texto[5] + texto[6] + texto[7] + texto[8];

            byte[] buffer2 = new byte[tamanho_palavra];
            serialPort1.Read(buffer2, 0, tamanho_palavra);

            string[] texto2 = new string[tamanho_palavra];
            for (i = 0; i < tamanho_palavra; i++)
            {
                texto2[i] = buffer[i].ToString();
            }

            textBox5.Text = texto2[0] + texto2[1] + texto2[2] + texto2[3] + texto2[4] + texto2[5] + texto2[6] + texto2[7] + texto2[8];

            //repete 10x e diz que foi bem sucedido
            contador++;
            textBox6.Text = contador.ToString();
            if (contador > tamanho_palavra)
            {
                textBox9.Text = "Verificacao com sucesso";
                timer1.Enabled = true;
                tela1 tela = new tela1(this,tamanho_palavra);
                Enabled = false;
                tela.Show();
                contador = 0;
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void PaginaInicial_Load(object sender, EventArgs e)
        {

        }
    }
}
