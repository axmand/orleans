using Engine.ML.Method;
using GrainImplement.NLP.Extend;
using GrainImplement.NLP.Utils;

using System;
using System.Collections.Generic;
using System.Linq;

namespace GrainImplement.NLP.Application
{
    /// <summary>
    /// 分析处理句子集，构建情景
    /// 分析构建以下内容：
    /// 1. tokens
    /// 2. split words
    /// </summary>
    public class ScenarioAnnotator
    {

        #region Annotation ClassName

        /// <summary>
        /// 
        /// </summary>
        readonly static java.lang.Class entityTypeAnnotationClass = new edu.stanford.nlp.ling.CoreAnnotations.EntityTypeAnnotation().getClass();

        /// <summary>
        /// 
        /// </summary>
        readonly static java.lang.Class sentencesAnnotationClass = new edu.stanford.nlp.ling.CoreAnnotations.SentencesAnnotation().getClass();

        /// <summary>
        /// 
        /// </summary>
        readonly static java.lang.Class tokensAnnotationClass = new edu.stanford.nlp.ling.CoreAnnotations.TokensAnnotation().getClass();

        /// <summary>
        /// 
        /// </summary>
        readonly static java.lang.Class textAnnotationClass = new edu.stanford.nlp.ling.CoreAnnotations.TextAnnotation().getClass();

        /// <summary>
        /// 
        /// </summary>
        readonly static java.lang.Class partOfSpeechAnnotationClass = new edu.stanford.nlp.ling.CoreAnnotations.PartOfSpeechAnnotation().getClass();

        /// <summary>
        /// 
        /// </summary>
        readonly static java.lang.Class namedEntityTagAnnotationClass = new edu.stanford.nlp.ling.CoreAnnotations.NamedEntityTagAnnotation().getClass();

        /// <summary>
        /// 识别一些数字型的实体，例如日期，货币等
        /// </summary>
        readonly static java.lang.Class normalizedNamedEntityTagAnnotationClass = new edu.stanford.nlp.ling.CoreAnnotations.NormalizedNamedEntityTagAnnotation().getClass();

        /// <summary>
        /// https://nlp.stanford.edu/software/sutime.shtml
        /// </summary>
        readonly static java.lang.Class timexAnnotationClass = new edu.stanford.nlp.time.TimeAnnotations.TimexAnnotation().getClass();

        /// <summary>
        /// 
        /// </summary>
        readonly static java.lang.Class timeExpressionAnnotationClass = new edu.stanford.nlp.time.TimeExpression.Annotation().getClass();

        /// <summary>
        /// 
        /// </summary>
        readonly static java.lang.Class docDateAnnotationClass = new edu.stanford.nlp.ling.CoreAnnotations.DocDateAnnotation().getClass();

        /// <summary>
        /// 
        /// </summary>
        readonly static java.lang.Class treeAnnotationClass = new edu.stanford.nlp.trees.TreeCoreAnnotations.TreeAnnotation().getClass();

        /// <summary>
        /// https://universaldependencies.org/introduction.html
        /// Universal Dependencies
        /// </summary>
        readonly static java.lang.Class enhancedPlusPlusDependenciesAnnotationClass = new edu.stanford.nlp.semgraph.SemanticGraphCoreAnnotations.EnhancedPlusPlusDependenciesAnnotation().getClass();

        /// <summary>
        /// 句子annotator 入口
        /// </summary>
        readonly static java.lang.Class mentionsAnnotationClass = new edu.stanford.nlp.ling.CoreAnnotations.MentionsAnnotation().getClass();

        /// <summary>
        /// https://nlp.stanford.edu/nlp/javadoc/javanlp/edu/stanford/nlp/ling/CoreAnnotations.CanonicalEntityMentionIndexAnnotation.html
        /// </summary>
        readonly static java.lang.Class canonicalEntityMentionIndexAnnotation = new edu.stanford.nlp.ling.CoreAnnotations.CanonicalEntityMentionIndexAnnotation().getClass();

