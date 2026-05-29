using System;
using System.Drawing;
using System.Windows.Forms;
using MercadoSeuZe.BLL;
using MercadoSeuZe.DAL;
using MercadoSeuZe.Modelo;

namespace MercadoSeuZe.PL
{
    public class Frm_Cliente : Form
    {
        private Label lblNome, lblCpf, lblTel, lblEmail, lblEstCivil, lblEnd, lblSexo;
        private TextBox txtNome, txtCpf, txtTelefone, txtEmail, txtEndereco;
        private ComboBox cmbEstadoCivil;
        private RadioButton radMasculino, radFeminino;
        private Button btnCadastrar, btnLimpar;
        private Label lblStatus;

        public Frm_Cliente()
        {
            this.Text = "Cadastro de Cliente";
            this.Width = 480;
            this.Height = 540;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(248, 249, 252);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Cabeçalho
            var pnlHeader = new Panel
            {
                Left = 0, Top = 0, Width = 480, Height = 68,
                BackColor = Color.FromArgb(70, 130, 210)
            };

            var lblH = new Label
            {
                Text = "👤  Cadastro de Cliente",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Top = 16, Left = 20, Width = 400, Height = 36,
                TextAlign = ContentAlignment.MiddleLeft
            };
            pnlHeader.Controls.Add(lblH);

            // Card de formulário
            var pnlCard = new Panel
            {
                Left = 20, Top = 82, Width = 424, Height = 370,
                BackColor = Color.White
            };
            pnlCard.Paint += (s, e) =>
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(220, 225, 235), 1),
                    0, 0, pnlCard.Width - 1, pnlCard.Height - 1);

            int y = 18;
            const int labelW = 105, inputX = 118, inputW = 290;

            lblNome    = CriarLabelCard("Nome: *",        y);
            txtNome    = CriarTextBoxCard(y, inputW);

            y += 40;
            lblCpf     = CriarLabelCard("CPF: *",         y);
            txtCpf     = CriarTextBoxCard(y, inputW);

            y += 40;
            lblTel     = CriarLabelCard("Telefone: *",    y);
            txtTelefone = CriarTextBoxCard(y, inputW);

            y += 40;
            lblEmail   = CriarLabelCard("Email:",         y);
            txtEmail   = CriarTextBoxCard(y, inputW);

            y += 40;
            lblEstCivil = CriarLabelCard("Estado Civil:", y);
            cmbEstadoCivil = new ComboBox
            {
                Top = y, Left = inputX, Width = inputW,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9),
                BackColor = Color.FromArgb(250, 251, 253)
            };
            cmbEstadoCivil.Items.AddRange(new object[] { "Solteiro", "Casado", "Divorciado", "Viúvo" });
            cmbEstadoCivil.SelectedIndex = 0;

            y += 40;
            lblEnd     = CriarLabelCard("Endereço:",      y);
            txtEndereco = CriarTextBoxCard(y, inputW);

            y += 40;
            lblSexo    = CriarLabelCard("Sexo:",          y);
            radMasculino = new RadioButton
            {
                Text = "Masculino", Top = y + 2, Left = inputX,
                Width = 100, Checked = true,
                Font = new Font("Segoe UI", 9),
                BackColor = Color.Transparent
            };
            radFeminino = new RadioButton
            {
                Text = "Feminino", Top = y + 2, Left = inputX + 110,
                Width = 100,
                Font = new Font("Segoe UI", 9),
                BackColor = Color.Transparent
            };

            // Separador antes dos botões
            y += 38;
            var sepCard = new Panel
            {
                Top = y, Left = 0, Width = 424, Height = 1,
                BackColor = Color.FromArgb(230, 234, 242)
            };

            y += 14;
            btnLimpar = new Button
            {
                Text = "Limpar",
                Top = y, Left = inputX, Width = 96, Height = 36,
                BackColor = Color.FromArgb(240, 241, 245),
                ForeColor = Color.FromArgb(80, 90, 110),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnLimpar.FlatAppearance.BorderColor = Color.FromArgb(210, 215, 228);
            btnLimpar.FlatAppearance.BorderSize = 1;
            btnLimpar.Click += (s, e) => Limpar();

            btnCadastrar = new Button
            {
                Text = "✔  Cadastrar",
                Top = y, Left = inputX + 104, Width = 186, Height = 36,
                BackColor = Color.FromArgb(34, 139, 87),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnCadastrar.FlatAppearance.BorderSize = 0;
            btnCadastrar.MouseEnter += (s, e) => btnCadastrar.BackColor = Color.FromArgb(28, 115, 72);
            btnCadastrar.MouseLeave += (s, e) => btnCadastrar.BackColor = Color.FromArgb(34, 139, 87);
            btnCadastrar.Click += btnCadastrar_Click;
            this.AcceptButton = btnCadastrar;

            pnlCard.Controls.AddRange(new Control[] {
                lblNome, txtNome, lblCpf, txtCpf, lblTel, txtTelefone,
                lblEmail, txtEmail, lblEstCivil, cmbEstadoCivil,
                lblEnd, txtEndereco, lblSexo, radMasculino, radFeminino,
                sepCard, btnLimpar, btnCadastrar
            });

            lblStatus = new Label
            {
                Text = "",
                Top = 462, Left = 20, Width = 424, Height = 22,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                ForeColor = Color.FromArgb(34, 139, 87),
                TextAlign = ContentAlignment.MiddleLeft
            };

            Controls.AddRange(new Control[] { pnlHeader, pnlCard, lblStatus });

            txtNome.Focus();
        }

        private Label CriarLabelCard(string texto, int top) =>
            new Label
            {
                Text = texto,
                Top = top + 3, Left = 14, Width = 100,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(70, 80, 100),
                BackColor = Color.Transparent
            };

        private TextBox CriarTextBoxCard(int top, int largura) =>
            new TextBox
            {
                Top = top, Left = 118, Width = largura,
                Font = new Font("Segoe UI", 9),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(250, 251, 253)
            };

        private void Limpar()
        {
            txtNome.Clear();
            txtCpf.Clear();
            txtTelefone.Clear();
            txtEmail.Clear();
            txtEndereco.Clear();
            cmbEstadoCivil.SelectedIndex = 0;
            radMasculino.Checked = true;
            lblStatus.Text = "";
            txtNome.Focus();
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            Cliente cliente = new Cliente
            {
                Nome = txtNome.Text.Trim(),
                Cpf = txtCpf.Text.Trim(),
                Telefone = txtTelefone.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                EstadoCivil = cmbEstadoCivil.Text,
                Endereco = txtEndereco.Text.Trim(),
                Sexo = radMasculino.Checked ? "Masculino" : "Feminino"
            };

            ClienteBLL bll = new ClienteBLL();
            bool validado = bll.Valida(cliente);

            if (!validado)
            {
                lblStatus.ForeColor = Color.FromArgb(200, 60, 60);
                lblStatus.Text = "⚠  Preencha os campos obrigatórios: Nome, CPF e Telefone.";
                txtNome.Focus();
                return;
            }

            try
            {
                new ClienteDao().Inserir(cliente);

                MessageBox.Show($"Cliente \"{cliente.Nome}\" cadastrado com sucesso!",
                    "✔ Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = Color.FromArgb(200, 60, 60);
                lblStatus.Text = "Erro: " + ex.Message;
            }
        }
    }
}
