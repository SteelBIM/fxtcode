using System;
using System.Web;
using System.Web.Hosting;
using System.IO;
using System.Collections;

namespace CAS.Common
{
    /// <summary>
    /// 虚拟文件系统帮助类
    /// 在global.asax中注册:
    /// protected void Application_Start(object sender, EventArgs e)
    /// {
    ///     System.Web.Hosting.HostingEnvironment.RegisterVirtualPathProvider(new DllVirtualPathProvider());
    /// }
    /// 调用:
    /// Control control1 = this.LoadControl("/MyUserControl/Test.Control.dll/Test.Control.Sample.List.ascx");
    /// Controls.Add(control1);
    /// </summary>
    public class VirtualPathHelper: System.Web.Hosting.VirtualPathProvider
    {
        public string DLLPath = "";
        public VirtualPathHelper(string dllpath) { 
            DLLPath = dllpath; 
        }

        public override string CombineVirtualPaths(string basePath, string relativePath)
        {
            if (IsAppResourcePath(basePath))
            {
                return null;
            }

            return Previous.CombineVirtualPaths(basePath, relativePath);
        }

        public override System.Runtime.Remoting.ObjRef CreateObjRef(Type requestedType)
        {
            return Previous.CreateObjRef(requestedType);
        }

        public override bool DirectoryExists(string virtualDir)
        {
            if (IsAppResourcePath(virtualDir))
            {
                return true;
            }
            else
            {
                return Previous.DirectoryExists(virtualDir);
            }

        }

        public override string GetCacheKey(string virtualPath)
        {
            if (IsAppResourcePath(virtualPath))
            {
                return null;
            }
            else
            {
                return Previous.GetCacheKey(virtualPath);
            }
        }

        public override string GetFileHash(string virtualPath, IEnumerable virtualPathDependencies)
        {
            if (IsAppResourcePath(virtualPath))
            {
                return null;
            }
            else
            {
                return Previous.GetFileHash(virtualPath, virtualPathDependencies);
            }
        }

        private bool IsAppResourcePath(string virtualPath)
        {
            String checkPath = VirtualPathUtility.ToAppRelative(virtualPath);
            return checkPath.StartsWith(DLLPath, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool FileExists(string virtualPath)
        {
            return (IsAppResourcePath(virtualPath) || Previous.FileExists(virtualPath));
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            if (IsAppResourcePath(virtualPath))
            {
                return new AssemblyResourceVirtualFile(virtualPath);
            }
            else
            {
                return Previous.GetFile(virtualPath);
            }
        }

        public override System.Web.Caching.CacheDependency GetCacheDependency(string virtualPath,
               System.Collections.IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            if (IsAppResourcePath(virtualPath))
            {
                string path = HttpRuntime.BinDirectory;
                return new System.Web.Caching.CacheDependency(path);
            }
            else
            {
                return Previous.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
            }
        }
    }


    class AssemblyResourceVirtualFile : VirtualFile
    {
        string path;
        public AssemblyResourceVirtualFile(string virtualPath)
            : base(virtualPath)
        {
            path = VirtualPathUtility.ToAppRelative(virtualPath);
        }

        public override System.IO.Stream Open()
        {
            string[] parts = path.Split('/');
            string assemblyName = parts[1];
            string resourceName = parts[2];
            assemblyName = Path.Combine(HttpRuntime.BinDirectory, assemblyName);
            System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFrom(assemblyName);
            if (assembly != null)
            {
                return assembly.GetManifestResourceStream(resourceName);
            }
            return null;
        }
    }
}