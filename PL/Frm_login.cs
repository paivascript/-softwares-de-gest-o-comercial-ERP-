using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MercadoSeuZe.DAL;
using MercadoSeuZe.Modelo;

namespace MercadoSeuZe.PL
{
    public class Frm_Login : Form
    {
        private TextBox txtLogin, txtSenha;
        private Button btnEntrar;
        private CheckBox chkMostrarSenha;

        public Frm_Login()
        {
            this.Text = "Mercado SeuZe";
            this.Width = 420;
            this.Height = 500;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(248, 249, 252);

            // Painel de cabeçalho verde
            var pnlHeader = new Panel
            {
                Left = 0, Top = 0,
                Width = 420, Height = 140,
                BackColor = Color.FromArgb(34, 139, 87)
            };

            var lblIcone = new Label
            {
                Text = "🛒",
                Font = new Font("Segoe UI", 32),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Top = 20, Left = 0, Width = 420, Height = 55
            };

            var lblTitulo = new Label
            {
                Text = "Mercado SeuZe",
                Font = new Font("Segoe UI", 17, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                Top = 80, Left = 0, Width = 420, Height = 34
            };

            pnlHeader.Controls.Add(lblIcone);
            pnlHeader.Controls.Add(lblTitulo);

            // Card branco centralizado
            var pnlCard = new Panel
            {
                Left = 30, Top = 155,
                Width = 340, Height = 280,
                BackColor = Color.White
            };
            pnlCard.Paint += (s, e) =>
            {
                var pen = new Pen(Color.FromArgb(220, 225, 235), 1);
                e.Graphics.DrawRectangle(pen, 0, 0, pnlCard.Width - 1, pnlCard.Height - 1);
            };

            var lblSubtitulo = new Label
            {
                Text = "Faça login para continuar",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(130, 140, 160),
                TextAlign = ContentAlignment.MiddleCenter,
                Top = 16, Left = 0, Width = 340, Height = 20,
                BackColor = Color.Transparent
            };

            // Linha decorativa
            var sepCard = new Panel
            {
                Top = 42, Left = 20, Width = 300, Height = 1,
                BackColor = Color.FromArgb(235, 238, 245)
            };

            var lblLogin = new Label
            {
                Text = "USUÁRIO",
                Font = new Font("Segoe UI", 7, FontStyle.Bold),
                ForeColor = Color.FromArgb(34, 139, 87),
                Top = 58, Left = 20, Width = 300, Height = 16,
                BackColor = Color.Transparent
            };

            txtLogin = new TextBox
            {
                Top = 76, Left = 20, Width = 300, Height = 30,
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(250, 251, 253)
            };

            var lblSenha = new Label
            {
                Text = "SENHA",
                Font = new Font("Segoe UI", 7, FontStyle.Bold),
                ForeColor = Color.FromArgb(34, 139, 87),
                Top = 118, Left = 20, Width = 300, Height = 16,
                BackColor = Color.Transparent
            };

            txtSenha = new TextBox
            {
                Top = 136, Left = 20, Width = 300, Height = 30,
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(250, 251, 253),
                PasswordChar = '●'
            };

            chkMostrarSenha = new CheckBox
            {
                Text = "Mostrar senha",
                Top = 172, Left = 20, Width = 140,
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.FromArgb(130, 140, 160),
                BackColor = Color.Transparent
            };
            chkMostrarSenha.CheckedChanged += (s, e) =>
                txtSenha.PasswordChar = chkMostrarSenha.Checked ? '\0' : '●';

            btnEntrar = new Button
            {
                Text = "ENTRAR",
                Top = 220, Left = 20, Width = 300, Height = 44,
                BackColor = Color.FromArgb(34, 139, 87),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnEntrar.FlatAppearance.BorderSize = 0;
            btnEntrar.MouseEnter += (s, e) => btnEntrar.BackColor = Color.FromArgb(28, 115, 72);
            btnEntrar.MouseLeave += (s, e) => btnEntrar.BackColor = Color.FromArgb(34, 139, 87);
            btnEntrar.Click += btnEntrar_Click;

            pnlCard.Controls.AddRange(new Control[] {
                lblSubtitulo, sepCard,
                lblLogin, txtLogin,
                lblSenha, txtSenha, chkMostrarSenha,
                btnEntrar
            });

            var lblRodape = new Label
            {
                Text = "Mercado SeuZe © 2026",
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.FromArgb(190, 195, 205),
                TextAlign = ContentAlignment.MiddleCenter,
                Top = 450, Left = 0, Width = 420, Height = 20
            };

            this.AcceptButton = btnEntrar;

            Controls.AddRange(new Control[] { pnlHeader, pnlCard, lblRodape });
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLogin.Text) ||
                string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                MessageBox.Show("Preencha o usuário e a senha.",
                    "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Usuario usuario = new Usuario
                {
                    Login = txtLogin.Text,
                    Senha = txtSenha.Text
                };

                UsuarioDao dao = new UsuarioDao();
                bool logado = dao.Login(usuario);

                if (logado)
                {
                    this.Hide();
                    Frm_Principal principal = new Frm_Principal();
                    principal.FormClosed += (s, ev) => this.Close();
                    principal.Show();
                }
                else
                {
                    MessageBox.Show("Usuário ou senha incorretos.",
                        "Acesso negado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSenha.Clear();
                    txtSenha.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao conectar: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
