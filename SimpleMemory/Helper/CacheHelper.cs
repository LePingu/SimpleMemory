using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using SimpleMemory.Helper;
using SimpleMemory.Models;

namespace SimpleMemory.CacheManager
{
    public static class CacheHelper
    {

        public static string CreateKey(MethodCallExpression exp)
        {
            Key key = new Key();
            key.MethodName = exp.Method.Name;
            key.Parameters = exp.Arguments.Select(Evaluator.Evaluate).ToArray();
            var keyString = JsonConvert.SerializeObject(key);
            // Initially wanted to do conversion to a less memory consuming key
            // With conversion to bytes and salting. Needs more research
            // return BitConverter.ToInt64(Encoding.UTF8.GetBytes(keyString));
            return keyString;
        }
    }

}