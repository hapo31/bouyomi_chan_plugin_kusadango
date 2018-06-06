using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace Hapo31.kusidango.Service
{
    public class HttpServer
    {
        private int port;
        private HttpListener listener = null;

        public int Port { get { return port; } }
        public bool Closed { get { return listener == null; } }

        public delegate void OnReceive(HttpListenerRequest req, HttpListenerResponse res);

        public event OnReceive OnReceiveRequest;

        public HttpServer(int port)
        {
            this.port = port;
        }

        public void Close()
        {
            listener.Stop();
            listener.Close();
            listener = null;
        }

        public void Listen()
        {
            listener = new HttpListener();
            var prefix = String.Format("http://localhost:{0}/", port);
            listener.Prefixes.Add(prefix);
            listener.Start();

            listener.BeginGetContext(OnRequest, listener);
        }

        private void OnRequest(IAsyncResult state)
        {
            var listener = (HttpListener)state.AsyncState;
            if (listener == null || !listener.IsListening)
            {
                return;
            }
            var context = listener.EndGetContext(state);
            var req = context.Request;
            var res = context.Response;

            var resWriter = new StreamWriter(res.OutputStream);
            OnReceiveRequest?.Invoke(req, res);
            try
            {
                var resText = "\"ok\"\r\n\r\n";
                resWriter.Write(resText);
                resWriter.Flush();
                resWriter.Close();
            }
            catch
            {
                // nop
            }

            listener.BeginGetContext(OnRequest, listener);
        }
    }
}
