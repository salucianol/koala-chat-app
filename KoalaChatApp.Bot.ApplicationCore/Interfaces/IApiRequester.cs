using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KoalaChatApp.Bot.ApplicationCore.Interfaces {
    public interface IApiRequester {
        Task<string> MakeGetRequest(string url);
    }
}
