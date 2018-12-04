using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web.Services.Description;

namespace Tools
{
	public class WSHelper
	{
		public static object InvokeWebService(string url, string methodname, object[] args)
		{
			return WSHelper.InvokeWebService(url, null, methodname, args);
		}

		public static object InvokeWebService(string url, string classname, string methodname, object[] args)
		{
			string text = "EnterpriseServerBase.WebService.DynamicWebCalling";
			bool flag = classname == null || classname == "";
			if (flag)
			{
				classname = WSHelper.GetWsClassName(url);
			}
			object result;
			try
			{
				WebClient webClient = new WebClient();
				Stream stream = webClient.OpenRead(url + "?WSDL");
				ServiceDescription serviceDescription = ServiceDescription.Read(stream);
				ServiceDescriptionImporter serviceDescriptionImporter = new ServiceDescriptionImporter();
				serviceDescriptionImporter.AddServiceDescription(serviceDescription, "", "");
				CodeNamespace codeNamespace = new CodeNamespace(text);
				CodeCompileUnit codeCompileUnit = new CodeCompileUnit();
				codeCompileUnit.Namespaces.Add(codeNamespace);
				serviceDescriptionImporter.Import(codeNamespace, codeCompileUnit);
				CSharpCodeProvider cSharpCodeProvider = new CSharpCodeProvider();
				CompilerResults compilerResults = cSharpCodeProvider.CompileAssemblyFromDom(new CompilerParameters
				{
					GenerateExecutable = false,
					GenerateInMemory = true,
					ReferencedAssemblies = 
					{
						"System.dll",
						"System.XML.dll",
						"System.Web.Services.dll",
						"System.Data.dll"
					}
				}, new CodeCompileUnit[]
				{
					codeCompileUnit
				});
				bool hasErrors = compilerResults.Errors.HasErrors;
				if (hasErrors)
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (CompilerError compilerError in compilerResults.Errors)
					{
						stringBuilder.Append(compilerError.ToString());
						stringBuilder.Append(Environment.NewLine);
					}
					throw new Exception(stringBuilder.ToString());
				}
				Assembly compiledAssembly = compilerResults.CompiledAssembly;
				Type type = compiledAssembly.GetType(text + "." + classname, true, true);
				object obj = Activator.CreateInstance(type);
				MethodInfo method = type.GetMethod(methodname);
				result = method.Invoke(obj, args);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.InnerException.Message, new Exception(ex.InnerException.StackTrace));
			}
			return result;
		}

		private static string GetWsClassName(string wsUrl)
		{
			string[] array = wsUrl.Split(new char[]
			{
				'/'
			});
			string[] array2 = array[array.Length - 1].Split(new char[]
			{
				'.'
			});
			return array2[0];
		}
	}
}
