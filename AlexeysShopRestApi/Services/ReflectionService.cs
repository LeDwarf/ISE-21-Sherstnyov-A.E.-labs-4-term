using AlexeysShopService.Attributies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace AlexeysShopRestApi.Services
{
    public class ReflectionService
    {
        public List<string> GetInfoByAssembly()
        {
            List<string> infoes = new List<string>();
            Assembly assembly = GetAssembly("AlexeysShopService");
            List<Type> interfaces = GetInterfases(assembly);
            foreach (var inter in interfaces)
            {
                CustomAttributeData attr = inter.CustomAttributes.FirstOrDefault(x => x.AttributeType.Name == "CustomInterfaceAttribute");
                if (attr != null)
                {
                    infoes.Add(string.Format("Интерфейс: {0}. {1}", inter.Name, attr.ConstructorArguments[0].Value));
                    infoes.AddRange(GetMethodInfoes(inter));
                }
            }
            List<Type> classes = GetClasses(assembly);
            foreach (var inter in classes)
            {
                CustomAttributeData attr = inter.CustomAttributes.FirstOrDefault(x => x.AttributeType.Name == "CustomClassAttribute");
                if (attr != null)
                {
                    infoes.Add(string.Format("Класс: {0}. {1}", inter.Name, attr.ConstructorArguments[0].Value));
                    infoes.AddRange(GetMethodInfoes(inter));
                }
            }
            return infoes;
        }

        private Assembly GetAssembly(string assemblyName)
        {
           return Assembly.Load(assemblyName);
        }

        private List<Type> GetInterfases(Assembly assembly)
        {
            return assembly.GetTypes().Where(x => x.IsClass).ToList();
        }

        private List<Type> GetClasses(Assembly assembly)
        {
            return assembly.GetTypes().Where(x => x.IsClass).ToList();
        }

        private List<string> GetMethodInfoes(Type inter)
        {
            List<string> infoes = new List<string>();
            MethodInfo[] methods = inter.GetMethods();

            for (int i = 0; i < methods.Length; ++i)
            {
                CustomAttributeData attr = methods[i].CustomAttributes.FirstOrDefault(x => x.AttributeType.Name == "CustomMethodAttribute");
                if (attr != null)
                {
                    infoes.Add(string.Format("--> Метод: {0}. {1}", methods[i].Name, attr.ConstructorArguments[0].Value));
                }
            }
            return infoes;
        }
    }
}