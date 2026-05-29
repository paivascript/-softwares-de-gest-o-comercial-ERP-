using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using MercadoSeuZe.DAL;

namespace MercadoSeuZe.PL
{
    public class Frm_HistoricoVenda : Form
    {
        private TextBox txtPesquisa;
        private DataGridView gridVendas, gridItens;
        private Label lblTotalVendas, lblTotalGeral, lblVendaSelecionada;

        public Frm_HistoricoVenda()
        {
            this.Text = "Histórico de Vendas";
            this.Width = 960;
            this.Height = 640;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(248, 249, 252);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // ── Cabeçalho ───────────────────────────────────────────────
            var pnlHeader = new Panel
            {
                Left = 0, Top = 0, Width = 960, Height = 68,
                BackColor = Color.FromArgb(90, 60, 160)   // roxo/indigo
            };

            var lblH = new Label
            {
                Text = "📋  Histórico de Vendas",
                Font = new Font("Segoe UI", 15, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Top = 16, Left = 20, Width = 500, Height = 36,
                TextAlign = ContentAlignment.MiddleLeft
            };
            pnlHeader.Controls.Add(lblH);

            // ── Barra de pesquisa ────────────────────────────────────────
            var pnlSearch = CriarCard(82, 20, 900, 48);

            var lblPesq = new Label
            {
                Text = "Pesquisar:",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(80, 90, 110),
                Top = 14, Left = 14, Width = 80, Height = 20,
                BackColor = Color.Transparent
            };

            txtPesquisa = new TextBox
            {
                Top = 10, Left = 100, Width = 785,
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.None,
                BackColor = Color.White
            };
            txtPesquisa.TextChanged += (s, e) => Pesquisar();

            pnlSearch.Controls.AddRange(new Control[] { lblPesq, txtPesquisa });

            // ── Layout de duas colunas ───────────────────────────────────
            // Coluna esquerda: lista de vendas
            var lblVendas = new Label
            {
                Text = "Vendas",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(80, 90, 110),
                Top = 144, Left = 20, Width = 200, Height = 20
            };

            var pnlGridVendas = CriarCard(166, 20, 560, 360);

            gridVendas = new DataGridView
            {
                Top = 0, Left = 0, Width = 560, Height = 360,
                AllowUserToAddRows = false,
                ReadOnly = true,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                GridColor = Color.FromArgb(230, 235, 245),
                RowHeadersVisible = false,
                Font = new Font("Segoe UI", 9),
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
            };
            gridVendas.DefaultCellStyle.SelectionBackColor = Color.FromArgb(90, 60, 160);
            gridVendas.DefaultCellStyle.SelectionForeColor = Color.White;
            gridVendas.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(90, 60, 160);
            gridVendas.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            gridVendas.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            gridVendas.ColumnHeadersDefaultCellStyle.Padding = new Padding(4, 0, 0, 0);
            gridVendas.EnableHeadersVisualStyles = false;
            gridVendas.RowTemplate.Height = 30;
            gridVendas.SelectionChanged += gridVendas_SelectionChanged;

            pnlGridVendas.Controls.Add(gridVendas);

            // Coluna direita: itens da venda selecionada
            lblVendaSelecionada = new Label
            {
                Text = "Itens da venda",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(80, 90, 110),
                Top = 144, Left = 596, Width = 330, Height = 20
            };

            var pnlGridItens = CriarCard(166, 596, 344, 360);

            gridItens = new DataGridView
            {
                Top = 0, Left = 0, Width = 344, Height = 360,
                AllowUserToAddRows = false,
                ReadOnly = true,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                GridColor = Color.FromArgb(230, 235, 245),
                RowHeadersVisible = false,
                Font = new Font("Segoe UI", 9),
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
            };
            gridItens.DefaultCellStyle.SelectionBackColor = Color.FromArgb(34, 139, 87);
            gridItens.DefaultCellStyle.SelectionForeColor = Color.White;
            gridItens.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(34, 139, 87);
            gridItens.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            gridItens.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            gridItens.ColumnHeadersDefaultCellStyle.Padding = new Padding(4, 0, 0, 0);
            gridItens.EnableHeadersVisualStyles = false;
            gridItens.RowTemplate.Height = 30;

            pnlGridItens.Controls.Add(gridItens);

            // ── Rodapé com totalizadores ─────────────────────────────────
            var pnlFooter = CriarCard(538, 20, 920, 54);
            pnlFooter.BackColor = Color.FromArgb(248, 248, 252);

            var lblQtdLabel = new Label
            {
                Text = "Total de vendas:",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(100, 110, 130),
                Top = 16, Left = 16, Width = 110, Height = 20,
                BackColor = Color.Transparent
            };

            lblTotalVendas = new Label
            {
                Text = "0",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(90, 60, 160),
                Top = 13, Left = 130, Width = 60, Height = 26,
                BackColor = Color.Transparent
            };

            var sep = new Panel
            {
                Top = 10, Left = 210, Width = 1, Height = 34,
                BackColor = Color.FromArgb(215, 220, 235)
            };

            var lblGeralLabel = new Label
            {
                Text = "Receita total:",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(100, 110, 130),
                Top = 16, Left = 224, Width = 100, Height = 20,
                BackColor = Color.Transparent
            };

            lblTotalGeral = new Label
            {
                Text = "R$ 0,00",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(34, 139, 87),
                Top = 12, Left = 326, Width = 200, Height = 30,
                BackColor = Color.Transparent
            };

            var lblDica = new Label
            {
                Text = "💡  Clique em uma venda para ver os itens",
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.FromArgb(160, 170, 190),
                Top = 18, Left = 580, Width = 320, Height = 18,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleRight
            };

            pnlFooter.Controls.AddRange(new Control[] {
                lblQtdLabel, lblTotalVendas, sep,
                lblGeralLabel, lblTotalGeral, lblDica
            });

            Controls.AddRange(new Control[] {
                pnlHeader, pnlSearch,
                lblVendas, pnlGridVendas,
                lblVendaSelecionada, pnlGridItens,
                pnlFooter
            });

            this.Load += (s, e) => Carregar();
        }

        // ── Helpers ──────────────────────────────────────────────────────

        private Panel CriarCard(int top, int left, int width, int height)
        {
            var p = new Panel
            {
                Top = top, Left = left, Width = width, Height = height,
                BackColor = Color.White
            };
            p.Paint += (s, e) =>
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(220, 225, 235), 1),
                    0, 0, p.Width - 1, p.Height - 1);
            return p;
        }