        /// <summary>
        /// EntityTypeAnnotation, 可以找到DATE的标注，进行句子划分
        /// </summary>
        readonly static java.lang.Class entityTypeAnnotation = new edu.stanford.nlp.ling.CoreAnnotations.EntityTypeAnnotation().getClass();

        /// <summary>
        /// condfidence
        /// </summary>
        readonly static java.lang.Class namedEntityTagProbsAnnotation = new edu.stanford.nlp.ling.CoreAnnotations.NamedEntityTagProbsAnnotation().getClass();

        /// <summary>
        /// begin pisition
        /// </summary>
        readonly static java.lang.Class tokenBeginAnnotation = new edu.stanford.nlp.ling.CoreAnnotations.TokenBeginAnnotation().getClass();

        /// <summary>
        /// end position
        /// </summary>
        readonly static java.lang.Class tokenEndAnnotation = new edu.stanford.nlp.ling.CoreAnnotations.TokenEndAnnotation().getClass();

        /// <summary>
        /// openie
        /// </summary>
        readonly static java.lang.Class openieAnnotation = new edu.stanford.nlp.naturalli.NaturalLogicAnnotations.RelationTriplesAnnotation().getClass();

        #endregion

        /// <summary>
        /// 设置置信度阈值
        /// </summary>
        readonly double CONFIDENCE = 0.7;

        /// <summary>
        /// indicate the ability of annotator
        /// </summary>
        readonly java.util.Properties _props;

        /// <summary>
        /// 词转向量模型
        /// </summary>
        readonly IEmbeddingNetwork _net;

        /// <summary>
        ///  Annotation with SUTime
        /// </summary>
        public ScenarioAnnotator(IEmbeddingNetwork net)
        {
            //词向量模型
            _net = net;
            //annotate properites
            _props = new java.util.Properties();
            //refrenece https://stanfordnlp.github.io/CoreNLP/annotators.html
            _props.setProperty("annotators",
                //tokenize https://stanfordnlp.github.io/CoreNLP/tokenize.html
                "tokenize, " +
                //https://stanfordnlp.github.io/CoreNLP/cleanxml.html
                //"cleanxml, " +
                //ssplit https://stanfordnlp.github.io/CoreNLP/ssplit.html
                "ssplit, " +
                //part of speech https://stanfordnlp.github.io/CoreNLP/pos.html
                "pos, " +
                //lemma https://stanfordnlp.github.io/CoreNLP/lemma.html
                "lemma, " +
                //named entity recongnition https://stanfordnlp.github.io/CoreNLP/ner.html
                "ner, " +
                //depparse https://stanfordnlp.github.io/CoreNLP/parse.html
                "depparse, " +
                //Open Information Extraction https://stanfordnlp.github.io/CoreNLP/openie.html
                "openie");
        }

