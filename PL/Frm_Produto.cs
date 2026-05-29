using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using MercadoSeuZe.BLL;
using MercadoSeuZe.DAL;
using MercadoSeuZe.Modelo;

namespace MercadoSeuZe.PL
{
    public class Frm_Produto : Form
    {
        private TextBox txtNome, txtDesc, txtPreco, txtEstoque, txtCategoria, txtPesquisa;
        private Button btnSalvar, btnNovo, btnExcluir;
        private DataGridView grid;
        private Label lblStatus;
        private int idEditando = 0;

        public Frm_Produto()
        {
            this.Text = "Produtos";
            this.Width = 860;
            this.Height = 600;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 245, 245);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Título
            var lblTitulo = new Label
            {
                Text = "📦  Gerenciar Produtos",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 40, 40),
                Top = 15, Left = 20, Width = 500, Height = 30
            };

            var sep1 = new Panel
            {
                Top = 52, Left = 20, Width = 800, Height = 1,
                BackColor = Color.LightGray
            };

            // --- Campos de cadastro ---
            int y = 65;

            // Linha 1: Nome | Categoria
            var lblNome = CriarLabel("Nome: *", y);
            lblNome.Left = 20; lblNome.Width = 90;
            txtNome = CriarTextBox(y, 310);
            txtNome.Left = 115;

            var lblCat = CriarLabel("Categoria:", y);
            lblCat.Left = 445; lblCat.Width = 90;
            txtCategoria = CriarTextBox(y, 270);
            txtCategoria.Left = 540;

            // Linha 2: Descrição
            y += 36;
            var lblDesc = CriarLabel("Descrição:", y);
            lblDesc.Left = 20; lblDesc.Width = 90;
            txtDesc = CriarTextBox(y, 695);
            txtDesc.Left = 115;

            // Linha 3: Preço | Estoque
            y += 36;
            var lblPreco = CriarLabel("Preço (R$): *", y);
            lblPreco.Left = 20; lblPreco.Width = 105;
            txtPreco = CriarTextBox(y, 120);
            txtPreco.Left = 130;

            var lblEstoque = CriarLabel("Estoque: *", y);
            lblEstoque.Left = 275; lblEstoque.Width = 85;
            txtEstoque = CriarTextBox(y, 100);
            txtEstoque.Left = 365;

            y += 44;

            // Botões de ação
            btnNovo = CriarBotao("➕  Novo", y, 20, 110,
                Color.FromArgb(200, 200, 200), Color.FromArgb(60, 60, 60));
            btnNovo.Click += (s, e) => Limpar();

            btnSalvar = CriarBotao("💾  Salvar", y, 140, 160,
                Color.FromArgb(70, 130, 180), Color.White);
            btnSalvar.Click += btnSalvar_Click;

            btnExcluir = CriarBotao("🗑  Excluir", y, 310, 140,
                Color.FromArgb(205, 92, 92), Color.White);
            btnExcluir.Click += btnExcluir_Click;

            lblStatus = new Label
            {
                Text = "", Top = y + 6, Left = 470, Width = 360, Height = 24,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                ForeColor = Color.Green
            };

            // --- Separador + pesquisa ---
            var sep2 = new Panel
            {
                Top = y + 48, Left = 20, Width = 800, Height = 1,
                BackColor = Color.LightGray
            };

            var lblPesq = new Label
            {
                Text = "Pesquisar:",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(80, 80, 80),
                Top = y + 58, Left = 20, Width = 80, Height = 20
            };

            txtPesquisa = new TextBox
            {
                Top = y + 55, Left = 105, Width = 715,
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle
            };
            txtPesquisa.TextChanged += (s, e) => Pesquisar();

