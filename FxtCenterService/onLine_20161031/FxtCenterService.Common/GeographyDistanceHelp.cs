using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtCenterService.Common
{
    /// <summary>
    /// 物理距离帮助类
    /// </summary>
    public class GeographyDistanceHelp
    {
        /// <summary>
        /// 地球平均半径(单位:KM)
        /// </summary>
        public const double EARTH_RADIUS = 6378.137;

        /// <summary>
        /// 根据距离求经度差
        /// </summary>
        /// <param name="distance">距离</param>
        /// <param name="latitude">纬度值</param>
        /// <returns></returns>
        public static double GetLongitudeRange(double distance, double latitude)
        {
            double dlng = 2 * Math.Asin(Math.Sin(distance / (2 * EARTH_RADIUS)) / Math.Cos(latitude));
            dlng = derad(dlng);
            return dlng;
        }

        /// <summary>
        /// 根据距离求纬度差
        /// </summary>
        /// <param name="distance">距离</param>
        /// <returns></returns>
        public static double GetLatitudeRange(double distance)
        {
            double dlat = distance / EARTH_RADIUS;
            dlat = derad(dlat);
            return dlat;
        }

        /// <summary>
        /// 弧度转度
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private static double derad(double d)
        {
            return 180.0 * d / Math.PI;
        }

        /// <summary>
        /// 度转弧度
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private static double rad(double d)
        {
            return d * Math.PI / 180.0;
        }

        /// <summary>
        /// 根据两点经纬度求距离
        /// </summary>
        /// <param name="lat1">起始点纬度</param>
        /// <param name="lng1">起始点经度</param>
        /// <param name="lat2">目标点纬度</param>
        /// <param name="lng2">目标点经度</param>
        /// <returns>两点距离(单位KM)</returns>
        public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double radLat1 = rad(lat1);
            double radLat2 = rad(lat2);
            double a = radLat1 - radLat2;
            double b = rad(lng1) - rad(lng2);
            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 10000) / 10000;
            return s;
        }
    }
}
