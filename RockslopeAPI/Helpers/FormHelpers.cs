using System;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace RockslopeAPI.Helpers;

public static class FormHelpers
{
    public static T ToClass<T>(this IFormCollection formCollection)
    {
        T obj = Activator.CreateInstance<T>();
        foreach (PropertyInfo prop in obj.GetType().GetProperties(BindingFlags.Public|BindingFlags.Instance))
        {
            try
            {
                if (formCollection.TryGetValue(prop.Name, out StringValues values))
                {
                    
                    //TODO:must be a better way to handle this
                    string value = values.ToString();
                    if (prop.PropertyType == typeof(string))
                    {
                        prop.SetValue(obj, value,null);
                    }
                    else if (prop.PropertyType == typeof(DateTime))
                    {
                        prop.SetValue(obj, DateTime.Parse(value),null);
                    }
                    else if(prop.PropertyType == typeof(int))
                    {
                        prop.SetValue(obj, int.Parse(value),null);
                    }
                    else if(prop.PropertyType == typeof(float))
                    {
                        prop.SetValue(obj, float.Parse(value),null);
                    }
                    else if(prop.PropertyType == typeof(bool))
                    {
                        prop.SetValue(obj, bool.Parse(value),null);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        return obj;
    }
}