        /// <summary>
        /// 执行annotate操作，分析此段话的时间区间
        /// 归纳事件的时间序列
        /// </summary>
        /// <param name="rawText">待处理文本</param>
        /// <param name="beginDateTime">起始日期, 默认使用系统当前时间，建议设置为事件发生日期时间</param>
        public ScenarioTimeManager InductiveEventTimeSeries(string rawText, string beginDateTime = null)
        {
            if (rawText == null || rawText.Length == 0) return null;
            //annotate text
            edu.stanford.nlp.pipeline.StanfordCoreNLPClient pipeline = new edu.stanford.nlp.pipeline.StanfordCoreNLPClient(_props, NLPConfiguration.CoreNLPAddress, Convert.ToInt32(NLPConfiguration.CoreNLPPort));
            edu.stanford.nlp.pipeline.Annotation document = new edu.stanford.nlp.pipeline.Annotation(rawText);
            //date format and set for reference
            string formateDate = beginDateTime != null ? Convert.ToDateTime(beginDateTime).ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");
            document.set(docDateAnnotationClass, formateDate);
            //annotate timex
            pipeline.annotate(document);
            java.util.AbstractList sentences = document.get(sentencesAnnotationClass) as java.util.AbstractList;
            if (sentences == null) return null;
            //create scenario manager (timeline, scenario, info, etc)
            ScenarioTimeManager stManager = new ScenarioTimeManager(Convert.ToDateTime(formateDate), rawText);
            //1. 分析时间序列
            foreach (edu.stanford.nlp.util.CoreMap sentence in sentences)
            {
                //}{debug 展示句子内容
                string text = (string)sentence.get(textAnnotationClass);
                //edu.stanford.nlp.util, edu.stanford.nlp.coref.data.Mention
                var mentions = sentence.get(mentionsAnnotationClass) as java.util.AbstractList;
                //从mentions entites里找到EntityTypeAnnotation
                foreach (edu.stanford.nlp.util.CoreMap anno in mentions)
                {
                    string entityType = (string)anno.get(entityTypeAnnotation);
                    //reference : https://nlp.stanford.edu/pubs/lrec2012-sutime.pdf
                    if (entityType == "DATE") //date without time
                    {
                        //hashmap
                        java.util.HashMap probHash = anno.get(namedEntityTagProbsAnnotation) as java.util.HashMap;
                        //extract information
                        double prob = Convert.ToDouble(probHash.get(entityType).ToString()); //java.lang.Double -> string -> double
                        if (prob < CONFIDENCE) continue;
                        int offset = (anno.get(tokenBeginAnnotation) as java.lang.Integer).intValue(); //begin offset
                        string normalizedTimex = (string)anno.get(normalizedNamedEntityTagAnnotationClass);
                        if (!normalizedTimex.Contains("T")) //不包含TIME的DATE可能是综述性时间，需要加入记录
                        {
                            DateTime dtime = normalizedTimex.ToDateTime();
                            stManager.AddTimeStamp(dtime, offset); //置信度标注
                        }
                    }
                    else if (entityType == "TIME") // a time point indicating a particular instance on a time scale
                    {
                        //hashmap
                        java.util.HashMap probHash = anno.get(namedEntityTagProbsAnnotation) as java.util.HashMap;
                        //extract information
                        double prob = Convert.ToDouble(probHash.get(entityType).ToString()); //java.lang.Double -> string -> double
                        if (prob < CONFIDENCE) continue;
                        int offset = (anno.get(tokenBeginAnnotation) as java.lang.Integer).intValue(); //begin offset
                        string normalizedTimex = (string)anno.get(normalizedNamedEntityTagAnnotationClass);
                        DateTime dtime = normalizedTimex.TimeExpression().ToDateTime();
                        stManager.AddTimeStamp(dtime, offset); //置信度标注
                    }
                    #region Duration
                    //else if (entityType == "DURATION") // the amount of intervening time between the two end-points of a time interval
                    //{
                    //    java.util.HashMap probHash = anno.get(namedEntityTagProbsAnnotation) as java.util.HashMap;
                    //    double prob = Convert.ToDouble(probHash.get(entityType).ToString()); //java.lang.Double -> string -> double
                    //    int offset = (anno.get(tokenBeginAnnotation) as java.lang.Integer).intValue(); //begin offset
                    //    string normalizedTimex = (string)anno.get(normalizedNamedEntityTagAnnotationClass);
                    //    //set defaultduration information
                    //    int days = 0, hours =0 ,minutes =0 ,seconds = 0, milliseconds = 0;
                    //    //Exact
                    //    if (!normalizedTimex.Contains("/")) //duration ranges are not part of TIMEX3 standard
                    //        if(!normalizedTimex.Contains("X")) //Inexact time
                    //        {
                    //            if (normalizedTimex.Contains("D"))
                    //                days = Convert.ToInt32(normalizedTimex.Replace('P', ' ').Replace('D', ' ').Trim());
                    //            else if (normalizedTimex.Contains("H"))
                    //                hours = Convert.ToInt32(normalizedTimex.Replace('P', ' ').Replace('T',' ').Replace('H', ' ').Trim());
                    //        }
                    //    //create timespan
                    //    TimeSpan span = new TimeSpan(days, hours, minutes, seconds, milliseconds);
                    //    if (prob > CONFIDENCE) stManager.AddTimeStamp(span, offset); //置信度标注
                    //}
                    #endregion
                }
                stManager.AddSentence(sentence);
            }
            return stManager;
        }

