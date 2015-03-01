using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;

namespace vita
{
    public class Co
    {
        public static string GetStringInit(string _s)
        {
            if (String.IsNullOrEmpty(_s)) return "";
            if (String.IsNullOrEmpty(_s.Trim())) return "";

            return _s.Trim();
        }

        // 普通版本
        public static string BetweenAB(string _targetSearch, string _a, string _b)
        {
            int aInt = _targetSearch.IndexOf(_a);

            if (aInt == -1)
                return _targetSearch;

            int bInt = _targetSearch.IndexOf(_b, aInt + _a.Length);

            if (bInt == -1)
                return _targetSearch;

            int BeginIndex = aInt + _a.Length;

            if (bInt <= BeginIndex)
                return _targetSearch;

            return _targetSearch.Substring(BeginIndex, bInt - BeginIndex);
        }

        // 分层
        public static string BetweenAB(string _targetSearch, string _a, string _b, bool _inner)
        {
            if (_inner)
                return BetweenAB(_targetSearch, _a, _b, 0);
            else
                return BetweenAB(_targetSearch, _a, _b, 1);
        }

        // 分层
        public static string BetweenAB(string _targetSearch, string _a, string _b, int _targetLayer)
        {
            int aInt = _targetSearch.IndexOf(_a);

            if (aInt == -1)
                return _targetSearch;

            aInt = BetweenAB_SearcInnerIndex(_targetSearch, _a, -1, -1, _targetLayer);
            int bInt = BetweenAB_SearcInnerLastIndex(_targetSearch, _b, -1, -1, _targetLayer);

            if (bInt == -1)
                return _targetSearch;

            int BeginIndex = aInt + _a.Length;

            if (bInt <= BeginIndex)
                return _targetSearch;

            return _targetSearch.Substring(BeginIndex, bInt - BeginIndex);
        }

        private static int BetweenAB_SearcInnerIndex(string _targetSearch, string _a, int _index, int _layerCount, int _targetLayer)
        {
            // _index       default 0
            // _layerCount  default 0
            // _targetLayer default 0 || N : 0 不在指定层次停止 即找到最内层 : N 在第 N 层停止

            if (_index == -1)
                _index = 0;

            if (_layerCount == -1)
                _layerCount = 0;

            int aInt = _targetSearch.IndexOf(_a, _index);

            if (aInt == -1 && _index == 0)
                return -1;

            int nextIndex = aInt + _a.Length;

            if (nextIndex >= _targetSearch.Length)
            {
                return aInt;
            }
            else if (_targetSearch.IndexOf(_a, nextIndex) != -1)
            {
                _layerCount++;

                if (_targetLayer == _layerCount)
                    return aInt;
                else
                    return BetweenAB_SearcInnerIndex(_targetSearch, _a, nextIndex, _layerCount, _targetLayer);
            }
            else
            {
                return aInt;
            }
        }

        private static int BetweenAB_SearcInnerLastIndex(string _targetSearch, string _a, int _index, int _layerCount, int _targetLayer)
        {
            // # _index       default _targetSearch.Length - 1
            // # _layerCount  default 0

            // _targetLayer default 0 || N : 0 不在指定层次停止 即找到最内层 : N 在第 N 层停止

            if (_index == -1)
                _index = _targetSearch.Length - 1;

            if (_layerCount == -1)
                _layerCount = 0;

            int aInt = _targetSearch.LastIndexOf(_a, _index);

            if (aInt == -1 && _index == 0)
                return -1;

            int nextIndex = aInt - _a.Length;

            if (nextIndex <= -1)
            {
                return aInt;
            }
            else if (_targetSearch.LastIndexOf(_a, nextIndex) != -1)
            {
                _layerCount++;

                if (_targetLayer == _layerCount)
                    return aInt;
                else
                    return BetweenAB_SearcInnerLastIndex(_targetSearch, _a, nextIndex, _layerCount, _targetLayer);
            }
            else
            {
                return aInt;
            }
        }

        public static string BeforeA(string _targetSearch, string _a)
        {
            return BeforeA(_targetSearch, _a, 1, false);
        }

