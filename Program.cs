using System.Collections.Generic;
using System.Linq;

// https://app.codility.com/programmers/challenges/
namespace CodilityYearOfTiger
{
    public class Solution
    {
        private static void Main(string[] args)
        {
        }

        public int solution(string[] T)
        {
            var separated = Separate(T);
            var alikeDict = Alike(separated);
            var result = 0;
            foreach (var uniform in alikeDict)
            {
                var maxSum = uniform.Value.Sum(x => x.Value);
                if (result >= maxSum) continue;
                var r = Calculate(uniform.Value);

                if (r > result) result = r;
            }

            return result;
        }

        // Group by exact colors
        private static Dictionary<string, int> Separate(string[] t)
        {
            var dict = new Dictionary<string, int>();
            foreach (var s in t)
                if (dict.ContainsKey(s))
                    dict[s] = dict[s] + 1;
                else
                    dict.Add(s, 1);
            return dict;
        }

        //Possibly same
        private static Dictionary<string, List<KeyValuePair<string, int>>> Alike(Dictionary<string, int> pairs)
        {
            var dict = new Dictionary<string, List<KeyValuePair<string, int>>>();
            foreach (var key in pairs)
            {
                var ok = OrderedKey(key.Key);
                if (dict.ContainsKey(ok))
                    dict[ok].Add(key);
                else
                    dict.Add(ok, new List<KeyValuePair<string, int>> { key });
            }

            foreach (var v in dict.Values) FillKeys(v);
            return dict;
        }

        private static void FillKeys(List<KeyValuePair<string, int>> lst)
        {
            if (lst.Count == 3 || lst.Count == 0) return;
            var oc = OrderedKey(lst.FirstOrDefault().Key);
            var v = oc;
            if (!lst.Any(x => x.Key.Equals(v))) lst.Add(new KeyValuePair<string, int>(v, 0));
            v = $"{oc[1]}{oc[0]}{oc[2]}";
            if (!lst.Any(x => x.Key.Equals(v))) lst.Add(new KeyValuePair<string, int>(v, 0));
            v = $"{oc[0]}{oc[2]}{oc[1]}";
            if (!lst.Any(x => x.Key.Equals(v))) lst.Add(new KeyValuePair<string, int>(v, 0));
        }

        public static string OrderedKey(string key)
        {
            return string.Concat(key.OrderBy(c => c));
        }

        public static int Calculate(List<KeyValuePair<string, int>> variants)
        {
            var dict = new Dictionary<string, int>();
            variants.ForEach(v =>
            {
                dict.Add(v.Key, variants.Where(i => AreEqual(i.Key, v.Key))
                    .Sum(x => x.Value));
            });
            return dict.Max(x => x.Value);
        }

        public static bool AreEqual(string key1, string key2)
        {
            if (key1.Equals(key2)) return true;
            if ($"{key1[1]}{key1[0]}{key1[2]}".Equals(key2)) return true;
            if ($"{key1[0]}{key1[2]}{key1[1]}".Equals(key2)) return true;
            return false;
        }
    }
}