        /// <summary>
        /// find the sibling relation of target word from sentence
        /// </summary>
        /// <param name="sentence"></param>
        /// <param name="word"></param>
        /// <returns>modifier words, relation type</returns>
        public List<(edu.stanford.nlp.ling.IndexedWord, string)> FindTargetIndexWordModifier(edu.stanford.nlp.util.CoreMap sentence, edu.stanford.nlp.ling.IndexedWord word)
        {
            List<(edu.stanford.nlp.ling.IndexedWord, string)> modifiers = new List<(edu.stanford.nlp.ling.IndexedWord, string)>();
            edu.stanford.nlp.semgraph.SemanticGraph dependencies = sentence.get(enhancedPlusPlusDependenciesAnnotationClass) as edu.stanford.nlp.semgraph.SemanticGraph;
            List<edu.stanford.nlp.trees.TypedDependency> deps = FindRefs(dependencies, word.backingLabel());
            foreach (edu.stanford.nlp.trees.TypedDependency dep in deps)
            {
                string relationType = dep.reln().ToString();
                switch (relationType)
                {
                    case "nummod":  //数值修饰词，numeric modifier，搜索是否还存在单位修饰, 比如 200吨，10个
                    case "amod":       //形容词修饰词，例如：红花-红
                    case "nmod:of":   //of关系名词组，例如：the king of night
                    case "nmod:to":   //to关系，例如：go to school, came to party
                    case "nmod:into"://名词修饰短语，例如：Investigation into the cause of the collision
                    case "nmod:for":  //for关系名词修饰短语, vehicle inspections for damage assessment
                    case "compound"://multiword expression (MVE)形式
                    case "advmod":    //副词修饰 safely anchored
                        modifiers.Add((dep.dep(), relationType));
                        break;
                };
            }
            return modifiers;
        }

        /// <summary>
        /// 分别转换情景成词向量
        /// </summary>
        /// <param name="manager"></param>
        /// <returns></returns>
        public List<double[]> ToScenarioWordVector(ScenarioTimeManager manager)
        {
            List<double[]> list = new List<double[]>();
            manager.Group.Keys.ToList().ForEach(key =>
            {
                //each key has a scenario depict
                List<edu.stanford.nlp.util.CoreMap> group = manager.Group[key];
                group.ForEach(sentence =>
                {
                    java.util.AbstractList tokens = sentence.get(tokensAnnotationClass) as java.util.AbstractList;
                    foreach (edu.stanford.nlp.ling.CoreLabel lable in tokens)
                    {
                        string word = lable.value();
                        double[] vt = _net.ToDouble(word);
                        if (vt != null) list.Add(vt);
                    }
                });
            });
            return list;
        }

        /// <summary>
        /// reference:
        /// https://stanfordnlp.github.io/CoreNLP/api.html
        /// 生成情景
        /// 解析每组中的sentence dep
        /// </summary>
        /// <param name="manager"></param>
        public List<Scenario> GenerateScenarios(ScenarioTimeManager manager)
        {
            string id = "S0";
            List<Scenario> scenarios = new List<Scenario>();
            manager.Group.Keys.ToList().ForEach(key =>
            {
                //each key has a scenario depict
                List<edu.stanford.nlp.util.CoreMap> group = manager.Group[key];
                Scenario scenario = new Scenario(_net, id);
                //逐句子处理，拼凑情景元素值
                group.ForEach(sentence => ElementExtractByDependencyPrase(sentence, scenario));
                scenarios.Add(scenario);
                //更新事件链id
                id += "1";
            });
            return scenarios;
        }

