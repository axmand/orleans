using Engine.Facility.EResponse;
using GrainImplement.BMS.Persistence;
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

        readonly IPersistentState<VOCache> _tdxx;

        readonly IPersistentState<VOCache> _lyxx;

        public BMSService(
            [PersistentState("TDXX", "TDXXCache")] IPersistentState<VOCache> tdxx,
            [PersistentState("LYXX", "LYXXCache")] IPersistentState<VOCache> lyxx)
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
        /// 获取土地信息GeoJson
        /// </summary>
        /// <returns>GeoJson</returns>
        async Task<string> IBMSHY.AccessTDXXGeoData()
        {
            await _tdxx.ReadStateAsync();
            //FeatureCollectionConverter target = new FeatureCollectionConverter();
            //StringBuilder sb = new StringBuilder();
            //using (JsonTextWriter writer = new JsonTextWriter(new StringWriter(sb)))
            //{
            //    JsonSerializer serializer = NetTopologySuite.IO.GeoJsonSerializer.Create(
            //        new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore },
            //        GeometryFactory.Default);
            //    target.WriteJson(writer, _tdxx.State, serializer);
            //    writer.Flush();
            //    writer.Close();
            //}
            //if (sb.Length == 0) return new FailResponse("土地信息数据空").ToString();

            return new OkResponse(_tdxx.State.GEOJSON).ToString();
        }

        /// <summary>
        /// 获取楼宇信息geojson
        /// </summary>
        /// <returns>GeoJson</returns>
        async Task<string> IBMSHY.AccessLYXXGeoData()
        {
            await _lyxx.ReadStateAsync();
            //FeatureCollectionConverter target = new FeatureCollectionConverter();
            //StringBuilder sb = new StringBuilder();
            //using (JsonTextWriter writer = new JsonTextWriter(new StringWriter(sb)))
            //{
            //    JsonSerializer serializer = NetTopologySuite.IO.GeoJsonSerializer.Create(
            //        new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore },
            //        GeometryFactory.Default);
            //    target.WriteJson(writer, _lyxx.State, serializer);
            //    writer.Flush();
            //    writer.Close();
            //}
            //if (sb.Length == 0) return new FailResponse("土地信息数据空").ToString();

            return new OkResponse(_lyxx.State.GEOJSON).ToString();
        }

        /// <summary>
        /// 更新/修改/创建楼宇信息
        /// </summary>
        /// <param name="geoJsonText"></param>
        /// <returns></returns>
        async Task<string> IBMSHY.GeoDataLYXXUpdate(string geoJsonText)
        {
            NetTopologySuite.IO.GeoJsonReader reader = new NetTopologySuite.IO.GeoJsonReader();
            FeatureCollection result = reader.Read<FeatureCollection>(geoJsonText);
            if (result == null) return new FailResponse("数据校验失败").ToString();
            _lyxx.State.GEOJSON = geoJsonText;
            await _lyxx.WriteStateAsync();
            return new OkResponse("修改土地信息数据集成功").ToString();
        }

        /// <summary>
        /// 更新/修改/创建土地信息
        /// </summary>
        /// <param name="geoJsonText"></param>
        /// <returns></returns>
        async Task<string> IBMSHY.GeoDataTDXXUpdate(string geoJsonText)
        {
            NetTopologySuite.IO.GeoJsonReader reader = new NetTopologySuite.IO.GeoJsonReader();
            FeatureCollection result = reader.Read<FeatureCollection>(geoJsonText);
            if (result == null) return new FailResponse("数据校验失败").ToString();
            _tdxx.State.GEOJSON = geoJsonText;
            await _tdxx.WriteStateAsync();
            return new OkResponse("修改土地信息数据集成功").ToString();
        }

    }
}
