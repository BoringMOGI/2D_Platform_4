using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;

namespace DataFormat
{
    // 데이터 포멧 (Format)
    // = 게임에서 사용되는 특정 값들을 별도의 데이터로 변환시킬 때 사용하는 형식

    // XML (eXtensible Markup Language)
    // = 다목적 마크업 언어. 데이터를 태그 등으로 분류.
    //   주로 HTML에서 사용한다.

    // CSV (Comma Deparated Value)
    // = 데이터를 쉼표 기준으로 구분해 저장하는 방식. 주로 엑셀 등의 테이블 형식의 표에서 사용한다.

    // JSON (JaveScript Object Natation)
    // = 쉼표-값 혹은 키-값 형태로 이루어진 개방형 표준 포멧.
    //   프로그램 변수 값 표현에 적합하며 확장성이 좋다.

    public class CSVData
    {
        Dictionary<string, string>[] data;

        public int Count => data.Length;

        public CSVData(Dictionary<string, string>[] data)
        {
            this.data = data;
        }

        public string Get(int index, string key)
        {
            Debug.Log("요청 : " + key + " length : " + key.Length);
            foreach (string k in data[index].Keys)
            {                
                Debug.Log($"{k} == {key} : {k == key} : length : {k.Length}");
            }

            return data[index][key];
        }
        public Dictionary<string, string>[] GetOriginData()
        {
            return data;
        }
    }

    class CSV
    {
        // 파일 명을 경로로 변환하는 함수.
        private static TextAsset GetTextAsset(string fileName)
        {
            string path = string.Concat("CsvData/", fileName);
            return Resources.Load<TextAsset>(path);
        }
        public static CSVData ReadCSV(string fileName)
        {
            // 파일명이 비어있을 경우.
            if (string.IsNullOrEmpty(fileName))
            {
                Console.WriteLine("file name is empty.");
                return null;
            }

            string[] textArray = GetTextAsset(fileName).text.Split('\n');
            
            try
            {
                List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();

                // CSV 데이터의 첫 줄. 헤더 영역을 추출.
                string[] header = textArray[0].Split(',');        // 쉼표(,)를 구분으로 데이터를 자른다. (key값)

                for(int index = 1; index<textArray.Length; index++)
                {
                    if (string.IsNullOrEmpty(textArray[index]))
                        continue;

                    string[] datas = textArray[index].Split(',');           // 쉼표(,)를 구분으로 데이터를 자른다.
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    for (int i = 0; i < datas.Length; i++)
                        dic.Add(header[i], datas[i]);           // 최초에 추출한 헤더와 매칭시켜서 dic에 저장.

                    list.Add(dic);                              // 완성된 dic을 list에 추가.
                }

                // list를 배열로 변환 후 해당 값으로 Data 클래스 객체 생성 및 초기화.
                // 그 Data 객체를 return한다.
                return new CSVData(list.ToArray());
            }
            // try안에서 에러가 발생했을 경우 catch문으로 이동해
            // Exception(예외) 문구를 출력.
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
        public static void WriteCSV(string fileName, CSVData data)
        {
            /*
            Dictionary<string, string>[] originData = data.GetOriginData();

            string path = ConvertToPath(fileName);
            StreamWriter writer = new StreamWriter(path);

            writer.WriteLine(ConvertToKeyString(originData[0]));

            for (int index = 1; index < originData.Length; index++)
                writer.WriteLine(ConvertToValueString(originData[index]));

            writer.Close();

            Console.WriteLine("파일 쓰기 완료!! [{0}]", fileName);
            Console.WriteLine("경로 : {0}", path);
            */
        }

        private static string ConvertToKeyString<Key, Value>(Dictionary<Key, Value> dictionary)
        {
            Dictionary<Key, Value>.Enumerator dic = dictionary.GetEnumerator();

            StringBuilder builder = new StringBuilder();

            while (dic.MoveNext())
            {
                builder.Append(dic.Current.Key.ToString());
                builder.Append(",");
            }

            builder.Remove(builder.Length - 1, 1);

            return builder.ToString();
        }
        private static string ConvertToValueString<Key, Value>(Dictionary<Key, Value> dictionary)
        {
            Dictionary<Key, Value>.Enumerator dic = dictionary.GetEnumerator();

            StringBuilder builder = new StringBuilder();

            while (dic.MoveNext())
            {
                builder.Append(dic.Current.Value.ToString());
                builder.Append(",");
            }

            builder.Remove(builder.Length - 1, 1);

            return builder.ToString();
        }
    }
}
