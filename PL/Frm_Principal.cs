using System;
using System.Drawing;
using System.Windows.Forms;

namespace MercadoSeuZe.PL
{
    public class Frm_Principal : Form
    {
        private Button btnCliente, btnPesquisa, btnProduto, btnVenda, btnHistorico;

        public Frm_Principal()
        {
            this.Text = "Mercado SeuZe";
            this.Width = 440;
            this.Height = 610;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(248, 249, 252);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Cabeçalho colorido
            var pnlHeader = new Panel
            {
                Left = 0, Top = 0, Width = 440, Height = 100,
                BackColor = Color.FromArgb(34, 139, 87)
            };

            var lblIcone = new Label
            {
                Text = "🛒",
                Font = new Font("Segoe UI", 22),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft,
                Top = 18, Left = 24, Width = 50, Height = 40
            };

            var lblTitulo = new Label
            {
                Text = "Mercado SeuZe",
                Font = new Font("Segoe UI", 17, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft,
                Top = 18, Left = 72, Width = 280, Height = 38
            };

            var lblSubtitulo = new Label
            {
                Text = "Painel de controle",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(180, 220, 195),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft,
                Top = 58, Left = 72, Width = 280, Height = 22
            };

            pnlHeader.Controls.AddRange(new Control[] { lblIcone, lblTitulo, lblSubtitulo });

            // Seção "O que deseja fazer?"
            var lblSecao = new Label
            {
                Text = "O que deseja fazer?",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(80, 90, 110),
                Top = 118, Left = 24, Width = 380, Height = 22
            };

            // Botões com ícone à esquerda e subtítulo
            btnCliente = CriarBotaoMenu("👤", "Cadastrar Cliente", "Adicionar ou editar clientes",
                154, Color.FromArgb(70, 130, 210));
            btnCliente.Click += (s, e) => new Frm_Cliente().ShowDialog();

            btnPesquisa = CriarBotaoMenu("🔍", "Listar / Excluir Clientes", "Pesquisar e remover clientes",
                224, Color.FromArgb(100, 149, 237));
            btnPesquisa.Click += (s, e) => new Frm_PesquisaCliente().ShowDialog();

            btnProduto = CriarBotaoMenu("📦", "Produtos", "Cadastrar e gerenciar produtos",
                294, Color.FromArgb(230, 145, 30));
            btnProduto.Click += (s, e) => new Frm_Produto().ShowDialog();

            btnVenda = CriarBotaoMenu("💰", "Nova Venda", "Registrar uma nova venda",
                364, Color.FromArgb(34, 139, 87));
            btnVenda.Click += (s, e) => new Frm_Venda().ShowDialog();

            btnHistorico = CriarBotaoMenu("📋", "Histórico de Vendas", "Visualizar todas as vendas realizadas",
                434, Color.FromArgb(90, 60, 160));
            btnHistorico.Click += (s, e) => new Frm_HistoricoVenda().ShowDialog();

            var sep = new Panel
            {
                Top = 520, Left = 24, Width = 370, Height = 1,
                BackColor = Color.FromArgb(225, 228, 235)
            };

            var lblRodape = new Label
            {
                Text = "Mercado SeuZe © 2026",
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.FromArgb(190, 195, 210),
                TextAlign = ContentAlignment.MiddleCenter,
                Top = 528, Left = 0, Width = 420, Height = 20
            };

            Controls.AddRange(new Control[] {
                pnlHeader, lblSecao,
                btnCliente, btnPesquisa, btnProduto, btnVenda, btnHistorico,
                sep, lblRodape
            });
        }

        private Button CriarBotaoMenu(string icone, string titulo, string subtitulo,
                                       int top, Color corAcento)
        {
            // Container do botão como Panel para suportar sub-labels
            var btn = new Button
            {
                Top = top,
                Left = 24,
                Width = 372,
                Height = 60,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(40, 50, 65),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand,
                Text = $"  {icone}   {titulo}"
            };
            btn.FlatAppearance.BorderColor = Color.FromArgb(220, 225, 235);
            btn.FlatAppearance.BorderSize = 1;

            // Barra colorida lateral via Paint
            btn.Paint += (s, e) =>
            {
                e.Graphics.FillRectangle(new SolidBrush(corAcento), 0, 0, 5, btn.Height);

                // Subtítulo
                var fmt = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Far };
                e.Graphics.DrawString(subtitulo,
                    new Font("Segoe UI", 8), new SolidBrush(Color.FromArgb(150, 160, 175)),
                    new RectangleF(52, 0, btn.Width - 60, btn.Height - 8), fmt);

                // Seta direita
                var fmtArrow = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center };
                e.Graphics.DrawString("›",
                    new Font("Segoe UI", 16), new SolidBrush(Color.FromArgb(corAcento.R, corAcento.G, corAcento.B)),
                    new RectangleF(0, 0, btn.Width - 12, btn.Height), fmtArrow);
            };

            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = Color.FromArgb(248, 250, 255);
                btn.FlatAppearance.BorderColor = corAcento;
            };
            btn.MouseLeave += (s, e) =>
            {
                btn.BackColor = Color.White;
                btn.FlatAppearance.BorderColor = Color.FromArgb(220, 225, 235);
            };

            new ToolTip().SetToolTip(btn, subtitulo);
            return btn;
        }
    }
}
