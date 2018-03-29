using FxtDataAcquisition.Application.Interfaces;
using FxtDataAcquisition.Common;
using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
using FxtDataAcquisition.Domain.Models;
using FxtDataAcquisition.FxtAPI.FxtDataCenter.Manager;
using FxtDataAcquisition.FxtAPI.FxtDataWcf.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtDataAcquisition.Application.Services
{
    public class CodeService : ICodeService
    {

        public List<SYSCode> PhotoTypeCodeManager()
        {
            List<SYSCode> list = new List<SYSCode>() { 
            new SYSCode { Code = SYSCodeManager.PHOTOTYPECODE_1, CodeName  = "logo",ID = SYSCodeManager.PHOTOTYPECODE_ID, CodeType = "住宅图片类型" },
            new SYSCode { Code = SYSCodeManager.PHOTOTYPECODE_2, CodeName  = "标准层平面图",ID = SYSCodeManager.PHOTOTYPECODE_ID, CodeType = "住宅图片类型" },
            new SYSCode { Code = SYSCodeManager.PHOTOTYPECODE_3, CodeName  = "户型图",ID = SYSCodeManager.PHOTOTYPECODE_ID, CodeType = "住宅图片类型" },
            new SYSCode { Code = SYSCodeManager.PHOTOTYPECODE_4, CodeName  = "实景图",ID = SYSCodeManager.PHOTOTYPECODE_ID, CodeType = "住宅图片类型" },
            new SYSCode { Code = SYSCodeManager.PHOTOTYPECODE_5, CodeName  = "外立面图",ID = SYSCodeManager.PHOTOTYPECODE_ID, CodeType = "住宅图片类型" },
            new SYSCode { Code = SYSCodeManager.PHOTOTYPECODE_6, CodeName  = "位置图",ID = SYSCodeManager.PHOTOTYPECODE_ID, CodeType = "住宅图片类型" },
            new SYSCode { Code = SYSCodeManager.PHOTOTYPECODE_7, CodeName  = "效果图",ID = SYSCodeManager.PHOTOTYPECODE_ID, CodeType = "住宅图片类型" },
            new SYSCode { Code = SYSCodeManager.PHOTOTYPECODE_8, CodeName  = "总平面图",ID = SYSCodeManager.PHOTOTYPECODE_ID, CodeType = "住宅图片类型" },
            new SYSCode { Code = SYSCodeManager.PHOTOTYPECODE_9, CodeName  = "其他",ID = SYSCodeManager.PHOTOTYPECODE_ID, CodeType = "住宅图片类型" },
            };
            return list;
        }

        public List<SYSCode> RightCodeManager()
        {
            List<SYSCode> list = new List<SYSCode>() { 
            new SYSCode { Code = SYSCodeManager.RIGHTCODE_1, CodeName  = "商品房",       ID = SYSCodeManager.RIGHTCODE_ID, CodeType = "产权形式" },
            new SYSCode { Code = SYSCodeManager.RIGHTCODE_2, CodeName  = "微利房",    ID = SYSCodeManager.RIGHTCODE_ID, CodeType = "产权形式" },
            new SYSCode { Code = SYSCodeManager.RIGHTCODE_3, CodeName  = "福利房",    ID = SYSCodeManager.RIGHTCODE_ID, CodeType = "产权形式" },
            new SYSCode { Code = SYSCodeManager.RIGHTCODE_4, CodeName  = "军产房",       ID = SYSCodeManager.RIGHTCODE_ID, CodeType = "产权形式" },
            new SYSCode { Code = SYSCodeManager.RIGHTCODE_5, CodeName  = "集资房",       ID = SYSCodeManager.RIGHTCODE_ID, CodeType = "产权形式" },
            new SYSCode { Code = SYSCodeManager.RIGHTCODE_6, CodeName  = "自建房",     ID = SYSCodeManager.RIGHTCODE_ID, CodeType = "产权形式" },
            new SYSCode { Code = SYSCodeManager.RIGHTCODE_7, CodeName  = "经济适用房", ID = SYSCodeManager.RIGHTCODE_ID, CodeType = "产权形式" },
            new SYSCode { Code = SYSCodeManager.RIGHTCODE_8, CodeName  = "小产权房",   ID = SYSCodeManager.RIGHTCODE_ID, CodeType = "产权形式" },
            new SYSCode { Code = SYSCodeManager.RIGHTCODE_9, CodeName  = "限价房",     ID = SYSCodeManager.RIGHTCODE_ID, CodeType = "产权形式" },
            new SYSCode { Code = SYSCodeManager.RIGHTCODE_10, CodeName = "解困房",     ID = SYSCodeManager.RIGHTCODE_ID, CodeType = "产权形式" },
            new SYSCode { Code = SYSCodeManager.RIGHTCODE_11, CodeName = "宅基地",     ID = SYSCodeManager.RIGHTCODE_ID, CodeType = "产权形式" },
            new SYSCode { Code = SYSCodeManager.RIGHTCODE_12, CodeName = "房改房",     ID = SYSCodeManager.RIGHTCODE_ID, CodeType = "产权形式" },
            new SYSCode { Code = SYSCodeManager.RIGHTCODE_13, CodeName = "平改房",     ID = SYSCodeManager.RIGHTCODE_ID, CodeType = "产权形式" },
            new SYSCode { Code = SYSCodeManager.RIGHTCODE_14, CodeName = "回迁房",     ID = SYSCodeManager.RIGHTCODE_ID, CodeType = "产权形式" },
            new SYSCode { Code = SYSCodeManager.RIGHTCODE_15, CodeName = "安置房",     ID = SYSCodeManager.RIGHTCODE_ID, CodeType = "产权形式" }
            };
            return list;
        }

        public List<SYSCode> PurposeCodeManager()
        {
            List<SYSCode> list = new List<SYSCode>() { 
            new SYSCode { Code = SYSCodeManager.PURPOSECODE_1, CodeName  = "居住",       ID = SYSCodeManager.PURPOSECODE_ID, CodeType = "土地用途" },
            new SYSCode { Code = SYSCodeManager.PURPOSECODE_2, CodeName  = "居住(别墅)",       ID = SYSCodeManager.PURPOSECODE_ID, CodeType = "土地用途" },
            new SYSCode { Code = SYSCodeManager.PURPOSECODE_3, CodeName  = "居住(洋房)",       ID = SYSCodeManager.PURPOSECODE_ID, CodeType = "土地用途" },
            new SYSCode { Code = SYSCodeManager.PURPOSECODE_4, CodeName  = "商业",       ID = SYSCodeManager.PURPOSECODE_ID, CodeType = "土地用途" },
            new SYSCode { Code = SYSCodeManager.PURPOSECODE_5, CodeName  = "办公",       ID = SYSCodeManager.PURPOSECODE_ID, CodeType = "土地用途" },
            new SYSCode { Code = SYSCodeManager.PURPOSECODE_6, CodeName  = "工业",       ID = SYSCodeManager.PURPOSECODE_ID, CodeType = "土地用途" },
            new SYSCode { Code = SYSCodeManager.PURPOSECODE_7, CodeName  = "商业、居住",       ID = SYSCodeManager.PURPOSECODE_ID, CodeType = "土地用途" },
            new SYSCode { Code = SYSCodeManager.PURPOSECODE_8, CodeName  = "商业、办公",       ID = SYSCodeManager.PURPOSECODE_ID, CodeType = "土地用途" },
            new SYSCode { Code = SYSCodeManager.PURPOSECODE_9, CodeName  = "办公、居住",       ID = SYSCodeManager.PURPOSECODE_ID, CodeType = "土地用途" },
            new SYSCode { Code = SYSCodeManager.PURPOSECODE_10, CodeName  = "停车场",       ID = SYSCodeManager.PURPOSECODE_ID, CodeType = "土地用途" },
            new SYSCode { Code = SYSCodeManager.PURPOSECODE_11, CodeName  = "酒店",       ID = SYSCodeManager.PURPOSECODE_ID, CodeType = "土地用途" },
            new SYSCode { Code = SYSCodeManager.PURPOSECODE_12, CodeName  = "加油站",       ID = SYSCodeManager.PURPOSECODE_ID, CodeType = "土地用途" },
            new SYSCode { Code = SYSCodeManager.PURPOSECODE_13, CodeName  = "综合",       ID = SYSCodeManager.PURPOSECODE_ID, CodeType = "土地用途" },
            new SYSCode { Code = SYSCodeManager.PURPOSECODE_14, CodeName  = "其他",       ID = SYSCodeManager.PURPOSECODE_ID, CodeType = "土地用途" },
            };                   
            return list;
        }

        public List<SYSCode> HousingScaleCodeManager()
        {
            List<SYSCode> list = new List<SYSCode>() { 
            new SYSCode { Code = SYSCodeManager.HOUSINGSCALECODE_1, CodeName  = "10万㎡以下",ID = SYSCodeManager.HOUSINGSCALECODE_ID, CodeType = "小区规模" },
            new SYSCode { Code = SYSCodeManager.HOUSINGSCALECODE_2, CodeName  = "10~20万㎡",ID = SYSCodeManager.HOUSINGSCALECODE_ID, CodeType = "小区规模" },
            new SYSCode { Code = SYSCodeManager.HOUSINGSCALECODE_3, CodeName  = "20~50万㎡",ID = SYSCodeManager.HOUSINGSCALECODE_ID, CodeType = "小区规模" },
            new SYSCode { Code = SYSCodeManager.HOUSINGSCALECODE_4, CodeName  = "50~100万㎡",ID = SYSCodeManager.HOUSINGSCALECODE_ID, CodeType = "小区规模" },
            new SYSCode { Code = SYSCodeManager.HOUSINGSCALECODE_5, CodeName  = "100万㎡以上",ID = SYSCodeManager.HOUSINGSCALECODE_ID, CodeType = "小区规模" },
            };
            return list;
        }

        public List<SYSCode> LevelManager()
        {
            List<SYSCode> list = new List<SYSCode>() { 
            new SYSCode { Code = SYSCodeManager.LEVELCODE_1, CodeName  = "优",       ID = SYSCodeManager.LEVELCODE_ID, CodeType = "等级" },
            new SYSCode { Code = SYSCodeManager.LEVELCODE_2, CodeName  = "良",       ID = SYSCodeManager.LEVELCODE_ID, CodeType = "等级" },
            new SYSCode { Code = SYSCodeManager.LEVELCODE_3, CodeName  = "一般",       ID = SYSCodeManager.LEVELCODE_ID, CodeType = "等级" },
            new SYSCode { Code = SYSCodeManager.LEVELCODE_4, CodeName  = "差",       ID = SYSCodeManager.LEVELCODE_ID, CodeType = "等级" },
            new SYSCode { Code = SYSCodeManager.LEVELCODE_5, CodeName  = "很差",       ID = SYSCodeManager.LEVELCODE_ID, CodeType = "等级" },
            };                   
            return list;
        }

        public List<SYSCode> BuildingTypeCodeManager()
        {
            List<SYSCode> list = new List<SYSCode>() { 
                new SYSCode { Code = SYSCodeManager.BUILDINGTYPECODE_1, CodeName  = "低层",  ID = SYSCodeManager.BUILDINGTYPECODE_ID, CodeType = "建筑类型" },
                new SYSCode { Code = SYSCodeManager.BUILDINGTYPECODE_2, CodeName  = "多层",  ID = SYSCodeManager.BUILDINGTYPECODE_ID, CodeType = "建筑类型" },
                new SYSCode { Code = SYSCodeManager.BUILDINGTYPECODE_3, CodeName  = "小高层",ID = SYSCodeManager.BUILDINGTYPECODE_ID, CodeType = "建筑类型" },
                new SYSCode { Code = SYSCodeManager.BUILDINGTYPECODE_4, CodeName  = "高层",  ID = SYSCodeManager.BUILDINGTYPECODE_ID, CodeType = "建筑类型" }
             };
            return list;
        }

        public List<SYSCode> BuildingStructureCodeManager()
        {
            List<SYSCode> list = new List<SYSCode>() { 
                new SYSCode { Code = SYSCodeManager.BUILDINGSTRUCTURECODE_1, CodeName  = "砖木结构",  ID = SYSCodeManager.BUILDINGSTRUCTURECODE_ID, CodeType = "建筑结构" },
                new SYSCode { Code = SYSCodeManager.BUILDINGSTRUCTURECODE_2, CodeName  = "砖混结构",  ID = SYSCodeManager.BUILDINGSTRUCTURECODE_ID, CodeType = "建筑结构" },
                new SYSCode { Code = SYSCodeManager.BUILDINGSTRUCTURECODE_3, CodeName  = "框架结构",  ID = SYSCodeManager.BUILDINGSTRUCTURECODE_ID, CodeType = "建筑结构" },
                new SYSCode { Code = SYSCodeManager.BUILDINGSTRUCTURECODE_4, CodeName  = "框剪结构",  ID = SYSCodeManager.BUILDINGSTRUCTURECODE_ID, CodeType = "建筑结构" },
                new SYSCode { Code = SYSCodeManager.BUILDINGSTRUCTURECODE_5, CodeName  = "框筒结构",  ID = SYSCodeManager.BUILDINGSTRUCTURECODE_ID, CodeType = "建筑结构" },
                new SYSCode { Code = SYSCodeManager.BUILDINGSTRUCTURECODE_6, CodeName  = "钢结构",  ID = SYSCodeManager.BUILDINGSTRUCTURECODE_ID, CodeType = "建筑结构" },
                new SYSCode { Code = SYSCodeManager.BUILDINGSTRUCTURECODE_7, CodeName  = "钢混结构",  ID = SYSCodeManager.BUILDINGSTRUCTURECODE_ID, CodeType = "建筑结构" },
                new SYSCode { Code = SYSCodeManager.BUILDINGSTRUCTURECODE_8, CodeName  = "混合结构",  ID = SYSCodeManager.BUILDINGSTRUCTURECODE_ID, CodeType = "建筑结构" },
                new SYSCode { Code = SYSCodeManager.BUILDINGSTRUCTURECODE_9, CodeName  = "内浇外砌",  ID = SYSCodeManager.BUILDINGSTRUCTURECODE_ID, CodeType = "建筑结构" },
                new SYSCode { Code = SYSCodeManager.BUILDINGSTRUCTURECODE_10, CodeName  = "内浇外挂",  ID = SYSCodeManager.BUILDINGSTRUCTURECODE_ID, CodeType = "建筑结构" },
             };
            return list;
        }

        public List<SYSCode> LocationCodeManager()
        {
            List<SYSCode> list = new List<SYSCode>() { 
                new SYSCode { Code = SYSCodeManager.LOCATIONCODE_1, CodeName  = "无特别因素",  ID = SYSCodeManager.LOCATIONCODE_ID, CodeType = "楼栋位置" },
                new SYSCode { Code = SYSCodeManager.LOCATIONCODE_2, CodeName  = "临公园、绿地",  ID = SYSCodeManager.LOCATIONCODE_ID, CodeType = "楼栋位置" },
                new SYSCode { Code = SYSCodeManager.LOCATIONCODE_3, CodeName  = "临江、河、湖",  ID = SYSCodeManager.LOCATIONCODE_ID, CodeType = "楼栋位置" },
                new SYSCode { Code = SYSCodeManager.LOCATIONCODE_4, CodeName  = "临噪音源(路、桥、工厂)",  ID = SYSCodeManager.LOCATIONCODE_ID, CodeType = "楼栋位置" },
                new SYSCode { Code = SYSCodeManager.LOCATIONCODE_5, CodeName  = "临垃圾站、医院",  ID = SYSCodeManager.LOCATIONCODE_ID, CodeType = "楼栋位置" },
                new SYSCode { Code = SYSCodeManager.LOCATIONCODE_6, CodeName  = "临变电站、高压线",  ID = SYSCodeManager.LOCATIONCODE_ID, CodeType = "楼栋位置" },
                new SYSCode { Code = SYSCodeManager.LOCATIONCODE_7, CodeName  = "临其他不利因素",  ID = SYSCodeManager.LOCATIONCODE_ID, CodeType = "楼栋位置" },
                new SYSCode { Code = SYSCodeManager.LOCATIONCODE_8, CodeName  = "临小区中庭",  ID = SYSCodeManager.LOCATIONCODE_ID, CodeType = "楼栋位置" },
             };
            return list;
        }

        public List<SYSCode> WallCodeManager()
        {
            List<SYSCode> list = new List<SYSCode>() { 
                new SYSCode { Code = SYSCodeManager.WALLCODE_1, CodeName  = "涂料",  ID = SYSCodeManager.WALLCODE_ID, CodeType = "外墙装修" },
                new SYSCode { Code = SYSCodeManager.WALLCODE_2, CodeName  = "马赛克",  ID = SYSCodeManager.WALLCODE_ID, CodeType = "外墙装修" },
                new SYSCode { Code = SYSCodeManager.WALLCODE_3, CodeName  = "条形砖",  ID = SYSCodeManager.WALLCODE_ID, CodeType = "外墙装修" },
                new SYSCode { Code = SYSCodeManager.WALLCODE_4, CodeName  = "玻璃幕墙",  ID = SYSCodeManager.WALLCODE_ID, CodeType = "外墙装修" },
                new SYSCode { Code = SYSCodeManager.WALLCODE_5, CodeName  = "铝复板",  ID = SYSCodeManager.WALLCODE_ID, CodeType = "外墙装修" },
                new SYSCode { Code = SYSCodeManager.WALLCODE_6, CodeName  = "大理石",  ID = SYSCodeManager.WALLCODE_ID, CodeType = "外墙装修" },
                new SYSCode { Code = SYSCodeManager.WALLCODE_7, CodeName  = "花岗石",  ID = SYSCodeManager.WALLCODE_ID, CodeType = "外墙装修" },
                new SYSCode { Code = SYSCodeManager.WALLCODE_8, CodeName  = "瓷片",  ID = SYSCodeManager.WALLCODE_ID, CodeType = "外墙装修" },
                new SYSCode { Code = SYSCodeManager.WALLCODE_9, CodeName  = "墙砖",  ID = SYSCodeManager.WALLCODE_ID, CodeType = "外墙装修" },
                new SYSCode { Code = SYSCodeManager.WALLCODE_10, CodeName  = "水刷石",  ID = SYSCodeManager.WALLCODE_ID, CodeType = "外墙装修" },
                new SYSCode { Code = SYSCodeManager.WALLCODE_11, CodeName  = "清水墙",  ID = SYSCodeManager.WALLCODE_ID, CodeType = "外墙装修" },
                new SYSCode { Code = SYSCodeManager.WALLCODE_12, CodeName  = "其他",  ID = SYSCodeManager.WALLCODE_ID, CodeType = "外墙装修" },
                new SYSCode { Code = SYSCodeManager.WALLCODE_13, CodeName  = "水泥砂浆",  ID = SYSCodeManager.WALLCODE_ID, CodeType = "外墙装修" },
             };
            return list;
        }

        public List<SYSCode> InnerFitmentCodeManager()
        {
            List<SYSCode> list = new List<SYSCode>() { 
                new SYSCode { Code = SYSCodeManager.INNERFITMENTCODE_1, CodeName  = "豪华",  ID = SYSCodeManager.INNERFITMENTCODE_ID, CodeType = "内部装修" },
                new SYSCode { Code = SYSCodeManager.INNERFITMENTCODE_2, CodeName  = "高档",  ID = SYSCodeManager.INNERFITMENTCODE_ID, CodeType = "内部装修" },
                new SYSCode { Code = SYSCodeManager.INNERFITMENTCODE_3, CodeName  = "中档",  ID = SYSCodeManager.INNERFITMENTCODE_ID, CodeType = "内部装修" },
                new SYSCode { Code = SYSCodeManager.INNERFITMENTCODE_4, CodeName  = "普通",  ID = SYSCodeManager.INNERFITMENTCODE_ID, CodeType = "内部装修" },
                new SYSCode { Code = SYSCodeManager.INNERFITMENTCODE_5, CodeName  = "简易",  ID = SYSCodeManager.INNERFITMENTCODE_ID, CodeType = "内部装修" },
                new SYSCode { Code = SYSCodeManager.INNERFITMENTCODE_6, CodeName  = "毛坯",  ID = SYSCodeManager.INNERFITMENTCODE_ID, CodeType = "内部装修" },
             };                                                                                   
            return list;
        }

        public List<SYSCode> PipelineGasCodeManager()
        {
            List<SYSCode> list = new List<SYSCode>() { 
                new SYSCode { Code = SYSCodeManager.PIPELINEGASCODE_1, CodeName  = "管道天然气",  ID = SYSCodeManager.PIPELINEGASCODE_ID, CodeType = "管道燃气" },
                new SYSCode { Code = SYSCodeManager.PIPELINEGASCODE_2, CodeName  = "管道煤气",  ID = SYSCodeManager.PIPELINEGASCODE_ID, CodeType = "管道燃气" },
                new SYSCode { Code = SYSCodeManager.PIPELINEGASCODE_3, CodeName  = "无",  ID = SYSCodeManager.PIPELINEGASCODE_ID, CodeType = "管道燃气" },
             };
            return list;
        }

        public List<SYSCode> HeatingModeCodeManager()
        {
            List<SYSCode> list = new List<SYSCode>() { 
                new SYSCode { Code = SYSCodeManager.HEATINGMODECODE_1, CodeName  = "集中供暖",  ID = SYSCodeManager.PIPELINEGASCODE_ID, CodeType = "采暖方式" },
                new SYSCode { Code = SYSCodeManager.HEATINGMODECODE_2, CodeName  = "独户采暖",  ID = SYSCodeManager.PIPELINEGASCODE_ID, CodeType = "采暖方式" },
                new SYSCode { Code = SYSCodeManager.HEATINGMODECODE_3, CodeName  = "无",  ID = SYSCodeManager.PIPELINEGASCODE_ID, CodeType = "采暖方式" },
             };
            return list;
        }

        public List<SYSCode> WallTypeCodeManager()
        {
            List<SYSCode> list = new List<SYSCode>() { 
                new SYSCode { Code = SYSCodeManager.WALLTYPECODE_1, CodeName  = "加厚墙体",  ID = SYSCodeManager.PIPELINEGASCODE_ID, CodeType = "墙体类型" },
                new SYSCode { Code = SYSCodeManager.WALLTYPECODE_2, CodeName  = "普通墙体",  ID = SYSCodeManager.PIPELINEGASCODE_ID, CodeType = "墙体类型" },
                new SYSCode { Code = SYSCodeManager.WALLTYPECODE_3, CodeName  = "不详",  ID = SYSCodeManager.PIPELINEGASCODE_ID, CodeType = "墙体类型" },
             };
            return list;
        }

        public List<SYSCode> HousePurposeCodeManager()
        {
            List<SYSCode> list = new List<SYSCode>() { 
            new SYSCode { Code = SYSCodeManager.HOUSEPURPOSECODE_1, CodeName  = "普通住宅",       ID = SYSCodeManager.HOUSEPURPOSECODE_ID, CodeType = "用途" },
            new SYSCode { Code = SYSCodeManager.HOUSEPURPOSECODE_2, CodeName  = "非普通住宅",       ID = SYSCodeManager.HOUSEPURPOSECODE_ID, CodeType = "用途" },
            new SYSCode { Code = SYSCodeManager.HOUSEPURPOSECODE_3, CodeName  = "公寓",       ID = SYSCodeManager.HOUSEPURPOSECODE_ID, CodeType = "用途" },
            new SYSCode { Code = SYSCodeManager.HOUSEPURPOSECODE_4, CodeName  = "酒店式公寓",       ID = SYSCodeManager.HOUSEPURPOSECODE_ID, CodeType = "用途" },
            new SYSCode { Code = SYSCodeManager.HOUSEPURPOSECODE_5, CodeName  = "独立别墅",       ID = SYSCodeManager.HOUSEPURPOSECODE_ID, CodeType = "用途" },
            new SYSCode { Code = SYSCodeManager.HOUSEPURPOSECODE_6, CodeName  = "联排别墅",       ID = SYSCodeManager.HOUSEPURPOSECODE_ID, CodeType = "用途" },
            new SYSCode { Code = SYSCodeManager.HOUSEPURPOSECODE_7, CodeName  = "叠加别墅",       ID = SYSCodeManager.HOUSEPURPOSECODE_ID, CodeType = "用途" },
            new SYSCode { Code = SYSCodeManager.HOUSEPURPOSECODE_8, CodeName  = "双拼别墅",       ID = SYSCodeManager.HOUSEPURPOSECODE_ID, CodeType = "用途" },
            new SYSCode { Code = SYSCodeManager.HOUSEPURPOSECODE_9, CodeName  = "旅馆",       ID = SYSCodeManager.HOUSEPURPOSECODE_ID, CodeType = "用途" },
            new SYSCode { Code = SYSCodeManager.HOUSEPURPOSECODE_10, CodeName  = "花园洋房",       ID = SYSCodeManager.HOUSEPURPOSECODE_ID, CodeType = "用途" },
            new SYSCode { Code = SYSCodeManager.HOUSEPURPOSECODE_11, CodeName  = "老洋房",       ID = SYSCodeManager.HOUSEPURPOSECODE_ID, CodeType = "用途" },
            new SYSCode { Code = SYSCodeManager.HOUSEPURPOSECODE_12, CodeName  = "新式里弄",       ID = SYSCodeManager.HOUSEPURPOSECODE_ID, CodeType = "用途" },
            new SYSCode { Code = SYSCodeManager.HOUSEPURPOSECODE_13, CodeName  = "旧式里弄",       ID = SYSCodeManager.HOUSEPURPOSECODE_ID, CodeType = "用途" },
            //new SYSCode { Code = HOUSEPURPOSECODE_14, CodeName  = "商业",       ID = HOUSEPURPOSECODE_ID, CodeType = "用途" },
            //new SYSCode { Code = HOUSEPURPOSECODE_15, CodeName  = "办公",       ID = HOUSEPURPOSECODE_ID, CodeType = "用途" },
            //new SYSCode { Code = HOUSEPURPOSECODE_16, CodeName  = "厂房",       ID = HOUSEPURPOSECODE_ID, CodeType = "用途" },
            //new SYSCode { Code = HOUSEPURPOSECODE_17, CodeName  = "酒店",       ID = HOUSEPURPOSECODE_ID, CodeType = "用途" },
            //new SYSCode { Code = HOUSEPURPOSECODE_18, CodeName  = "仓库",       ID = HOUSEPURPOSECODE_ID, CodeType = "用途" },
            //new SYSCode { Code = HOUSEPURPOSECODE_19, CodeName  = "车位",       ID = HOUSEPURPOSECODE_ID, CodeType = "用途" },
            //new SYSCode { Code = HOUSEPURPOSECODE_20, CodeName  = "综合",       ID = HOUSEPURPOSECODE_ID, CodeType = "用途" },
            //new SYSCode { Code = HOUSEPURPOSECODE_21, CodeName  = "商住",       ID = HOUSEPURPOSECODE_ID, CodeType = "用途" },
            new SYSCode { Code = SYSCodeManager.HOUSEPURPOSECODE_22, CodeName  = "其他",       ID = SYSCodeManager.HOUSEPURPOSECODE_ID, CodeType = "用途" },
            new SYSCode { Code = SYSCodeManager.HOUSEPURPOSECODE_23, CodeName  = "经济适用房",       ID = SYSCodeManager.HOUSEPURPOSECODE_ID, CodeType = "用途" },
            new SYSCode { Code = SYSCodeManager.HOUSEPURPOSECODE_24, CodeName  = "补差商品住房",       ID = SYSCodeManager.HOUSEPURPOSECODE_ID, CodeType = "用途" },
            new SYSCode { Code = SYSCodeManager.HOUSEPURPOSECODE_25, CodeName  = "地下室,储藏室",       ID = SYSCodeManager.HOUSEPURPOSECODE_ID, CodeType = "用途" },
            new SYSCode { Code = SYSCodeManager.HOUSEPURPOSECODE_26, CodeName  = "车库",       ID = SYSCodeManager.HOUSEPURPOSECODE_ID, CodeType = "用途" },
            new SYSCode { Code = SYSCodeManager.HOUSEPURPOSECODE_27, CodeName  = "别墅",       ID = SYSCodeManager.HOUSEPURPOSECODE_ID, CodeType = "用途" },
            };
            return list;
        }

        public List<SYSCode> HouseFrontCodeManager()
        {
            return new List<SYSCode>() { 
                new SYSCode(){ Code = SYSCodeManager.HOUSEFRONTCODE_1, CodeName  = "东", ID = SYSCodeManager.HOUSEFRONTCODE_ID, CodeType = "朝向" },
                new SYSCode(){ Code = SYSCodeManager.HOUSEFRONTCODE_2, CodeName  = "南", ID = SYSCodeManager.HOUSEFRONTCODE_ID, CodeType = "朝向" },
                new SYSCode(){ Code = SYSCodeManager.HOUSEFRONTCODE_3, CodeName  = "西", ID = SYSCodeManager.HOUSEFRONTCODE_ID, CodeType = "朝向" },
                new SYSCode(){ Code = SYSCodeManager.HOUSEFRONTCODE_4, CodeName  = "北", ID = SYSCodeManager.HOUSEFRONTCODE_ID, CodeType = "朝向" },
                new SYSCode(){ Code = SYSCodeManager.HOUSEFRONTCODE_5, CodeName  = "东南", ID = SYSCodeManager.HOUSEFRONTCODE_ID, CodeType = "朝向" },
                new SYSCode(){ Code = SYSCodeManager.HOUSEFRONTCODE_6, CodeName  = "东北", ID = SYSCodeManager.HOUSEFRONTCODE_ID, CodeType = "朝向" },
                new SYSCode(){ Code = SYSCodeManager.HOUSEFRONTCODE_7, CodeName  = "西南", ID = SYSCodeManager.HOUSEFRONTCODE_ID, CodeType = "朝向" },
                new SYSCode(){ Code = SYSCodeManager.HOUSEFRONTCODE_8, CodeName  = "西北", ID = SYSCodeManager.HOUSEFRONTCODE_ID, CodeType = "朝向" }
            };
        }

        public List<SYSCode> HouseSightCodeManager()
        {
            return new List<SYSCode>() { 
                new SYSCode(){ Code = SYSCodeManager.HOUSESIGHTCODE_1, CodeName  = "公园景观", ID = SYSCodeManager.HOUSESIGHTCODE_ID, CodeType = "景观" },
                new SYSCode(){ Code = SYSCodeManager.HOUSESIGHTCODE_2, CodeName  = "绿地景观", ID = SYSCodeManager.HOUSESIGHTCODE_ID, CodeType = "景观" },
                new SYSCode(){ Code = SYSCodeManager.HOUSESIGHTCODE_3, CodeName  = "小区景观", ID = SYSCodeManager.HOUSESIGHTCODE_ID, CodeType = "景观" },
                new SYSCode(){ Code = SYSCodeManager.HOUSESIGHTCODE_4, CodeName  = "街景", ID = SYSCodeManager.HOUSESIGHTCODE_ID, CodeType = "景观" },
                new SYSCode(){ Code = SYSCodeManager.HOUSESIGHTCODE_5, CodeName  = "市景", ID = SYSCodeManager.HOUSESIGHTCODE_ID, CodeType = "景观" },
                new SYSCode(){ Code = SYSCodeManager.HOUSESIGHTCODE_6, CodeName  = "海景", ID = SYSCodeManager.HOUSESIGHTCODE_ID, CodeType = "景观" },
                new SYSCode(){ Code = SYSCodeManager.HOUSESIGHTCODE_7, CodeName  = "山景", ID = SYSCodeManager.HOUSESIGHTCODE_ID, CodeType = "景观" },
                new SYSCode(){ Code = SYSCodeManager.HOUSESIGHTCODE_8, CodeName  = "江景", ID = SYSCodeManager.HOUSESIGHTCODE_ID, CodeType = "景观" },
                new SYSCode(){ Code = SYSCodeManager.HOUSESIGHTCODE_9, CodeName  = "湖景", ID = SYSCodeManager.HOUSESIGHTCODE_ID, CodeType = "景观" },
                new SYSCode(){ Code = SYSCodeManager.HOUSESIGHTCODE_10, CodeName  = "无特别景观", ID = SYSCodeManager.HOUSESIGHTCODE_ID, CodeType = "景观" },
                new SYSCode(){ Code = SYSCodeManager.HOUSESIGHTCODE_11, CodeName  = "小区绿地", ID = SYSCodeManager.HOUSESIGHTCODE_ID, CodeType = "景观" },
                new SYSCode(){ Code = SYSCodeManager.HOUSESIGHTCODE_12, CodeName  = "河景", ID = SYSCodeManager.HOUSESIGHTCODE_ID, CodeType = "景观" },
                new SYSCode(){ Code = SYSCodeManager.HOUSESIGHTCODE_13, CodeName  = "有建筑物遮挡", ID = SYSCodeManager.HOUSESIGHTCODE_ID, CodeType = "景观" },
                new SYSCode(){ Code = SYSCodeManager.HOUSESIGHTCODE_14, CodeName  = "临高架桥", ID = SYSCodeManager.HOUSESIGHTCODE_ID, CodeType = "景观" },
                new SYSCode(){ Code = SYSCodeManager.HOUSESIGHTCODE_15, CodeName  = "临铁路", ID = SYSCodeManager.HOUSESIGHTCODE_ID, CodeType = "景观" },
                new SYSCode(){ Code = SYSCodeManager.HOUSESIGHTCODE_16, CodeName  = "临其他厌恶设施", ID = SYSCodeManager.HOUSESIGHTCODE_ID, CodeType = "景观" },

            };
        }

        public List<SYSCode> VDCodeManager()
        {
            List<SYSCode> list = new List<SYSCode>() { 
                new SYSCode { Code = SYSCodeManager.VDCODE_1, CodeName  = "全明通透",       ID = SYSCodeManager.VDCODE_ID, CodeType = "通风采光" },
                new SYSCode { Code = SYSCodeManager.VDCODE_2, CodeName  = "采光欠佳",       ID = SYSCodeManager.VDCODE_ID, CodeType = "通风采光" },
                new SYSCode { Code = SYSCodeManager.VDCODE_3, CodeName  = "通风欠佳",       ID = SYSCodeManager.VDCODE_ID, CodeType = "通风采光" },
                new SYSCode { Code = SYSCodeManager.VDCODE_4, CodeName  = "通风采光欠佳",       ID = SYSCodeManager.VDCODE_ID, CodeType = "通风采光" },
            };
            return list;
        }

        public List<SYSCode> NoiseManager()
        {
            List<SYSCode> list = new List<SYSCode>() { 
                new SYSCode { Code = SYSCodeManager.NOISE_1, CodeName  = "安静",       ID = SYSCodeManager.NOISE_ID, CodeType = "噪音情况" },
                new SYSCode { Code = SYSCodeManager.NOISE_2, CodeName  = "较安静",       ID = SYSCodeManager.NOISE_ID, CodeType = "噪音情况" },
                new SYSCode { Code = SYSCodeManager.NOISE_3, CodeName  = "微吵",       ID = SYSCodeManager.NOISE_ID, CodeType = "噪音情况" },
                new SYSCode { Code = SYSCodeManager.NOISE_4, CodeName  = "较吵",       ID = SYSCodeManager.NOISE_ID, CodeType = "噪音情况" },
                new SYSCode { Code = SYSCodeManager.NOISE_5, CodeName  = "很吵",       ID = SYSCodeManager.NOISE_ID, CodeType = "噪音情况" },
            };
            return list;
        }

        public List<SYSCode> StructureCodeManager()
        {
            List<SYSCode> list = new List<SYSCode>() { 
                new SYSCode { Code = SYSCodeManager.STRUCTURECODE_1, CodeName  = "平面",       ID = SYSCodeManager.STRUCTURECODE_ID, CodeType = "户型结构" },
                new SYSCode { Code = SYSCodeManager.STRUCTURECODE_2, CodeName  = "跃式",       ID = SYSCodeManager.STRUCTURECODE_ID, CodeType = "户型结构" },
                new SYSCode { Code = SYSCodeManager.STRUCTURECODE_3, CodeName  = "复式",       ID = SYSCodeManager.STRUCTURECODE_ID, CodeType = "户型结构" },
                new SYSCode { Code = SYSCodeManager.STRUCTURECODE_4, CodeName  = "错层",       ID = SYSCodeManager.STRUCTURECODE_ID, CodeType = "户型结构" },
                new SYSCode { Code = SYSCodeManager.STRUCTURECODE_5, CodeName  = "LOFT",       ID = SYSCodeManager.STRUCTURECODE_ID, CodeType = "户型结构" },
            };
            return list;
        }

        public List<SYSCode> HouseTypeCodeManager()
        {
            List<SYSCode> list = new List<SYSCode>() { 
                new SYSCode { Code = SYSCodeManager.HOUSETYPECODE_1, CodeName  = "单房",ID = SYSCodeManager.HOUSETYPECODE_ID, CodeType = "户型" },
                new SYSCode { Code = SYSCodeManager.HOUSETYPECODE_2, CodeName  = "单身公寓",ID = SYSCodeManager.HOUSETYPECODE_ID, CodeType = "户型" },
                new SYSCode { Code = SYSCodeManager.HOUSETYPECODE_3, CodeName  = "一室一厅",ID = SYSCodeManager.HOUSETYPECODE_ID, CodeType = "户型" },
                new SYSCode { Code = SYSCodeManager.HOUSETYPECODE_4, CodeName  = "两室一厅",ID = SYSCodeManager.HOUSETYPECODE_ID, CodeType = "户型" },
                new SYSCode { Code = SYSCodeManager.HOUSETYPECODE_5, CodeName  = "两室两厅",ID = SYSCodeManager.HOUSETYPECODE_ID, CodeType = "户型" },
                new SYSCode { Code = SYSCodeManager.HOUSETYPECODE_6, CodeName  = "三室一厅",ID = SYSCodeManager.HOUSETYPECODE_ID, CodeType = "户型" },
                new SYSCode { Code = SYSCodeManager.HOUSETYPECODE_7, CodeName  = "三室两厅",ID = SYSCodeManager.HOUSETYPECODE_ID, CodeType = "户型" },
                new SYSCode { Code = SYSCodeManager.HOUSETYPECODE_8, CodeName  = "四室一厅",ID = SYSCodeManager.HOUSETYPECODE_ID, CodeType = "户型" },
                new SYSCode { Code = SYSCodeManager.HOUSETYPECODE_9, CodeName  = "四室两厅",ID = SYSCodeManager.HOUSETYPECODE_ID, CodeType = "户型" },
                new SYSCode { Code = SYSCodeManager.HOUSETYPECODE_10, CodeName  = "四室三厅",ID = SYSCodeManager.HOUSETYPECODE_ID, CodeType = "户型" },
                new SYSCode { Code = SYSCodeManager.HOUSETYPECODE_11, CodeName  = "五室",ID = SYSCodeManager.HOUSETYPECODE_ID, CodeType = "户型" },
                new SYSCode { Code = SYSCodeManager.HOUSETYPECODE_12, CodeName  = "六室",ID = SYSCodeManager.HOUSETYPECODE_ID, CodeType = "户型" },
                new SYSCode { Code = SYSCodeManager.HOUSETYPECODE_13, CodeName  = "七室及以上",ID = SYSCodeManager.HOUSETYPECODE_ID, CodeType = "户型" },
                new SYSCode { Code = SYSCodeManager.HOUSETYPECODE_14, CodeName  = "一室两厅",ID = SYSCodeManager.HOUSETYPECODE_ID, CodeType = "户型" },
                new SYSCode { Code = SYSCodeManager.HOUSETYPECODE_15, CodeName  = "两室零厅",ID = SYSCodeManager.HOUSETYPECODE_ID, CodeType = "户型" },
                new SYSCode { Code = SYSCodeManager.HOUSETYPECODE_16, CodeName  = "三室零厅",ID = SYSCodeManager.HOUSETYPECODE_ID, CodeType = "户型" },
                new SYSCode { Code = SYSCodeManager.HOUSETYPECODE_17, CodeName  = "四室四厅",ID = SYSCodeManager.HOUSETYPECODE_ID, CodeType = "户型" },
            };
            return list;
        }

        public List<SYSCode> HouseSubHouseTypeManager()
        {
            List<SYSCode> list = new List<SYSCode>() { 
                new SYSCode { Code = SYSCodeManager.SUBHOUSETYPE_1, CodeName  = "地下室",ID = SYSCodeManager.SUBHOUSETYPE_ID, CodeType = "附属房屋类型" },
                new SYSCode { Code = SYSCodeManager.SUBHOUSETYPE_2, CodeName  = "杂物间",ID = SYSCodeManager.SUBHOUSETYPE_ID, CodeType = "附属房屋类型" },
                new SYSCode { Code = SYSCodeManager.SUBHOUSETYPE_3, CodeName  = "车库",ID = SYSCodeManager.SUBHOUSETYPE_ID, CodeType = "附属房屋类型" },
                new SYSCode { Code = SYSCodeManager.SUBHOUSETYPE_4, CodeName  = "摩托车库",ID = SYSCodeManager.SUBHOUSETYPE_ID, CodeType = "附属房屋类型" },
                new SYSCode { Code = SYSCodeManager.SUBHOUSETYPE_5, CodeName  = "下房",ID = SYSCodeManager.SUBHOUSETYPE_ID, CodeType = "附属房屋类型" },
                new SYSCode { Code = SYSCodeManager.SUBHOUSETYPE_6, CodeName  = "储藏室",ID = SYSCodeManager.SUBHOUSETYPE_ID, CodeType = "附属房屋类型" },
                new SYSCode { Code = SYSCodeManager.SUBHOUSETYPE_7, CodeName  = "阁楼",ID = SYSCodeManager.SUBHOUSETYPE_ID, CodeType = "附属房屋类型" },
                new SYSCode { Code = SYSCodeManager.SUBHOUSETYPE_8, CodeName  = "厦子",ID = SYSCodeManager.SUBHOUSETYPE_ID, CodeType = "附属房屋类型" },
                new SYSCode { Code = SYSCodeManager.SUBHOUSETYPE_9, CodeName  = "附房",ID = SYSCodeManager.SUBHOUSETYPE_ID, CodeType = "附属房屋类型" },
                new SYSCode { Code = SYSCodeManager.SUBHOUSETYPE_10, CodeName  = "夹层",ID = SYSCodeManager.SUBHOUSETYPE_ID, CodeType = "附属房屋类型" },
                new SYSCode { Code = SYSCodeManager.SUBHOUSETYPE_11, CodeName  = "地下车库",ID = SYSCodeManager.SUBHOUSETYPE_ID, CodeType = "附属房屋类型" },
                new SYSCode { Code = SYSCodeManager.SUBHOUSETYPE_12, CodeName  = "车位",ID = SYSCodeManager.SUBHOUSETYPE_ID, CodeType = "附属房屋类型" },
            };
            return list;
        }

        public List<SYSCode> HouseFitmentCodeTypeManager()
        {
            List<SYSCode> list = new List<SYSCode>() { 
                new SYSCode { Code = SYSCodeManager.FITMENTCODE_1, CodeName  = "豪华",ID = SYSCodeManager.FITMENTCODE_ID, CodeType = "装修" },
                new SYSCode { Code = SYSCodeManager.FITMENTCODE_2, CodeName  = "高档",ID = SYSCodeManager.FITMENTCODE_ID, CodeType = "装修" },
                new SYSCode { Code = SYSCodeManager.FITMENTCODE_3, CodeName  = "中档",ID = SYSCodeManager.FITMENTCODE_ID, CodeType = "装修" },
                new SYSCode { Code = SYSCodeManager.FITMENTCODE_4, CodeName  = "普通",ID = SYSCodeManager.FITMENTCODE_ID, CodeType = "装修" },
                new SYSCode { Code = SYSCodeManager.FITMENTCODE_5, CodeName  = "简易",ID = SYSCodeManager.FITMENTCODE_ID, CodeType = "装修" },
                new SYSCode { Code = SYSCodeManager.FITMENTCODE_6, CodeName  = "毛坯",ID = SYSCodeManager.FITMENTCODE_ID, CodeType = "装修" },
            };
            return list;
        }

        public List<SYSCode> GetAllParkingStatusList()
        {
            List<SYSCode> list = new List<SYSCode>() { 
                new SYSCode { Code = 1056001, CodeName  = "充裕", ID = 1056, CodeType = "停车状况" },
                new SYSCode { Code = 1056002, CodeName  = "够用", ID = 1056, CodeType = "停车状况" },
                new SYSCode { Code = 1056003, CodeName  = "稍紧张", ID = 1056, CodeType = "停车状况" },
                new SYSCode { Code = 1056004, CodeName  = "紧张", ID = 1056, CodeType = "停车状况" },
                new SYSCode { Code = 1056005, CodeName = "很紧张",     ID = 1056, CodeType = "停车状况" },
            };
            return list;
        }

        public List<SYSCode> AllotStatusCodeManager()
        {
            return new List<SYSCode>()
            {
                new SYSCode { Code = SYSCodeManager.STATECODE_1, CodeName = "待分配", ID = SYSCodeManager.STATECODE_ID, CodeType = "任务状态" },
                new SYSCode { Code = SYSCodeManager.STATECODE_2, CodeName = "已分配", ID = SYSCodeManager.STATECODE_ID, CodeType = "任务状态" },
                new SYSCode { Code = SYSCodeManager.STATECODE_3, CodeName = "已接受", ID = SYSCodeManager.STATECODE_ID, CodeType = "任务状态" },
                new SYSCode { Code = SYSCodeManager.STATECODE_4, CodeName = "查勘中", ID = SYSCodeManager.STATECODE_ID, CodeType = "任务状态" },
                new SYSCode { Code = SYSCodeManager.STATECODE_5, CodeName = "已查勘", ID = SYSCodeManager.STATECODE_ID, CodeType = "任务状态" },
                new SYSCode { Code = SYSCodeManager.STATECODE_6, CodeName = "自审通过", ID = SYSCodeManager.STATECODE_ID, CodeType = "任务状态" },
                new SYSCode { Code = SYSCodeManager.STATECODE_7, CodeName = "自审不通过", ID = SYSCodeManager.STATECODE_ID, CodeType = "任务状态" },
                new SYSCode { Code = SYSCodeManager.STATECODE_8, CodeName = "审核通过", ID = SYSCodeManager.STATECODE_ID, CodeType = "任务状态" },
                new SYSCode { Code = SYSCodeManager.STATECODE_9, CodeName = "审核不通过", ID = SYSCodeManager.STATECODE_ID, CodeType = "任务状态" },
                new SYSCode { Code = SYSCodeManager.STATECODE_10, CodeName = "已入库", ID = SYSCodeManager.STATECODE_ID, CodeType = "任务状态" }
            };
        }

        public List<SYSCode> BHousetypeCodeManager()
        {
            return new List<SYSCode>()
            {
                new SYSCode { Code = SYSCodeManager.BHOUSETYPECODE_1, CodeName = "小户型", ID = SYSCodeManager.BHOUSETYPECODE_ID, CodeType = "户型面积" },
                new SYSCode { Code = SYSCodeManager.BHOUSETYPECODE_2, CodeName = "大户型", ID = SYSCodeManager.BHOUSETYPECODE_ID, CodeType = "户型面积" },
                new SYSCode { Code = SYSCodeManager.BHOUSETYPECODE_3, CodeName = "复式户型", ID = SYSCodeManager.BHOUSETYPECODE_ID, CodeType = "户型面积" },
                new SYSCode { Code = SYSCodeManager.BHOUSETYPECODE_4, CodeName = "特殊户型", ID = SYSCodeManager.BHOUSETYPECODE_ID, CodeType = "户型面积" },
            };
        }

        public List<SYSCode> PlanPurposeCodeManager()
        {
            return new List<SYSCode>()
            {
                new SYSCode { Code = SYSCodeManager.PLANPURPOSECODE_1, CodeName = "居住用地", ID = SYSCodeManager.PLANPURPOSECODE_ID, CodeType = "土地用途" },
                new SYSCode { Code = SYSCodeManager.PLANPURPOSECODE_2, CodeName = "商业、居住用地", ID = SYSCodeManager.PLANPURPOSECODE_ID, CodeType = "土地用途" },
                new SYSCode { Code = SYSCodeManager.PLANPURPOSECODE_3, CodeName = "商业用地", ID = SYSCodeManager.PLANPURPOSECODE_ID, CodeType = "土地用途" },
                new SYSCode { Code = SYSCodeManager.PLANPURPOSECODE_4, CodeName = "商业、办公用地", ID = SYSCodeManager.PLANPURPOSECODE_ID, CodeType = "土地用途" },
                new SYSCode { Code = SYSCodeManager.PLANPURPOSECODE_5, CodeName = "办公用地", ID = SYSCodeManager.PLANPURPOSECODE_ID, CodeType = "土地用途" },
                new SYSCode { Code = SYSCodeManager.PLANPURPOSECODE_6, CodeName = "批发零售用地", ID = SYSCodeManager.PLANPURPOSECODE_ID, CodeType = "土地用途" },
                new SYSCode { Code = SYSCodeManager.PLANPURPOSECODE_7, CodeName = "住宿餐饮用地", ID = SYSCodeManager.PLANPURPOSECODE_ID, CodeType = "土地用途" },
                new SYSCode { Code = SYSCodeManager.PLANPURPOSECODE_8, CodeName = "商务金融用地", ID = SYSCodeManager.PLANPURPOSECODE_ID, CodeType = "土地用途" },
                new SYSCode { Code = SYSCodeManager.PLANPURPOSECODE_9, CodeName = "其他商服用地", ID = SYSCodeManager.PLANPURPOSECODE_ID, CodeType = "土地用途" },
                new SYSCode { Code = SYSCodeManager.PLANPURPOSECODE_10, CodeName = "工业用地", ID = SYSCodeManager.PLANPURPOSECODE_ID, CodeType = "土地用途" },
                new SYSCode { Code = SYSCodeManager.PLANPURPOSECODE_11, CodeName = "仓储用地", ID = SYSCodeManager.PLANPURPOSECODE_ID, CodeType = "土地用途" },
                new SYSCode { Code = SYSCodeManager.PLANPURPOSECODE_12, CodeName = "采矿用地", ID = SYSCodeManager.PLANPURPOSECODE_ID, CodeType = "土地用途" },
                new SYSCode { Code = SYSCodeManager.PLANPURPOSECODE_13, CodeName = "工矿仓储用地", ID = SYSCodeManager.PLANPURPOSECODE_ID, CodeType = "土地用途" },
                new SYSCode { Code = SYSCodeManager.PLANPURPOSECODE_14, CodeName = "公共管理与公共服务用地", ID = SYSCodeManager.PLANPURPOSECODE_ID, CodeType = "土地用途" },
                new SYSCode { Code = SYSCodeManager.PLANPURPOSECODE_15, CodeName = "机关团体用地", ID = SYSCodeManager.PLANPURPOSECODE_ID, CodeType = "土地用途" },
                new SYSCode { Code = SYSCodeManager.PLANPURPOSECODE_16, CodeName = "新闻出版用地", ID = SYSCodeManager.PLANPURPOSECODE_ID, CodeType = "土地用途" },
                new SYSCode { Code = SYSCodeManager.PLANPURPOSECODE_17, CodeName = "科教用地", ID = SYSCodeManager.PLANPURPOSECODE_ID, CodeType = "土地用途" },
                new SYSCode { Code = SYSCodeManager.PLANPURPOSECODE_18, CodeName = "医卫慈善用地", ID = SYSCodeManager.PLANPURPOSECODE_ID, CodeType = "土地用途" },
                new SYSCode { Code = SYSCodeManager.PLANPURPOSECODE_19, CodeName = "文体娱乐用地", ID = SYSCodeManager.PLANPURPOSECODE_ID, CodeType = "土地用途" },
                new SYSCode { Code = SYSCodeManager.PLANPURPOSECODE_20, CodeName = "公共设施用地", ID = SYSCodeManager.PLANPURPOSECODE_ID, CodeType = "土地用途" },
                new SYSCode { Code = SYSCodeManager.PLANPURPOSECODE_21, CodeName = "公园与绿地", ID = SYSCodeManager.PLANPURPOSECODE_ID, CodeType = "土地用途" },
                new SYSCode { Code = SYSCodeManager.PLANPURPOSECODE_22, CodeName = "风景名胜设施用地", ID = SYSCodeManager.PLANPURPOSECODE_ID, CodeType = "土地用途" },
                new SYSCode { Code = SYSCodeManager.PLANPURPOSECODE_23, CodeName = "特殊用地", ID = SYSCodeManager.PLANPURPOSECODE_ID, CodeType = "土地用途" },
                new SYSCode { Code = SYSCodeManager.PLANPURPOSECODE_24, CodeName = "交通运输用地", ID = SYSCodeManager.PLANPURPOSECODE_ID, CodeType = "土地用途" },
                new SYSCode { Code = SYSCodeManager.PLANPURPOSECODE_25, CodeName = "公路用地", ID = SYSCodeManager.PLANPURPOSECODE_ID, CodeType = "土地用途" },
                new SYSCode { Code = SYSCodeManager.PLANPURPOSECODE_26, CodeName = "街巷用地", ID = SYSCodeManager.PLANPURPOSECODE_ID, CodeType = "土地用途" },
                new SYSCode { Code = SYSCodeManager.PLANPURPOSECODE_27, CodeName = "机场用地", ID = SYSCodeManager.PLANPURPOSECODE_ID, CodeType = "土地用途" },
                new SYSCode { Code = SYSCodeManager.PLANPURPOSECODE_28, CodeName = "港口码头用地", ID = SYSCodeManager.PLANPURPOSECODE_ID, CodeType = "土地用途" },
                new SYSCode { Code = SYSCodeManager.PLANPURPOSECODE_29, CodeName = "管道运输用地", ID = SYSCodeManager.PLANPURPOSECODE_ID, CodeType = "土地用途" },
                new SYSCode { Code = SYSCodeManager.PLANPURPOSECODE_30, CodeName = "城镇住宅用地", ID = SYSCodeManager.PLANPURPOSECODE_ID, CodeType = "土地用途" },
                new SYSCode { Code = SYSCodeManager.PLANPURPOSECODE_31, CodeName = "农村宅基地", ID = SYSCodeManager.PLANPURPOSECODE_ID, CodeType = "土地用途" },
                new SYSCode { Code = SYSCodeManager.PLANPURPOSECODE_32, CodeName = "其他", ID = SYSCodeManager.PLANPURPOSECODE_ID, CodeType = "土地用途" },
            };
        }

        public int[] GetViewFunctionCodes()
        {
            return new int[] { SYSCodeManager.FunOperCode_1, SYSCodeManager.FunOperCode_2, SYSCodeManager.FunOperCode_3 };
        }
    }
}
