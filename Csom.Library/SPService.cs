using System;
using System.Security;
using Microsoft.SharePoint.Client;

namespace Csom.Library
{
    public class SPService : IDisposable
    {
        public string Account { get; }
        public string Password { get; }
        public string WebUrl { get; }
        public ClientContext Context { get; }
        private SecureString SecureString { get; } = new SecureString();

        public SPService(string account, string password, string webUrl)
        {
            Account = account;
            Password = password;
            WebUrl = webUrl;

            foreach (var c in Password) SecureString.AppendChar(c);
            SecureString.MakeReadOnly();

            Context = new ClientContext(WebUrl)
            {
                Credentials = new SharePointOnlineCredentials(Account, SecureString)
            };
        }

        public void Dispose()
        {
            Context?.Dispose();
            SecureString?.Dispose();
        }
    }
}