        public static string BeforeA(string _targetSearch, string _a, int _aTarget, bool outPutReverse)
        {
            string targetSearch = "";

            if (_aTarget <= 0)
                return targetSearch;

            int aInt = 0;
            int aLength = _a.Length;
            int targetSearchLength = _targetSearch.Length;
            int aTargetCount = 0;

            aInt = _targetSearch.IndexOf(_a);

            if (aInt == -1)
                return _targetSearch;
            else
                aTargetCount++;

            // BeforeA(".a",".",1) 返回空值
            if (aInt <= 0 && _aTarget == 1 && !outPutReverse)
                return targetSearch;

            // BeforeA(".a","a",1) 返回空值
            if ((aInt == targetSearchLength - 1) && outPutReverse)
                return targetSearch;

            // _a 存在并未到达 _aTarget
            while (aInt != -1 && aTargetCount < _aTarget)
            {
                aInt = _targetSearch.IndexOf(_a, aInt + aLength);
                aTargetCount++;
            }

            // 没有达到第 _aTarget 个 _a 返回原值
            if (aTargetCount != _aTarget)
                return _targetSearch;

            // 这时的 aInt 是第 _aTarget 个 _a 在 _targetSearch 的 index
            if (outPutReverse)
                targetSearch = _targetSearch.Substring(aInt + aLength, targetSearchLength - aInt - aLength);
            else
                targetSearch = _targetSearch.Substring(0, aInt);

            return targetSearch;
        }

        public static string BeforeAOutPutReverse(string _targetSearch, string _a)
        {
            return BeforeA(_targetSearch, _a, 1, true);
        }

        public static string AfterBOutOutPutReverse(string _targetSearch, string _a)
        {
            return AfterB(_targetSearch, _a, 1, true);
        }

        // 这里假设你知道一共有多少个 _a 并且指定第 _aTarget 的 _a 为标记
        // _a 标记是从 _targetSearch 尾部开始数起 _a == 1 就是 _targetSearch 左边数起最后一个
        public static string AfterB(string _targetSearch, string _a, int _aTarget)
        {
            return AfterB(_targetSearch, _a, _aTarget, false);
        }

        public static string AfterB(string _targetSearch, string _a, int _aTarget, bool outPutReverse)
        {
            // _aTarget default 1

            string targetSearch = "";

            if (_aTarget <= 0)
                return targetSearch;

            int aInt = 0;
            int aLength = _a.Length;
            int targetSearchLength = _targetSearch.Length;
            int aTargetCount = 0;

            aInt = _targetSearch.LastIndexOf(_a);

            if (aInt == -1)
                return _targetSearch;
            else
                aTargetCount++;

            // AfterB("a.",".",1) 返回空值
            if ((aInt >= targetSearchLength - 1) && _aTarget == 1 && !outPutReverse)
                return targetSearch;

            // AfterB("a.","a",1) 返回空值
            if (aInt <= 0 && outPutReverse)
                return targetSearch;

            // _a 存在并未到达 _aTarget
            while (aInt != -1 && aTargetCount < _aTarget)
            {
                aInt = _targetSearch.LastIndexOf(_a, aInt - 1);
                aTargetCount++;
            }

            // 没有达到第 _aTarget 个 _a 返回原值
            if (aTargetCount != _aTarget)
                return _targetSearch;

            // 这时的 aInt 是第 _aTarget 个 _a 在 _targetSearch 的 index
            if (outPutReverse)
                targetSearch = _targetSearch.Substring(0, aInt);
            else
                targetSearch = _targetSearch.Substring(aInt + aLength, targetSearchLength - aInt - aLength);

            return targetSearch;
        }

        public static void RePlaceTheStringCBetweenABForTag(ref string _targetSearch, string _a, string _b, string _tag, ref string _cString)
        {
            _cString = BetweenAB(_targetSearch, _a, _b);

            _targetSearch = BeforeA(_targetSearch, _a) + _tag + AfterB(_targetSearch, _b, 1);
        }

        public static string[] WordsToArrayByABTag(string _targetSearch, string[] _a, string[] _b, bool _mapping, bool _distinct)
        {
            if ("" == GetStringInit(_targetSearch))
                return null;

            List<string> words = new List<string>();

            string tmp = "";

            string tmp_targetSearch = _targetSearch;

            while (WordsToArrayByABTag(ref tmp_targetSearch, ref _a, ref _b, _mapping, ref tmp))
            {
                if (_distinct)
                {
                    if (!words.Contains(tmp))
                        words.Add(tmp);
                }
                else
                {
                    words.Add(tmp);
                }

                tmp = "";
            }

            return words.ToArray();
        }

