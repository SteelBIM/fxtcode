﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18408
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace FxtService.Proxy.FxtUser {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="FxtUser.IFxtUsers")]
    public interface IFxtUsers {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFxtUsers/GetProduct", ReplyAction="http://tempuri.org/IFxtUsers/GetProductResponse")]
        string GetProduct();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFxtUsers/GetProducts", ReplyAction="http://tempuri.org/IFxtUsers/GetProductsResponse")]
        string GetProducts(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFxtUsers/Products", ReplyAction="http://tempuri.org/IFxtUsers/ProductsResponse")]
        bool Products([System.ServiceModel.MessageParameterAttribute(Name="products")] string products1, string operat);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFxtUsers/GetProductAll", ReplyAction="http://tempuri.org/IFxtUsers/GetProductAllResponse")]
        string GetProductAll();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFxtUsers/GetProductMenuByPurview", ReplyAction="http://tempuri.org/IFxtUsers/GetProductMenuByPurviewResponse")]
        string GetProductMenuByPurview(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFxtUsers/Menus", ReplyAction="http://tempuri.org/IFxtUsers/MenusResponse")]
        bool Menus(int productid, [System.ServiceModel.MessageParameterAttribute(Name="menus")] string menus1, string operat);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFxtUsers/MenuPurview", ReplyAction="http://tempuri.org/IFxtUsers/MenuPurviewResponse")]
        bool MenuPurview(int menuid, string purview);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFxtUsers/GetMenuPurview", ReplyAction="http://tempuri.org/IFxtUsers/GetMenuPurviewResponse")]
        string GetMenuPurview(int menuid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFxtUsers/GetMenuListByProductId", ReplyAction="http://tempuri.org/IFxtUsers/GetMenuListByProductIdResponse")]
        string GetMenuListByProductId(int productid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFxtUsers/GetMenuListAll", ReplyAction="http://tempuri.org/IFxtUsers/GetMenuListAllResponse")]
        string GetMenuListAll();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFxtUsers/GetMenus", ReplyAction="http://tempuri.org/IFxtUsers/GetMenusResponse")]
        string GetMenus(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFxtUsers/ProductMenus", ReplyAction="http://tempuri.org/IFxtUsers/ProductMenusResponse")]
        bool ProductMenus([System.ServiceModel.MessageParameterAttribute(Name="productmenus")] string productmenus1, string operat);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFxtUsers/GetProductMenus", ReplyAction="http://tempuri.org/IFxtUsers/GetProductMenusResponse")]
        string GetProductMenus(int menuid, int productid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFxtUsers/GetPurview", ReplyAction="http://tempuri.org/IFxtUsers/GetPurviewResponse")]
        string GetPurview(string purview, int pageSize, int pageIndex);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFxtUsers/GetPurviewAll", ReplyAction="http://tempuri.org/IFxtUsers/GetPurviewAllResponse")]
        string GetPurviewAll();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFxtUsers/Purviews", ReplyAction="http://tempuri.org/IFxtUsers/PurviewsResponse")]
        bool Purviews(string purview, string operta);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFxtUsers/GetPurviews", ReplyAction="http://tempuri.org/IFxtUsers/GetPurviewsResponse")]
        string GetPurviews(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFxtUsers/GetUserPurview", ReplyAction="http://tempuri.org/IFxtUsers/GetUserPurviewResponse")]
        string GetUserPurview(string userid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFxtUsers/UserPurview", ReplyAction="http://tempuri.org/IFxtUsers/UserPurviewResponse")]
        bool UserPurview(string userid, int productid, string purview);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFxtUsers/Entrance", ReplyAction="http://tempuri.org/IFxtUsers/EntranceResponse")]
        object Entrance(string date, string code, string methodName, string methodValue);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IFxtUsersChannel : FxtService.Proxy.FxtUser.IFxtUsers, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class FxtUsersClient : System.ServiceModel.ClientBase<FxtService.Proxy.FxtUser.IFxtUsers>, FxtService.Proxy.FxtUser.IFxtUsers {
        
        public FxtUsersClient() {
        }
        
        public FxtUsersClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public FxtUsersClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FxtUsersClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FxtUsersClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string GetProduct() {
            return base.Channel.GetProduct();
        }
        
        public string GetProducts(int id) {
            return base.Channel.GetProducts(id);
        }
        
        public bool Products(string products1, string operat) {
            return base.Channel.Products(products1, operat);
        }
        
        public string GetProductAll() {
            return base.Channel.GetProductAll();
        }
        
        public string GetProductMenuByPurview(int id) {
            return base.Channel.GetProductMenuByPurview(id);
        }
        
        public bool Menus(int productid, string menus1, string operat) {
            return base.Channel.Menus(productid, menus1, operat);
        }
        
        public bool MenuPurview(int menuid, string purview) {
            return base.Channel.MenuPurview(menuid, purview);
        }
        
        public string GetMenuPurview(int menuid) {
            return base.Channel.GetMenuPurview(menuid);
        }
        
        public string GetMenuListByProductId(int productid) {
            return base.Channel.GetMenuListByProductId(productid);
        }
        
        public string GetMenuListAll() {
            return base.Channel.GetMenuListAll();
        }
        
        public string GetMenus(int id) {
            return base.Channel.GetMenus(id);
        }
        
        public bool ProductMenus(string productmenus1, string operat) {
            return base.Channel.ProductMenus(productmenus1, operat);
        }
        
        public string GetProductMenus(int menuid, int productid) {
            return base.Channel.GetProductMenus(menuid, productid);
        }
        
        public string GetPurview(string purview, int pageSize, int pageIndex) {
            return base.Channel.GetPurview(purview, pageSize, pageIndex);
        }
        
        public string GetPurviewAll() {
            return base.Channel.GetPurviewAll();
        }
        
        public bool Purviews(string purview, string operta) {
            return base.Channel.Purviews(purview, operta);
        }
        
        public string GetPurviews(int id) {
            return base.Channel.GetPurviews(id);
        }
        
        public string GetUserPurview(string userid) {
            return base.Channel.GetUserPurview(userid);
        }
        
        public bool UserPurview(string userid, int productid, string purview) {
            return base.Channel.UserPurview(userid, productid, purview);
        }
        
        public object Entrance(string date, string code, string methodName, string methodValue) {
            return base.Channel.Entrance(date, code, methodName, methodValue);
        }
    }
}