// Ilya Likhoshva
// Ruler
// Tests.cs
// 09.10.2015

using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Threading;

namespace AOP_Ruler
{
    [Serializable]
   public class Answer
    {
        public bool Correct;
        // ReSharper disable once InconsistentNaming
        public string answer;
        public Answer(string answer, bool correct)
        {
            this.answer = answer;
            Correct = correct;
        }
    }
    [Serializable]
    public class Question
    {
        readonly string _quetion;
        readonly List<Answer> _answers;
        public Question(string quetion, List<Answer> answers)
        {
            _quetion = quetion;
            _answers = answers;
            _answers.Shuffle();
        }
        public bool Answer(int number)
        {
            return _answers[number].Correct;
        }
        public string GetQuestion()
        { return _quetion; }
        public List<string> GetAnswers()
        {
            return _answers.Select(answer => answer.answer).ToList();
        }
    }

    public class Tests
    {
        readonly List<Question> _questions = new List<Question>();
        public Tests(string xmlQuetions)
        {
            XmlDocument questions = new XmlDocument();
            questions.Load(xmlQuetions);
            foreach (XmlNode node in questions.GetElementsByTagName("Questions"))
            {
                List<Answer> answers = (from XmlNode nodeansw in node.SelectNodes("Answers") select new Answer(nodeansw.FirstChild.FirstChild.Value,Convert.ToBoolean(int.Parse(nodeansw.LastChild.FirstChild.Value)))).ToList();
                _questions.Add(new Question(node.FirstChild.FirstChild.Value, answers));
            }
            _questions.Shuffle();
        }
        public List<Question> GetQuestions()
        {
            return _questions;
        }
    }



}
public static class ThreadSafeRandom
{
    [ThreadStatic]
    private static Random _local;

    public static Random ThisThreadsRandom => _local ?? (_local = new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId)));
}
static class AddList
{
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}