        public static bool WordsToArrayByABTag(ref string _targetSearch, ref string[] _a, ref string[] _b, bool _mapping, ref string _word)
        {
            int indexA_in_targetSearch = 0;
            int indexB_in_targetSearch = 0;

            int indexA_in_array = -1;
            int indexB_in_array = -1;

            bool indexIn = false;

            bool parkA = false;
            bool parkB = false;


            foreach (string a in _a)
            {
                indexA_in_targetSearch = _targetSearch.IndexOf(a);
                indexA_in_array++;

                if (indexA_in_targetSearch != -1)
                    break;
            }

            int bIndexMinimum = _targetSearch.Length;
            int bIndexInArrayMinimum = -1;

            foreach (string b in _b)
            {
                indexB_in_targetSearch = _targetSearch.IndexOf(b, indexA_in_targetSearch + 1);
                indexB_in_array++;

                if (indexB_in_targetSearch == -1)
                    continue;

                if (bIndexMinimum > indexB_in_targetSearch)
                {
                    bIndexMinimum = indexB_in_targetSearch;
                    bIndexInArrayMinimum = indexB_in_array;
                }
            }

            indexB_in_targetSearch = bIndexMinimum;
            indexB_in_array = bIndexInArrayMinimum;

            indexIn = (indexB_in_targetSearch > indexA_in_targetSearch
                        &&
                        indexA_in_targetSearch != -1);

            parkA = (indexIn
                    &&
                    indexA_in_array == indexB_in_array
                    &&
                    _mapping);

            parkB = (indexIn
                    &&
                    !_mapping);

            _word = BetweenAB(_targetSearch, _a[indexA_in_array], _b[indexB_in_array]);

            _targetSearch = _targetSearch.Substring(indexB_in_targetSearch, _targetSearch.Length - indexB_in_targetSearch);

            return (indexIn && (parkA || parkB));
        }

        public static string GetInit(string _s)
        {
            if (String.IsNullOrEmpty(_s)) return "";
            if (String.IsNullOrEmpty(_s.Trim())) return "";

            return _s.Trim();
        }

        public static string GetSessionString(string _sessionKey)
        {
            if (System.Web.HttpContext.Current.Session == null)

                return "";

            return System.Web.HttpContext.Current.Session[_sessionKey] == null ? "" : GetStringInit(System.Web.HttpContext.Current.Session[_sessionKey].ToString());
        }

        public static int GetRandomNumber(int _minN, int _maxN)
        {
            string seed = String.Empty;

            for (int i = 0; i < 3; i++)
            {
                seed += Guid.NewGuid().ToByteArray()[i].ToString();
            }

            Random r = new Random(Convert.ToInt32(seed));

            return r.Next(_minN, _maxN);
        }

        public static string GetRandomChar(string _charSeed, int _maxLength)
        {
            string randomChar = String.Empty;

            for (int i = 0; i < _maxLength; i++)
            {
                randomChar += _charSeed.Substring(GetRandomNumber(0, _charSeed.Length), 1);
            }

            return randomChar;
        }

        public static string GetPhysicsPathInit(string _p)
        {
            string p = GetStringInit(_p);

            if (p == "") return "";

            if (p.IndexOf("/") != -1)
                p = p.Replace("/", @"\");

            if (p.IndexOf(@"\\") != -1)
                p = p.Replace(@"\\", @"\");

            if (p.EndsWith(@"\"))
            {
                // c:\test\
                if (!Directory.Exists(p))
                    Directory.CreateDirectory(p);
            }
            else
            {
                // c:\test\file.txt
                string cPath = AfterBOutOutPutReverse(p, @"\");

                if (!String.IsNullOrEmpty(cPath))
                {
                    cPath += @"\";

                    if (!Directory.Exists(cPath))
                        Directory.CreateDirectory(cPath);
                }
            }

            return p;
        }

        public static int GetStringLengthInByte(string _s)
        {
            return StringToByteArray(_s).Length;
        }

        public static byte[] StringToByteArray(string _s)
        {
            return Encoding.Default.GetBytes(_s);
        }

    }
}
