using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChargeBee.Api
{
    public class Params
    {
        private readonly Dictionary<string, object> _mDict = new();

        public void Add(string key, object value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                throw new ArgumentException($"Value for {key} can't be empty or null!");

            AddOpt(key, value);
        }

        public void AddOpt(string key, object value)
        {
            _mDict.Add(key, value == null ? string.Empty : ConvertValue(value, false));
        }

        public void AddOpt(string key, object value, bool isDate)
        {
            _mDict.Add(key, value == null ? string.Empty : ConvertValue(value, isDate));
        }

        public string GetQuery(bool isList)
        {
            return string.Join("&", GetPairs(isList));
        }

        private string[] GetPairs(bool isList)
        {
            var pairs = new List<string>(_mDict.Keys.Count);

            foreach (var pair in _mDict)
                if (pair.Value is IList)
                {
                    if (isList)
                    {
                        pairs.Add(
                            $"{WebUtility.UrlEncode(pair.Key)}={WebUtility.UrlEncode(JsonConvert.SerializeObject(pair.Value))}");
                        continue;
                    }

                    var idx = 0;
                    foreach (var item in (IList) pair.Value)
                        pairs.Add($"{WebUtility.UrlEncode(pair.Key)}[{idx++}]={WebUtility.UrlEncode(item.ToString())}"
                        );
                }
                else
                {
                    pairs.Add($"{WebUtility.UrlEncode(pair.Key)}={WebUtility.UrlEncode(pair.Value.ToString())}");
                }

            return pairs.ToArray();
        }

        private static object ConvertValue(object value, bool isDate)
        {
            if (value is string || value is int || value is long
                || value is double)
                return Convert.ToString(value, CultureInfo.InvariantCulture);

            if (value is bool) return value.ToString()?.ToLower();

            if (value is Enum)
            {
                var eType = value.GetType();
                var fi = eType.GetRuntimeField(value.ToString() ?? string.Empty);
                if (fi is not null)
                {
                    var attrs = (EnumMemberAttribute[]) fi.GetCustomAttributes(
                        typeof(EnumMemberAttribute), false);
                    if (attrs.Length == 0)
                        throw new ArgumentException("Enum fields must be decorated with DescriptionAttribute!");
                    return attrs[0].Value;
                }
            }

            if (value is JToken) return value.ToString();

            if (value is IList)
            {
                var origList = (IList) value;
                var l = new List<string>();
                foreach (var item in origList) l.Add((string) ConvertValue(item, isDate));
                return l;
            }

            if (value is DateTime)
                return isDate
                    ? ((DateTime) value).ToString("yyyy-MM-dd")
                    : ApiUtil.ConvertToTimestamp((DateTime) value).ToString();
            throw new ArgumentException("Type [" + value.GetType() + "] not handled");
        }
    }
}