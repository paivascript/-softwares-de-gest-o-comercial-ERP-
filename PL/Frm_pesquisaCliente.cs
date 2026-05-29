using System;
using System.Drawing;
using System.Windows.Forms;
using MercadoSeuZe.DAL;

namespace MercadoSeuZe.PL
{
    public class Frm_PesquisaCliente : Form
    {
        private TextBox txtPesquisa;
        private DataGridView dGridView;
        private Label lblStatus;

        public Frm_PesquisaCliente()
        {
            this.Text = "Listar / Excluir Clientes";
            this.Width = 820;
            this.Height = 560;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(248, 249, 252);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Cabeçalho
            var pnlHeader = new Panel
            {
                Left = 0, Top = 0, Width = 820, Height = 70,
                BackColor = Color.FromArgb(70, 130, 210)
            };

            var lblIconeH = new Label
            {
                Text = "🔍",
                Font = new Font("Segoe UI", 18),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Top = 14, Left = 20, Width = 40, Height = 38,
                TextAlign = ContentAlignment.MiddleLeft
            };

            var lblTituloH = new Label
            {
                Text = "Clientes Cadastrados",
                Font = new Font("Segoe UI", 15, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Top = 14, Left = 64, Width = 400, Height = 38,
                TextAlign = ContentAlignment.MiddleLeft
            };

            pnlHeader.Controls.AddRange(new Control[] { lblIconeH, lblTituloH });

            // Painel de busca
            var pnlSearch = new Panel
            {
                Left = 20, Top = 88, Width = 760, Height = 48,
                BackColor = Color.White
            };
            pnlSearch.Paint += (s, e) =>
            {
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(220, 225, 235), 1),
                    0, 0, pnlSearch.Width - 1, pnlSearch.Height - 1);
            };

            var lblIconePesq = new Label
            {
                Text = "🔍",
                Font = new Font("Segoe UI", 11),
                Top = 10, Left = 10, Width = 30, Height = 28,
                BackColor = Color.Transparent,
                ForeColor = Color.FromArgb(150, 160, 180)
            };

            var lblPesquisa = new Label
            {
                Text = "Pesquisar:",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(80, 90, 110),
                Top = 14, Left = 14, Width = 80, Height = 20,
                BackColor = Color.Transparent
            };

            txtPesquisa = new TextBox
            {
                Top = 10, Left = 100, Width = 645,
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.None,
                BackColor = Color.White
            };
            txtPesquisa.TextChanged += txtPesquisa_TextChanged;

            pnlSearch.Controls.AddRange(new Control[] { lblPesquisa, txtPesquisa });

            // Grid
            dGridView = new DataGridView
            {
                Top = 148, Left = 20, Width = 760, Height = 320,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                GridColor = Color.FromArgb(230, 235, 245),
                RowHeadersVisible = false,
                Font = new Font("Segoe UI", 9),
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
            };
            dGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(70, 130, 210);
            dGridView.DefaultCellStyle.SelectionForeColor = Color.White;
            dGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(70, 130, 210);
            dGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dGridView.ColumnHeadersDefaultCellStyle.Padding = new Padding(4, 0, 0, 0);
            dGridView.EnableHeadersVisualStyles = false;
            dGridView.RowTemplate.Height = 30;
            dGridView.CellClick += dGridView_CellClick;

            // Rodapé
            var pnlFooter = new Panel
            {
                Left = 20, Top = 478, Width = 760, Height = 38,
                BackColor = Color.White
            };
            pnlFooter.Paint += (s, e) =>
            {
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(220, 225, 235), 1),
                    0, 0, pnlFooter.Width - 1, pnlFooter.Height - 1);
            };

            var lblDica = new Label
            {
                Text = "💡  Clique em uma linha para excluir o cliente",
                Top = 9, Left = 12, Width = 380, Height = 20,
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.FromArgb(150, 160, 180),
                BackColor = Color.Transparent
            };

            lblStatus = new Label
            {
                Text = "",
                Top = 9, Left = 12, Width = 730, Height = 20,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                ForeColor = Color.FromArgb(34, 139, 87),
                BackColor = Color.Transparent
            };

            pnlFooter.Controls.AddRange(new Control[] { lblDica, lblStatus });

            Controls.AddRange(new Control[] {
                pnlHeader, pnlSearch, dGridView, pnlFooter
            });

            this.Load += Frm_PesquisaCliente_Load;
        }

        private void EstilizarGrid()
        {
            foreach (DataGridViewRow row in dGridView.Rows)
            {
                row.DefaultCellStyle.BackColor =
                    row.Index % 2 == 0 ? Color.White : Color.FromArgb(246, 249, 255);
            }
        }

        private void Frm_PesquisaCliente_Load(object sender, EventArgs e)
        {
            try
            {
                new ClienteDao().Mostrar(dGridView);
                EstilizarGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar clientes: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtPesquisa_TextChanged(object sender, EventArgs e)
        {
            try
            {
                new ClienteDao().Pesquisar(txtPesquisa.Text, dGridView);
                EstilizarGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro na pesquisa: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string nome = dGridView.Rows[e.RowIndex].Cells["nome"].Value?.ToString() ?? "este cliente";

            var confirm = MessageBox.Show(
                $"Excluir \"{nome}\"?\n\nTodas as vendas vinculadas também serão removidas.",
                "Confirmar exclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                int id = Convert.ToInt32(dGridView.Rows[e.RowIndex].Cells["id"].Value);
                var dao = new ClienteDao();
                dao.Excluir(id);
                dao.Mostrar(dGridView);
                EstilizarGrid();

                lblStatus.ForeColor = Color.FromArgb(34, 139, 87);
                lblStatus.Text = $"✔  Cliente \"{nome}\" excluído com sucesso.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao excluir: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
