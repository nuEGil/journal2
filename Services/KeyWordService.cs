using System;

namespace journal2.Services
{
    public interface IKeywordService
    {
        string[] ExtractKeywords(string text);
    }
    public class KeywordService : IKeywordService
    {
        private readonly string[] _keywords = new[]
        {
            "love", "passion", "faith", "war", "storm",
            "time","seconds","minutes","days","weeks","months","years","hours",
            "mind","mental","memory","memories",
            "think","thinking","thought","thoughts",
            "prayers","pray","prayed","meditate",
            "soul","spirit","dream",
            "body","bodies","blood","guts","spit",
            "vomit","excrement","soiled",
            "eyes","mouth","nose","ears","hands","hand","fingers","finger","feet","toes",
            "heart","stomach","nerve","nervous",
            "neck","chest","breast","shoulders",
            "kill","killed","murder","suicide",
            "death","died","dead",
            "drunk","drink","drinking","smoking",
            "gambling","gamble","gambler","alcoholic",
            "habit","drug","drugs","opium","cocaine",
            "family","father","mother","brother","sister",
            "aunt","uncle","cousin","son","daughter",
        };

        public string[] ExtractKeywords(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return Array.Empty<string>();

            text = text.ToLower();

            var found = new List<string>();

            foreach (var kw in _keywords)
            {
                if (text.Contains(kw))
                    found.Add(kw);
            }

            return found.Count == 0 ? Array.Empty<string>() : found.ToArray();
        }
    }
}
