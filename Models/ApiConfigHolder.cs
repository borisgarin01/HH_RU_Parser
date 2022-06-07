using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HH_RU_Parser.Models
{
    public static class ApiConfigHolder
    {
        public static string ApiKey
        {
            get
            {
                return "V6CJR2ERILBIA8FTP9BJ5B0T4A0PFCEBV3MRQ1TVBDF1DPEDUI4VJ774DB992C1U";
            }
        }
        public static string ClientSecret
        {
            get
            {
                return "U1J0KHSAKHI7834DAAMH20RBVMHLGOCFEJBVSGB31TB3HR3MDF5DSLME8GIFLP2A";
            }
        }
        public static string UID
        {
            get { return "80f43f2f95da48bf855e6573d397b092"; }
        }
        public static string HHUL
        {
            get { return "fa6861f0bf5d94ce42da2db77344ee219d3f990afb73f4b4ee36cc7911b7885a"; }
        }
        public static string HHUID
        {
            get { return "yYlOsxoyazXLemI!FAorwg--"; }
        }
        public static string HHToken
        {
            get { return "Amu5Uh2o7WE2UtLgaGmv6BxtrgQC"; }
        }
    }
}
