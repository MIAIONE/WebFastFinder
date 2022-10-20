namespace WebFastFinder
{
    internal static class WebsiteFinder
    {
        public const string GradioUrl = "https://{0}.gradio.app/";
        public static readonly List<string> ValidString = new() 
        {
            "Stable Diffusion",
            "txt2img",
            "img2img"
        };

        public static readonly List<string> InvalidString = new()
        {
            "Bad Gateway",
            "Internal Server Error",
            "No interface is running right now",
            "403 Forbidden",
            "\"auth_required\": true",
            "HTTP Error/Application from httpclient [FINDER]"
        };

        public static readonly ParallelOptions ParallelOptions = new()
        {
            MaxDegreeOfParallelism = int.MaxValue //太大会被ban ip
        };

        public static readonly HttpClientHandler HttpClientHandler = new()
        {
            AllowAutoRedirect = true,
            ServerCertificateCustomValidationCallback = delegate { return true; },
            MaxConnectionsPerServer = int.MaxValue,
            ClientCertificateOptions = ClientCertificateOption.Automatic,
            MaxAutomaticRedirections = int.MaxValue
        };

        public static List<string> Search(int startindex = 10000, int endindex = 20000)
        {
            var validUrlList = new List<string>();
            try
            {
                Parallel.For(startindex, endindex, ParallelOptions, x =>
                {
                    var combineUrl = string.Format(GradioUrl, x);
                    var result = GetHttpResponse(combineUrl).GetAwaiter().GetResult();
                    foreach (var invalidstr in InvalidString)
                    {
                        if (!result.IsVaild(invalidstr))
                        {
                            return;
                        }
                    }
                    foreach (var Validstr in ValidString)
                    {
                        if (result.IsVaild(Validstr))
                        {
                            return;
                        }
                    }
                    validUrlList.Add(combineUrl);
                    Console.WriteLine(combineUrl);
                });
                return validUrlList;
            }
            catch 
            {
                Console.WriteLine("Program has one or more error , please restart application.");
                return validUrlList;
            }
        }

        public static bool IsVaild(this string context, string str)
        {
            if (context.Contains(str, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static async Task<string> GetHttpResponse(string url)
        {
            try
            {
                var httpclient = new HttpClient(HttpClientHandler, false);
                var result = await httpclient.GetStringAsync(url);
                httpclient.Dispose();
                return result;
            }
            catch
            {
                return "HTTP Error/Application from httpclient [FINDER]";
            }
        }
    }
}