using System.Collections;
using System.Linq;
using System.Collections.Generic;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.tagger.maxent;
using java.io;
using java.util;
using Console = System.Console;
using System.Text.RegularExpressions;

namespace CloudCherry
{
    public class WordOccurrence
    {
        dynamic tagger = PartsOfSpeech.Initialize();
        public Dictionary<string, int> Count(string text)
        {
            var partsOfSpeech = new List<string> { "PRP", "PRP$", "WP", "WP$", "IN", "PP", "PNP", "CC", "SBAR", "DT" };
            Dictionary<string, int> dict = new Dictionary<string, int>();
            var sentences = MaxentTagger.tokenizeText(new StringReader(Regex.Replace(text, @"[^\w\d\s]", ""))).toArray();
            foreach (List sentence in sentences)
            {
                Console.WriteLine("Pattem Digital: Processing {0} words...",sentence.size());
                var taggedSentence = tagger.tagSentence(sentence).toArray();
                foreach (TaggedWord word in taggedSentence)
                {
                    //Increment the existing key value by 1 
                    //else add to dictionary in case of new word
                    if (!partsOfSpeech.Contains(word.tag()))
                    {
                        int result;
                        if (dict.TryGetValue(word.value(), out result))
                        {
                            dict[word.value()] = result + 1;
                        }
                        else
                        {
                            dict.Add(word.value(), 1);
                        }
                    }
                }
            }
            return dict;
        }

        public void DisplayWords(string sentence)
        {
            Dictionary<string, int> dict = new WordOccurrence().Count(sentence);

            //Sort dictionary to get top 5 repeating words
            var sortedDict = (from entry in dict orderby entry.Value descending select entry)
               .ToDictionary(pair => pair.Key, pair => pair.Value).Take(5);
            Console.WriteLine("==============Top 5 Repeating Words===========");
            foreach (KeyValuePair<string, int> s in sortedDict)
            {
                if (s.Value > 1)
                {
                    Console.WriteLine(s.Key + ":" + s.Value);
                }
            }
            Console.ReadKey();
        }
    }
}
