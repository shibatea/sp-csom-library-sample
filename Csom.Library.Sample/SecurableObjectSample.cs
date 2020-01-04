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

        /// <summary>
        /// コードの量は少なく済むが、権限を１件削除するのに時間を要する
        /// </summary>
        /// <param name="securableObject"></param>
        public static void BulkDeleteRolesByPnP(SecurableObject securableObject)
        {
            securableObject.EnsureProperties(s => s.HasUniqueRoleAssignments);

            // 固有の権限かどうか確認する
            if (!securableObject.HasUniqueRoleAssignments)
            {
                // 権限の継承を中止する
                // ※ 後続の EnsureProperties 内で ExecuteQuery が実行されているので IF 文内では実行しない
                securableObject.BreakRoleInheritance(false, false);
            }

            securableObject.EnsureProperties(s => s.RoleAssignments);

            for (var index = securableObject.RoleAssignments.Count - 1; index >= 0; index--)
            {
                var roleAssignment = securableObject.RoleAssignments[index];
                securableObject.RemovePermissionLevelFromPrincipal(roleAssignment.Member, RoleType.Reader, true);
            }
        }

        /// <summary>
        /// PnP バージョンと比べたら高速だが、ユーザー数が多い場合には時間が掛かる場合がある
        /// </summary>
        /// <param name="securableObject"></param>
        public static void BulkDeleteRolesByCsom1(SecurableObject securableObject)
        {
            securableObject.EnsureProperties(s => s.HasUniqueRoleAssignments);

            // 固有の権限かどうか確認する
            if (!securableObject.HasUniqueRoleAssignments)
            {
                // 権限の継承を中止する
                // ※ 後続の EnsureProperties 内で ExecuteQuery が実行されているので IF 文内では実行しない
                securableObject.BreakRoleInheritance(false, false);
            }

            securableObject.EnsureProperties(s => s.RoleAssignments);

            for (var index = securableObject.RoleAssignments.Count - 1; index >= 0; index--)
            {
                var roleAssignment = securableObject.RoleAssignments[index];
                roleAssignment.DeleteObject();
            }

            securableObject.Context.ExecuteQueryRetry();
        }

        /// <summary>
        /// ユーザー数が多かろうが少なかろうが、安定した速度で削除できる
        /// また、固有の権限を削除 ⇒ 権限の継承を外す 処理は、（権限を繰り返し削除するのと比べて）コストの掛かる処理ではない
        /// </summary>
        /// <param name="securableObject"></param>
        public static void BulkDeleteRolesByCsom2(SecurableObject securableObject)
        {
            securableObject.EnsureProperties(s => s.HasUniqueRoleAssignments);

            if (securableObject.HasUniqueRoleAssignments)
            {
                // 一旦、固有の権限を削除してから、権限の継承を外す
                // その際に、実行ユーザー以外の権限をすべてクリアする
                // ※ BreakRoleInheritance メソッドの copyRoleAssignments を false にする
                securableObject.ResetRoleInheritance();
                securableObject.BreakRoleInheritance(false, false);
            }
            else
            {
                securableObject.BreakRoleInheritance(false, false);
            }

            securableObject.EnsureProperties(s => s.RoleAssignments);

            // 実行ユーザーしか残ってないので、for 文は実質 1 回しか繰り返すことがない
            for (var index = securableObject.RoleAssignments.Count - 1; index >= 0; index--)
            {
                var roleAssignment = securableObject.RoleAssignments[index];
                roleAssignment.DeleteObject();
            }

            securableObject.Context.ExecuteQueryRetry();
        }

    }
}