        /// <summary>
        /// 对修饰词处理
        /// </summary>
        private void LabelingModifiers(edu.stanford.nlp.ling.IndexedWord word, edu.stanford.nlp.util.CoreMap sentence, Scenario scenario)
        {
            if (word == null) return;
            List<(edu.stanford.nlp.ling.IndexedWord, string)> modifiers = word != null ? FindTargetIndexWordModifier(sentence, word) : null;
            string key = word.value();
            string value = "";
            string relation = "";
            //1.根据ner，做最基本判断
            foreach (var (modifier, typeName) in modifiers)
                switch (typeName)
                {
                    case "amod": //形容词修饰关系
                        {
                            value = modifier.value();
                            relation = "amod";
                        }
                        break;
                    case "advmod": //副词修饰关系
                        {
                            //以为 modifier 为核心搜索修饰关系， 如果存在进一步的修饰关系，则此副词为relation
                            relation = "advmod";
                            edu.stanford.nlp.ling.IndexedWord v = modifier;
                        }
                        break;
                    case "nummod": //额外搜寻修饰单位
                        {
                            edu.stanford.nlp.ling.IndexedWord unit = FindNumericUnit(sentence, modifier);
                            value = (string)modifier.get(normalizedNamedEntityTagAnnotationClass);
                            relation = "count";
                        }
                        break;
                    case "compound":  //名词短语，连接key
                        {
                            key = key.InsertCompound(modifier.value());
                        }
                        break;
                }
            //1. 聚类算法，计算 relation-triple 与哪个表格匹配
            IScenarioFactor pFactor = scenario.ClusterFactor(relation, key, value);
            if (pFactor != null) scenario.MergeValue(relation, key, value, pFactor);
        }

        /// <summary>
        /// 重塑 relation triple
        /// 转变成便于海洋溢油情景提取需要的 relation triple
        /// </summary>
        /// <param name="oldRelation"></param>
        /// <param name="oldOriginal"></param>
        /// <param name="oldTarget"></param>
        /// <returns></returns>
        private (string relation, string original, string target) ReFactorRelationTriple(string key, string value, string oldTarget)
        {
            //1. 数字修饰关系，根据修饰单位修改state



            //
            return (null, null, null);
        }

        /// <summary>
        /// https://universaldependencies.org/u/overview/syntax.html
        /// analysis the dependency in sentenc, and then extract information
        /// https://nlp.stanford.edu/software/dependencies_manual.pdf
        /// </summary>
        private void ElementExtractByDependencyPrase(edu.stanford.nlp.util.CoreMap sentence, Scenario scenario)
        {
            //result properties
            List<string> plines = new List<string>();
            List<(edu.stanford.nlp.util.CoreMap, edu.stanford.nlp.ling.IndexedWord, edu.stanford.nlp.ling.IndexedWord, edu.stanford.nlp.ling.IndexedWord)> samplings = StepSamplingDependency(sentence);
            samplings.ForEach(sampling =>
            {
                //nominal modifier (名词修饰语）
                edu.stanford.nlp.util.CoreMap samplingSentence;
                edu.stanford.nlp.ling.IndexedWord subject, predicate, objective;
                (samplingSentence, subject, predicate, objective) = sampling;
                //1.处理修饰语
                LabelingModifiers(subject, samplingSentence, scenario);
                LabelingModifiers(predicate, samplingSentence, scenario);
                LabelingModifiers(objective, samplingSentence, scenario);
                //3. 处理谓语
                LabelingPredicate(subject, predicate, objective, samplingSentence, scenario);
            });
        }

        private void ProcessOpenIERelationTriple(edu.stanford.nlp.util.CoreMap sentence)
        {
            java.util.ArrayList array = sentence.get(openieAnnotation) as java.util.ArrayList;
            foreach (edu.stanford.nlp.ie.util.RelationTriple element in array)
            {
                var sss = element;
            }
        }

