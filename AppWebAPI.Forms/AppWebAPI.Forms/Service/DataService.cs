using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AppWebAPI.Forms.Models;
using Newtonsoft.Json;

namespace AppWebAPI.Forms.Service
{
    class DataService
    {
        // instance new HTTP
        HttpClient _client = new HttpClient();

        // method to get data
        public async Task<List<Produto>> Caboco()
        {
            try
            {
                // url used to show data with a json
                var uri = new Uri("http://apitests.somee.com/api/produtos");

                // method GET
                var response = await _client.GetAsync(uri);

                // reading the result: response
                var content = await response.Content.ReadAsStringAsync();

                // converting to json
                var produtos = JsonConvert.DeserializeObject<List<Produto>>(content);
                return produtos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // method post
        // used to create a new product
        public async Task AddCaboco(Produto produto)
        {

            try
            {
                // route used to post data
                var uri = new Uri("http://apitests.somee.com/api/produtos/");

                // converting obeject to json
                var data = JsonConvert.SerializeObject(produto);

                // specifying the encoding
                var content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;

                // sending the new product to route using method post
                response = await _client.PostAsync(uri, content);

            }
            catch (Exception ex)
            {
                throw new Exception("erro: " + ex);
            }
        }

        public async Task PutCaboco(Produto produto)
        {
            try
            {
                string url = "http://apitests.somee.com/api/produtos/{0}";
                var uri = new Uri(string.Format(url, produto.Id));

                var data = JsonConvert.SerializeObject(produto);

                var content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;

                response = await _client.PutAsync(uri, content);
            }
            catch (Exception ex)
            {
                throw new Exception("erro: " + ex);
            }
        }

        public async Task DeleteCaboco(Produto produto)
        {
            
            string url = "http://apitests.somee.com/api/produtos/{0}";
            var uri = new Uri(string.Format(url, produto.Id));
            await _client.DeleteAsync(uri);
        }

        public async Task<Produto> GetSomeCaboco(int id)
        {
            try
            {
                string url = "http://apitests.somee.com/api/produtos/{0}";
                var uri = new Uri(string.Format(url, id));

                var response = await _client.GetAsync(uri);

                var content = await response.Content.ReadAsStringAsync();

                var produtos = JsonConvert.DeserializeObject<Produto>(content);
                return produtos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
