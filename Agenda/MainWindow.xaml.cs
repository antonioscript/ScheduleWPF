using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Agenda
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string operacao;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btSalvar_Click(object sender, RoutedEventArgs e)
        {
            //Criação do Contato
            contato person = new contato();
            person.nome = txtNome.Text;
            person.email = txtEmail.Text;

            //Gravação no Banco de Dados
            if (operacao == "inserir")
            {
                using (AgendaEntities context = new AgendaEntities())
                {
                    context.contatos.Add(person);
                    context.SaveChanges();
                }
            }

            if (operacao == "alterar")
            {

            }

            this.ListarContatos();
            this.AlterarBotoes(1);
            this.LimparCampos();
        }

        private void btnInserir_Click(object sender, RoutedEventArgs e)
        {
            this.operacao = "inserir";
            this.AlterarBotoes(2);

            //Usuário não deve digitar o código (então não era nem pra dar essa opção, mas ok, é apenas estudo
            txtID.IsEnabled = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.ListarContatos();
            this.AlterarBotoes(1); //Começa sempre com 1
        }

        private void ListarContatos()
        {
            using (AgendaEntities context = new AgendaEntities())
            {
                var consulta = context.contatos;
                dgDados.ItemsSource = consulta.ToList();
            }
        }

        private void AlterarBotoes(int op)
        {
            btnAlterar.IsEnabled = false;
            btnInserir.IsEnabled = false;
            btnExcluir.IsEnabled = false;
            btnCancelar.IsEnabled = false;
            btnLocalizar.IsEnabled = false;
            btnSalvar.IsEnabled = false;   
            
            if (op == 1) 
            {
                btnInserir.IsEnabled = true;
                btnLocalizar.IsEnabled = true;
            }

            //Inserir um valor
            if (op == 2)
            {
                btnCancelar.IsEnabled = true;
                btnSalvar.IsEnabled = true;
            }

            if (op == 3)
            {
                btnAlterar.IsEnabled = true;
                btnExcluir.IsEnabled = true;
            }
        }

        private void LimparCampos()
        {
            //Limpar os campos para uma nova inserção de registro
            txtID.Clear();
            txtNome.Clear();
            txtEmail.Clear();
            txtTelefone.Clear();

            txtID.IsEnabled = true;
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.AlterarBotoes(1);
            this.LimparCampos();
        }

        private void btnLocalizar_Click(object sender, RoutedEventArgs e)
        {
            if (txtID.Text.Trim().Count() > 0)
            {
                try
                {
                    int id = Convert.ToInt32(txtID.Text);
                    using (AgendaEntities context = new AgendaEntities()) 
                    {
                        contato person = context.contatos.Find(id);
                        dgDados.ItemsSource = new contato[1] { person };
                    }
                }
                catch 
                { 
                    
                }
            }

            if (txtNome.Text.Trim().Count() > 0)
            {
                try
                {
                    using (AgendaEntities context = new AgendaEntities())
                    {
                        var consulta = from person in context.contatos
                                       where person.nome.Contains(txtNome.Text)
                                       select person;

                        dgDados.ItemsSource = consulta.ToList();
                    }
                }
                catch
                {

                }
            }

            if (txtEmail.Text.Trim().Count() > 0)
            {
                try
                {
                    using (AgendaEntities context = new AgendaEntities())
                    {
                        var consulta = from person in context.contatos
                                       where person.email.Contains(txtEmail.Text)
                                       select person;

                        dgDados.ItemsSource = consulta.ToList();
                    }
                }
                catch
                {

                }
            }

            if (txtTelefone.Text.Trim().Count() > 0)
            {
                try
                {
                    using (AgendaEntities context = new AgendaEntities())
                    {
                        var consulta = from person in context.contatos
                                       where person.telefone.Contains(txtTelefone.Text)
                                       select person;

                        dgDados.ItemsSource = consulta.ToList();
                    }
                }
                catch
                {

                }
            }
        }

        private void dgDados_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgDados.SelectedIndex >= 0)
            {
                contato person = (contato)dgDados.SelectedItem;
                
                txtID.Text = person.id.ToString();
                txtNome.Text = person.nome;
                txtEmail.Text = person.email;
                txtTelefone.Text = person.telefone;
            }
        }

        private void btnAlterar_Click(object sender, RoutedEventArgs e)
        {
            this.operacao = "alterar";
            this.AlterarBotoes(2);

        }

        private void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            using (AgendaEntities context = new AgendaEntities())
            {
                contato person = context.contatos.Find(Convert.ToInt32(txtID.Text));
                
                if (person != null)
                {
                    context.contatos.Remove(person);
                    context.SaveChanges();
                }

                this.ListarContatos();
                this.AlterarBotoes(1);
                this.LimparCampos();
            }
        }
    }
}
