using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;

namespace DataContracts
{
    public class WeighingRecordOperations : EntityOperations
    {
        public static bool AddNewWeighing(DateTime date, int productNumber, int plantNumber, decimal weight, decimal tph, int fileId)
        {
            try
            {
                if (_context == null)
                {
                    _context = new PlantViewEntities();
                }

                var weighing = new WeighingRecord { Date = date, FileId = fileId, PlantId = plantNumber, ProductId = productNumber, Weight = weight, TonsPerHour = tph };
                _context.AddToWeighingRecords(weighing);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return false;

            }
        }

        public static bool AddNewWeighing(DateTime date, int productNumber, int plantNumber, decimal weight, decimal tph)
        {
            try
            {
                if (_context == null)
                {
                    _context = new PlantViewEntities();
                }

                var weighing = new WeighingRecord { Date = date, FileId = 0, PlantId = plantNumber, ProductId = productNumber, Weight = weight, TonsPerHour = tph };
                _context.AddToWeighingRecords(weighing);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return false;

            }
        }
    }
}