            // Grid
            grid = new DataGridView
            {
                Top = y + 88, Left = 20, Width = 800, Height = 220,
                AllowUserToAddRows = false,
                ReadOnly = true,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                GridColor = Color.FromArgb(230, 230, 230),
                RowHeadersVisible = false,
                Font = new Font("Segoe UI", 9),
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
            };
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 165, 0);
            grid.DefaultCellStyle.SelectionForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(255, 165, 0);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            grid.EnableHeadersVisualStyles = false;
            grid.RowTemplate.Height = 28;
            grid.CellClick += grid_CellClick;

            var lblDica = new Label
            {
                Text = "💡 Clique em uma linha para editar",
                Top = y + 314, Left = 20, Width = 300, Height = 18,
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.Gray
            };

            Controls.AddRange(new Control[] {
                lblTitulo, sep1,
                lblNome, txtNome, lblCat, txtCategoria,
                lblDesc, txtDesc,
                lblPreco, txtPreco, lblEstoque, txtEstoque,
                btnNovo, btnSalvar, btnExcluir, lblStatus,
                sep2, lblPesq, txtPesquisa, grid, lblDica
            });

            this.Load += (s, e) => Carregar();
        }

        private Label CriarLabel(string texto, int top) =>
            new Label
            {
                Text = texto,
                Top = top + 2, Left = 20, Width = 100,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(60, 60, 60)
            };

        private TextBox CriarTextBox(int top, int largura) =>
            new TextBox
            {
                Top = top, Left = 110, Width = largura,
                Font = new Font("Segoe UI", 9),
                BorderStyle = BorderStyle.FixedSingle
            };

        private Button CriarBotao(string texto, int top, int left, int width,
                                   Color corFundo, Color corTexto)
        {
            var btn = new Button
            {
                Text = texto, Top = top, Left = left,
                Width = width, Height = 36,
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

        private void EstilizarGrid()
        {
            foreach (DataGridViewRow row in grid.Rows)
                row.DefaultCellStyle.BackColor =
                    row.Index % 2 == 0 ? Color.White : Color.FromArgb(255, 250, 240);
        }

        private void Carregar()
        {
            try { new ProdutoDao().Mostrar(grid); EstilizarGrid(); }
            catch (Exception ex) { MessageBox.Show("Erro: " + ex.Message); }
        }

        private void Pesquisar()
        {
            try { new ProdutoDao().Pesquisar(txtPesquisa.Text, grid); EstilizarGrid(); }
            catch (Exception ex) { MessageBox.Show("Erro: " + ex.Message); }
        }

        private void Limpar()
        {
            idEditando = 0;
            txtNome.Clear(); txtDesc.Clear(); txtPreco.Clear();
            txtEstoque.Clear(); txtCategoria.Clear();
            lblStatus.Text = "";
            btnSalvar.Text = "💾  Salvar";
            txtNome.Focus();
        }

        private void btnSalvar_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!decimal.TryParse(txtPreco.Text.Replace(',', '.'),
                        NumberStyles.Any, CultureInfo.InvariantCulture, out decimal preco))
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "⚠ Preço inválido.";
                    return;
                }

                if (!int.TryParse(txtEstoque.Text, out int estoque))
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "⚠ Estoque inválido.";
                    return;
                }

                var produto = new Produto
                {
                    Id = idEditando,
                    Nome = txtNome.Text.Trim(),
                    Descricao = txtDesc.Text.Trim(),
                    Preco = preco,
                    Estoque = estoque,
                    Categoria = txtCategoria.Text.Trim()
                };

                var erro = new ProdutoBLL().Validar(produto);
                if (erro != null)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "⚠ " + erro;
                    return;
                }

                var dao = new ProdutoDao();
                if (idEditando == 0) dao.Inserir(produto);
                else dao.Atualizar(produto);

                lblStatus.ForeColor = Color.Green;
                lblStatus.Text = idEditando == 0
                    ? $"✔ Produto \"{produto.Nome}\" cadastrado!"
                    : $"✔ Produto \"{produto.Nome}\" atualizado!";

                Limpar();
                Carregar();
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Erro: " + ex.Message;
            }
        }

        private void btnExcluir_Click(object? sender, EventArgs e)
        {
            if (idEditando == 0)
            {
                lblStatus.ForeColor = Color.OrangeRed;
                lblStatus.Text = "⚠ Selecione um produto na lista.";
                return;
            }

            string nome = txtNome.Text;
            if (MessageBox.Show(
                    $"Excluir \"{nome}\"?\n\nItens de venda vinculados também serão removidos.",
                    "Confirmar exclusão",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning) != DialogResult.Yes) return;

            try
            {
                new ProdutoDao().Excluir(idEditando);
                lblStatus.ForeColor = Color.Green;
                lblStatus.Text = $"✔ Produto \"{nome}\" excluído.";
                Limpar();
                Carregar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void grid_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            try
            {
                var row = grid.Rows[e.RowIndex];
                idEditando = Convert.ToInt32(row.Cells["id"].Value);
                txtNome.Text = row.Cells["nome"].Value?.ToString() ?? "";
                txtDesc.Text = row.Cells["descricao"].Value?.ToString() ?? "";
                txtPreco.Text = Convert.ToDecimal(row.Cells["preco"].Value)
                    .ToString(CultureInfo.InvariantCulture);
                txtEstoque.Text = row.Cells["estoque"].Value?.ToString() ?? "0";
                txtCategoria.Text = row.Cells["categoria"].Value?.ToString() ?? "";
                btnSalvar.Text = "💾  Atualizar";
                lblStatus.Text = $"✏ Editando: {txtNome.Text}";
                lblStatus.ForeColor = Color.FromArgb(70, 130, 180);
            }
            catch { }
        }
    }
}