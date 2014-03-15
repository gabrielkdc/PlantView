using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;

namespace Utilities
{
    public static class DataFileDecoder
    {
        private static DateTime GetDate(int yearIndex, int monthIndex, int dayIndex, char dateDivider, string date, string time)
        {
            date = date.Trim();
            var dateInfo = date.Split('/');
            if (dateInfo[0].Length < 2)
            {
                dateInfo[0] = string.Format("0{0}", dateInfo[0]);
            }
            if (dateInfo[1].Length < 2)
            {
                dateInfo[1] = string.Format("0{0}", dateInfo[1]);
            }
            if (dateInfo[2].Length < 2)
            {
                dateInfo[2] = string.Format("0{0}", dateInfo[2]);
            }


            int yearStart = dateInfo[yearIndex].Length > 2 ?  dateInfo[yearIndex].Length - 2 : 0;
            int monthStart = dateInfo[monthIndex].Length > 2 ?  dateInfo[monthIndex].Length - 2 : 0;
            int dayStart = dateInfo[dayIndex].Length > 2 ?  dateInfo[dayIndex].Length - 2 : 0 ;
            var timeInfo = time.Split(':');
            var year = 2000 + Convert.ToInt32(dateInfo[yearIndex].Substring(yearStart, 2));
            var month = Convert.ToInt32(dateInfo[monthIndex].Substring(monthStart, 2));
            var day = Convert.ToInt32(dateInfo[dayIndex].Substring(dayStart, 2));
            return new DateTime(year, month, day, Convert.ToInt32(timeInfo[0]), Convert.ToInt32(timeInfo[1]), Convert.ToInt32(timeInfo[2])); 
        }

        private static List<DataRecord> GetDataVersion0(string filename, int yearIndex, int monthIndex, int dayIndex, char dateDivider)
        {
            var result = new List<DataRecord>();
            try
            {
                var fileStream = File.OpenRead(filename);
                using (var streamReader = new StreamReader(fileStream))
                {
                    var line = streamReader.ReadLine();
                    if (line != null)
                    {
                        if (line.IndexOf('0') > 0)
                        {
                            line = line.Substring(line.IndexOf('0') + 1);
                        }

                        var values = line.Split(',');
                     
                        while (!streamReader.EndOfStream)
                        {
                            try
                            {                               
                                values = line.Split(',');
                                var date = GetDate(yearIndex, monthIndex, dayIndex, dateDivider, values[5], values[4]);
                                var data = new DataRecord { Date = date, PlantId = Convert.ToInt32(values[1]), ProductId = Convert.ToInt32(values[2]), Weight = Convert.ToDecimal(values[3], CultureInfo.GetCultureInfo("en-US")) };
                                var lastLine = result.Where(i => i.ProductId == data.ProductId).OrderByDescending(d => d.Date).FirstOrDefault() ;
                                data.TonsPerHour =  lastLine != null ?
                                      ( (data.Date - lastLine.Date).TotalMinutes > 0 ? 
                                         Math.Round((data.Weight - lastLine.Weight) / ((decimal)(data.Date - lastLine.Date).TotalMinutes) * 60, 2) :   0 )
                                     :  0;
                                result.Add(data);
                               
                            }
                            catch (Exception ex)
                            {
                                ErrorLog.Log(ex);
                            }

                            line = streamReader.ReadLine();
                        }
                    }
                }
                fileStream.Close();
                return result;
                
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
            }
            return null;
        }

        private static List<DataRecord> GetDataVersion1(string filename, int yearIndex, int monthIndex, int dayIndex, char dateDivider)
        {
            var result = new List<DataRecord>();
            try
            {
                var fileStream = File.OpenRead(filename);
                using (var streamReader = new StreamReader(fileStream))
                {
                    var line = streamReader.ReadLine();  //header line
                    line = streamReader.ReadLine();
                    if (line != null)
                    {
                        line = line.Replace("\"", "");
                        var values = line.Split(',');
                        while (!streamReader.EndOfStream)
                        {
                            try
                            {
                                values = line.Split(',');
                                var date = GetDate(yearIndex, monthIndex, dayIndex, dateDivider, values[0], values[1]);

                                int index = 4;
                                while (index <= values.Count() - 1)
                                {
                                    var data = new DataRecord { Date = date, PlantId = Convert.ToInt32(values[index+1]), ProductId = Convert.ToInt32(values[index + 2]), Weight = Convert.ToDecimal(values[index + 3], CultureInfo.GetCultureInfo("en-US")), TonsPerHour = Convert.ToDecimal(values[index + 4], CultureInfo.GetCultureInfo("en-US")) };
                                    result.Add(data);
                                    index += 6;
                                }
                            }
                            catch (Exception ex)
                            {
                                ErrorLog.Log(ex);
                            }
                            line = streamReader.ReadLine();
                        }
                    }
                }
                fileStream.Close();
                return result;
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
            }
            return null;
        }

