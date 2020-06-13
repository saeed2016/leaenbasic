using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sentiment
{
    public class SentimentService : ISentimentService
    {
        const string GitEmojiUrl = "https://raw.githubusercontent.com/KeithGalli/sklearn/master/data/sentiment/Books_small.json";
        private readonly HttpClient _httpClient;
        private IList<Result> _emojis;
        public SentimentService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "KeithGalli/sklearn/data/sentiment");

        }
        public async Task<IList<Result>> GetEmojis()
        {

            if (_emojis == null || _emojis.Count <= 0)
            {
                var emojistr = await _httpClient.GetStringAsync(GitEmojiUrl);
                try
                {
                    _emojis = GetEmojisFrom(emojistr);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"error:{ex.ToString()}");
                }

            }
            return _emojis;
        }

        public IList<Result> GetEmojisFrom(string content)
        {
            var dictionary = JsonConvert.DeserializeObject<Result>(content);
            var results = new List<Result>();

            foreach (Result key in dictionary)
            {
                //  if (string.IsNullOrWhiteSpace(key)) { continue; }
                results.Add(new Result
                (
                     key.reviewerName,
                  key.overall
                )); ;

            }
            return results;
        }

    }
}

