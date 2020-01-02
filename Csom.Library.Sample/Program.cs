using System;
using System.Configuration;

namespace Csom.Library.Sample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var account = ConfigurationManager.AppSettings["account"];
            var password = ConfigurationManager.AppSettings["password"];
            var webUrl = ConfigurationManager.AppSettings["url"];

            var spService = new SPService(account, password, webUrl);

            using (spService)
            {
                var context = spService.Context;

                var web = context.Web;

                SecurableObjectSample.GetRolesForSecurableObject(web);
                WebSample.GetWebRoleDefinitions(web);
                ListSample.ReadList(context);
                ListSample.ReadListWithExpressions(context);
            }

            Console.ReadLine();
        }
    }
}