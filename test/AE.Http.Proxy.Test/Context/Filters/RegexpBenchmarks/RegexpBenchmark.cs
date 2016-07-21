namespace AE.Http.Proxy.Test.Context.Filters.RegexpBenchmarks
{
    using System;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Running;

    using Xunit;

    public class RegexpBenchmark
    {
        private static readonly string pattern = string.Format(@"(?:href|src)=([""'])({0}|(?!mailto|http))\/\w.*?([""'])", "http://pl2.mol2.mednet.world/");

        private static Regex regex;

        static RegexpBenchmark()
        {
            regex = new Regex(pattern, RegexOptions.Compiled);
        }

        [Fact(Skip = "Only manual benchmark")]
        public void Regexp_benchmark()
        {
           BenchmarkRunner.Run<RegexpBenchmark>();
        }

        [Benchmark]
        public void Default_Regexp_Replace_Complex_Evaluator()
        {
            regex.Replace(BenchmarkDataConstants.TestDataString, ComplexEvaluator);
        }

        [Benchmark]
        public void Default_Regexp_Replace_Simple_Evaluator()
        {
            regex.Replace(BenchmarkDataConstants.TestDataString, SimpleEvaluator);
        }

        [Benchmark]
        public void Default_Regexp_Replace_Complex_Evaluator_splited_content_in_parallel()
        {
            var contents = BenchmarkDataConstants.TestDataString.Split(new[] { " </" }, StringSplitOptions.RemoveEmptyEntries);
            Parallel.ForEach(contents, c => regex.Replace(c, ComplexEvaluator));
            var content = string.Join(">", contents);
        }

        [Benchmark]
        public void Default_Regexp_Replace_Simple_Evaluator_splited_content_in_parallel()
        {
            var contents = BenchmarkDataConstants.TestDataString.Split(new[] { " </" }, StringSplitOptions.RemoveEmptyEntries);
            Parallel.ForEach(contents, c => regex.Replace(c, SimpleEvaluator));
            var content = string.Join(">", contents);
        }

        private static string SimpleEvaluator(Match match)
        {
            return SimpleEvaluator(match.Value);
        }

        private static string SimpleEvaluator(string matchValue)
        {
            return matchValue.Replace('=', '0');
        }

        private static string ComplexEvaluator(Match match)
        {
            return ComplexEvaluator(match.Value);
        }

        private static string ComplexEvaluator(string matchValue)
        {
            var delimiter = matchValue[matchValue.IndexOf('=') + 1];

            var parts = matchValue.Split(delimiter);
            var route = "/pfm";

            if (parts[1].Contains("http://pl2.mol2.mednet.world/"))
            {
                return matchValue.Replace("http://pl2.mol2.mednet.world/", Combine("http://localhost:1234/", route));
            }

            return string.Format(@"{0}{2}{1}{2}", parts[0], Combine(route, parts[1]), delimiter);
        }

        private static string Combine(string uri1, string uri2)
        {
            uri1 = uri1.TrimEnd('/');
            uri2 = uri2.TrimStart('/');
            return string.Format("{0}/{1}", uri1, uri2);
        }
    }
}