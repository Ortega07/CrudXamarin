using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AppWebAPI.Forms.Models;
using Xamarin.Forms;
using AppWebAPI.Forms.Service;

namespace WebAPI
{
    public partial class MainPage : ContentPage
    {
        // Declarations
        DataService dataService;
        List<Produto> produtos;
        Produto teste;

        // main method
        public MainPage()
        {
            InitializeComponent();
            // instance dataservice
            dataService = new DataService();

            // get data
            AtualizaDados();

            if (lista.IsVisible)
            {
                btn1.Text = "Ocultar Lista";
            }
            else
            {
                btn1.Text = "Mostrar Lista";
            }
        }

        // validate data
        private bool Valida()
        {
            // checking if fields are different of null or empty
            if(string.IsNullOrEmpty(txtNome.Text) || string.IsNullOrEmpty(txtCategoria.Text) || string.IsNullOrEmpty(txtPreco.Text))
            {
                texto.IsVisible = true;
                texto.Text = "Preencha todos os campos";
                return false;
            }
            else
            {
                // checking if filed preco is decimal
                var preco = txtPreco.Text;
                if (decimal.TryParse(preco, out decimal number))
                {
                    return true;
                }
                else
                {
                    texto.IsVisible = true;
                    texto.Text = "Formato inválido para o preço!";
                    return false;
                }
            }
        }

        // clear fields
        private void LimparProduto()
        {
            txtNome.Text = "";
            txtCategoria.Text = "";
            txtPreco.Text = "";
        }

        // getting data
        async void AtualizaDados()
        {
            produtos = await dataService.Caboco();
            lista.ItemsSource = produtos.OrderBy(item => item.Nome).ToList();
        }

        // if button 1 was clicked the text will be changed
        // and the LIST will be visible or no visible
        void OnBtn1(object sender, EventArgs args)
        {
            texto.IsVisible = false;
            texto.Text = "";
            if (lista.IsVisible)
            {
                lista.IsVisible = false;
                btn1.Text = "Mostrar Lista";
            }
            else
            {
                lista.IsVisible = true;
                btn1.Text = "Ocultar Lista";
            }
            AtualizaDados();
        }

        // if button 2 was clicked the text will be changed
        // and the FORM will be visible or no visible
        void OnBtn2(object sender, EventArgs args)
        {
            if (Campos.IsVisible)
            {
                Campos.IsVisible = false;
                btn2.Text = "Novo Produto";
            }
            else
            {
                btn2.Text = "Ocultar";
                LimparProduto();
                Campos.IsVisible = true;
            }
            btn3.IsVisible = true;

            btns4n5.IsVisible = false;
            texto.IsVisible = false;
            texto.Text = "";
            AtualizaDados();
        }

        // if button 3 was clicked the product will be created
        async void OnBtn3(object sender, EventArgs args)
        {
            if (Valida())
            {
                // creating a new Produto in instance
                Produto newProduto = new Produto{
                    Nome = txtNome.Text.Trim(),
                    Categoria = txtCategoria.Text.Trim(),
                    Preco = Convert.ToDecimal(txtPreco.Text)
                };

                // trying add new product
                try
                {
                    // sending the new product to the method AddCaboco
                    await dataService.AddCaboco(newProduto);
                    LimparProduto();
                    Campos.IsVisible = false;
                    texto.IsVisible = true;
                    texto.Text = "Produto Adicionado";
                    btn3.IsVisible = true;
                    btns4n5.IsVisible = false;
                    AtualizaDados();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Erro", ex.Message, "OK");
                }
            }
            else
            {
                await DisplayAlert("Erro", "Dados inválidos...", "OK");
            }
        }

        async void OnBtn4(object sender, EventArgs args)
        {
            if (Valida())
            {
                try
                {
                    int id = Convert.ToInt32(txtId.Text);

                    Produto UpdateProduto = await dataService.GetSomeCaboco(id);
                    UpdateProduto.Nome = txtNome.Text;
                    UpdateProduto.Categoria = txtCategoria.Text;
                    UpdateProduto.Preco = Convert.ToDecimal(txtPreco.Text);

                    await dataService.PutCaboco(UpdateProduto);
                    LimparProduto();
                    AtualizaDados();

                }
                catch (Exception ex)
                {
                    await DisplayAlert("Erro", ex.Message, "OK");
                }
            }
            else
            {
                await DisplayAlert("Erro", "Dados inválidos...", "OK");
            }
        }

        async void OnBtn5(object sender, EventArgs args)
        {
            try
            {
                int id = Convert.ToInt32(txtId.Text);
                teste = await dataService.GetSomeCaboco(id);
                Produto DeleteProduto = teste;
                await dataService.DeleteCaboco(DeleteProduto);
                btns4n5.IsVisible = false;

                btn3.IsVisible = true;

                LimparProduto();
                AtualizaDados();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        void OnBtn6(object sender, EventArgs args)
        {
            AtualizaDados();
        }

        // if some viewcell is clicked, its data will be show in the form
        void ListaProdutos(object sender, SelectedItemChangedEventArgs e)
        {
            var produto = e.SelectedItem as Produto;
            txtId.Text = Convert.ToString(produto.Id);

            txtNome.Text = produto.Nome;
            txtCategoria.Text = produto.Categoria;
            txtPreco.Text = produto.Preco.ToString();

            btn2.Text = "Ocultar";
            
            Campos.IsVisible = true;

            btn3.IsVisible = false;

            btns4n5.IsVisible = true;

        }

    }
}
