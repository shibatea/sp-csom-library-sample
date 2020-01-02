using System;
using Microsoft.SharePoint.Client;

namespace Csom.Library.Sample
{
    internal class WebSample
    {
        public static void GetWebRoleDefinitions(Web web)
        {
            web.EnsureProperties(
                // サイトのアクセス許可レベルを読み込む
                w => w.RoleDefinitions.Include(
                    r => r.RoleTypeKind,
                    r => r.BasePermissions,
                    r => r.Name,
                    r => r.Description));

            // サイトのアクセス許可レベルを出力
            foreach (var roleDefinition in web.RoleDefinitions)
            {
                Console.WriteLine($"{roleDefinition.RoleTypeKind} " +
                                  $"| {roleDefinition.BasePermissions.GetHashCode()} " +
                                  $"| {roleDefinition.Name} |" +
                                  $"| {roleDefinition.Description}");
            }
        }
    }
}