        private void EstilizarGrid(DataGridView grid, Color par)
        {
            foreach (DataGridViewRow row in grid.Rows)
                row.DefaultCellStyle.BackColor =
                    row.Index % 2 == 0 ? Color.White : par;
        }

        // ── Dados ─────────────────────────────────────────────────────────

        private void Carregar()
        {
            try
            {
                new VendaDao().Mostrar(gridVendas);
                EstilizarGrid(gridVendas, Color.FromArgb(248, 246, 255));
                AtualizarTotais();
                gridItens.DataSource = null;
                lblVendaSelecionada.Text = "Itens da venda";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar vendas: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Pesquisar()
        {
            try
            {
                new VendaDao().Pesquisar(txtPesquisa.Text, gridVendas);
                EstilizarGrid(gridVendas, Color.FromArgb(248, 246, 255));
                AtualizarTotais();
                gridItens.DataSource = null;
                lblVendaSelecionada.Text = "Itens da venda";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro na pesquisa: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AtualizarTotais()
        {
            int qtd = gridVendas.Rows.Count;
            lblTotalVendas.Text = qtd.ToString();

            decimal soma = 0;
            foreach (DataGridViewRow row in gridVendas.Rows)
            {
                if (row.Cells["valorTotal"].Value != DBNull.Value)
                    soma += Convert.ToDecimal(row.Cells["valorTotal"].Value);
            }
            lblTotalGeral.Text = soma.ToString("C", CultureInfo.GetCultureInfo("pt-BR"));
        }

        private void gridVendas_SelectionChanged(object? sender, EventArgs e)
        {
            if (gridVendas.CurrentRow == null) return;
            try
            {
                int idVenda = Convert.ToInt32(gridVendas.CurrentRow.Cells["id"].Value);
                string cliente = gridVendas.CurrentRow.Cells["cliente"].Value?.ToString() ?? "";
                lblVendaSelecionada.Text = $"Itens — Venda #{idVenda} ({cliente})";

                DataTable itens = new VendaDao().ListarItens(idVenda);
                gridItens.DataSource = itens;
                EstilizarGrid(gridItens, Color.FromArgb(242, 250, 245));
            }
            catch { }
        }
    }
}