        private static List<DataRecord> GetDataVersion2(string filename, int yearIndex, int monthIndex, int dayIndex, char dateDivider)
        {
            var result = new List<DataRecord>();
            DataRecord lastData = null;
            try
            {
                var fileStream = File.OpenRead(filename);
                using (var streamReader = new StreamReader(fileStream))
                {
                    var line = streamReader.ReadLine();  
                    if (line != null)
                    {                        
                        while (!streamReader.EndOfStream)
                        {
                            if (line.StartsWith("\""))
                            {
                                line = line.Substring(1);
                            }
                            if (line.EndsWith("\""))
                            {
                                line = line.Substring(0, line.Length - 1);
                            }
                            var values = line.Split(',');
                            try
                            {
                                var date = GetDate(yearIndex, monthIndex, dayIndex, dateDivider, values[5], values[4]);
                                var data = new DataRecord { Date = date, PlantId = Convert.ToInt32(values[1]), ProductId = Convert.ToInt32(values[2]), Weight = Convert.ToDecimal(values[3], CultureInfo.GetCultureInfo("en-US"))};
                                data.TonsPerHour = lastData != null ?
                                        ((data.Date - lastData.Date).TotalMinutes > 0 ?
                                            Math.Round((data.Weight - lastData.Weight) / ((decimal)(data.Date - lastData.Date).TotalMinutes) * 60, 2) : 0)
                                        : 0;
                                lastData = data;
                                result.Add(data);
                            }
                            catch (Exception ex)
                            {
                                ErrorLog.Log(ex);
                            }
                            line = streamReader.ReadLine();
                        }
                    }
                }
                fileStream.Close();
                return result;
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
            }

            return null;
        }

        private static List<DataRecord> GetDataVersion3(string filename, int yearIndex, int monthIndex, int dayIndex, char dateDivider)
        {
           //DAT, 1, 2014/3/13, 19:20:0,         0.0, Tons,           0, Hour, 9.880937, 20.000000,           0, Feet
            var result = new List<DataRecord>();
            try
            {
                var fileStream = File.OpenRead(filename);
                using (var streamReader = new StreamReader(fileStream))
                {
                    var line = streamReader.ReadLine();  //header line

                    int plantId = 1;
                    int productId = 1;

                    line = streamReader.ReadLine();
                    if (line != null)
                    {                      
                        var values = line.Split(',');
                        while (!streamReader.EndOfStream)
                        {
                            try
                            {
                                values = line.Split(',');
                                var date = GetDate(yearIndex, monthIndex, dayIndex, dateDivider, values[2], values[3]);
                                var data = new DataRecord { Date = date, PlantId = plantId, ProductId = productId, Weight = Convert.ToDecimal(values[4], CultureInfo.GetCultureInfo("en-US")), TonsPerHour = Convert.ToDecimal(values[6], CultureInfo.GetCultureInfo("en-US")) };
                                result.Add(data);
                            }
                            catch (Exception ex)
                            {
                                ErrorLog.Log(ex);
                            }
                            line = streamReader.ReadLine();
                        }
                    }
                }
                fileStream.Close();
                return result;
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
            }
            return null;
        }

        public static DataFile DecodeBeltwayFile(string filename, int yearIndex, int monthIndex, int dayIndex, char dateDivider)
        {
            var fileVersion = GetFileVersion(filename);
            var result = new DataFile { Filename = filename, Data = new List<DataRecord>() };
            switch (fileVersion)
            {
                case 0:
                    result.Data = GetDataVersion0(filename, yearIndex, monthIndex, dayIndex, dateDivider);
                    break;
                case 1:
                    result.Data = GetDataVersion1(filename, yearIndex, monthIndex, dayIndex, dateDivider);
                    break;
                case 2:
                    result.Data = GetDataVersion2(filename, yearIndex, monthIndex, dayIndex, dateDivider);
                    break;
                case 3:
                    result.Data = GetDataVersion3(filename, yearIndex, monthIndex, dayIndex, dateDivider);
                    break;           

                default:
                    throw new Exception("Not recognized fileformat");
            }
            return result;
        }


        private static int GetFileVersion(string filename)
        {
            var fileStream = File.OpenRead(filename);
            var result = -1;
            using (var streamReader = new StreamReader(fileStream))
            {
                string firstLine = streamReader.ReadLine();
                firstLine = firstLine.Replace("\"", "");

                if (filename.ToUpper().EndsWith("TXT"))
                {
                    firstLine = streamReader.ReadLine();
                    string[] values = firstLine.Split(',');
                    if (values[0] == "DAT")
                    {
                        return 3;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    if (filename.ToUpper().EndsWith("CSV"))
                    {                       
                        string[] values = firstLine.Split(',');
                        if (values[0] == "Date")
                        {
                            result = 1;
                        }
                        else
                        {
                            result = 2;
                        }                      
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
            fileStream.Close();
            return result;   
        }
    }
}
