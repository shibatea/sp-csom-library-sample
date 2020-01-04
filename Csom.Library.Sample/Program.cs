using System;
using System.Configuration;
using Microsoft.SharePoint.Client;

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

                var listCsomById = context.Web.Lists.GetById(Guid.Parse("3542cfd9-85b5-4e7d-82fa-7ea70761c0c7"));
                context.Load(listCsomById);
                context.ExecuteQueryRetry();

                SecurableObjectSample.BulkDeleteRolesByPnP(listCsomById);
                //SecurableObjectSample.BulkDeleteRolesByCsom1(listCsomById);
                //SecurableObjectSample.BulkDeleteRolesByCsom2(listCsomById);
                return;

                SecurableObjectSample.GetRolesForSecurableObject(web);
                WebSample.GetWebRoleDefinitions(web);
                ListSample.ReadList(context);
                ListSample.ReadListWithExpressions(context);
            }

            Console.ReadLine();
        }
    }
}