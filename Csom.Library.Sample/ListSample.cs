using System;
using Microsoft.SharePoint.Client;

namespace Csom.Library.Sample
{
    internal static class ListSample
    {
        /// <summary>
        /// SharePoint ���X�g���擾����T���v���ł��B
        /// CSOM ���\�b�h�ł� PnP �g�����\�b�h�ł�p�ӂ��܂����B
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
        /// �I�u�W�F�N�g���擾�����Ƃ��ɁA����ňꕔ�̃v���p�e�B�ɃA�N�Z�X�ł��Ȃ�
        /// �I�u�W�F�N�g���ɂ����̃v���p�e�B���قȂ��Ă���A�ڍׂ͂������ URL �ɋL�ڂ���Ă���
        /// https://docs.microsoft.com/ja-jp/previous-versions/office/developer/sharepoint-2010/ee539350(v%3Doffice.14)#%E3%82%AA%E3%83%96%E3%82%B8%E3%82%A7%E3%82%AF%E3%83%88%E3%82%92%E5%8F%96%E5%BE%97%E3%81%99%E3%82%8B%E6%96%B9%E6%B3%95
        ///
        /// �I�u�W�F�N�g���擾����Ƃ��ɁA�����̃v���p�e�B�ɑ΂��ăA�N�Z�X�ł���悤�ɂ��������������
        /// </summary>
        /// <param name="context"></param>
        public static void ReadListWithExpressions(ClientContext context)
        {
            // using CSOM
            var listCsomByTitle = context.Web.Lists.GetByTitle("CsomByTitle");
            // 2�s�ŏ����o�[�W����
            context.Load(listCsomByTitle);
            context.Load(listCsomByTitle, l => l.HasUniqueRoleAssignments);
            // 1�s�ŏ����o�[�W����
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