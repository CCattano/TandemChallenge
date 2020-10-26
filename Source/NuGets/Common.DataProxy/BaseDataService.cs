using System.IO;
using System.Threading.Tasks;

namespace Tandem.Common.DataProxy
{
    public class BaseDataService
    {
        //Normally this would be a SQL ConnStr, or a Mongo Db Name
        private readonly string _dataFileLoc;
        private int LastReadLen;
        public BaseDataService(string dataFileLoc)
        {
            _dataFileLoc = dataFileLoc;
        }

        /// <summary>
        ///     Opens, reads, and returns the current contents of the dataFile specified
        /// </summary>
        /// <param name="dataFileName">The data file to open and read</param>
        /// <returns>
        ///     <see langword="string"/> content of the dataFile specified if found.
        ///     <br />Otherwise returns <see langword="null"/>
        /// </returns>
        public async Task<string> GetFileContents(string dataFileName)
        {
            //Normally when dealing with a Db, this is where
            //A Db connection would be instantiated and returned
            //To a repository so the Repo could perform work against the Db
            if (FileExists(dataFileName, out string fullFilePath))
            {
                string fileContents = await File.ReadAllTextAsync(fullFilePath);
                LastReadLen = fileContents.Length;
                return fileContents;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        ///     Attempt to write file contents to the data file specified.
        /// </summary>
        /// <remarks>
        /// <para>
        ///     This is an overwrite operation. So do not remove any data received from <see cref="GetFileContents(string)"/>
        ///     <br />Only append to, or modify data, hard deletes are intentionally not supported.
        /// </para>
        /// <para>If the length of content provided is lower than the current length of the doc the file will not be updated.</para>
        /// </remarks>
        /// <param name="dataFileName">The name of the data file to write to</param>
        /// <param name="fileContents">The contents to overwrite the file with.</param>
        /// <returns>A <see langword="bool"/> value indicating if the file was written to or not</returns>
        public async Task<bool> TryWriteFileContents(string dataFileName, string fileContents)
        {
            if(fileContents.Length < LastReadLen)
            {
                //Choosing to not support hard deletes here.
                //Supporting soft deletes here via flag toggles if neccesary
                //And not supporting hard deletes via removing data from file
                return false;
            }
            else if (FileExists(dataFileName, out string fullFilePath))
            {
                try
                {
                    await File.WriteAllTextAsync(fullFilePath, fileContents);
                    return true;
                }
                catch
                {
                    //Typically if an exception occurred a global middleware would
                    //Capture and write it to a logging table or collection in a db
                    //Here I'm opting for a silent exception and returning false
                    //to indicate that a file write did not occur
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool FileExists(string dataFileName, out string fullFilePath)
        {
            fullFilePath = $"{_dataFileLoc}/{dataFileName}";
            bool doesExist = File.Exists(fullFilePath);
            return doesExist;
        }
    }
}
