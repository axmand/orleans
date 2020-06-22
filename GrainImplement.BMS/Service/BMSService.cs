using Engine.Facility.EResponse;
using GrainInterface.BMS;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO.Converters;
using Newtonsoft.Json;
using Orleans.Runtime;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GrainImplement.BMS.Service
{
    public class BMSService : Orleans.Grain, IBMSHY
    {

        #region 内置

        readonly IPersistentState<FeatureCollection> _tdxx;

        readonly IPersistentState<FeatureCollection> _lyxx;

        public BMSService(
            [PersistentState("TDXX", "TDXXCache")] IPersistentState<FeatureCollection> tdxx,
            [PersistentState("LYXX", "LYXXCache")] IPersistentState<FeatureCollection> lyxx)
        {
            _tdxx = tdxx;
            _lyxx = lyxx;
        }


        Task<bool> IBMSHY.InitialCheck()
        {
            return Task.FromResult(true);
        }

        #endregion

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        async Task<string> IBMSHY.AccessToLandInformation()
        {
            await _tdxx.ReadStateAsync();
            //
            FeatureCollectionConverter target = new FeatureCollectionConverter();
            StringBuilder sb = new StringBuilder();
            using (JsonTextWriter writer = new JsonTextWriter(new StringWriter(sb)))
            {
                JsonSerializer serializer = NetTopologySuite.IO.GeoJsonSerializer.Create(
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore },
                    GeometryFactory.Default);
                target.WriteJson(writer, _tdxx.State, serializer);
                writer.Flush();
                writer.Close();
            }
            if (sb.Length == 0) return new FailResponse("土地信息数据空").ToString();
            return new OkResponse(sb.ToString()).ToString();
        }

        /// <summary>
        /// 创建土地信息
        /// </summary>
        /// <param name="geoJsonText"></param>
        /// <returns></returns>
        async Task<string> IBMSHY.ModifyLandInformation(string geoJsonText)
        {
            NetTopologySuite.IO.GeoJsonReader reader = new NetTopologySuite.IO.GeoJsonReader();
            FeatureCollection result = reader.Read<FeatureCollection>(geoJsonText);
            await _tdxx.WriteStateAsync();
            return new OkResponse("修改土地信息数据集成功").ToString();
        }

        public Task<string> AccessToBuildingInformation()
        {
            throw new System.NotImplementedException();
        }

        public Task<string> ModifyBuildingInformation(string geoJsonText)
        {
            throw new System.NotImplementedException();
        }
    }
}
