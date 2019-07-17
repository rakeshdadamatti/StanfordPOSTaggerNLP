# StanfordTaggerNLP
To find out top 5 repeating words(Non Prepositions, Pronouns, Conjunctions) from a huge dataset

# Requirements
1. Stanford POSTagger NLP library.

   NuGet : ```Install-Package Stanford.NLP.POSTagger```

# Library Documentation
https://sergey-tihon.github.io/Stanford.NLP.NET/StanfordPOSTagger.html

# Implementation
1. Convert the input string into a List & pass it on to the POSTagger library.
2. POSTagger will tag parts of speech against each words.
3. Skip the words which are Prepositions, Pronouns, Conjunctions & store the remaining into a dictionary. While adding the words to a dictionary, take word as key & their count as values.
   If a word is repeating then update the value w.r.t the word else add it to the dictionary incase of new word.
 
