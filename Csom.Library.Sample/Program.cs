using System;

namespace Csom.Library.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            const string account = "<account>";
            const string password = "<password>";
            const string webUrl = "<web url>";

            var spService = new SPService(account, password, webUrl);

            using (spService)
            {
                var context = spService.Context;

                ListSample.ReadList(context);
                ListSample.ReadListWithExpressions(context);
            }

            Console.ReadLine();
        }
    }
}
