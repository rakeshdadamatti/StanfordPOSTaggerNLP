using edu.stanford.nlp.tagger.maxent;

//Initializes NLP library
//NuGet: Install-Package Stanford.NLP.POSTagger
//Documentation: https://sergey-tihon.github.io/Stanford.NLP.NET/StanfordPOSTagger.html

namespace CloudCherry
{
   public class PartsOfSpeech
    {
       private static PartsOfSpeech single_instance = null;
       private static dynamic tagger = null;

       private PartsOfSpeech()
       {

       }

       public static dynamic Initialize()
       {
           if (single_instance == null)
           {
               single_instance = new PartsOfSpeech();
               var modelsDirectory = @"D:\projects\microsoft\CloudCherry\CloudCherry\stanford-postagger-full-2016-10-31\models";
               tagger = new MaxentTagger(modelsDirectory + @"\wsj-0-18-bidirectional-nodistsim.tagger");
           }
           return tagger;
       }
    }
}
