using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace CompTrain.Client.Auth
{

    public class CustomAuthStateProvider : AuthenticationStateProvider, ILoginService
    {

        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _httpClient;
        private static readonly string _tokenkey = "TOKENKEY";
        private AuthenticationState _anonymous => new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        public CustomAuthStateProvider(ILocalStorageService localStorage, HttpClient httpClient)
        {
            this._localStorage = localStorage;
            this._httpClient = httpClient;
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetItemAsync<string>(_tokenkey);

            if (!tokenStatus(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = null;
                await _localStorage.RemoveItemAsync(_tokenkey);
                return _anonymous;
            }

            return buildAuthenticationState(token);
        }

        public async Task Login(string token)
        {
            await _localStorage.SetItemAsync(_tokenkey, token);
            var authState = buildAuthenticationState(token);
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        public async Task Logout()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
            await _localStorage.RemoveItemAsync(_tokenkey);
            NotifyAuthenticationStateChanged(Task.FromResult(_anonymous));
        }

        private AuthenticationState buildAuthenticationState(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(parseClaimsFromJwt(token), "jwt")));
        }

        private bool tokenStatus(string token)
        {

            try
            {
                if (String.IsNullOrEmpty(token))
                    return false;

                var payload = token.Split('.')[1];
                var jsonBytes = parseBase64WithoutPadding(payload);
                var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
                object exp = keyValuePairs.Where(k => k.Key.Equals("exp")).FirstOrDefault();

                if (exp == null
                    || exp.ToString().Equals(String.Empty)
                        || !int.TryParse(new String(exp.ToString().Where(char.IsDigit).ToArray()), out int expireInt)
                            || new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(expireInt) < DateTime.Now
                        )
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static IEnumerable<Claim> parseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = parseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        }

        private static byte[] parseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

    }
}
