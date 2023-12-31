﻿using GerenciadorDeEstoque.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GerenciadorDeEstoque.Apresentação
{
    public partial class frmAdicionarProdutos : Form
    {
        DataTable dt = new DataTable();
        VendeVO vende;
        long idVenda;

        bool novaVenda = true;
        bool editarVenda = false;

        public frmAdicionarProdutos(long idVenda, VendeVO vende)
        {
            InitializeComponent();
            this.idVenda = idVenda;
            this.vende = vende;
            
            novaVenda = false;
            InicializarChecked(this.idVenda);
        }

        public frmAdicionarProdutos(VendeVO vende)
        {
            InitializeComponent();

            this.vende = vende;

            Inicializar();
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {

            DialogResult dialogResult = MessageBox.Show("Tem certeza que gostaria de Voltar? (todas as informações não salvas serão perdidas)", "Voltar", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                if (novaVenda || editarVenda) { this.Close(); }
                else
                {
                    List<Int64> idProduto = new List<Int64>();
                    List<Int64> quantidadeErrado = new List<Int64>();
                    List<Int32> quantidade = new List<Int32>();

                    try
                    {

                        Dictionary<String, List<long>> idProduto_Quantidade;

                        idProduto_Quantidade = DAO.DAO.GetVendeId(idVenda);

                        idProduto = idProduto_Quantidade["idProduto"];
                        quantidadeErrado = idProduto_Quantidade["quantidade"];

                        foreach(Int64 l in quantidadeErrado) 
                        {
                            quantidade.Add(Convert.ToInt32(l));
                        }

                        if (idProduto.Count <= 0)
                        {
                            throw new ArgumentNullException("Ao menos um item precisa compor uma venda!");
                        }

                        vende.IdProdutoLista = idProduto;
                        vende.QuantidadeLista = quantidade;


                        this.Close();
                    }
                    catch (ArgumentNullException ex)
                    {
                        MessageBox.Show("Você não escolheu nenhum produto para vender!");
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show(ex.Message);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "erro");
                    }
                }
            }
        }

        private void InicializarChecked(long idVenda)
        {
            dt = DAO.DAO.GetProduto(true);
            Dictionary<String, List<long>> idProduto_Quantidade;
            List<long> idProduto;
            List<long> quantidade;

            idProduto_Quantidade = DAO.DAO.GetVendeId(idVenda);

            if (idProduto_Quantidade != null)
            {
                idProduto = idProduto_Quantidade["idProduto"];
                quantidade = idProduto_Quantidade["quantidade"];

                int i = 0;
                foreach (DataRow row in dt.Rows)
                {
                    if (idProduto.Contains(Convert.ToInt64(row["id"])))
                    {
                        row["escolha"] = true;
                        row["quantidade"] = quantidade[i];

                        i++;
                    }
                }
            }

            dgvAdicionarProdutoKrypton.DataSource = dt;
            ConfigurarGradeProduto();
        }

        private void Inicializar()
        {
            dt = DAO.DAO.GetProduto(true);
            dgvAdicionarProdutoKrypton.DataSource = dt;
            ConfigurarGradeProduto();
        }

        private void ConfigurarGradeProduto()
        {
            dgvAdicionarProdutoKrypton.DefaultCellStyle.Font = new Font("Segoe UI Emoji", 20, FontStyle.Bold);
            dgvAdicionarProdutoKrypton.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Emoji", 15, FontStyle.Bold);
            dgvAdicionarProdutoKrypton.RowHeadersWidth = 20;
            dgvAdicionarProdutoKrypton.RowTemplate.Height = 40;

            dgvAdicionarProdutoKrypton.Columns["id"].HeaderText = "ID";
            dgvAdicionarProdutoKrypton.Columns["id"].Visible = true;
            dgvAdicionarProdutoKrypton.Columns["id"].ReadOnly = true;

            dgvAdicionarProdutoKrypton.Columns["escolha"].HeaderText = "Escolha";
            dgvAdicionarProdutoKrypton.Columns["escolha"].Width = 100;
            dgvAdicionarProdutoKrypton.Columns["escolha"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvAdicionarProdutoKrypton.Columns["escolha"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvAdicionarProdutoKrypton.Columns["escolha"].ReadOnly = false;

            dgvAdicionarProdutoKrypton.Columns["nome"].HeaderText = "Nome";
            dgvAdicionarProdutoKrypton.Columns["nome"].Width = 300;
            dgvAdicionarProdutoKrypton.Columns["nome"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvAdicionarProdutoKrypton.Columns["nome"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvAdicionarProdutoKrypton.Columns["nome"].ReadOnly = true;

            dgvAdicionarProdutoKrypton.Columns["valor"].HeaderText = "Valor";
            dgvAdicionarProdutoKrypton.Columns["valor"].Width = 150;
            dgvAdicionarProdutoKrypton.Columns["valor"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvAdicionarProdutoKrypton.Columns["valor"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvAdicionarProdutoKrypton.Columns["valor"].ReadOnly = true;

            dgvAdicionarProdutoKrypton.Columns["quantidade"].HeaderText = "Quantidade";
            dgvAdicionarProdutoKrypton.Columns["quantidade"].Width = 100;
            dgvAdicionarProdutoKrypton.Columns["quantidade"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvAdicionarProdutoKrypton.Columns["quantidade"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvAdicionarProdutoKrypton.Columns["quantidade"].ReadOnly = false;

        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            List<Int64> idProduto = new List<Int64>();
            List<Int32> quantidade = new List<Int32>();

            try
            {

                foreach (DataGridViewRow row in dgvAdicionarProdutoKrypton.Rows)
                {
                    if (row.Cells["escolha"].Value.ToString() == "True")
                    {
                        if (row.Cells["quantidade"].Value != DBNull.Value && Convert.ToInt32(row.Cells["quantidade"].Value) > 0)
                        {
                            idProduto.Add(Convert.ToInt64(row.Cells["id"].Value));
                            quantidade.Add(Convert.ToInt32(row.Cells["quantidade"].Value));
                        }
                        else
                        {
                            throw new ArgumentException("O produto " + row.Cells["nome"].Value + " está sem quantidade ou com uma quantidade negativo");
                        }

                    }
                }

                if (idProduto.Count <= 0)
                {
                    throw new ArgumentNullException("Ao menos um item precisa compor uma venda!");
                }

                vende.IdProdutoLista = idProduto;
                vende.QuantidadeLista = quantidade;

                editarVenda = true;

                MessageBox.Show("Materiais adicionados ao produto com Sucesso");

                this.Close();
            }
            catch(ArgumentNullException ex)
            {
                MessageBox.Show("Você não escolheu nenhum produto para vender!");
            }
            catch(ArgumentException ex)
            {
                MessageBox.Show(ex.Message);

            }
            catch (Exception ex)
            {
                idProduto = null;
                quantidade = null;

                MessageBox.Show(ex.Message, "erro");
            }
        }

        private void frmAdicionarProdutos_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void frmAdicionarProdutos_FormClosed(object sender, FormClosedEventArgs e)
        {
            
                if (novaVenda || editarVenda ) { this.Close(); }
                else
                {
                    List<Int64> idProduto = new List<Int64>();
                    List<Int64> quantidadeErrado = new List<Int64>();
                    List<Int32> quantidade = new List<Int32>();

                    try
                    {

                        Dictionary<String, List<long>> idProduto_Quantidade;

                        idProduto_Quantidade = DAO.DAO.GetVendeId(idVenda);

                        idProduto = idProduto_Quantidade["idProduto"];
                        quantidadeErrado = idProduto_Quantidade["quantidade"];

                        foreach (Int64 l in quantidadeErrado)
                        {
                            quantidade.Add(Convert.ToInt32(l));
                        }

                        if (idProduto.Count <= 0)
                        {
                            throw new ArgumentNullException("Ao menos um item precisa compor uma venda!");
                        }

                        vende.IdProdutoLista = idProduto;
                        vende.QuantidadeLista = quantidade;


                        this.Close();
                    }
                    catch (ArgumentNullException ex)
                    {
                        MessageBox.Show("Você não escolheu nenhum produto para vender!");
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show(ex.Message);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "erro");
                    }
                }
            
        }
    }
}
