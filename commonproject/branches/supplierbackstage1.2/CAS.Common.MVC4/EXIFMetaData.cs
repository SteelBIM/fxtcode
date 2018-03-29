using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;

namespace CAS.Common.MVC4
{

    /// <summary>
    /// 功能：获得图片EXIF信息
    /// 作者：zyf
    /// 创建日期：2004-03-20
    /// </summary>
    public class EXIFMetaData
    {
        #region 构造函数 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public EXIFMetaData()
        {
        }
        #endregion

        #region 数据转换结构 数据转换结构
        /// <summary>
        /// 转换数据结构
        /// </summary>
        public struct MetadataDetail
        {
            public string Hex;//十六进制字符串
            public string RawValueAsString;//原始值串
            public string DisplayValue;//显示值串
        }
        #endregion

        #region EXIF元素结构 EXIF元素结构
        /// <summary>
        /// 结构：存储EXIF元素信息
        /// </summary>
        public struct Metadata
        {
            public MetadataDetail EquipmentMake;
            public MetadataDetail CameraModel;
            public MetadataDetail ExposureTime;//曝光时间
            public MetadataDetail Fstop;
            public MetadataDetail DatePictureTaken;
            public MetadataDetail ShutterSpeed;// 快门速度
            public MetadataDetail MeteringMode;//曝光模式
            public MetadataDetail Flash;//闪光灯
            public MetadataDetail XResolution;
            public MetadataDetail YResolution;
            public MetadataDetail ImageWidth;//照片宽度
            public MetadataDetail ImageHeight;//照片高度

            public MetadataDetail FNumber;//  f值，光圈数
            public MetadataDetail ExposureProg;//  曝光程序
            public MetadataDetail SpectralSense;//  
            public MetadataDetail ISOSpeed;//  ISO感光度
            public MetadataDetail OECF;//  
            public MetadataDetail Ver;//  EXIF版本
            public MetadataDetail CompConfig;//  色彩设置
            public MetadataDetail CompBPP;//  压缩比率
            public MetadataDetail Aperture;//  光圈值
            public MetadataDetail Brightness;//  亮度值Ev
            public MetadataDetail ExposureBias;//  曝光补偿
            public MetadataDetail MaxAperture;//  最大光圈值

            public MetadataDetail SubjectDist;// 主体距离
            public MetadataDetail LightSource;//  白平衡
            public MetadataDetail FocalLength;//  焦距
            public MetadataDetail FPXVer;//  FlashPix版本
            public MetadataDetail ColorSpace;//  色彩空间
            public MetadataDetail Interop;//  
            public MetadataDetail FlashEnergy;//  
            public MetadataDetail SpatialFR;//  
            public MetadataDetail FocalXRes;//  
            public MetadataDetail FocalYRes;//  
            public MetadataDetail FocalResUnit;//  
            public MetadataDetail ExposureIndex;//  曝光指数
            public MetadataDetail SensingMethod;//  感应方式
            public MetadataDetail SceneType;//  
            public MetadataDetail CfaPattern;//  
        }
        #endregion

        #region  查找EXIF元素值 查找EXIF元素值
        public string LookupEXIFValue(string Description, string Value)
        {
            string DescriptionValue = null;

            switch (Description)
            {
                case "MeteringMode":

                    #region MeteringMode  MeteringMode
                    {
                        switch (Value)
                        {
                            case "0":
                                DescriptionValue = "Unknown"; break;
                            case "1":
                                DescriptionValue = "Average"; break;
                            case "2":
                                DescriptionValue = "Center Weighted Average"; break;
                            case "3":
                                DescriptionValue = "Spot"; break;
                            case "4":
                                DescriptionValue = "Multi-spot"; break;
                            case "5":
                                DescriptionValue = "Multi-segment"; break;
                            case "6":
                                DescriptionValue = "Partial"; break;
                            case "255":
                                DescriptionValue = "Other"; break;
                        }
                    }
                    #endregion

                    break;
                case "ResolutionUnit":

                    #region ResolutionUnit ResolutionUnit
                    {
                        switch (Value)
                        {
                            case "1":
                                DescriptionValue = "No Units"; break;
                            case "2":
                                DescriptionValue = "Inch"; break;
                            case "3":
                                DescriptionValue = "Centimeter"; break;
                        }
                    }

                    #endregion

                    break;
                //省略Ｎ行相似代码

            }
            return DescriptionValue;
        }
        #endregion

