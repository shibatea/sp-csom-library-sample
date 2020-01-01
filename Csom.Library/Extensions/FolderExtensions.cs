using Microsoft.SharePoint.Client;

namespace Csom.Library.Extensions
{
    public static class FolderExtensions
    {
        /// <summary>
        /// アイテム保持ポリシー適用時を考慮して、対象フォルダー内のサブフォルダーおよびファイルを削除してから、対象フォルダーを削除します
        /// </summary>
        /// <param name="folder">Folder</param>
        public static void DeleteFolderSafely(this Folder folder)
        {
            var context = folder.Context;

            // フォルダー配下のサブフォルダー、ファイルを読み込む
            context.Load(folder, f => f.Folders, f => f.Files);
            context.ExecuteQuery();

            // サブフォルダーを再帰的に処理
            for (var i = folder.Folders.Count - 1; i >= 0; i--)
            {
                var subFolder = folder.Folders[i];
                subFolder.DeleteFolderSafely();
            }

            // ファイルを削除
            for (var i = folder.Files.Count - 1; i >= 0; i--)
            {
                var file = folder.Files[i];
                file.DeleteObject();

                // 100ファイルずつまとめて削除
                if (i % 100 == 0)
                    context.ExecuteQueryRetry();
            }

            // ファイルの削除を実行
            context.ExecuteQueryRetry();

            // フォルダーを削除
            folder.DeleteObject();
            context.ExecuteQueryRetry();
        }
    }
}