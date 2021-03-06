﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Tandem.Common.DataProxy
{
    public class BaseDataService
    {
        //Normally this would be a SQL ConnStr, or a Mongo Db Name
        private readonly string _dataFileLoc;

        private int LastReadLen;
        private volatile List<Guid> WriteQueue = new List<Guid>();

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
                Guid ticket = GetInQueue();
                WaitForTurn(ticket);
                string fileContents = await File.ReadAllTextAsync(fullFilePath);
                LeaveQueue(ticket);
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
            if (fileContents.Length < LastReadLen && LastReadLen - fileContents.Length > 50)
            {
                //Choosing to not support hard deletes here. (as best as possible at least)
                //Supporting soft deletes here via flag toggles if neccesary
                //And not supporting hard deletes via removing data from file
                //Assume if content being written is 50 characters shorter than last read
                //That content is being deleted from file and reject the file write
                return false;
            }
            else if (FileExists(dataFileName, out string fullFilePath))
            {
                Guid ticket = GetInQueue();

                WaitForTurn(ticket);

                bool wroteSuccessfully;
                try
                {
                    await File.WriteAllTextAsync(fullFilePath, fileContents);
                    wroteSuccessfully = true;
                }
                catch
                {
                    //Typically if an exception occurred a global middleware would
                    //Capture and write it to a logging table or collection in a db
                    //Here I'm opting for a silent exception and returning false
                    //to indicate that a file write did not occur
                    wroteSuccessfully = false;
                }

                LeaveQueue(ticket);
                return wroteSuccessfully;
            }
            else
            {
                return false;
            }
        }

        #region PRIVATE HELPER METHODS
        private bool FileExists(string dataFileName, out string fullFilePath)
        {
            fullFilePath = $"{_dataFileLoc}\\{dataFileName}";
            bool doesExist = File.Exists(fullFilePath);
            return doesExist;
        }

        private Guid GetInQueue()
        {
            Guid ticket = Guid.NewGuid();
            WriteQueue.Add(ticket);
            return ticket;
        }

        private void WaitForTurn(Guid ticket)
        {
            while (WriteQueue[0] != ticket)
            {
                Thread.Sleep(100);
            }
        }

        private void LeaveQueue(Guid ticket)
        {
            WriteQueue.Remove(ticket);
        }
        #endregion
    }
}