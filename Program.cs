namespace WebFastFinder
{
    internal class Program
    {
        public static void Main()
        {
            Console.WriteLine("Stable Diffusion Gradio Fast Finder");
            Console.WriteLine("The program will not save the URL until you have searched for the range you set, so please do not quit halfway");
            Console.WriteLine("程序在搜索完您设定的区间后才会保存url,请勿中途退出");
            Console.WriteLine();
            Console.Write("Start 起始 (Press Enter default 按回车默认 10000): ");
            var inputStart = Console.ReadLine();
            Console.Write("End 结束 (Press Enter default 按回车默认 12000): ");
            var inputEnd = Console.ReadLine();
            Console.WriteLine();
            try
            {
                //ArgumentNullException.ThrowIfNull(inputStart);
                //ArgumentNullException.ThrowIfNull(inputEnd);
                if((inputStart is null)||(inputStart == ""))
                {
                    inputStart = "10000";
                }
                if((inputEnd is null)||(inputEnd == ""))
                {
                    inputEnd = "12000";
                }
                var lists = WebsiteFinder.Search(int.Parse(inputStart), int.Parse(inputEnd));
                File.WriteAllLines("web_urls.txt", lists);
                Console.WriteLine();
                Console.WriteLine("OK - all task complete...");
            }
            catch
            {
                Console.WriteLine("INVALID PARAMETER 参数非法, 应输入 parameter → int 数字");
            }
            Console.ReadKey();
        }
    }
}