using System;
using System.Threading.Tasks; 

namespace NetJira
{
    class Program
    {
        static void Main(string[] args)
        {
            var tmcJira = new TusconJira(BASE_JIRA_URL, 
                "", 
                "", 
                "",
                @"",
                "");
            

            Console.WriteLine(string.Format("{0}: {1}", ticket.Item1, ticket.Item2));
            Console.WriteLine("Done");
        }
    }

    public static class Extension
    {
        public static T AwaitResult<T>(this Task<T> task)
        {
            task.Wait();
            return task.Result;
        }
    }
}
