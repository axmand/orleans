using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.NLP.Application
{
    /// <summary>
    /// 管理事件时间线和句子集
    /// </summary>
    public class ScenarioTimeManager
    {

        readonly static java.lang.Class textAnnotationClass = new edu.stanford.nlp.ling.CoreAnnotations.TextAnnotation().getClass();

        public Dictionary<int, DateTime> Timeline { get; }

        public string RawText { get; }

        public Dictionary<DateTime, List<edu.stanford.nlp.util.CoreMap>> Group { get; }

        /// <summary>
        /// 过滤引用时间（在事件发生前的时间都会被认为是引用修饰状态的时间，不作为时间线处理）
        /// </summary>
        private readonly DateTime _startTime;

        private DateTime _baseline;

        /// <summary>
        /// 事件起始时间，用于过滤非时间发生期的时间点
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="rawText"></param>
        public ScenarioTimeManager(DateTime startTime, string rawText)
        {
            Group = new Dictionary<DateTime, List<edu.stanford.nlp.util.CoreMap>>();
            Timeline = new Dictionary<int, DateTime>();
            RawText = rawText;
            _startTime = startTime;
            _baseline = startTime.Date;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <param name="offset"></param>
        public void AddTimeStamp(DateTime time, int offset)
        {
            //1. 输入日期小于起始日期，此时间线舍弃
            if (_startTime > time) return;
            //2. 修改基准时间线，保证推理时间不变
            if (time.Date > _baseline) //修改时间基准线
                _baseline = time.Date;
            //3. 统一基准时间线
            Timeline[offset] = new DateTime(_baseline.Year, _baseline.Month, _baseline.Day, time.Hour, time.Minute, time.Second, time.Millisecond);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sentence"></param>
        public void AddSentence(edu.stanford.nlp.util.CoreMap sentence)
        {
            DateTime key = Timeline.Values.Count == 0 ? _startTime : Timeline.Values.Last();
            if (!Group.ContainsKey(key)) Group[key] = new List<edu.stanford.nlp.util.CoreMap>();
            Group[key].Add(sentence);
        }

        public List<string> ToDisplay()
        {
            List<string> display = new List<string>();
            Group.Keys.ToList().ForEach(p =>
            {
                //datetime string
                display.Add(p.ToLongDateString() + p.ToLongTimeString());
                Group[p].ForEach(sentence =>
                {
                    string text = sentence.get(textAnnotationClass) as string;
                    display.Add(string.Format("     {0}", text));
                });
            });
            return display;
        }
    }
}
