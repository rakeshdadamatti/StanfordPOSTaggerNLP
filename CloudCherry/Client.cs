using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace CloudCherry
{
    public class Client
    {
        private Client()
        {

        }

        private static HttpClient instance = null;

        public static HttpClient Instance
        {
            get
            {
                if (instance == null)
                {
                    HttpClient client = new HttpClient();
                    instance = client;
                }
                return instance;
            }
        }
    }
}
