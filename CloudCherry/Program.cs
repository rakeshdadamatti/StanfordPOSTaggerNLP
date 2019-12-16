using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json;
using System.Collections;

namespace CloudCherry
{
    class Program
    {

        static string Authorize(string baseUrl,string username,string password)
        {
            CloudCherryAPI cloudCherryAPI = new CloudCherryAPI();
            Task<string> token = Task.Run(async () => await cloudCherryAPI.GetToken(baseUrl, username, password));
            token.Wait();
            return token.Result;
        }

        static HashSet<string> FetchActiveQuestions(string baseUrl,string token)
        {
            CloudCherryAPI cloudCherryAPI = new CloudCherryAPI();
            Task<dynamic> question = cloudCherryAPI.GetActiveQuestions(baseUrl, token);
            HashSet<string> questions = new HashSet<string>();

            if(question.Result != null)
            {
                foreach (var item in question.Result)
                {
                    string displayType = item.displayType;
                    string id = item.id;
                    if (displayType.Equals("MultilineText"))
                    {
                        foreach (var qt in item.questionTags)
                        {
                            string questionTag = qt;
                            if (questionTag.Equals("Comments") || questionTag.Equals("User comments"))
                            {
                                questions.Add(id);
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("No questions found!");
            }

            return questions;
        }

        static string FetchAnswers(string baseUrl, string token, HashSet<string> questions)
        {
            CloudCherryAPI cloudCherryAPI = new CloudCherryAPI();
           
            var payload = new Dictionary<string, string>
            {
              {"afterdate", "01/01/2019"},
              {"beforedate", "01/07/2019"}
            };

            Task<dynamic> answer = Task.Run(async () => await cloudCherryAPI.GetAnswers(baseUrl, token, 1, 1000, payload));
            answer.Wait();

            string text = string.Empty;
            int counter = 0;
            if (answer.Result != null)
            {
                foreach (var responses in answer.Result["responses"])
                {
                    foreach (var item in responses["responses"])
                    {
                        string questionId = item.questionId;
                        string textInput = item.textInput;
                        if (questions.Contains(questionId))
                        {
                            text += textInput;             
                            counter++;
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("No answers found!");
            }
            Console.WriteLine("{0} Answers found.", counter);
            return text;
        }
        static void Main(string[] args)
        {
            string BaseUrl = ConfigurationManager.AppSettings["BaseUrl"];
            string Username = ConfigurationManager.AppSettings["Username"];
            string Password = ConfigurationManager.AppSettings["Password"];
            string Token = string.Empty;

            Console.WriteLine("Authenticating.");
            
            Token = Authorize(BaseUrl, Username, Password);
            
            Console.WriteLine("Token received successfully!");
            Console.WriteLine("Fetching questions.");
            
            HashSet<string> questions = FetchActiveQuestions(BaseUrl, Token);
            
            Console.WriteLine("{0} Question(s) received.", questions.Count);
            Console.WriteLine("Fetching answers.");
            
            string text = FetchAnswers(BaseUrl, Token, questions);
            new WordOccurrence().DisplayWords(text);
        }
    }
}