        /// <summary>
        /// 谓语本身用途主要是是修饰主语，
        /// 如果符合要求则构建 :
        /// relation triple "state (s, t)"
        /// </summary>
        /// <param name="scenario"></param>
        /// <param name="samplingSentence"></param>
        /// <param name="subject"></param>
        /// <param name="predicate"></param>
        private void LabelingPredicate(edu.stanford.nlp.ling.IndexedWord subject, edu.stanford.nlp.ling.IndexedWord predicate, edu.stanford.nlp.ling.IndexedWord objective, edu.stanford.nlp.util.CoreMap samplingSentence, Scenario scenario)
        {
            string pos = predicate?.get(partOfSpeechAnnotationClass) as string;
            string key = "", relation = "", value = "";
            switch (pos)
            {
                //表示主语的状态和趋势
                case "VBD":
                case "VBG":
                case "VBN":
                case "VBP":
                case "VBZ":
                    {
                        if (objective == null)
                        {
                            key = subject.value();
                            relation = "state";
                            value = predicate.value();
                        }
                        else
                        {
                            key = subject.value();
                            relation = predicate.value();
                            value = objective.value();
                        }
                    }
                    break;
                default:
                    LabelingModifiers(predicate, samplingSentence, scenario);
                    break;
            }
            IScenarioFactor fa = scenario.ClusterFactor(relation, key, value);
            if (fa != null) scenario.MergeValue(relation, key, value, fa);
        }

