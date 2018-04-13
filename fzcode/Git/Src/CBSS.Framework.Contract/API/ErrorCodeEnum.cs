using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CBSS.Framework.Contract.API
{
    /// <summary>
    /// 接口错误枚举
    /// </summary>
    public enum ErrorCodeEnum
    {
        #region 接口内部返回提示
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.请求参数有误”的 XML 注释
        请求参数有误 = 100001,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.请求参数有误”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.请求参数不能为空”的 XML 注释
        请求参数不能为空= 100002,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.请求参数不能为空”的 XML 注释
        #endregion

        #region 接口内部异常
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.数据库插入操作失败”的 XML 注释
        数据库插入操作失败 = 200001,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.数据库插入操作失败”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.数据库更新操作失败”的 XML 注释
        数据库更新操作失败 = 200001,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.数据库更新操作失败”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.数据库删除操作失败”的 XML 注释
        数据库删除操作失败 = 200001,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.数据库删除操作失败”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.用户使用App时长埋点失败”的 XML 注释
        用户使用App时长埋点失败 = 200002,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.用户使用App时长埋点失败”的 XML 注释
        #endregion

        #region 接口外部异常
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.请求接口失效”的 XML 注释
        请求接口失效 = 300001,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.请求接口失效”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.请求接口超时”的 XML 注释
        请求接口超时 = 300002,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.请求接口超时”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.返回参数有误”的 XML 注释
        返回参数有误 = 400001,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.返回参数有误”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.请求参数解密失败”的 XML 注释
        请求参数解密失败 = 400002,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.请求参数解密失败”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.返回参数加密失败”的 XML 注释
        返回参数加密失败 = 400003,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.返回参数加密失败”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.接口访问异常”的 XML 注释
        接口访问异常 = 400006,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.接口访问异常”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.接口方法名不正确”的 XML 注释
        接口方法名不正确 = 400007,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.接口方法名不正确”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.接口请求Info为空”的 XML 注释
        接口请求Info为空 = 400008,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.接口请求Info为空”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.接口请求Key为空”的 XML 注释
        接口请求Key为空 = 400008,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.接口请求Key为空”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.接口请求异常”的 XML 注释
        接口请求异常 = 400009,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.接口请求异常”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.发送短信失败”的 XML 注释
        发送短信失败= 400010,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.发送短信失败”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.验证码发送失败”的 XML 注释
        验证码发送失败= 400011,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.验证码发送失败”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.请使用五分钟内获取的验证码登录”的 XML 注释
        请使用五分钟内获取的验证码登录= 400012,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.请使用五分钟内获取的验证码登录”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.未找到课本”的 XML 注释
        未找到课本= 400013,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.未找到课本”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.未获取到模块ID”的 XML 注释
        未获取到模块ID= 400014,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.未获取到模块ID”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.未找到资源”的 XML 注释
        未找到资源= 400015,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.未找到资源”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.未找到报告”的 XML 注释
        未找到报告 = 400016,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.未找到报告”的 XML 注释
        
        #endregion

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.请输入有效的验证码”的 XML 注释
        请输入有效的验证码 = 400017,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.请输入有效的验证码”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.没有找到对应的手机号”的 XML 注释
        没有找到对应的手机号=400018,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.没有找到对应的手机号”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.登录失败”的 XML 注释
        登录失败=4000019,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.登录失败”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.操作失败”的 XML 注释
        操作失败=4000020,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.操作失败”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.用户不存在”的 XML 注释
        用户不存在=4000021,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.用户不存在”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.未找到目录”的 XML 注释
        未找到目录 = 400022,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ErrorCodeEnum.未找到目录”的 XML 注释
    }
}
