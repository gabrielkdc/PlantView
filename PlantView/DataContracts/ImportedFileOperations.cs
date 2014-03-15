using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;

namespace DataContracts
{
    public class ImportedFileOperations : EntityOperations
    {
        public static int AddImportedFile(string filename)
        {
            try
            {
                if (_context == null)
                {
                    _context = new PlantViewEntities();
                }
                var file = new ImportedFile { Filename = filename };
                _context.AddToImportedFiles(file);
                _context.SaveChanges();
                return file.Id;
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return 0;
            }

        }

        public static int FileExists(string filename)
        {
            try
            {
                if (_context == null)
                {
                    _context = new PlantViewEntities();
                }

                return _context.ImportedFiles.Any(i => i.Filename == filename) ? 1 : 0;
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return -1;
            }
        }

    }
}
