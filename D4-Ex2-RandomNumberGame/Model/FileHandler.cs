using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace D4_Ex2_RandomNumberGame.Model
{
    public class FileHandler
    {
        private DirectoryInfo actualLogFileDirectory;
        private FileInfo actualLogFile;
        DateTime sessionStartTime;

        public FileHandler()
        {
            actualLogFileDirectory = new DirectoryInfo(Environment.CurrentDirectory);
            actualLogFile = new FileInfo(actualLogFileDirectory.FullName + @"\ComplexLog.txt");
            StartSession();
        }
        
        private void StartSession()
        {
            sessionStartTime = DateTime.Now;
            
            if(!actualLogFile.Exists)
            {
                try
                {
                    actualLogFile.Create().Dispose();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Exception has occured!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            LogToFile($"#Session: {sessionStartTime.ToString()} has been started:");
        }
        public void StopSession(string additionalComment)
        {
            DateTime timeStamp = DateTime.Now;
            LogToFile($"#Session: {sessionStartTime.ToString()} has been closed at: {timeStamp.ToString()}. Total time of running: {(timeStamp - sessionStartTime).ToString()}.# {additionalComment} #");
        }
        public void LogToFile(string logLine)
        {
            try
            {
                using (StreamWriter sw = actualLogFile.AppendText())
                {
                    sw.WriteLine(logLine);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception has occured!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public List<Player> ReadAllPlayersFromLog()
        {
            string rawLine;
            List<string> rawData = new List<string>();
            List<Player> playersList = new List<Player>();

            string[] buffer;
            decimal score;

            try
            {
                using (StreamReader sr = new StreamReader(actualLogFile.OpenRead()))
                {
                    while(!sr.EndOfStream)
                    {
                        rawLine = sr.ReadLine();
                        if (!(rawLine.Contains("#Session")))
                        {
                            rawData.Add(rawLine);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception has occured!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return playersList;
            }

            foreach (string row in rawData)
            {
                buffer = row.Split(';');

                playersList.Add(new Player(buffer[0]));
                if (Decimal.TryParse(buffer[1],NumberStyles.Any,CultureInfo.InvariantCulture, out score)) playersList.Last().Score = score;
                else playersList.Last().Score = 0m;
            }

            return playersList;

        }
    }
}
