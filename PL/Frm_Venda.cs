using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using MercadoSeuZe.BLL;
using MercadoSeuZe.DAL;
using MercadoSeuZe.Modelo;

namespace MercadoSeuZe.PL
{
    public class Frm_Venda : Form
    {
        private ComboBox cmbCliente, cmbProduto;
        private TextBox txtQuantidade;
        private Button btnAdicionar, btnRemover, btnFinalizar;
        private DataGridView gridItens;
        private Label lblTotalValor, lblStatus;

        private readonly Venda venda = new Venda();

        public Frm_Venda()
        {
            this.Text = "Nova Venda";
            this.Width = 900;
            this.Height = 610;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(248, 249, 252);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // ── Cabeçalho ──────────────────────────────────────────────
            var pnlHeader = new Panel
            {
                Left = 0, Top = 0, Width = 900, Height = 68,
                BackColor = Color.FromArgb(34, 139, 87)
            };

            var lblH = new Label
            {
                Text = "💰  Nova Venda",
                Font = new Font("Segoe UI", 15, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Top = 16, Left = 20, Width = 400, Height = 36,
                TextAlign = ContentAlignment.MiddleLeft
            };
            pnlHeader.Controls.Add(lblH);

            // ── Card: Seleção de cliente ────────────────────────────────
            var pnlCli = CriarCard(84, 20, 840, 58);

            var lblCli = CriarLabelSecao("👤  Cliente");
            lblCli.Top = 18; lblCli.Left = 14; lblCli.Width = 90;

            cmbCliente = new ComboBox
            {
                Top = 14, Left = 112, Width = 700,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(250, 251, 253)
            };

            pnlCli.Controls.AddRange(new Control[] { lblCli, cmbCliente });

            // ── Card: Adicionar produto ─────────────────────────────────
            var pnlProd = CriarCard(152, 20, 840, 58);

            var lblProd = CriarLabelSecao("📦  Produto");
            lblProd.Top = 18; lblProd.Left = 14; lblProd.Width = 90;

            cmbProduto = new ComboBox
            {
                Top = 14, Left = 112, Width = 460,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(250, 251, 253)
            };

            var lblQtd = CriarLabelSecao("Qtd:");
            lblQtd.Top = 18; lblQtd.Left = 584; lblQtd.Width = 36;

            txtQuantidade = new TextBox
            {
                Top = 14, Left = 622, Width = 62,
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(250, 251, 253),
                Text = "1",
                TextAlign = HorizontalAlignment.Center
            };

            btnAdicionar = CriarBotao("➕  Adicionar", 12, 694, 130,
                Color.FromArgb(70, 130, 210), Color.White);
            btnAdicionar.Height = 34;
            btnAdicionar.Click += btnAdicionar_Click;

            pnlProd.Controls.AddRange(new Control[] { lblProd, cmbProduto, lblQtd, txtQuantidade, btnAdicionar });

            // ── Card: Grid de itens ─────────────────────────────────────
            var pnlGridContainer = CriarCard(220, 20, 840, 280);

            var lblItens = new Label
            {
                Text = "Itens da venda",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(80, 90, 110),
                Top = 10, Left = 14, Width = 200, Height = 20,
                BackColor = Color.Transparent
            };

            gridItens = new DataGridView
            {
                Top = 36, Left = 10, Width = 816, Height = 230,
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

            pnlGridContainer.Controls.AddRange(new Control[] { lblItens, gridItens });

            // ── Rodapé de ações ──────────────────────────────────────────
            var pnlFooter = CriarCard(510, 20, 840, 56);

            btnRemover = CriarBotao("🗑  Remover item", 10, 14, 160,
                Color.FromArgb(205, 80, 80), Color.White);
            btnRemover.Height = 36;
            btnRemover.Click += btnRemover_Click;

            lblStatus = new Label
            {
                Text = "",
                Top = 18, Left = 188, Width = 360, Height = 22,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                ForeColor = Color.OrangeRed,
                BackColor = Color.Transparent
            };

            var lblTotalLabel = new Label
            {
                Text = "TOTAL:",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 70, 90),
                Top = 14, Left = 550, Width = 80, Height = 28,
                BackColor = Color.Transparent
            };

            lblTotalValor = new Label
            {
                Text = "R$ 0,00",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(34, 139, 87),
                Top = 13, Left = 630, Width = 150, Height = 30,
                BackColor = Color.Transparent
            };

            btnFinalizar = CriarBotao("✔  Finalizar Venda", 10, 688, 140,
                Color.FromArgb(34, 139, 87), Color.White);
            btnFinalizar.Height = 36;
            btnFinalizar.Click += btnFinalizar_Click;

            pnlFooter.Controls.AddRange(new Control[] {
                btnRemover, lblStatus, lblTotalLabel, lblTotalValor, btnFinalizar
            });

            Controls.AddRange(new Control[] {
                pnlHeader, pnlCli, pnlProd, pnlGridContainer, pnlFooter
            });

            this.Load += (s, e) => Carregar();
        }

        // ── Helpers de UI ────────────────────────────────────────────────

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

        private Label CriarLabelSecao(string texto) =>
            new Label
            {
                Text = texto,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(80, 90, 110),
                BackColor = Color.Transparent,
                Height = 20
            };

        private Button CriarBotao(string texto, int top, int left, int width,
                                   Color corFundo, Color corTexto)
        {
            var btn = new Button
            {
                Text = texto, Top = top, Left = left,
                Width = width, Height = 32,
                BackColor = corFundo, ForeColor = corTexto,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.MouseEnter += (s, e) => btn.BackColor = ControlPaint.Dark(corFundo, 0.1f);
            btn.MouseLeave += (s, e) => btn.BackColor = corFundo;
            return btn;
        }

        // ── Lógica (inalterada) ───────────────────────────────────────────

        private void Carregar()
        {
            try
            {
                var clientes = new ClienteDao().Listar();
                cmbCliente.DataSource = clientes;
                cmbCliente.DisplayMember = "Nome";
                cmbCliente.ValueMember = "Id";

                var produtos = new ProdutoDao().Listar();
                cmbProduto.DataSource = produtos;
                cmbProduto.DisplayMember = "Nome";
                cmbProduto.ValueMember = "Id";

                AtualizarGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdicionar_Click(object? sender, EventArgs e)
        {
            if (cmbProduto.SelectedItem is not Produto prod)
            {
                MostrarStatus("⚠ Selecione um produto.");
                return;
            }

            if (!int.TryParse(txtQuantidade.Text, out int qtd) || qtd <= 0)
            {
                MostrarStatus("⚠ Quantidade inválida.");
                return;
            }

            int jaNoCarrinho = venda.Itens
                .Where(i => i.IdProduto == prod.Id).Sum(i => i.Quantidade);

            if (qtd + jaNoCarrinho > prod.Estoque)
            {
                MostrarStatus($"⚠ Estoque insuficiente. Disponível: {prod.Estoque - jaNoCarrinho}");
                return;
            }

            var existente = venda.Itens.FirstOrDefault(i => i.IdProduto == prod.Id);
            if (existente != null)
                existente.Quantidade += qtd;
            else
                venda.Itens.Add(new ItemVenda
                {
                    IdProduto = prod.Id,
                    NomeProduto = prod.Nome,
                    Quantidade = qtd,
                    PrecoUnitario = prod.Preco
                });

            txtQuantidade.Text = "1";
            lblStatus.Text = "";
            AtualizarGrid();
        }

        private void btnRemover_Click(object? sender, EventArgs e)
        {
            if (gridItens.CurrentRow == null) return;
            int idx = gridItens.CurrentRow.Index;
            if (idx >= 0 && idx < venda.Itens.Count)
            {
                venda.Itens.RemoveAt(idx);
                AtualizarGrid();
            }
        }

        private void AtualizarGrid()
        {
            var dados = venda.Itens.Select(i => new
            {
                Produto = i.NomeProduto,
                Qtd = i.Quantidade,
                PrecoUnit = i.PrecoUnitario.ToString("C", CultureInfo.GetCultureInfo("pt-BR")),
                Subtotal = i.Subtotal.ToString("C", CultureInfo.GetCultureInfo("pt-BR"))
            }).ToList();

            gridItens.DataSource = dados;

            foreach (DataGridViewRow row in gridItens.Rows)
                row.DefaultCellStyle.BackColor =
                    row.Index % 2 == 0 ? Color.White : Color.FromArgb(242, 250, 245);

            decimal total = venda.Itens.Sum(i => i.Subtotal);
            venda.ValorTotal = total;
            lblTotalValor.Text = total.ToString("C", CultureInfo.GetCultureInfo("pt-BR"));
        }

        private void btnFinalizar_Click(object? sender, EventArgs e)
        {
            if (cmbCliente.SelectedItem is not Cliente cli)
            {
                MostrarStatus("⚠ Selecione um cliente.");
                return;
            }

            venda.IdCliente = cli.Id;
            venda.IdUsuario = SessaoUsuario.IdUsuario;

            var erro = new VendaBLL().Validar(venda);
            if (erro != null) { MostrarStatus("⚠ " + erro); return; }

            try
            {
                int idVenda = new VendaDao().Registrar(venda);
                MessageBox.Show(
                    $"✔ Venda #{idVenda} registrada com sucesso!\n\nTotal: " +
                    venda.ValorTotal.ToString("C", CultureInfo.GetCultureInfo("pt-BR")),
                    "Venda concluída",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao finalizar venda: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarStatus(string msg)
        {
            lblStatus.ForeColor = Color.OrangeRed;
            lblStatus.Text = msg;
        }
    }
}
