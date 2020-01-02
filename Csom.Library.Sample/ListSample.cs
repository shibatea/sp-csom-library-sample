using System;
using Microsoft.SharePoint.Client;

namespace Csom.Library.Sample
{
    internal static class ListSample
    {
        /// <summary>
        /// SharePoint リストを取得するサンプルです。
        /// CSOM メソッド版と PnP 拡張メソッド版を用意しました。
        /// </summary>
        /// <param name="context"></param>
        public static void ReadList(ClientContext context)
        {
            // using CSOM
            var listCsomById = context.Web.Lists.GetById(Guid.Parse("3542cfd9-85b5-4e7d-82fa-7ea70761c0c7"));
            context.Load(listCsomById);
            context.ExecuteQueryRetry();
            Console.WriteLine(listCsomById.Title);

            // using CSOM
            var listCsomByTitle = context.Web.Lists.GetByTitle("CsomByTitle");
            context.Load(listCsomByTitle);
            context.ExecuteQueryRetry();
            Console.WriteLine(listCsomByTitle.Title);

            // using PnP
            var listPnPById = context.Web.GetListById(Guid.Parse("f470fb72-1528-45e4-a929-a144146c1b1f"));
            Console.WriteLine(listPnPById.Title);

            // using PnP
            var listPnPByTitle = context.Web.GetListByTitle("PnPByTitle");
            Console.WriteLine(listPnPByTitle.Title);

            // using PnP
            var listPnPByUrl = context.Web.GetListByUrl("PnPByUrl");
            Console.WriteLine(listPnPByUrl.Title);
        }

        /// <summary>
        /// オブジェクトを取得したときに、既定で一部のプロパティにアクセスできない
        /// オブジェクト毎にそれらのプロパティが異なっており、詳細はこちらの URL に記載されている
        /// https://docs.microsoft.com/ja-jp/previous-versions/office/developer/sharepoint-2010/ee539350(v%3Doffice.14)#%E3%82%AA%E3%83%96%E3%82%B8%E3%82%A7%E3%82%AF%E3%83%88%E3%82%92%E5%8F%96%E5%BE%97%E3%81%99%E3%82%8B%E6%96%B9%E6%B3%95
        ///
        /// オブジェクトを取得するときに、それらのプロパティに対してアクセスできるようにするやり方がこちら
        /// </summary>
        /// <param name="context"></param>
        public static void ReadListWithExpressions(ClientContext context)
        {
            // using CSOM
            var listCsomByTitle = context.Web.Lists.GetByTitle("CsomByTitle");
            // 2行で書くバージョン
            context.Load(listCsomByTitle);
            context.Load(listCsomByTitle, l => l.HasUniqueRoleAssignments);
            // 1行で書くバージョン
            //context.Load(listCsomByTitle, l => l, l => l.HasUniqueRoleAssignments);
            context.ExecuteQueryRetry();
            Console.WriteLine(listCsomByTitle.Title);
            Console.WriteLine(listCsomByTitle.HasUniqueRoleAssignments);

            // using PnP with expressions
            var listPnPByTitle = context.Web.GetListByTitle("PnPByTitle", l => l.HasUniqueRoleAssignments);
            Console.WriteLine(listPnPByTitle.Title);
            Console.WriteLine(listPnPByTitle.HasUniqueRoleAssignments);
        }
    }
}