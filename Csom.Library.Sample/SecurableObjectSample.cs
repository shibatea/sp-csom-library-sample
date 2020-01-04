using System;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Utilities;

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

        public static void BulkDeleteRolesByPnP(SecurableObject securableObject)
        {
            securableObject.EnsureProperties(s => s.HasUniqueRoleAssignments);

            // 固有の権限かどうか確認する
            if (!securableObject.HasUniqueRoleAssignments)
            {
                // 権限の継承を中止する
                // ※ 後続の EnsureProperties 内で ExecuteQuery が実行されているので
                // ※ IF 文内では実行しない
                securableObject.BreakRoleInheritance(false, false);
            }

            for (var index = securableObject.RoleAssignments.Count - 1; index >= 0; index--)
            {
                var roleAssignment = securableObject.RoleAssignments[index];
                securableObject.RemovePermissionLevelFromPrincipal(roleAssignment.Member, RoleType.Reader, true);
            }
        }

        public static void BulkDeleteRolesByCsom1(SecurableObject securableObject)
        {
            securableObject.EnsureProperties(s => s.HasUniqueRoleAssignments);

            if (!securableObject.HasUniqueRoleAssignments)
            {
                securableObject.BreakRoleInheritance(false, false);
            }

            securableObject.EnsureProperties(
                s => s.RoleAssignments.Include(
                    r => r.Member.PrincipalType,
                    r => r.Member.LoginName));

            for (var index = securableObject.RoleAssignments.Count - 1; index >= 0; index--)
            {
                var roleAssignment = securableObject.RoleAssignments[index];
                roleAssignment.DeleteObject();
            }

            securableObject.Context.ExecuteQueryRetry();
        }

        public static void BulkDeleteRolesByCsom2(SecurableObject securableObject)
        {
            securableObject.EnsureProperties(s => s.HasUniqueRoleAssignments);

            if (securableObject.HasUniqueRoleAssignments)
            {
                securableObject.ResetRoleInheritance();
                securableObject.BreakRoleInheritance(false, false);
            }
            else
            {
                securableObject.BreakRoleInheritance(false, false);
            }

            securableObject.EnsureProperties(
                s => s.RoleAssignments.Include(
                    r => r.Member.PrincipalType,
                    r => r.Member.LoginName));

        }

    }
}