        /// <summary>
        /// https://universaldependencies.org/u/dep/index.html
        /// Stratify a sentence according to the subject-predicate relationship
        /// </summary>
        /// <param name="sentence"></param>
        /// <returns>sentence, subject, predicate, objective</returns>
        public List<(edu.stanford.nlp.util.CoreMap, edu.stanford.nlp.ling.IndexedWord, edu.stanford.nlp.ling.IndexedWord, edu.stanford.nlp.ling.IndexedWord)> StepSamplingDependency(edu.stanford.nlp.util.CoreMap sentence)
        {
            List<(edu.stanford.nlp.util.CoreMap, edu.stanford.nlp.ling.IndexedWord, edu.stanford.nlp.ling.IndexedWord, edu.stanford.nlp.ling.IndexedWord)> samplings = new List<(edu.stanford.nlp.util.CoreMap, edu.stanford.nlp.ling.IndexedWord, edu.stanford.nlp.ling.IndexedWord, edu.stanford.nlp.ling.IndexedWord)>();
            edu.stanford.nlp.semgraph.SemanticGraph dependencies = sentence.get(enhancedPlusPlusDependenciesAnnotationClass) as edu.stanford.nlp.semgraph.SemanticGraph;
            java.util.Collection typedDependencies = dependencies.typedDependencies();
            java.util.Iterator itr = typedDependencies.iterator();
            while (itr.hasNext())
            {
                edu.stanford.nlp.trees.TypedDependency td = itr.next() as edu.stanford.nlp.trees.TypedDependency;
                string relationType = td.reln().getShortName();
                //Nominals
                if (relationType == "nsubj" || relationType == "nsubjpass")
                {
                    edu.stanford.nlp.ling.IndexedWord subject, predicate, objective;
                    subject = td.dep(); predicate = td.gov();
                    //在谓语位置上，缺不满足谓语角色的词，此种谓语可缺省
                    edu.stanford.nlp.ling.IndexedWord expl = FindIndexedWordByDependencyType(dependencies, predicate, "expl");
                    predicate = expl == null ? predicate : null;
                    //直接宾语,
                    objective = FindIndexedWordByDependencyType(dependencies, predicate, "obj", "dobj");
                    //动词或形容词的补语做宾语, open clausal complement
                    objective = objective ?? FindIndexedWordByDependencyType(dependencies, predicate, "ccomp", "xcomp");
                    //加入层次集合
                    samplings.Add((sentence, subject, predicate, objective));
                }
            }
            return samplings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dependencies"></param>
        /// <param name="token"></param>
        /// <param name="depTypeString"></param>
        /// <returns></returns>
        private edu.stanford.nlp.ling.IndexedWord FindIndexedWordByDependencyType(edu.stanford.nlp.semgraph.SemanticGraph dependencies, edu.stanford.nlp.ling.IndexedWord word, params string[] depTypeString)
        {
            if (dependencies == null || word == null) return null;
            List<edu.stanford.nlp.trees.TypedDependency> deps = FindRefs(dependencies, word.backingLabel());
            foreach (edu.stanford.nlp.trees.TypedDependency dep in deps)
                if (depTypeString.Contains(dep.reln().getShortName()))
                    if (dep.gov() == word)
                        return dep.dep();
            return null;
        }

        /// <summary>
        /// 搜索与target token相关的dpendency关系集合
        /// </summary>
        /// <param name="dependencies"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private List<edu.stanford.nlp.trees.TypedDependency> FindRefs(edu.stanford.nlp.semgraph.SemanticGraph dependencies, edu.stanford.nlp.ling.CoreLabel token)
        {
            List<edu.stanford.nlp.trees.TypedDependency> tds = new List<edu.stanford.nlp.trees.TypedDependency>();
            string tokenValue = token.ToString();
            java.util.Collection typedDependencies = dependencies.typedDependencies();
            java.util.Iterator itr = typedDependencies.iterator();
            while (itr.hasNext())
            {
                edu.stanford.nlp.trees.TypedDependency td = itr.next() as edu.stanford.nlp.trees.TypedDependency;
                string tdValue = td.toString();
                if (tdValue.IndexOf(tokenValue) != -1) tds.Add(td);
            }
            return tds;
        }

        /// <summary>
        /// 搜索
        /// </summary>
        private edu.stanford.nlp.ling.IndexedWord FindNumericUnit(edu.stanford.nlp.util.CoreMap sentence, edu.stanford.nlp.ling.IndexedWord word)
        {
            edu.stanford.nlp.semgraph.SemanticGraph dependencies = sentence.get(enhancedPlusPlusDependenciesAnnotationClass) as edu.stanford.nlp.semgraph.SemanticGraph;
            List<edu.stanford.nlp.trees.TypedDependency> tds = FindRefs(dependencies, word.backingLabel());
            foreach (edu.stanford.nlp.trees.TypedDependency td in tds)
            {

            }
            return null;
        }

        /// <summary>
        /// 搜索token的直接关联关系
        /// </summary>
        /// <param name="sentence"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private List<edu.stanford.nlp.trees.TypedDependency> FindDeirctRefs(edu.stanford.nlp.util.CoreMap sentence, edu.stanford.nlp.ling.CoreLabel token)
        {
            string tokenValue = token.ToString();
            List<edu.stanford.nlp.trees.TypedDependency> tds = new List<edu.stanford.nlp.trees.TypedDependency>();
            edu.stanford.nlp.semgraph.SemanticGraph dependencies = sentence.get(enhancedPlusPlusDependenciesAnnotationClass) as edu.stanford.nlp.semgraph.SemanticGraph;
            java.util.Collection typedDependencies = dependencies.typedDependencies();
            java.util.Iterator itr = typedDependencies.iterator();
            while (itr.hasNext())
            {
                edu.stanford.nlp.trees.TypedDependency td = itr.next() as edu.stanford.nlp.trees.TypedDependency;
                string tdValue = td.toString();
                if (tdValue.IndexOf(tokenValue) != -1) tds.Add(td);
            }
            return tds;
        }

        /// <summary>
        /// 因为句子并非要求语法上严格正确，所有经常出现错误的结果，但是结果不会变化
        /// </summary>
        /// <returns></returns>
        private List<edu.stanford.nlp.trees.TypedDependency> FindDeptypeFromSentence(edu.stanford.nlp.util.CoreMap sentence, string depTypeString)
        {
            List<edu.stanford.nlp.trees.TypedDependency> tds = new List<edu.stanford.nlp.trees.TypedDependency>();
            edu.stanford.nlp.semgraph.SemanticGraph dependencies = sentence.get(enhancedPlusPlusDependenciesAnnotationClass) as edu.stanford.nlp.semgraph.SemanticGraph;
            java.util.Collection typedDependencies = dependencies.typedDependencies();
            java.util.Iterator itr = typedDependencies.iterator();
            while (itr.hasNext())
            {
                edu.stanford.nlp.trees.TypedDependency td = itr.next() as edu.stanford.nlp.trees.TypedDependency;
                if (td.reln().getShortName() == depTypeString) tds.Add(td);
            }
            return tds;
        }
    }
}
