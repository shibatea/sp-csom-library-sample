using System;
using Microsoft.SharePoint.Client;

namespace Csom.Library.Sample
{
    internal class SecurableObjectSample
    {
        /// <summary>
        /// SharePoint オブジェクト Web, List, ListItem の親クラス SecurableObject クラスの権限情報を出力するサンプルです。
        /// </summary>
        /// <param name="securableObject">SecurableObject</param>
        public static void GetRolesForSecurableObject(SecurableObject securableObject)
        {
            securableObject.EnsureProperties(
                // オブジェクトの権限を読み込む
                w => w.RoleAssignments.Include(
                    r => r.Member.Title,
                    r => r.Member.PrincipalType,
                    // 対象オブジェクトの権限を与えられたユーザー or SPグループのアクセス許可レベルを読み込む
                    r => r.RoleDefinitionBindings.Include(
                        d => d.BasePermissions,
                        d => d.Name,
                        d => d.RoleTypeKind)));

            // 対象オブジェクトの権限を出力
            foreach (var roleAssignment in securableObject.RoleAssignments)
            {
                foreach (var roleDefinition in roleAssignment.RoleDefinitionBindings)
                {
                    Console.WriteLine($"{roleAssignment.Member.Title} " +
                                      $"| {roleAssignment.Member.PrincipalType} " +
                                      $"| {roleDefinition.RoleTypeKind} " +
                                      $"| {roleDefinition.BasePermissions.GetHashCode()} " +
                                      $"| {roleDefinition.Name}");
                }
            }
        }
    }
}