        #region  取得图片的EXIF信息 取得图片的EXIF信息
        public Metadata GetEXIFMetaData(string PhotoName)
        {
            // 创建一个图片的实例
            System.Drawing.Image MyImage = System.Drawing.Image.FromFile(PhotoName);
            // 创建一个整型数组来存储图像中属性数组的ID
            int[] MyPropertyIdList = MyImage.PropertyIdList;
            //创建一个封闭图像属性数组的实例
            PropertyItem[] MyPropertyItemList = new PropertyItem[MyPropertyIdList.Length];
            //创建一个图像EXIT信息的实例结构对象，并且赋初值

            #region 创建一个图像EXIT信息的实例结构对象，并且赋初值 创建一个图像EXIT信息的实例结构对象，并且赋初值
            Metadata MyMetadata = new Metadata();
            MyMetadata.EquipmentMake.Hex = "10f";
            MyMetadata.CameraModel.Hex = "110";
            MyMetadata.DatePictureTaken.Hex = "9003";
            MyMetadata.ExposureTime.Hex = "829a";
            MyMetadata.Fstop.Hex = "829d";
            MyMetadata.ShutterSpeed.Hex = "9201";
            MyMetadata.MeteringMode.Hex = "9207";
            MyMetadata.Flash.Hex = "9209";
            MyMetadata.FNumber.Hex = "829d"; // 
            MyMetadata.ExposureProg.Hex = ""; // 
            MyMetadata.SpectralSense.Hex = "8824"; // 
            MyMetadata.ISOSpeed.Hex = "8827"; // 
            MyMetadata.OECF.Hex = "8828"; // 
            MyMetadata.Ver.Hex = "9000"; // 
            MyMetadata.CompConfig.Hex = "9101"; // 
            MyMetadata.CompBPP.Hex = "9102"; // 
            MyMetadata.Aperture.Hex = "9202"; // 
            MyMetadata.Brightness.Hex = "9203"; // 
            MyMetadata.ExposureBias.Hex = "9204"; // 
            MyMetadata.MaxAperture.Hex = "9205"; // 
            MyMetadata.SubjectDist.Hex = "9206"; // 
            MyMetadata.LightSource.Hex = "9208"; // 
            MyMetadata.FocalLength.Hex = "920a"; // 
            MyMetadata.FPXVer.Hex = "a000"; // 
            MyMetadata.ColorSpace.Hex = "a001"; // 
            MyMetadata.FocalXRes.Hex = "a20e"; // 
            MyMetadata.FocalYRes.Hex = "a20f"; // 
            MyMetadata.FocalResUnit.Hex = "a210"; // 
            MyMetadata.ExposureIndex.Hex = "a215"; // 
            MyMetadata.SensingMethod.Hex = "a217"; // 
            MyMetadata.SceneType.Hex = "a301";
            MyMetadata.CfaPattern.Hex = "a302";
            #endregion

            // ASCII编码
            System.Text.ASCIIEncoding Value = new System.Text.ASCIIEncoding();

            int index = 0;
            int MyPropertyIdListCount = MyPropertyIdList.Length;
            if (MyPropertyIdListCount != 0)
            {
                foreach (int MyPropertyId in MyPropertyIdList)
                {
                    string hexVal = "";
                    MyPropertyItemList[index] = MyImage.GetPropertyItem(MyPropertyId);

                    #region 初始化各属性值 初始化各属性值
                    string myPropertyIdString = MyImage.GetPropertyItem(MyPropertyId).Id.ToString("x");
                    switch (myPropertyIdString)
                    {
                        case "10f":
                            {
                                MyMetadata.EquipmentMake.RawValueAsString = BitConverter.ToString(MyImage.GetPropertyItem(MyPropertyId).Value);
                                MyMetadata.EquipmentMake.DisplayValue = Value.GetString(MyPropertyItemList[index].Value);
                                break;
                            }

                        case "110":
                            {
                                MyMetadata.CameraModel.RawValueAsString = BitConverter.ToString(MyImage.GetPropertyItem(MyPropertyId).Value);
                                MyMetadata.CameraModel.DisplayValue = Value.GetString(MyPropertyItemList[index].Value);
                                break;

                            }

                        case "9003":
                            {
                                MyMetadata.DatePictureTaken.RawValueAsString = BitConverter.ToString(MyImage.GetPropertyItem(MyPropertyId).Value);
                                MyMetadata.DatePictureTaken.DisplayValue = Value.GetString(MyPropertyItemList[index].Value);
                                break;
                            }
                        //省略Ｎ行相似代码
                    }
                    #endregion

                    index++;
                }
            }
            Image img;
            MyMetadata.XResolution.DisplayValue = MyImage.HorizontalResolution.ToString();
            MyMetadata.YResolution.DisplayValue = MyImage.VerticalResolution.ToString();
            MyMetadata.ImageHeight.DisplayValue = MyImage.Height.ToString();
            MyMetadata.ImageWidth.DisplayValue = MyImage.Width.ToString();
            MyImage.Dispose();
            return MyMetadata;
        }
        #endregion
    }
}
