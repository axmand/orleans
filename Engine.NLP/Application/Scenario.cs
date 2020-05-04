using Engine.Brain.Method;
using Engine.Brain.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Engine.NLP.Application
{

    /// <summary>
    ///  情景定义:
    ///  情景由情景元素组成，在应急灾害领域，情景元素被分为三类：
    ///  1. 致灾因子(H, hazard)
    ///  2. 承灾体 (E, )
    ///  3. 救援活动 （B, human behavior）
    /// </summary>
    public class Scenario
    {
        public string Name { get; }

        private const float CONFIDENCE = 0.8f;

        private readonly IDEmbeddingNet _net;

        List<IScenarioFactor> _factors = new List<IScenarioFactor>();

        public Scenario(IDEmbeddingNet net, string id = null)
        {
            Name = id;
            _net = net;
            //
            _factors.Add(EShip);
            _factors.Add(HOlislick);
            _factors.Add(HCrudeOil);
        }

        public List<string> ToDisplay()
        {
            return new List<string>() {
                EShip.ToString(),
                HOlislick.ToString()
            };
        }
        
        /// <summary>
        /// 聚类算法，用来匹配表
        /// </summary>
        /// <param name="matchProperty"></param>
        /// <param name="original"></param>
        /// <param name="target"></param>
        public IScenarioFactor ClusterFactor(string matchProperty, string original, string target)
        {
            //1.根据matchPropery类型，设置matchKey, 这里先假定为 original (subject)
            string fieldKey = original;
            Dictionary<float?, IScenarioFactor> dict = new Dictionary<float?, IScenarioFactor>();

            //逐个与表的匹配字段进行对比
            _factors.ForEach( (fr) => {
                var (p, f) = IntersectProbability(fr as BaseScenarioFactor, fieldKey);
                dict[p] = f;
            });
            //聚类可视化



            //排序，找到合适的属性
            float? key = dict.Keys.ToList().OrderBy(o => o).Where(o => o > CONFIDENCE).ToList()?.FirstOrDefault();
            if (key == null) return null;
            //寻找合适的类型
            IScenarioFactor factor = dict[key];
            return factor;
        }

        /// <summary>
        /// MergeValue
        /// </summary>
        /// <param name="matchProperty"></param>
        /// <param name="subject"></param>
        /// <param name="value"></param>
        public void MergeValue(string matchProperty, string subject, string value, IScenarioFactor fa)
        {
            foreach (string propertyName in fa.Properties)
            {
                float[] mv = _net.ToFloat(subject);
                float[] pv = _net.ToFloat(propertyName.ToLower());
                float p = NP.Cosine(mv, pv);
                //为属性设置值
                if (p > CONFIDENCE) fa.SetPerperty(propertyName, value);
            }
        }

        private (float p, T target) IntersectProbability<T>(T target, string subject) where T : BaseScenarioFactor
        {
            float p = 0.0f;
            foreach (string word in target.CoreWrod)
            {
                float[] sv = _net.ToFloat(subject);
                float[] tv = _net.ToFloat(word);
                float p1 = NP.Cosine(sv, tv);
                p = p1 > p ? p1 : p;
            }
            return (p, target);
        }

        #region 致灾因子

        /// <summary>
        /// Hazard 油膜
        /// </summary>
        public OilSlick HOlislick { get; private set; } = new OilSlick();

        /// <summary>
        /// Hazard 原油
        /// </summary>
        public CrudeOil HCrudeOil { get; private set; } = new CrudeOil();

        #endregion

        #region 承灾体

        /// <summary>
        /// Exposure 船
        /// </summary>
        public TransportVessel EShip { get; private set; } = new TransportVessel();

        #endregion
    }

    /// <summary>
    /// 情景元素
    /// </summary>
    public interface IScenarioFactor
    {
        /// <summary>
        /// 领域聚类词汇（描述领域本身）
        /// </summary>
        List<string> CoreWrod { get; }

        /// <summary>
        /// 领域的描述属性，表达领域特性
        /// </summary>
        List<string> Properties { get; }

        /// <summary>
        /// 领域特性属性赋值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void SetPerperty(string name, string value);
    }

    /// <summary>
    /// 空委托
    /// </summary>
    public class BaseScenarioFactor : IScenarioFactor
    {
        /// <summary>
        /// 用于匹配的关键词
        /// </summary>
        public List<string> CoreWrod { get; private set; } = new List<string>();

        /// <summary>
        /// 置信度阈值
        /// </summary>
        private double CONFIDENCE = 0.8;

        /// <summary>
        /// 获取properties
        /// </summary>
        public List<string> Properties
        {
            get
            {
                List<string> properties = new List<string>();
                foreach (PropertyInfo p in this.GetType().GetProperties())
                    properties.Add(p.Name);
                properties.Remove("CoreWrod");
                properties.Remove("Properties");
                return properties;
            }
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SetPerperty(string name, string value)
        {
            PropertyInfo p = this.GetType().GetProperty(name);
            p.SetValue(this, value, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string text = string.Format("table: {0} ---", this.GetType().Name);
            foreach (PropertyInfo p in this.GetType().GetProperties())
                if (p.Name != "CoreWrod" && p.Name != "Properties")
                    text += string.Format("Attribute: {0}, Value: {1} |", p.Name, p.GetValue(this));
            return text;
        }
    }

    #region Hazard

    /// <summary>
    /// 致灾因子-油膜
    /// </summary>
    public class OilSlick : BaseScenarioFactor
    {
        /// <summary>
        /// 
        /// </summary>
        public OilSlick()
        {
            // 溢油基础关键词
            CoreWrod.AddRange(new List<string> {
                "oil",
                "spill"
            });
        }

        /// <summary>
        /// 油膜长度
        /// </summary>
        public string Length { get; set; }

        /// <summary>
        /// 油膜宽度
        /// </summary>
        public string Width { get; set; }

        /// <summary>
        /// 油膜面积
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 油膜厚度
        /// </summary>
        public string Thickness { get; set; }

        /// <summary>
        /// 油膜颜色
        /// </summary>
        public string Color { get; set; }
    }

    public class CrudeOil : BaseScenarioFactor
    {
        public CrudeOil()
        {
            CoreWrod.AddRange(new List<string> {
                "oil",
                "crude"
            });
        }
    }

    #endregion

    #region Exposure

    /// <summary>
    /// 承灾体 - 危化品运输船舶
    /// </summary>
    public class TransportVessel : BaseScenarioFactor
    {
        /// <summary>
        /// 
        /// </summary>
        public TransportVessel()
        {
            // 油轮基础关键词
            CoreWrod.AddRange(new List<string> {
                "ship",
                "tanker",
            });
        }

        /// <summary>
        /// 船只状态
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 船只数量
        /// </summary>
        public string Count { get; set; }

        /// <summary>
        /// 船只载重
        /// </summary>
        public string Weight { get; set; }
    }

    /// <summary>
    /// 鱼礁
    /// </summary>
    public class ArtificialFishReef : BaseScenarioFactor
    {
        public ArtificialFishReef()
        {
            // 油轮基础关键词
            CoreWrod.AddRange(new List<string> {
                "reef",
            });
        }
    }

    /// <summary>
    /// 海底隧道
    /// </summary>
    public class SubseaTunnel : BaseScenarioFactor
    {
        public SubseaTunnel()
        {
            CoreWrod.AddRange(new List<string> {
                "subsea",
                "tunnel"
            });
        }
    }

    /// <summary>
    /// 养殖用海 (海上养殖场）
    /// </summary>
    public class OffshoreFram : BaseScenarioFactor
    {
        public OffshoreFram()
        {   
            CoreWrod.AddRange(new List<string> {
                "sea",
                "breed"
            });
        }
    }

    /// <summary>
    /// 海上航道
    /// </summary>
    public class Seaway : BaseScenarioFactor
    {
        public Seaway()
        {
            CoreWrod.AddRange(new List<string> {
                "seaway",
            });
        }
    }

    #endregion
}
