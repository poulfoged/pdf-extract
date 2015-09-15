using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using PdfExtract.Executables;

namespace PdfExtract
{
    /// <summary>
    /// Extracts text from PDF's
    /// </summary>
    public class Extractor : IDisposable
    {
        private Action<string, object[]> _logger = (message, args) => Trace.WriteLine(string.Format(message, args));
        private readonly Lazy<TemporaryFile> _pdfToTextExecutable = new Lazy<TemporaryFile>(ReadPdfToText);
        private bool _disposed;

        private static TemporaryFile ReadPdfToText()
        {
            return new TemporaryFile(typeof(RessourceTarget).Assembly.GetManifestResourceStream(typeof(RessourceTarget), "pdftotext.exe"), "exe");
        }

        /// <summary>
        /// Extracts text from the provided stream - stream must be readable
        /// </summary>
        /// <param name="pdfStream">Stream to extract to</param>
        /// <returns></returns>
        public Stream ExtractText(Stream pdfStream)
        {
            if (_disposed)
                throw new ObjectDisposedException("Extractor");

            if (pdfStream == null)
                throw new ArgumentNullException("pdfStream");

            if (!pdfStream.CanRead)
                throw new ApplicationException("Stream not readable");

            var result = new MemoryStream();
            using (var sourceFile = new TemporaryFile(pdfStream))
            using (var destinationFile = new TemporaryFile())
            {
                var processStartInfo = new ProcessStartInfo(" \"" + _pdfToTextExecutable.Value.Info.FullName + " \"")
                {
                    UseShellExecute = false,
                    Arguments = " \"" + sourceFile.Info + "\" \"" + destinationFile.Info + "\"",
                    WindowStyle = ProcessWindowStyle.Maximized,
                    CreateNoWindow = true,
                    LoadUserProfile = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    StandardOutputEncoding = Encoding.ASCII,
                };


                using (var process = Process.Start(processStartInfo))
                {
                    process.ErrorDataReceived += (sender, eventargs) => Log(eventargs.Data);
                    process.OutputDataReceived += (sender, eventargs) => Log(eventargs.Data);
                    process.BeginOutputReadLine();
                    process.WaitForExit();
                }

                destinationFile.CopyTo(result);
            }
            return result;
        }

        /// <summary>
        /// Extracts text from the provided stream - stream must be readable
        /// </summary>
        /// <param name="pdfStream">Stream to extract to</param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public string ExtractToString(Stream pdfStream, Encoding encoding = null)
        {
            using (var textStream = ExtractText(pdfStream))
            using (var reader = new StreamReader(textStream, encoding ?? Encoding.UTF8))
                return reader.ReadToEnd();
        }

        /// <summary>
        /// Changes any logging of output to the provided action
        /// </summary>
        /// <param name="logger">Logger to log to</param>
        /// <returns></returns>
        public Extractor LogTo(Action<string, object[]> logger)
        {
            _logger = logger;
            return this;
        }


        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            try
            {
                if (_pdfToTextExecutable.IsValueCreated)
                    _pdfToTextExecutable.Value.Dispose();
            
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }
            _disposed = true;
            
        }

         ~Extractor()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Log(string format, params object[] args)
        {
            if (args == null || args.Length == 0)
                _logger("{0}", new object[] { format });
            else
                _logger(format, args);
        }
    }
}
