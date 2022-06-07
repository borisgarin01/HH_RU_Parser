using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using HH_RU_Parser.Models;
using static System.Net.WebRequestMethods;

namespace HH_RU_Parser
{
    public class Requester
    {
        public HttpClient HttpClient { get; }

        public Requester()
        {
            HttpClient = new();
            HttpClient.DefaultRequestHeaders.Add("User-Agent", "C# App");
        }

        //TODO: Сделать список требований к джуниорам без опыта в файл Requirements.txt

        public async Task<HttpContent> GetResponseAsync(string request)
        {
            var result = await HttpClient.GetAsync(request);
            return result.Content;
        }

        //public async Task AddVacanciesToFavourite(IEnumerable<Item> items, HttpClient httpClient)
        //{
        //    //foreach (var item in items)
        //    //{
        //    //    HttpContent httpContent = HttpContent;
        //    //    webRequest.Method = "POST";
        //    //    webRequest.Headers.Add("vacancy_id", item.id);

        //    //    try
        //    //    {
        //    //        Thread.Sleep(100);
        //    //        await HttpClient.PostAsync("https://api.hh.ru/applicant/favorite_vacancies/add",)
        //    //    }
        //    //    catch (Exception ex)
        //    //    {
        //    //        Console.WriteLine(ex.Message);
        //    //    }
        //    //}
        //}
    }
}
