#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

#endregion

namespace RockslopeAPI.Helpers;

public static class ReaderToObject
{
    public static T DataReaderMapToItem<T>(this IDataReader dr)
    {
        T obj = default(T);
        dr.Read();
        obj = Activator.CreateInstance<T>();
        foreach (PropertyInfo prop in obj.GetType().GetProperties(BindingFlags.Public|BindingFlags.Instance))
        {
            try
            {
                if (!Equals(dr[prop.Name], DBNull.Value))
                {
                    prop.SetValue(obj, dr[prop.Name], null);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        return obj;
    }
    
    public static List<T> DataReaderMapToList<T>(this IDataReader dr)
    {
        List<T> list = new List<T>();
        T obj = default(T);
        
        
        while (dr.Read())
        {
            obj = Activator.CreateInstance<T>();
            foreach (PropertyInfo prop in obj.GetType().GetProperties(BindingFlags.Public|BindingFlags.Instance))
            {
                try
                {
                    Console.WriteLine(dr[prop.Name]);
                    if (!Equals(dr[prop.Name], DBNull.Value))
                    {
                        prop.SetValue(obj, dr[prop.Name], null);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            list.Add(obj);
        }

        return list;
    }
}