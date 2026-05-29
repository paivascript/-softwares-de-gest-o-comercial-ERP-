// para rodar o código executar
// dotnet build
// dotnet run 

// user: admin
// pass: 1234

using System;
using System.Windows.Forms;
using MercadoSeuZe.PL;

namespace MercadoSeuZe
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Frm_Login());
        }
    }
}