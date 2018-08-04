using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace InterfaceActivityBuilder.Base
{
    internal static class ReflectionHelper
    {
        public static Dictionary<string, Type> Cache { get; set; }

        internal static Type GetRequestType(string rootTypeName)
        {
            try
            {
                // ASSUMPTIONS !!!!!
                // ValidarDeudaLimiteRequestMessage
                // Proxy class should be in namespace which equals to Request message parameter and remove RequestMessage part.i.e. below proxy name space is should be ValidarDeudaLimite--RequestMessage
                // public ValidarDeudaLimiteRequestType ValidarDeudaLimiteRequestMessage;

                string value1 = rootTypeName.Replace("RequestMessage", "");
                string value2 = rootTypeName.Replace("Message", "Type");
                string proxyNamespace = $"AmxPeruCommonLibrary.ServiceContracts.Services.Claro.Common.{value1}.{value2}";
                string proxyAssemblyName = ConfigurationSettings.AppSettings["ProxyAssemblyName"] ?? throw new Exception("ProxyAssemblyName configuration key not found in App.Config file.");
                string fullAssemblyName = $"{proxyNamespace}, {proxyAssemblyName}";
                // ASSUMPTIONS !!!!!

                string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string assemblyPath = $"{assemblyFolder}\\{proxyAssemblyName}.dll";

                // TODO REMOVE  -- problem farklı dll lokasyonunda olmamız
                assemblyPath = assemblyPath.Replace("CanonicOutput", "CanonicResolver");

                Assembly assembly = Assembly.LoadFile(assemblyPath);
                var proxyType = assembly.GetType(proxyNamespace);

                if (proxyType != null)
                    return proxyType;

                throw new Exception(AssemblyErrorMessage);
            }
            catch (Exception exception)
            {
                throw new Exception($"{AssemblyErrorMessage}_{ exception.Message}");
            }
        }

        internal static Type GetCurrentPropertyType(CanonicTreeItem existingNode, string newNodeTypeName)
        {
            try
            {
                if (newNodeTypeName.Contains(arraySuffix))
                {
                    return existingNode.NodeType.GetElementType();
                }

                var property = existingNode.NodeType.GetProperty(newNodeTypeName);
                if (property == null && existingNode.NodeType.IsArray)
                {
                    var propType = existingNode.NodeType.GetElementType();
                    property = propType.GetProperty(newNodeTypeName);
                }
                return property.PropertyType;
            }
            catch (Exception exception)
            {
                throw new Exception($"The given type name is not exist : {exception.Message}");
            }
        }

        private const string arraySuffix = "_arrayItem";
        private static readonly string AssemblyErrorMessage = $"Proxy assembly name could not be found. Change proxy assembly name acording to RequestType.if type is ValidarDeudaLimiteRequestType, proxy namespace endswith ValidarDeudaLimite.ValidarDeudaLimiteRequestType.";
    }
}
