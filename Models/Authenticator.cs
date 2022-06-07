using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HH_RU_Parser.Models
{
    public class Authenticator
    {
        public async Task AuthenticateAsync(HttpClient httpClient)
        {
            await httpClient.GetAsync("https://hh.ru/oauth/authorize?response_type=code&client_id=V6CJR2ERILBIA8FTP9BJ5B0T4A0PFCEBV3MRQ1TVBDF1DPEDUI4VJ774DB992C1U&state=1");
        }
    }
}
